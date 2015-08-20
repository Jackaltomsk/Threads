namespace Server.Service.Windows.IoC
{
	using LightInject;
	
	using Server.Db.Infrastructure;

	/// <summary>
	/// Регистрация сервиса получения контекста.
	/// </summary>
	internal class DbContextRoot : ICompositionRoot
	{
		public void Compose(IServiceRegistry serviceRegistry)
		{
			serviceRegistry.Register<UsersRepository, UsersRepository>(new PerContainerLifetime());
			serviceRegistry.Register<CoordinatesRepository, CoordinatesRepository>(new PerContainerLifetime());
		}
	}
}