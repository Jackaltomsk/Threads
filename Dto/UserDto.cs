namespace Dto
{
	using System;
	using System.Collections.Generic;

	using ProtoBuf;

	/// <summary>
	/// Пользователь.
	/// </summary>
	[ProtoContract]
	public class UserDto : BaseDto
	{
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public UserDto()
		{
			Coordinates = new List<CoordinatesDto>();
		}
		
		/// <summary>
		/// Пароль.
		/// </summary>
		[ProtoMember(1)]
		public Guid Password { get; set; }
		
		/// <summary>
		/// Координаты, загруженные пользователем.
		/// </summary>
		[ProtoMember(2, AsReference = true)]
		public virtual List<CoordinatesDto> Coordinates { get; set; }
	}
}