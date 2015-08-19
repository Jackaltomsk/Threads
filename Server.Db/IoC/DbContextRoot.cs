namespace Server.Db.IoC
{
	using System.Data.Entity;

	using LightInject;

	/// <summary>
	/// Регистрация сервиса получения контекста.
	/// </summary>
	internal class DbContextRoot : ICompositionRoot
	{
		public void Compose(IServiceRegistry serviceRegistry)
		{
			serviceRegistry.Register<DbContext, LocalDbEntities>();
		}
	}
}