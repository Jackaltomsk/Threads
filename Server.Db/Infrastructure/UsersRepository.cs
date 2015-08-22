namespace Server.Db.Infrastructure
{
	using System;
	using System.Data.Entity.Validation;
	using System.Linq;

	using Logging;

	/// <summary>
	/// Репозиторий пользователей.
	/// </summary>
	public class UsersRepository : BaseRepository
	{
		/// <summary>
		/// Реализует создание пользователя.
		/// </summary>
		/// <param name="name">Идентификатор пользователя.</param>
		/// <returns>возвращает созданного пользователя.</returns>
		public User Create(int name)
		{
			using (var ctx = GetContext())
			{
				try
				{
					var set = ctx.Set<User>();
					var user = set.FirstOrDefault(u => u.Name == name);

					// Если пользователь с таким идентификатором в базе не найден, создадим нового.
					if (user == null) user = set.Create();

					user.Name = name;
					user.Password = Guid.NewGuid();

					// Добавляем в контекст только если создан новый пользователь.
					if (user.Id == 0) set.Add(user);

					ctx.SaveChanges();

					return user;
				}
				catch (DbEntityValidationException)
				{
					var errors = ctx.GetValidationErrors().ToList();

					errors.ForEach(e => e.ValidationErrors.ToList()
						.ForEach(r => Logger.Error(string.Format("Ошибка валидации модели: сущность {0}, свойство {1}, ошибка {2}", e.Entry.Entity, r.PropertyName, r.ErrorMessage))));

					throw;
				}
			}
		}

		/// <summary>
		/// Реализует проверку валидности пользователя.
		/// </summary>
		/// <param name="userName">Имя пользователя.</param>
		/// <param name="password">Пароль.</param>
		/// <returns>Возвращает признак валидности имени пользователя/пароля.</returns>
		public bool IsUserValid(int userName, Guid password)
		{
			using (var ctx = GetContext())
			{
				try
				{
					var set = ctx.Set<User>();
					var user = set.FirstOrDefault(u => u.Name == userName && u.Password == password);

					return user != null;
				}
				catch (DbEntityValidationException)
				{
					var errors = ctx.GetValidationErrors().ToList();

					errors.ForEach(e => e.ValidationErrors.ToList()
						.ForEach(r => Logger.Error(string.Format("Ошибка валидации модели: сущность {0}, свойство {1}, ошибка {2}", e.Entry.Entity, r.PropertyName, r.ErrorMessage))));

					throw;
				}
			}
		}

		/// <summary>
		/// Реализует удаление пользователей.
		/// </summary>
		/// <param name="users">Массив пользователей.</param>
		/// <returns>Возвращает количество удаленных записей.</returns>
		public int Remove(params User[] users)
		{
			if (users == null) return 0;

			using (var ctx = GetContext())
			{
				try
				{
					var set = ctx.Set<User>();
					var filteredUsers = users.Where(u => u != null && u.Id > 0).ToArray();

					foreach (var user in filteredUsers)
					{
						set.Attach(user);
					}

					set.RemoveRange(filteredUsers);
					ctx.SaveChanges();

					return filteredUsers.Length;
				}
				catch (DbEntityValidationException)
				{
					var errors = ctx.GetValidationErrors().ToList();

					errors.ForEach(e => e.ValidationErrors.ToList()
						.ForEach(r => Logger.Error(string.Format("Ошибка валидации модели: сущность {0}, свойство {1}, ошибка {2}", e.Entry.Entity, r.PropertyName, r.ErrorMessage))));

					throw;
				}
			}
		}
	}
}
