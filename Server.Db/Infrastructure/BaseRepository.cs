namespace Server.Db.Infrastructure
{
	using System.Data.Entity;

	using global::IoC;

	/// <summary>
	/// Базовый репозиторий.
	/// </summary>
	public abstract class BaseRepository
	{
		
		/// <summary>
		/// Ревлизует создание контекста.
		/// </summary>
		/// <returns></returns>
		public DbContext GetContext()
		{
			var container = LightInjectCore.Get();
			return container.GetInstance<DbContext>();
		}
	}
}
