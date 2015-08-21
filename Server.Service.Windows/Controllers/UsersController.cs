namespace Server.Service.Windows.Controllers
{
	using System;
	using System.Web.Http;

	using AutoMapper;

	using Dto;

	using global::IoC;
	
	using Server.Db.Infrastructure;

	[RoutePrefix("api/users")]
	public class UsersController : ApiController
	{
		/// <summary>
		/// Репозиторий пользователей.
		/// </summary>
		private UsersRepository _rep;

		public UsersController()
		{
			var container = LightInjectCore.Get();
			_rep = container.GetInstance<UsersRepository>();
		}
		
		/// <summary>
		/// Реализует создание или обновление пароля у пользователя.
		/// </summary>
		/// <param name="id">Идентификатор пользователя, у которого следует обновить пароль.</param>
		/// <returns>Возвращает сущность пользователя.</returns>
		[HttpGet]
		[Route("create/{id:int?}")]
		public IHttpActionResult Create(int id = 0)
		{
			try
			{
				var user = _rep.Create(id);
				var userDto = Mapper.Map<UserDto>(user);

				return Ok(userDto);
			}
			catch (Exception ex)
			{
				Logging.Logger.Error("Ошибка создания/обновления пароля пользователя.", ex);
				return InternalServerError(ex);
			}
		}
	}
}