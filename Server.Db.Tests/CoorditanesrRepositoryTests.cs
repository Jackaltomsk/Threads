namespace Server.Db.Tests
{
	using System;

	using NUnit.Framework;

	using Server.Db.Infrastructure;

	[TestFixture]
	public class UserRepositoryTests
	{
		[Test]
		public void CoordinatesCreation()
		{
			var coordsRep = new CoordinatesRepository();
			var usersRep = new UsersRepository();

			var user = usersRep.Create(1);

			var coords = new Coordinates { UserId = user.Id, Date = DateTime.Now };
			var putCount = coordsRep.Put(coords);

			try
			{
				Assert.That(putCount, Is.EqualTo(1));
				Assert.That(coords.Id, Is.GreaterThan(0));

				var getted = coordsRep.Get(user.Name);

				Assert.That(getted.Length, Is.EqualTo(1));
				Assert.That(getted[0].Id, Is.EqualTo(coords.Id));
			}
			finally
			{
				var count = coordsRep.Remove(coords);
				Assert.That(count, Is.EqualTo(1));
				
				usersRep.Remove(user);
			}
		}

		[Test]
		public void CoordinatesRemove()
		{
			var coordsRep = new UsersRepository();
			var count = coordsRep.Remove(null);

			Assert.That(count, Is.EqualTo(0));
		}
	}
}