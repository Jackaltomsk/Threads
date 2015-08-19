namespace Server.Db.Infrastructure
{
	using System;
	using System.Linq;

	/// <summary>
	/// Репозиторий пользователей.
	/// </summary>
	public class UsersRepository : BaseRepository
	{
		/// <summary>
		/// Реализует создание пользователя.
		/// </summary>
		/// <param name="id">Идентификатор пользователя.</param>
		/// <returns>возвращает созданного пользователя.</returns>
		public Users Create(long id = 0)
		{
			using (var ctx = GetContext())
			{
				var set = ctx.Set<Users>();
				var user = set.FirstOrDefault(u => u.Id == id);

				// Если пользователь с таким идентификатором в базе не найден, создадим нового.
				if (user == null) user = set.Create();

				user.Password = Guid.NewGuid();

				// Добавляем в контекст только если создан новый пользователь.
				if (user.Id == 0) set.Add(user);

				ctx.SaveChanges();

				return user;
			}
		}

		/// <summary>
		/// Реализует удаление пользователей.
		/// </summary>
		/// <param name="users">Массив пользователей.</param>
		/// <returns>Возвращает количество удаленных записей.</returns>
		public int Remove(params Users[] users)
		{
			if (users == null) return 0;

			using (var ctx = GetContext())
			{
				var set = ctx.Set<Users>();
				var filteredUsers = users.Where(u => u != null && u.Id > 0).ToArray();

				foreach (var user in filteredUsers)
				{
					set.Attach(user);
				}

				set.RemoveRange(filteredUsers);
				ctx.SaveChanges();
				
				return filteredUsers.Length;
			}
		}
	}
}
