namespace Client.Commands
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Net.Http;

	/// <summary>
	/// Абстрактный класс команды-запроса к сервису.
	/// </summary>
	internal abstract class BaseApiCommand : BaseCommand
	{
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
		/// Реализует парсинг команды. При успешном парсинге аргументы помещаются в
		/// </summary>
		/// <param name="args">Аргументы команды.</param>
		/// <returns>Возвращает массив валидационных ошибок.</returns>
		public bool ParseArgs(string[] args)
		{
			// Проверяем, подходит ли команда по основным параметрам.
			if (args.Length != _argumentTypes.Length) return false;

			if (!IsValid(args[0])) return false;

			for (int i = 1; i < _argumentTypes.Length; i++)
			{
				switch (_argumentTypes[i].Name)
				{
					case "String":
					{
						_commandArguments.Add(args[i]);
						break;
					}
					case "Int32" :
					{
						int outInt;
						var parsed = Int32.TryParse(args[i], out outInt);

						if (parsed)
							_commandArguments.Add(args[i]);

						break;
					}
					case "DateTime":
					{
						DateTime dt;
						var parsed = DateTime.TryParseExact(args[i], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);

						if (parsed)
							_commandArguments.Add(args[i]);

						break;
					}
					default:
					{
						return false;
					}
				}
			}

			return args.Length == _commandArguments.Count;
		}

		/// <summary>
		/// Реализует запрос к сервису.
		/// </summary>
		/// <param name="baseAdress">Базовый адрес..</param>
		/// <returns>Возвращает ответ сервиса.</returns>
		public virtual string[] ProcessRequest(string baseAdress)
		{
			var query = string.Format(_queryFormat, _commandArguments);

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(new Uri(baseAdress), query);
				var response = client.GetAsync(client.BaseAddress).Result;

				var data = response.Content.ReadAsAsync(_returnType).Result;
				return this.ReturnTypeToString(data);
			}
		}

		/// <summary>
		/// Реализуе тпреобразование ответа сервера на команду в массив строк.
		/// </summary>
		/// <param name="data">Ответ сервера.</param>
		/// <returns>Возвращает массив строковых представлений ответа сервера.</returns>
		protected abstract string[] ReturnTypeToString(object data);
	}
}