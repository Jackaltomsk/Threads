namespace Client
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	using Client.Commands;

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
			
			
			Console.WriteLine("Введите команду:");
			var input = string.Empty;
			BaseApiCommand currentCommand = null;

			while ((input = Console.ReadLine()) != "exit")
			{
				try
				{
					if (input == "stop")
					{
						if (currentCommand is ImitationCommand)
						{
							currentCommand.Stop();
							Console.WriteLine("Выполнение имитации остановлено.");
						}
					}
					else
					{
						var splittedIinput = input.Split(' ');
						currentCommand = createdCommands.FirstOrDefault(c => c.ParseArgs(splittedIinput));

						if (currentCommand != null)
						{
							currentCommand.PerformAdditionalLogic();
							var data = currentCommand.ProcessRequest("http://localhost:9000");

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
