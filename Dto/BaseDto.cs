namespace Dto
{
	/// <summary>
	/// Базовый класс DTO.
	/// </summary>
	public abstract class BaseDto
	{
		/// <summary>
		/// Идентификатор.
		/// </summary>
		public long Id { get; set; }

		public override string ToString()
		{
			return string.Format("{0}, {1}", this.GetType().Name, Id);
		}
	}
}
