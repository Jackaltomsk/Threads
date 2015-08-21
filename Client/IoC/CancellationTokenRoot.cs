namespace Client.IoC
{
	using System.Threading;

	using LightInject;

	/// <summary>
	/// Регистрация сервиса получения сорса токена.
	/// </summary>
	internal class CancellationTokenRoot : ICompositionRoot
	{
		public void Compose(IServiceRegistry serviceRegistry)
		{
			serviceRegistry.Register<CancellationTokenSource, CancellationTokenSource>(new PerContainerLifetime());
		}
	}
}