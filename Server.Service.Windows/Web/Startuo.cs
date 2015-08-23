namespace Server.Service.Windows.Web
{
	using System;
	using System.Net.Http.Headers;
	using System.Web.Http;

	using AutoMapper;

	using Dto;
	using Dto.Converters;

	using Microsoft.Owin;
	using Microsoft.Owin.Security.OAuth;

	using Newtonsoft.Json;

	using Owin;

	using Server.Db;

	using WebApiContrib.Formatting;

	/// <summary>
	/// Реализует конфигурирование OWIN-сервера.
	/// </summary>
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			ConfigureAuth(app);
			
			var webApiConfiguration = ConfigureWebApi();
			app.UseWebApi(webApiConfiguration);

			Mapper.CreateMap<User, UserDto>();
			Mapper.CreateMap<UserDto, User>();
			Mapper.CreateMap<Coordinates, CoordinatesDto>();
			Mapper.CreateMap<CoordinatesDto, Coordinates>();
		}

		private HttpConfiguration ConfigureWebApi()
		{
			var config = new HttpConfiguration();
			config.Routes.MapHttpRoute("DefaultId", "api/{controller}/{id}", new { id = RouteParameter.Optional });
			config.Routes.MapHttpRoute("DefaultAction", "api/{controller}/{action}/{id}", new { id = RouteParameter.Optional });

			// Добавляем протобуф-форматтер.
			config.Formatters.Add(new ProtoBufFormatter());
			
			// Делаем json-форматтер прпиоритетным перед xml.
			config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
			
			// Настраиваем обработку даты-времени в нужном формате.
			var settings = config.Formatters.JsonFormatter.SerializerSettings;
			settings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
			settings.Formatting = Formatting.Indented;
			settings.Converters.Add(new DateTimeConverter());
			
			return config;
		}

		/// <summary>
		/// Реализует конфигурирование аутентификации.
		/// </summary>
		/// <param name="app">Строитель.</param>
		private void ConfigureAuth(IAppBuilder app)
		{
			var oAuthOptions = new OAuthAuthorizationServerOptions
			{
				TokenEndpointPath = new PathString("/Token"),
				Provider = new OAuthProvider(),
				AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),

				// Сделано для демонстрации.
				AllowInsecureHttp = true
			};
			app.UseOAuthAuthorizationServer(oAuthOptions);
			app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
		}
	}
}