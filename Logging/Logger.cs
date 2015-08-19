namespace Logging
{
	using System;
	using System.Runtime.CompilerServices;

	using NLog;

	/// <summary>
	/// Обёртка для Nlog.
	/// </summary>
	public static class Logger
	{
		/// <summary>
		/// статический конструктор.
		/// </summary>
		static Logger()
		{
			LogManager.EnableLogging();
		}

		/// <summary>
		/// Трассировка.
		/// </summary>
		/// <param name="message"> Сообщение. </param>
		/// <param name="exception"> Исключение. </param>
		/// <param name="callerPath"> Путь вызова. </param>
		/// <param name="callerMember"> Активатор. </param>
		/// <param name="callerLine"> Атрибут номера строки вызова. </param>
		public static void Trace(string message, Exception exception = null,
			[CallerFilePath] string callerPath = "",
			[CallerMemberName] string callerMember = "",
			[CallerLineNumber] int callerLine = 0)
		{
			Log(LogLevel.Trace, message, exception, callerPath, callerMember, callerLine);
		}

		/// <summary>
		/// Отладка.
		/// </summary>
		/// <param name="message"> Сообщение. </param>
		/// <param name="exception"> Исключение. </param>
		/// <param name="callerPath"> Путь вызова. </param>
		/// <param name="callerMember"> Активатор. </param>
		/// <param name="callerLine"> Атрибут номера строки вызова. </param>
		public static void Debug(string message, Exception exception = null,
			[CallerFilePath] string callerPath = "",
			[CallerMemberName] string callerMember = "",
			[CallerLineNumber] int callerLine = 0)
		{
			Log(LogLevel.Debug, message, exception, callerPath, callerMember, callerLine);
		}

		/// <summary>
		/// Информация.
		/// </summary>
		/// <param name="message"> Сообщение. </param>
		/// <param name="exception"> Исключение. </param>
		/// <param name="callerPath"> Путь вызова. </param>
		/// <param name="callerMember"> Активатор. </param>
		/// <param name="callerLine"> Атрибут номера строки вызова. </param>
		public static void Info(string message, Exception exception = null,
			[CallerFilePath] string callerPath = "",
			[CallerMemberName] string callerMember = "",
			[CallerLineNumber] int callerLine = 0)
		{
			Log(LogLevel.Info, message, exception, callerPath, callerMember, callerLine);
		}

		/// <summary>
		/// Предупреждение.
		/// </summary>
		/// <param name="message"> Сообщение. </param>
		/// <param name="exception"> Исключение. </param>
		/// <param name="callerPath"> Путь вызова. </param>
		/// <param name="callerMember"> Активатор. </param>
		/// <param name="callerLine"> Атрибут номера строки вызова. </param>
		public static void Warn(string message, Exception exception = null,
			[CallerFilePath] string callerPath = "",
			[CallerMemberName] string callerMember = "",
			[CallerLineNumber] int callerLine = 0)
		{
			Log(LogLevel.Warn, message, exception, callerPath, callerMember, callerLine);
		}

		/// <summary>
		/// Ошибка.
		/// </summary>
		/// <param name="message"> Сообщение. </param>
		/// <param name="exception"> Исключение. </param>
		/// <param name="callerPath"> Путь вызова. </param>
		/// <param name="callerMember"> Активатор. </param>
		/// <param name="callerLine"> Атрибут номера строки вызова. </param>
		public static void Error(string message, Exception exception = null,
			[CallerFilePath] string callerPath = "",
			[CallerMemberName] string callerMember = "",
			[CallerLineNumber] int callerLine = 0)
		{
			Log(LogLevel.Error, message, exception, callerPath, callerMember, callerLine);
		}

		/// <summary>
		/// Критическая ошибка.
		/// </summary>
		/// <param name="message"> Сообщение. </param>
		/// <param name="exception"> Исключение. </param>
		/// <param name="callerPath"> Путь вызова. </param>
		/// <param name="callerMember"> Активатор. </param>
		/// <param name="callerLine"> Атрибут номера строки вызова. </param>
		public static void Fatal(string message, Exception exception = null,
			[CallerFilePath] string callerPath = "",
			[CallerMemberName] string callerMember = "",
			[CallerLineNumber] int callerLine = 0)
		{
			Log(LogLevel.Fatal, message, exception, callerPath, callerMember, callerLine);
		}

		/// <summary>
		/// Запись сообщения в лог.
		/// </summary>
		/// <param name="level"> Тип лога. </param>
		/// <param name="message"> Сообщение. </param>
		/// <param name="exception"> Исключение. </param>
		/// <param name="callerPath"> Путь вызова. </param>
		/// <param name="callerMember"> Активатор. </param>
		/// <param name="callerLine"> Атрибут номера строки вызова. </param>
		private static void Log(LogLevel level, string message, Exception exception = null, string callerPath = "", string callerMember = "", int callerLine = 0)
		{
			var logger = LogManager.GetLogger(callerPath);

			var logEvent = new LogEventInfo(level, callerPath, message) { Exception = exception };
			logEvent.Properties.Add("callerpath", callerPath);
			logEvent.Properties.Add("callermember", callerMember);
			logEvent.Properties.Add("callerline", callerLine);
			logger.Log(logEvent);
		}
	}
}