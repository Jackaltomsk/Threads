namespace Server.Service.Windows
{
	using System;

	using Server.Service.Windows.Web;

	using Topshelf;

	public class Program
	{
		static void Main(string[] args)
		{
			var service = new WebApiService();
			service.Start();
			Console.ReadLine();
			service.Stop();

			/*HostFactory.Run(x =>
			{
				x.Service<WebApiService>(s =>
				{
					s.ConstructUsing(name => new WebApiService());
					s.WhenStarted(svc => svc.Start());
					s.WhenStopped(svc => svc.Stop());
				});

				x.RunAsLocalSystem();
				x.SetDescription("Тестовый сервис для эмуляции нагрузки");
				x.SetDisplayName("Сервис GPS координат");
				x.SetServiceName("WebApiService");
			}); */
		}
	}
}
