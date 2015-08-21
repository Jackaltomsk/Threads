namespace Dto
{
	using System;

	using Dto.Converters;

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
		public long UserId { get; set; }
		
		/// <summary>
		/// Дата загрузки.
		/// </summary>
		[ProtoMember(4)]
		public DateTime Date { get; set; }

		public override string ToString()
		{
			return string.Format("Lat: {0}, Long: {1}, Date: {2}", Latitude, Longtitude, DateTimeConverter.Convert(Date));
		}
	}
}
