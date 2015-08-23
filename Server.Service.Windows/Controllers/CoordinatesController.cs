namespace Server.Service.Windows.Controllers
{
	using System;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Web.Http;

	using AutoMapper;

	using Dto;

	using global::IoC;

	using Server.Db;
	using Server.Db.Infrastructure;

	[Authorize]
	[RoutePrefix("api/coordinates")]
	public class CoordinatesController : ApiController
	{
		/// <summary>
		/// Репозиторий пользователей.
		/// </summary>
		private readonly CoordinatesRepository _rep;

		public CoordinatesController()
		{
			var container = LightInjectCore.Get();
			_rep = container.GetInstance<CoordinatesRepository>();
		}
		
		/// <summary>
		/// Реализует добавление записи координат в БД.
		/// </summary>
		/// <param name="coordsDto">Координаты для добавления.</param>
		/// <returns>Возвращает сущность пользователя.</returns>
		[HttpPut]
		[Route("put")]
		public async Task<IHttpActionResult> Create([FromBody]CoordinatesDto coordsDto)
		{
			if (coordsDto == null)
				return BadRequest("Не передан необходимый параметр.");
			
			try
			{
				var coords = Mapper.Map<Coordinates>(coordsDto);
				var count = await _rep.PutAsync(coords);
				return Ok(count);
			}
			catch (Exception ex)
			{
				Logging.Logger.Error("Ошибка добавления координат.", ex);
				return InternalServerError();
			}
		}

		/// <summary>
		/// Реализует получение истории координат.
		/// </summary>
		/// <param name="requetsDto">Параметры запроса истории координат.</param>
		/// <returns>Возвращает сущность пользователя.</returns>
		[HttpPost]
		[Route("history")]
		public async Task<IHttpActionResult> History([FromBody]HistoryCoordinatesDto requetsDto)
		{
			if (requetsDto == null)
				return BadRequest("Не передан необходимый параметр.");

			try
			{
				var coords = await _rep.GetAsync(requetsDto.UserName, requetsDto.StartDate, requetsDto.EndDate);
				var coordsDto = coords.Select(Mapper.Map<CoordinatesDto>).ToArray();

				return Ok(coordsDto);
			}
			catch (Exception ex)
			{
				Logging.Logger.Error("Ошибка получения координат.", ex);
				return InternalServerError();
			}
		}
	}
}