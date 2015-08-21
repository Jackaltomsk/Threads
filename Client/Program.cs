namespace Client
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using System.Reflection;
	using System.Threading;

	using Client.Commands;

	using global::IoC;

	class Program
	{
		static void Main(string[] args)
		{
			var commandTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(BaseApiCommand))).ToArray();
			var createdCommands = new List<BaseApiCommand>(commandTypes.Length);

			foreach (var cmm in commandTypes)
			{
				var created = (BaseApiCommand)Activator.CreateInstance(cmm);
				createdCommands.Add(created);
			}

			//var container = LightInjectCore.Get();
			//var scope = container.BeginScope();
			
			Console.WriteLine("Введите команду:");
			var input = string.Empty;

			while ((input = Console.ReadLine()) != "exit")
			{

				try
				{
					if (input == "stop")
					{
						var container = LightInjectCore.Get();
						var tokenSource = container.GetInstance<CancellationTokenSource>();
						tokenSource.Cancel();
						Console.WriteLine("Выполнение остановлено.");

						//scope.Dispose();
						//scope = container.BeginScope();
					}
					else
					{
						var splittedIinput = input.Split(' ');
						var command = createdCommands.FirstOrDefault(c => c.ParseArgs(splittedIinput));

						if (command != null)
						{
							command.PerformAdditionalLogic();
							var data = command.ProcessRequest("http://localhost:9000");

							foreach (var str in data)
							{
								Console.WriteLine(str);
							}
						}
						else Console.WriteLine("Неверная команда.");
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Ошибка выполнения команды: " + ex.Message);
				}
			}
		}
	}
}
