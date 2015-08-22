namespace Server.Service.Windows.Web
{
	using System.Collections.Generic;
	using System.Net.Http.Headers;
	using System.Web.Http;

	using AutoMapper;

	using Dto;
	using Dto.Converters;

	using Newtonsoft.Json;

	using Owin;

	using Server.Db;

	using WebApiContrib.Formatting;

	public class Startup
	{
		// This method is required by Katana:
		public void Configuration(IAppBuilder app)
		{
			var webApiConfiguration = ConfigureWebApi();

			// Use the extension method provided by the WebApi.Owin library:
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

			config.Formatters.Add(new ProtoBufFormatter());
			config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
			
			var settings = config.Formatters.JsonFormatter.SerializerSettings;
			settings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
			settings.Formatting = Formatting.Indented;
			settings.Converters.Add(new DateTimeConverter());
			
			return config;
		}
	}
}