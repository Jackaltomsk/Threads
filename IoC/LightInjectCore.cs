namespace IoC
{
	using System.Collections.Concurrent;
	using System.Reflection;

	using LightInject;

	/// <summary>
	/// Содержит в себе методы работы с LightInject контейнером. Хранит в себе словарь "сборка - контейнер".
	/// </summary>
	public static class LightInjectCore
	{
		/// <summary>
		/// Экземпляр контейнера LightInject.
		/// </summary>
		private static readonly ConcurrentDictionary<Assembly, ServiceContainer> _instanceDictionary;

		/// <summary>
		/// Статичный конструктор с инициализацией словаря.
		/// </summary>
		static LightInjectCore()
		{
			_instanceDictionary = new ConcurrentDictionary<Assembly, ServiceContainer>();
		}

		/// <summary>
		/// Реализует получение или создание(если еще не создан) экземпляра контейнера LightInject для вызывающей сборки.
		/// </summary>
		/// <param name="callingAssembly">Сборка, в которой искать привязки.</param>
		/// <returns>Возвращает экземпляр контейнера для вызывающей сборки.</returns>
		public static ServiceContainer Get(Assembly callingAssembly = null)
		{
			// Если не передана сборка, берем текущую вызывающую.
			var assembly = callingAssembly ?? Assembly.GetCallingAssembly();

			// Если уже создали контейнер под эту сборку, просто отдаем ее.
			if (_instanceDictionary.ContainsKey(assembly)) 
				return _instanceDictionary[assembly];

			var container = new ServiceContainer();
			// Регистрируем все сервисы, находящиеся в сборке.
			container.RegisterAssembly(assembly);
			_instanceDictionary.AddOrUpdate(assembly, container, (a, c) => c);

			return container;
		}
	}
}