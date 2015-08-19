﻿namespace Server.Db.Infrastructure
{
	using System;
	using System.Linq;

	/// <summary>
	/// Репозиторий координат.
	/// </summary>
	public class CoordinatesRepository : BaseRepository
	{
		/// <summary>
		/// Реализует сохранение координат в БД.
		/// </summary>
		/// <param name="coordinates"></param>
		/// <returns>Возвращает количество сохраненных записей.</returns>
		public int Put(params Coordinates[] coordinates)
		{
			using (var ctx = GetContext())
			{
				var set = ctx.Set<Coordinates>();
				set.AddRange(coordinates.Where(c => c != null));

				return ctx.SaveChanges();
			}
		}

		/// <summary>
		/// Реализует получение координат из БД с учетом переданного интервала времени и идентификатора пользователя.
		/// </summary>
		/// <param name="id">Идентификатор пользователя.</param>
		/// <param name="intervalStart">Начало временного интервала.</param>
		/// <param name="intervalEnd">Конец временного интервала.</param>
		/// <returns>Возвращает координаты, удовлетворяющие запросу.</returns>
		public Coordinates[] Get(long id, DateTime intervalStart, DateTime intervalEnd)
		{
			using (var ctx = GetContext())
			{
				var set = ctx.Set<Coordinates>();
				var coords = set.Where(c => c.UsersId == id && (c.Date >= intervalStart && c.Date <= intervalEnd)).ToArray();

				return coords;
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
		}
	}
}
