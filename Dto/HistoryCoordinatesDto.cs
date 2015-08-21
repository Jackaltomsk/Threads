namespace Dto
{
	using System;

	using ProtoBuf;

	/// <summary>
	/// Сущность запроса истории координат.
	/// </summary>
	[ProtoContract]
	public class HistoryCoordinatesDto : BaseDto
	{
		/// <summary>
		/// Идентификатор загружающего пользователя.
		/// </summary>
		[ProtoMember(1)]
		public long UserId { get; set; }
		
		/// <summary>
		/// Начало временного интервала.
		/// </summary>
		[ProtoMember(2)]
		public DateTime? StartDate { get; set; }

		/// <summary>
		/// Окончание временного интервала.
		/// </summary>
		[ProtoMember(3)]
		public DateTime? EndDate { get; set; }
	}
}
