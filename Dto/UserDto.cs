namespace Dto
{
	using System;
	using System.Collections.Generic;

	using ProtoBuf;

	/// <summary>
	/// ������������.
	/// </summary>
	[ProtoContract]
	public class UserDto : BaseDto
	{
		/// <summary>
		/// ����������� �� ���������.
		/// </summary>
		public UserDto()
		{
			Coordinates = new List<CoordinatesDto>();
		}
		
		/// <summary>
		/// ������.
		/// </summary>
		[ProtoMember(1)]
		public Guid Password { get; set; }
		
		/// <summary>
		/// ����������, ����������� �������������.
		/// </summary>
		[ProtoMember(2, AsReference = true)]
		public virtual List<CoordinatesDto> Coordinates { get; set; }
	}
}