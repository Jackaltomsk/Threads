namespace Client.Commands
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Команда запроса времени с сервера.
	/// </summary>
	internal class ServerTimeCommand : BaseApiCommand
	{
		public ServerTimeCommand()
		{
			this._argumentTypes = new Type[] {};
			this._verb = "time";
			_commandArguments = new List<string>(_argumentTypes.Length);
			_queryFormat = "/api/common/serverTime";
			_returnType = typeof(DateTime);
		}

		protected override string[] ReturnTypeToString(object data)
		{
			return new[] { ((DateTime)data).ToString("O") };
		}
	}
}