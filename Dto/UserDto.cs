namespace Dto
{
	using System;

	using ProtoBuf;

	/// <summary>
	/// ������������.
	/// </summary>
	[ProtoContract]
	public class UserDto : BaseDto
	{
		/// <summary>
		/// ������.
		/// </summary>
		[ProtoMember(1)]
		public Guid Password { get; set; }

		/// <summary>
		/// ����� ������������.
		/// </summary>
		public int Name { get; set; }

		public override string ToString()
		{
			return string.Format("{0}, {1}, {2}", Id, Name, Password);
		}
	}
}