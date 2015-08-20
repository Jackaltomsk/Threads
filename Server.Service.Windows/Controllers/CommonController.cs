namespace Server.Service.Windows.Controllers
{
	using System;
	using System.Web.Http;

	/// <summary>
	/// Контроллер общих методов.
	/// </summary>
	[RoutePrefix("api/common")]
	public class CommonController : ApiController
	{
		/// <summary>
		/// Реализует получение текущего времени на сервере.
		/// </summary>
		/// <returns></returns>
		[Route("serverTime")]
		[HttpGet]
		public IHttpActionResult GetServerTime()
		{
			return Ok(DateTime.Now);
		}
	}
}