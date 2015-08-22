namespace Client.Commands
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Net;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Threading.Tasks;

	using Client.Formatters;

	using Dto.Converters;

	using Logging;

	/// <summary>
	/// Абстрактный класс команды-запроса к сервису.
	/// </summary>
	internal abstract class BaseApiCommand
	{
		/// <summary>
		/// Слово команды.
		/// </summary>
		protected string _verb;
		
		/// <summary>
		/// Типы в аргументах.
		/// </summary>
		protected Type[] _argumentTypes;

		/// <summary>
		/// Тип объектов в ответе сервера.
		/// </summary>
		protected Type _returnType;

		/// <summary>
		/// Аргументы команды, заполняются после парсинга.
		/// </summary>
		protected List<string> _commandArguments;

		/// <summary>
		/// Строка для формата аргументов в запрос.
		/// </summary>
		protected string _queryFormat;

		/// <summary>
		/// Метод запроса.
		/// </summary>
		protected RequestMethod _method = RequestMethod.GET;

		/// <summary>
		/// Параметры для отправки.
		/// </summary>
		protected object _bodyParameters;

		/// <summary>
		/// Аутентификационный токен.
		/// </summary>
		protected string _token;
		
		/// <summary>
		/// Проверяет, валидна ли команда для данного запроса.
		/// </summary>
		/// <param name="verb">Запрос команды.</param>
		/// <returns>Возвращает признак валидности команды для запроса.</returns>
		protected bool IsValid(string verb)
		{
			return _verb == verb;
		}
		
		/// <summary>
		/// Реализует парсинг команды. При успешном парсинге аргументы помещаются в
		/// </summary>
		/// <param name="args">Аргументы команды.</param>
		/// <returns>Возвращает признак того, что команда распознана.</returns>
		public bool ParseArgs(string[] args)
		{
			// Проверяем, подходит ли команда по основным параметрам.
			if (args.Length != _argumentTypes.Length + 1) return false;

			if (!IsValid(args[0])) return false;

			_commandArguments.Clear();

			for (int i = 0; i < _argumentTypes.Length; i++)
			{
				switch (_argumentTypes[i].Name)
				{
					case "String":
					{
						_commandArguments.Add(args[i + 1]);
						break;
					}
					case "Int32" :
					{
						int outInt;
						var parsed = Int32.TryParse(args[i + 1], out outInt);

						if (parsed)
							_commandArguments.Add(args[i + 1]);

						break;
					}
					case "DateTime":
					{
						DateTime dt;
						var parsed = DateTime.TryParseExact(args[i + 1], DateTimeConverter.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);

						if (parsed)
							_commandArguments.Add(DateTimeConverter.Convert(dt));

						break;
					}
					default:
					{
						return false;
					}
				}
			}

			return _argumentTypes.Length == _commandArguments.Count;
		}

		/// <summary>
		/// Метод дополнительной логики.
		/// </summary>
		public virtual void PerformAdditionalLogic()
		{
			return;
		}

		/// <summary>
		/// Реализует запрос к сервису.
		/// </summary>
		/// <param name="baseAdress">Базовый адрес..</param>
		/// <returns>Возвращает ответ сервиса.</returns>
		public virtual string[] ProcessRequest(string baseAdress)
		{
			try
			{
				var query = string.Format(_queryFormat, _commandArguments);

				using (var client = new HttpClient())
				{
					client.BaseAddress = new Uri(new Uri(baseAdress), query);

					Task<HttpResponseMessage> response;

					switch (_method)
					{
						case RequestMethod.GET:
							{
								response = client.GetAsync(client.BaseAddress);
								break;
							}
						case RequestMethod.POST:
							{
								response = client.PostAsync(client.BaseAddress, _bodyParameters, FormatterFactory.CreateJsonFormatter());
								break;
							}
						case RequestMethod.PUT:
							{
								response = client.PutAsync(client.BaseAddress, _bodyParameters, FormatterFactory.CreateJsonFormatter());
								break;
							}
						case RequestMethod.DELETE:
							{
								response = client.DeleteAsync(client.BaseAddress);
								break;
							}
						default:
							{
								throw new NotImplementedException("Не реализована обработка типа: " + _method);
							}
					}

					if (!string.IsNullOrEmpty(_token))
						client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token); 

					var resp = response.Result;

					if (resp.StatusCode == HttpStatusCode.OK)
					{
						var data = resp.Content.ReadAsAsync(_returnType, new[] { FormatterFactory.CreateJsonFormatter() }).Result;
						return this.ReturnTypeToString(data);
					}
					
					return new[] { "Запрос вернул код: " + resp.StatusCode };
					
				}
			}
			catch (AggregateException ex)
			{
				Console.WriteLine(string.Format("Ошибка выполнения команды {0}: {1}", this._verb, ex.InnerException.Message));
				Logger.Error("Ошибка выполнения команды.", ex.InnerException);
			}

			return new string[] { };
		}

		/// <summary>
		/// Реализует останов команды.
		/// </summary>
		/// <param name="previousCommand">Предыдущая команда.</param>
		public virtual void Stop(BaseApiCommand previousCommand = null)
		{
			return;
		}

		/// <summary>
		/// Реализуе тпреобразование ответа сервера на команду в массив строк.
		/// </summary>
		/// <param name="data">Ответ сервера.</param>
		/// <returns>Возвращает массив строковых представлений ответа сервера.</returns>
		protected abstract string[] ReturnTypeToString(object data);

		public override string ToString()
		{
			return _verb;
		}
	}
}