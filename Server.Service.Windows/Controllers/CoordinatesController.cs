namespace Server.Service.Windows.Controllers
{
	using System;
	using System.Web.Http;

	using AutoMapper;

	using Dto;

	using global::IoC;

	using Server.Db;
	using Server.Db.Infrastructure;

	[RoutePrefix("api/coordinates")]
	public class CoordinatesController : ApiController
	{
		/// <summary>
		/// Репозиторий пользователей.
		/// </summary>
		private CoordinatesRepository _rep;

		public CoordinatesController()
		{
			var container = LightInjectCore.Get();
			_rep = container.GetInstance<CoordinatesRepository>();
		}
		
		/// <summary>
		/// Реализует добавление записи координат в БД.
		/// </summary>
		/// <returns>Возвращает сущность пользователя.</returns>
		[HttpPut]
		[Route("put")]
		public IHttpActionResult Create([FromBody]CoordinatesDto coordsDto)
		{
			try
			{
				var coords = Mapper.Map<Coordinates>(coordsDto);
				var count = _rep.Put(coords);
				return Ok(count);
			}
			catch (Exception ex)
			{
				Logging.Logger.Error("Ошибка добавления координат.", ex);
				return InternalServerError(ex);
			}
		}

		/// <summary>
		/// Реализует добавление записи координат в БД.
		/// </summary>
		/// <returns>Возвращает сущность пользователя.</returns>
		[HttpGet]
		[Route("{id:long}/{start:DateTime?}/{end:DateTime?}")]
		public IHttpActionResult Get(long id, DateTime? start, DateTime? end)
		{
			try
			{
				var coords = _rep.Get(id, start, end);
				var coordsdto = Mapper.Map<Coordinates[]>(coords);

				return Ok(coordsdto);
			}
			catch (Exception ex)
			{
				Logging.Logger.Error("Ошибка добавления координат.", ex);
				return InternalServerError(ex);
			}
		}
	}
}