namespace Dto
{
	using System;

	/// <summary>
	/// Сущность координат.
	/// </summary>
	public class CoordinatesDto : BaseDto
	{
		/// <summary>
		/// Широта.
		/// </summary>
		public decimal Latitude { get; set; }
		
		/// <summary>
		/// Долгота.
		/// </summary>
		public decimal Longtitude { get; set; }
		
		/// <summary>
		/// Идентификатор загружающего пользователя.
		/// </summary>
		public long UsersId { get; set; }
		
		/// <summary>
		/// Дата загрузки.
		/// </summary>
		public DateTime Date { get; set; }
	
		/// <summary>
		/// Сущность пользователя.
		/// </summary>
		public virtual UserDto User { get; set; }
	}
}
