
namespace Client.Formatters
{
	using System.Net.Http.Formatting;

	using Dto.Converters;

	using WebApiContrib.Formatting;

	/// <summary>
	/// Содержит в себе методы создания форматтеров.
	/// </summary>
	internal static class FormatterFactory
	{
		/// <summary>
		/// Реализует создание json-форматтера.
		/// </summary>
		/// <returns>Возвращает json-форматтер.</returns>
		public static JsonMediaTypeFormatter CreateJsonFormatter()
		{
			var formatter = new JsonMediaTypeFormatter();
			formatter.SerializerSettings.Converters.Add(new DateTimeConverter());

			return formatter;
		}

		/// <summary>
		/// Реализует создание protobuf-форматтера.
		/// </summary>
		/// <returns>Возвращает protobuf-форматтер.</returns>
		public static ProtoBufFormatter CreateProtoBufFormatter()
		{
			var formatter = new ProtoBufFormatter();
			return formatter;
		}
	}
}
