namespace Server.Db.Infrastructure
{
	using System.Data.Entity;
	using System.Threading.Tasks;

	using global::IoC;

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
			return container.GetInstance<DbContext>();
		}
		
		/// <summary>
		/// Ревлизует асинхронное создание контекста.
		/// </summary>
		/// <returns>Возвращает текущий контекст БД.</returns>
		public async Task<DbContext> GetContextAsync()
		{
			return await Task.Run(() => this.GetContext());
		}
	}
}