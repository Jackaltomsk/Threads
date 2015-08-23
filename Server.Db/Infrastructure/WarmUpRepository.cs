namespace Server.Db.Infrastructure
{
	/// <summary>
	/// Реализует разогрев контекста при холодном старте.
	/// </summary>
	public class WarmUpRepository : BaseRepository
	{
		/// <summary>
		/// Задача разогрева контекста при старте.
		/// </summary>
		public async void WarmUp()
		{
			await GetContextAsync();
		}
	}
}
