namespace Dto
{
	using System;

	using ProtoBuf;

	/// <summary>
	/// �������� ���������.
	/// </summary>
	[ProtoContract]
	public class CoordinatesDto : BaseDto
	{
		/// <summary>
		/// ������.
		/// </summary>
		[ProtoMember(1)]
		public decimal Latitude { get; set; }
		
		/// <summary>
		/// �������.
		/// </summary>
		[ProtoMember(2)]
		public decimal Longtitude { get; set; }
		
		/// <summary>
		/// ������������� ������������ ������������.
		/// </summary>
		[ProtoMember(3)]
		public long UsersId { get; set; }
		
		/// <summary>
		/// ���� ��������.
		/// </summary>
		[ProtoMember(4)]
		public DateTime Date { get; set; }
	
		/// <summary>
		/// �������� ������������.
		/// </summary>
		[ProtoMember(5, AsReference = true)]
		public virtual UserDto User { get; set; }
	}
}
