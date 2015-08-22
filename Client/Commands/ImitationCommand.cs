namespace Client.Commands
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net.Http;
	using System.Net.Http.Formatting;
	using System.Net.Http.Headers;
	using System.Threading;

	using Client.Formatters;

	using Dto;
	using Dto.Converters;

	using Logging;

	/// <summary>
	/// Команда запуска имитации пользователей.
	/// </summary>
	internal class ImitationCommand : BaseApiCommand
	{
		/// <summary>
		/// Аргументы запроса.
		/// </summary>
		private string _coordsPutFormat;

		/// <summary>
		/// Возвращаемый тип после операции добавления координат.
		/// </summary>
		private Type _coordsReturnType;

		/// <summary>
		/// Пул рабочих потоков.
		/// </summary>
		private List<Thread> _threads;

		public ImitationCommand()
		{
			this._argumentTypes = new[] { typeof(int), typeof(DateTime) };
			this._verb = "start";
			_commandArguments = new List<string>(_argumentTypes.Length);
			_queryFormat = "/api/users/create/{0}";
			_coordsPutFormat = "/api/coordinates/put"; 
			_returnType = typeof(UserDto);
			_coordsReturnType = typeof(int);
			_method = RequestMethod.POST;
			_threads = new List<Thread>();
		}

		public override string[] ProcessRequest(string baseAdress)
		{
			Stop();

			var usersCount = int.Parse(_commandArguments[0]);
			var startDate = DateTimeConverter.Convert(_commandArguments[1]);

			for (var i = 0; i < usersCount; i++)
			{
				var userName = i;
				var thread = new Thread(() => Work(userName, baseAdress, startDate)) { IsBackground = true };
				_threads.Add(thread);
			}

			_threads.ForEach(t => t.Start());

			return new string[] { };
		}

		/// <summary>
		/// Реализует выполнение работы по имитации действий пользователей.
		/// </summary>
		/// <param name="userName">Имя пользователя.</param>
		/// <param name="baseAdress">Адрес сервиса.</param>
		/// <param name="startDate">Дата добавления координат, из команды.</param>
		private void Work(int userName, string baseAdress, DateTime startDate)
		{
			try
			{
				UserDto user;

				// Используем протобуф для четных пользователей.
				var useProtobuf = (userName % 2) == 0;

				using (var client = new HttpClient())
				{
					if (useProtobuf) 
						client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-protobuf"));

					var createQquery = string.Format(_queryFormat, userName);
					client.BaseAddress = new Uri(new Uri(baseAdress), createQquery);
					var data = client.GetAsync(client.BaseAddress).ContinueWith(t => 
						t.Result.Content.ReadAsAsync(_returnType, new MediaTypeFormatter[] { FormatterFactory.CreateJsonFormatter(), FormatterFactory.CreateProtoBufFormatter() }).Result).
						Result;

					user = (UserDto)data;

					Console.WriteLine(user);
					Logger.Trace(user.ToString());
				}

				var authClient = new AuthClient(baseAdress);
				var tokenDictionary = authClient.GetTokenDictionary(user.Name, user.Password).Result;
				var token = tokenDictionary["access_token"];

				// Случайные числа для координат.
				var random = new Random(user.Name);
				var lat = random.NextDouble();
				var lng = random.NextDouble();

				while (true)
				{
					var parameters = new CoordinatesDto
										{
											Date = startDate,
											Latitude = (decimal)((lat - random.NextDouble()) * 90),
											Longtitude = (decimal)((lng - random.NextDouble()) * 180),
											UserId = user.Id
										};

					using (var client = new HttpClient())
					{
						if (useProtobuf) 
							client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-protobuf"));

						client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); 

						client.BaseAddress = new Uri(new Uri(baseAdress), _coordsPutFormat);
						var data = client.PutAsync(client.BaseAddress, parameters, useProtobuf ? (MediaTypeFormatter)FormatterFactory.CreateProtoBufFormatter() : FormatterFactory.CreateJsonFormatter())
								.ContinueWith(t => t.Result.Content.ReadAsAsync(_coordsReturnType, new MediaTypeFormatter[] { FormatterFactory.CreateJsonFormatter(), FormatterFactory.CreateProtoBufFormatter() }).Result).
								Result;

						if ((int)data == 1)
						{
							var message = string.Format("Добавлена запись координат, пользователь {0}.", userName);
#if DEBUG
							Console.WriteLine(message);
#endif
							Logger.Trace(message);
						}
					}

					Thread.Sleep(1000);
				}
			}
			catch (ThreadAbortException){}
			catch (AggregateException ex)
			{
				Console.WriteLine(ex.InnerExceptions[0].Message);
				Logger.Error("Ошибка имитации.", ex.InnerExceptions[0]);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Logger.Error("Ошибка имитации.", ex);
			}
		}

		public override void Stop(BaseApiCommand previuosCommand = null)
		{
			foreach (var thread in _threads)
			{
				thread.Abort();
			}

			_threads.Clear();
		}

		protected override string[] ReturnTypeToString(object data)
		{
			var coordinates = (CoordinatesDto[])data;
			return coordinates.Select(c => c.ToString()).ToArray();
		}
	}
}