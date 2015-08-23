namespace Server.Db.Infrastructure
{
	using System.Data.Entity;
	using System.Threading.Tasks;

	using global::IoC;

	using Logging;

	/// <summary>
	/// Базовый репозиторий.
	/// </summary>
	public abstract class BaseRepository
	{

		/// <summary>
		/// Ревлизует создание контекста.
		/// </summary>
		/// <returns>Возвращает текущий контекст БД.</returns>
		public DbContext GetContext()
		{
			var container = LightInjectCore.Get();
			var context = container.GetInstance<DbContext>();
			context.Database.Connection.Open();

			Logger.Trace("Создан контекст БД.");

			return context;
		}
		
		/// <summary>
		/// Ревлизует асинхронное создание контекста.
		/// </summary>
		/// <returns>Возвращает текущий контекст БД.</returns>
		public async Task<DbContext> GetContextAsync()
		{
			Logger.Trace("Контекст БД запрошен асинхронно.");
			return await Task.Run(() => this.GetContext());
		}
	}
}