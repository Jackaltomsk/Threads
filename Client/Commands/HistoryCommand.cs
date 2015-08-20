namespace Client.Commands
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Dto;

	/// <summary>
	/// Команда запроса времени с сервера.
	/// </summary>
	internal class HistoryCommand : BaseApiCommand
	{
		public HistoryCommand()
		{
			this._argumentTypes = new[] { typeof(int), typeof(DateTime), typeof(DateTime) };
			this._verb = "history";
			_commandArguments = new List<string>(_argumentTypes.Length);
			_queryFormat = "/api/coordinates/{0}/{1}/{2}";
			_returnType = typeof(CoordinatesDto[]);
		}

		protected override string[] ReturnTypeToString(object data)
		{
			var coordinates = (CoordinatesDto[])data;
			return coordinates.Select(c => c.ToString()).ToArray();
		}
	}
}