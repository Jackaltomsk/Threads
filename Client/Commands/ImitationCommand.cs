namespace Client.Commands
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net.Http;
	using System.Threading;
	using System.Threading.Tasks;

	using Client.Formatters;

	using Dto;
	using Dto.Converters;

	using global::IoC;

	using LightInject;

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

		public ImitationCommand()
		{
			this._argumentTypes = new[] { typeof(int), typeof(DateTime) };
			this._verb = "start";
			_commandArguments = new List<string>(_argumentTypes.Length);
			_queryFormat = "/api/users/create";
			_coordsPutFormat = "/api/coordinates/put"; 
			_returnType = typeof(UserDto);
			_coordsReturnType = typeof(int);
			_method = RequestMethod.POST;
		}

		public override string[] ProcessRequest(string baseAdress)
		{
			
			var usersCount = int.Parse(_commandArguments[0]);
			var startDate = DateTimeConverter.Convert(_commandArguments[1]);

			var container = LightInjectCore.Get();
			var ts = container.GetInstance<CancellationTokenSource>();
			
			var token = ts.Token;

			for (int i = 0; i < usersCount; i++)
			{
				Task.Run(() =>
				//Task.Factory.StartNew(() =>
				{
					UserDto user;

						using (var client = new HttpClient())
						{
							var createQquery = string.Format(_queryFormat, _commandArguments);
							client.BaseAddress = new Uri(new Uri(baseAdress), createQquery);
							var data = client.GetAsync(client.BaseAddress, token)
									.ContinueWith((t) => t.Result.Content.ReadAsAsync(_returnType, new[] { FormatterFactory.CreateJsonFormatter() }, token), token)
									.Result.Result;
							
							user = (UserDto)data;

							Console.WriteLine(user);
						}

						while (!token.IsCancellationRequested)
							{
								_bodyParameters = new CoordinatesDto
												{
													Date = startDate,
													Latitude = user.Id,
													Longtitude = 2 * user.Id,
													UserId = user.Id
												};

								using (var client = new HttpClient())
								{
									client.BaseAddress = new Uri(new Uri(baseAdress), _coordsPutFormat);
									var data = client.PutAsync(client.BaseAddress, _bodyParameters, FormatterFactory.CreateJsonFormatter(), token)
										.ContinueWith(t => t.Result.Content.ReadAsAsync(_coordsReturnType, new[] { FormatterFactory.CreateJsonFormatter() }, token), token).Result.Result;
									
									Console.WriteLine(string.Format("Добавлена запись координат, пользователь {0}.", user.Id));
								}

								Thread.Sleep(1000);
							}
						}, token);
			}

			return new string[] { };
		}

		protected override string[] ReturnTypeToString(object data)
		{
			var coordinates = (CoordinatesDto[])data;
			return coordinates.Select(c => c.ToString()).ToArray();
		}
	}
}