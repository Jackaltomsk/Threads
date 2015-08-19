namespace Dto
{
	using System;

	/// <summary>
	/// �������� ���������.
	/// </summary>
	public class CoordinatesDto : BaseDto
	{
		/// <summary>
		/// ������.
		/// </summary>
		public decimal Latitude { get; set; }
		
		/// <summary>
		/// �������.
		/// </summary>
		public decimal Longtitude { get; set; }
		
		/// <summary>
		/// ������������� ������������ ������������.
		/// </summary>
		public long UsersId { get; set; }
		
		/// <summary>
		/// ���� ��������.
		/// </summary>
		public DateTime Date { get; set; }
	
		/// <summary>
		/// �������� ������������.
		/// </summary>
		public virtual UserDto User { get; set; }
	}
}
