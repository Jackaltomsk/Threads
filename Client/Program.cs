namespace Client
{
	using System;

	using Client.Commands;

	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Введите команду:");
			
			var input = /*Console.ReadLine()*/"time".Split(' ');

			var serverDate = new ServerTimeCommand();
			var parsed = serverDate.ParseArgs(input);

			if (parsed)
			{
				var data = serverDate.ProcessRequest("http://localhost:9000");

				foreach (var str in data)
				{
					Console.WriteLine(str);
				}
			}
		}
	}
}
