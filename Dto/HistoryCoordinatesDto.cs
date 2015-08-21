namespace Dto
{
	using System;

	using ProtoBuf;

	/// <summary>
	/// �������� ������� ������� ���������.
	/// </summary>
	[ProtoContract]
	public class HistoryCoordinatesDto : BaseDto
	{
		/// <summary>
		/// ������������� ������������ ������������.
		/// </summary>
		[ProtoMember(1)]
		public long UserId { get; set; }
		
		/// <summary>
		/// ������ ���������� ���������.
		/// </summary>
		[ProtoMember(2)]
		public DateTime? StartDate { get; set; }

		/// <summary>
		/// ��������� ���������� ���������.
		/// </summary>
		[ProtoMember(3)]
		public DateTime? EndDate { get; set; }
	}
}
