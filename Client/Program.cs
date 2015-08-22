namespace Client
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Threading.Tasks;

	using Client.Commands;

	internal class Program
	{
		private static void Main(string[] args)
		{
			var commandTypes =
				Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(BaseApiCommand))).ToArray();
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
							Console.WriteLine();
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

						Console.WriteLine();
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Ошибка выполнения команды: " + ex.Message);
				}
			}
		}

		/*private static async Task Run()
		{
			// Create an http client provider:
			string hostUriString = "http://localhost:9000";
			var provider = new AuthClient(hostUriString);
			string _accessToken;
			Dictionary<string, string> _tokenDictionary;

			try
			{
				// Pass in the credentials and retrieve a token dictionary:
				_tokenDictionary = await provider.GetTokenDictionary("john@example.com", "password");
				_accessToken = _tokenDictionary["access_token"];
			}
			catch (AggregateException ex)
			{
				// If it's an aggregate exception, an async error occurred:
				Console.WriteLine(ex.InnerExceptions[0].Message);
				Console.WriteLine("Press the Enter key to Exit...");
				Console.ReadLine();
				return;
			}
			catch (Exception ex)
			{
				// Something else happened:
				Console.WriteLine(ex.Message);
				Console.WriteLine("Press the Enter key to Exit...");
				Console.ReadLine();
				return;
			}

			// Write the contents of the dictionary:
			foreach (var kvp in _tokenDictionary)
			{
				Console.WriteLine("{0}: {1}", kvp.Key, kvp.Value);
				Console.WriteLine("");
			}
		}*/
	}
}
