namespace Dto
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Пользователь.
	/// </summary>
	public class UserDto : BaseDto
	{
		/// <summary>
		/// Пароль.
		/// </summary>
		public Guid Password { get; set; }
		
		/// <summary>
		/// Координаты, загруженные пользователем.
		/// </summary>
		public virtual List<CoordinatesDto> Coordinates { get; set; }
	}
}