namespace Server.Service.Windows.Controllers
{
	using System;
	using System.Web.Http;

	using AutoMapper;

	using Dto;

	using global::IoC;

	using Logging;

	using Server.Db.Infrastructure;

	[RoutePrefix("api/users")]
	public class UsersController : ApiController
	{
		/// <summary>
		/// Репозиторий пользователей.
		/// </summary>
		private readonly UsersRepository _rep;

		public UsersController()
		{
			var container = LightInjectCore.Get();
			_rep = container.GetInstance<UsersRepository>();
		}
		
		/// <summary>
		/// Реализует создание пользователя или обновление его пароля.
		/// </summary>
		/// <param name="id">Имя пользователя.</param>
		/// <returns>Возвращает сущность пользователя.</returns>
		[HttpGet]
		[Route("create/{id:int}")]
		public IHttpActionResult Create(int id)
		{
			try
			{
				Logger.Trace(string.Format("Запрос на создание пользоватея/изменение пароля с именем [{0}].", id));

				var user = _rep.Create(id);
				var userDto = Mapper.Map<UserDto>(user);

				return Ok(userDto);
			}
			catch (Exception ex)
			{
				Logger.Error("Ошибка создания/обновления пароля пользователя.", ex);
				return InternalServerError(ex);
			}
		}
	}
}