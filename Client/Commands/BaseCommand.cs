namespace Client.Commands
{
	/// <summary>
	/// Класс базовой команды.
	/// </summary>
	internal abstract class BaseCommand
	{
		/// <summary>
		/// Команда.
		/// </summary>
		protected string _verb;

		/// <summary>
		/// Проверяет, валидна ли команда для данного запроса.
		/// </summary>
		/// <param name="verb">Запрос команды.</param>
		/// <returns>Возвращает признак валидности команды для запроса.</returns>
		protected bool IsValid(string verb)
		{
			// Проверяем, подходит ли команда по основным параметрам.
			return _verb != verb;
		}

		public override string ToString()
		{
			return _verb;
		}
	}
}
