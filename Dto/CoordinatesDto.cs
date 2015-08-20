namespace Dto
{
	using System;

	using ProtoBuf;

	/// <summary>
	/// Сущность координат.
	/// </summary>
	[ProtoContract]
	public class CoordinatesDto : BaseDto
	{
		/// <summary>
		/// Широта.
		/// </summary>
		[ProtoMember(1)]
		public decimal Latitude { get; set; }
		
		/// <summary>
		/// Долгота.
		/// </summary>
		[ProtoMember(2)]
		public decimal Longtitude { get; set; }
		
		/// <summary>
		/// Идентификатор загружающего пользователя.
		/// </summary>
		[ProtoMember(3)]
		public long UsersId { get; set; }
		
		/// <summary>
		/// Дата загрузки.
		/// </summary>
		[ProtoMember(4)]
		public DateTime Date { get; set; }
	
		/// <summary>
		/// Сущность пользователя.
		/// </summary>
		[ProtoMember(5, AsReference = true)]
		public virtual UserDto User { get; set; }
	}
}
