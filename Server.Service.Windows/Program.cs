namespace Server.Service.Windows
{
	using System;

	using Microsoft.Owin.Hosting;

	using Server.Service.Windows.Web;

	class Program
	{
		static void Main(string[] args)
		{
			string url = "http://localhost:9000";

			using (WebApp.Start<Startup>(url))
			{
				Console.WriteLine("Нажмите Enter для выхода.");
				Console.ReadLine();
			}
		}
	}
}
