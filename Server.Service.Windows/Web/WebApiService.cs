namespace Server.Service.Windows.Web
{
	using System;

	using Microsoft.Owin.Hosting;

	/// <summary>
	/// Сожерит в себе методы создания м запуска/останова сервиса.
	/// </summary>
	internal class WebApiService
	{
		/// <summary>
		/// Объект сервиса.
		/// </summary>
		private IDisposable _service;

		/// <summary>
		/// Адрес сервиса.
		/// </summary>
		private readonly string _url;

		/// <summary>
		/// Конструктор.
		/// </summary>
		public WebApiService()
		{
			_url = "http://localhost:9000";
		}

		/// <summary>
		/// Метод запуска сервиса.
		/// </summary>
		public void Start()
		{
			_service = WebApp.Start<Startup>(_url);
		}

		/// <summary>
		/// Метод останова сервиса.
		/// </summary>
		public void Stop()
		{
			_service.Dispose();
		}
	}
}