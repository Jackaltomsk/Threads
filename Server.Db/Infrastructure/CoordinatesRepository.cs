namespace Server.Db.Infrastructure
{
	using System;
	using System.Data.Entity;
	using System.Data.Entity.Validation;
	using System.Linq;
	using System.Threading.Tasks;

	using Logging;

	/// <summary>
	/// Репозиторий координат.
	/// </summary>
	public class CoordinatesRepository : BaseRepository
	{
		/// <summary>
		/// Реализует сохранение координат в БД.
		/// </summary>
		/// <param name="coordinates">Координаты.</param>
		/// <returns>Возвращает количество сохраненных записей.</returns>
		public async Task<int> PutAsync(params Coordinates[] coordinates)
		{
			using (var ctx = await this.GetContextAsync())
			{
				try
				{
					var set = ctx.Set<Coordinates>();
					set.AddRange(coordinates.Where(c => c != null));

					return await ctx.SaveChangesAsync();
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
		/// Реализует получение координат из БД с учетом переданного интервала времени и идентификатора пользователя.
		/// </summary>
		/// <param name="name">Имя пользователя.</param>
		/// <param name="intervalStart">Начало временного интервала.</param>
		/// <param name="intervalEnd">Конец временного интервала.</param>
		/// <returns>Возвращает координаты, удовлетворяющие запросу.</returns>
		public async Task<Coordinates[]> GetAsync(int name, DateTime? intervalStart = null, DateTime? intervalEnd = null)
		{
			using (var ctx = await GetContextAsync())
			{
				try
				{
					var set = ctx.Set<Coordinates>();
					intervalStart = intervalStart ?? DateTime.MinValue;
					intervalEnd = intervalEnd ?? DateTime.MaxValue;

					var coords = await set.Where(c => c.User.Name == name && (c.Date >= intervalStart && c.Date <= intervalEnd)).ToArrayAsync();
					return coords;
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
		/// Реализует удаление координат.
		/// </summary>
		/// <param name="coordinates">Массив координат.</param>
		/// <returns>Возвращает количество удаленных записей.</returns>
		public int Remove(params Coordinates[] coordinates)
		{
			if (coordinates == null) return 0;

			using (var ctx = GetContext())
			{
				try
				{
					var set = ctx.Set<Coordinates>();
					var filteredCoords = coordinates.Where(u => u != null && u.Id > 0).ToArray();

					foreach (var coords in filteredCoords)
					{
						set.Attach(coords);
					}

					set.RemoveRange(filteredCoords);
					ctx.SaveChanges();

					return filteredCoords.Length;
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
