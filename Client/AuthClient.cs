namespace Client
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Net.Http;
	using System.Threading.Tasks;

	using Newtonsoft.Json;

	/// <summary>
	/// Клиент аутентификации.
	/// </summary>
	public static class AuthClient
	{
		/// <summary>
		/// Аутентифицированные за сессию клиенты.
		/// </summary>
		private static readonly ConcurrentDictionary<int, Dictionary<string, string>> AuthenticatedClients;

		static AuthClient()
		{
			AuthenticatedClients = new ConcurrentDictionary<int, Dictionary<string, string>>();
		}
		
		/// <summary>
		/// Реализует аутентификацию на сервере.
		/// </summary>
		/// <param name="baseAdress">Адрес сервера.</param>
		/// <param name="userName">Имя пользователя.</param>
		/// <param name="password">Пароль.</param>
		/// <returns>Возвращает задачу, результатом работы которой является аутентификации.</returns>
		public static async Task<string> GetToken(string baseAdress, int userName, Guid password)
		{
			HttpResponseMessage response;

			var pairs = new List<KeyValuePair<string, string>>
							{
								new KeyValuePair<string, string>("grant_type", "password"),
								new KeyValuePair<string, string>("username", userName.ToString()),
								new KeyValuePair<string, string>("password", password.ToString())
							};
			
			var content = new FormUrlEncodedContent(pairs);

			using (var client = new HttpClient())
			{
				var tokenEndpoint = new Uri(new Uri(baseAdress), "Token");
				response = await client.PostAsync(tokenEndpoint, content);
			}

			var responseContent = await response.Content.ReadAsStringAsync();
			
			if (!response.IsSuccessStatusCode)
			{
				throw new Exception(string.Format("Error: {0}", responseContent));
			}

			var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
			AuthenticatedClients.AddOrUpdate(userName, dict, (n, d) => d);

			return dict["access_token"];
		}

		/// <summary>
		/// Реализует получение токена из кэша.
		/// </summary>
		/// <param name="userName">Имя пользователя.</param>
		/// <returns>Возвращает аутентификационный токен, если он есть в кэше. Иначе возвращает пустую строку.</returns>
		public static string GetTokenFromCache(int userName)
		{
			if (AuthenticatedClients.ContainsKey(userName)) 
				return AuthenticatedClients[userName]["access_token"];
			else
				return string.Empty;
			
		}
	}
}