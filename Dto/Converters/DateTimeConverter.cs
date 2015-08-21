namespace Dto.Converters
{
	using System;
	using System.Globalization;

	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;

	/// <summary>
	/// Конвертер для (де)сериализации даты-времени.
	/// Работает с форматами двух типов: "20.08.2011" и "20.08.2011 13:52:11".
	/// </summary>
	public class DateTimeConverter : DateTimeConverterBase
	{
		/// <summary>
		/// Формат даты-времени.
		/// </summary>
		private const string DateAndTime = "dd.MM.yyyy HH:mm:ss";
		
		/// <summary>
		/// Формат даты.
		/// </summary>
		public const string DateFormat = "dd.MM.yyyy";

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value == null)
				throw new ArgumentNullException("value", "Не передано значение даты-времени.");

			var dt = (DateTime)value;
			var val = Convert(dt);

			writer.WriteValue(val);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.Value == null)
				throw new NullReferenceException("Ошибка перевода строки в дату: строка не передана.");

			return Convert(reader.Value.ToString());
		}

		/// <summary>
		/// Реализует перевод даты-времени в необходимое строкове представление.
		/// </summary>
		/// <param name="dt"></param>
		/// <returns>Возвращает строковое представление даты.</returns>
		public static string Convert(DateTime dt)
		{
			if (dt.Hour == 0 && dt.Minute == 0 && dt.Second == 0)
				return dt.ToString(DateFormat, CultureInfo.InvariantCulture);
			
			return dt.ToString(DateAndTime, CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Реализует перевод даты-времени в необходимое строкове представление.
		/// </summary>
		/// <param name="value">Строка с датой.</param>
		/// <returns>Возвращает дату.</returns>
		public static DateTime Convert(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				throw new ArgumentNullException("value", "Не передана строка с датой.");
			
			DateTime dt;
			var parsed = DateTime.TryParseExact(value, DateAndTime, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
			if (parsed) return dt;

			parsed = DateTime.TryParseExact(value, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
			if (parsed) return dt;

			throw new Exception(string.Format("Невозможно перевести в дату строку вида [{0}].", value));
		}
	}
}