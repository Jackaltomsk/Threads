namespace Server.Db.Models
{
	/// <summary>
	/// Базовый класс для БД-моделей.
	/// </summary>
	public abstract class BaseDb
	{
		/// <summary>
		/// Идентификатор.
		/// </summary>
		public abstract long Id { get; set; }

		public override string ToString()
		{
			return Id.ToString();
		}
	}
}
