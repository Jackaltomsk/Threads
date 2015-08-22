namespace Client.Commands
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Dto;
	using Dto.Converters;

	/// <summary>
	/// Команда запроса истории GPS с сервера.
	/// </summary>
	internal class HistoryCommand : BaseApiCommand
	{
		public HistoryCommand()
		{
			this._argumentTypes = new[] { typeof(int), typeof(DateTime), typeof(DateTime) };
			this._verb = "history";
			_commandArguments = new List<string>(_argumentTypes.Length);
			_queryFormat = "/api/coordinates/history/get";
			_returnType = typeof(CoordinatesDto[]);
			_method = RequestMethod.POST;
		}

		public override void PerformAdditionalLogic()
		{
			_bodyParameters = CreateRequestParameter();
		}

		private object CreateRequestParameter()
		{
			var historyDto = new HistoryCoordinatesDto
							{
								UserName = int.Parse(_commandArguments[0]),
								StartDate = DateTimeConverter.Convert(_commandArguments[1]),
								EndDate = DateTimeConverter.Convert(_commandArguments[2])
							};

			return historyDto;
		}

		protected override string[] ReturnTypeToString(object data)
		{
			var coordinates = (CoordinatesDto[])data;
			return coordinates.Length > 1 ? coordinates.Select(c => c.ToString()).ToArray() : new[] { "Не найдено ни одной записи." };
		}
	}
}