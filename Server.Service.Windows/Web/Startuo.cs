namespace Server.Service.Windows.Web
{
	using System.Web.Http;

	using AutoMapper;

	using Dto;

	using Owin;

	using Server.Db;

	public class Startup
	{
		// This method is required by Katana:
		public void Configuration(IAppBuilder app)
		{
			var webApiConfiguration = ConfigureWebApi();

			// Use the extension method provided by the WebApi.Owin library:
			app.UseWebApi(webApiConfiguration);

			Mapper.CreateMap<User, UserDto>();
			Mapper.CreateMap<Coordinates, CoordinatesDto>();
			Mapper.CreateMap<Coordinates[], CoordinatesDto[]>();
		}


		private HttpConfiguration ConfigureWebApi()
		{
			var config = new HttpConfiguration();
			config.Routes.MapHttpRoute(
				"DefaultApi",
				"api/{controller}/{id}",
				new { id = RouteParameter.Optional });
			
			return config;
		}
	}
}