namespace Dto
{
	using ProtoBuf;

	/// <summary>
	/// Базовый класс DTO.
	/// </summary>
	[ProtoContract, ProtoInclude(100, typeof(UserDto)), ProtoInclude(200, typeof(CoordinatesDto))]
	public abstract class BaseDto
	{
		/// <summary>
		/// Идентификатор.
		/// </summary>
		[ProtoMember(1)]
		public long Id { get; set; }

		public override string ToString()
		{
			return string.Format("{0}, {1}", this.GetType().Name, Id);
		}
	}
}
