namespace Dto
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// ������������.
	/// </summary>
	public class UserDto : BaseDto
	{
		/// <summary>
		/// ������.
		/// </summary>
		public Guid Password { get; set; }
		
		/// <summary>
		/// ����������, ����������� �������������.
		/// </summary>
		public virtual List<CoordinatesDto> Coordinates { get; set; }
	}
}