namespace Client.Commands
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net.Http;
	using System.Threading;

	using Client.Formatters;

	using Dto;
	using Dto.Converters;

	using global::IoC;

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
				thread.Start();
			}

			return new string[] { };
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="baseAdress"></param>
		/// <param name="startDate"></param>
		private void Work(int userName, string baseAdress, DateTime startDate)
		{
			UserDto user;

			try
			{
				using (var client = new HttpClient())
				{
					var createQquery = string.Format(_queryFormat, userName);
					client.BaseAddress = new Uri(new Uri(baseAdress), createQquery);
					var data = client.GetAsync(client.BaseAddress).ContinueWith(
								(t) => t.Result.Content.ReadAsAsync(_returnType, new[] { FormatterFactory.CreateJsonFormatter() }).Result)
							.Result;

					user = (UserDto)data;

					Console.WriteLine(user);
				}

				var random = new Random(user.Name);
				var lat = random.NextDouble();
				var lng = random.NextDouble();

				while (true)
				{
					var parameters = new CoordinatesDto
									{
										Date = startDate,
										Latitude = (decimal)(lat - random.NextDouble()),
										Longtitude = (decimal)(lng - random.NextDouble()),
										UserId = user.Id
									};

					using (var client = new HttpClient())
					{
						client.BaseAddress = new Uri(new Uri(baseAdress), _coordsPutFormat);
						var data = client.PutAsync(client.BaseAddress, parameters, FormatterFactory.CreateJsonFormatter())
								.ContinueWith(t => t.Result.Content.ReadAsAsync(_coordsReturnType, new[] { FormatterFactory.CreateJsonFormatter() }))
								.Result.Result;

						Console.WriteLine(string.Format("Добавлена запись координат, пользователь {0}.", userName));
					}

					Thread.Sleep(1000);
				}
			}
			catch (Exception ex)
			{
				Logger.Error("Ошибка имитации.", ex);
			}
		}

		public override void Stop()
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