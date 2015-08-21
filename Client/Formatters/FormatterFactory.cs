
namespace Client.Formatters
{
	using System.Net.Http.Formatting;

	using Dto.Converters;

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
	}
}
