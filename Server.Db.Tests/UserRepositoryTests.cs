namespace Server.Db.Tests
{
	using System;

	using NUnit.Framework;

	using Server.Db.Infrastructure;

	[TestFixture]
	public class CoorditanesrRepositoryTests
	{
		[Test]
		public async void UserCreationAsync()
		{
			var userRep = new UsersRepository();
			var user = await userRep.CreateAsync(102);

			try
			{
				Assert.That(user.Id, Is.GreaterThan(0));
				Assert.That(user.Name, Is.EqualTo(102));
				var changedUser = await userRep.CreateAsync(user.Name);
				
				Assert.That(changedUser.Id, Is.EqualTo(user.Id));
				Assert.That(changedUser.Password, Is.Not.EqualTo(user.Password));
			}
			finally
			{
				var count = userRep.Remove(user);
				Assert.That(count, Is.EqualTo(1));
			}
		}

		[Test]
		public async void UserValidationAsync()
		{
			var userRep = new UsersRepository();
			var user = await userRep.CreateAsync(102);

			try
			{
				Assert.That(user.Id, Is.GreaterThan(0));
				Assert.That(user.Name, Is.EqualTo(102));
				
				Assert.IsTrue(await userRep.IsUserValid(user.Name, user.Password));
				Assert.IsFalse(await userRep.IsUserValid(user.Name + 1, user.Password));
				Assert.IsFalse(await userRep.IsUserValid(user.Name, Guid.Empty));
			}
			finally
			{
				var count = userRep.Remove(user);
				Assert.That(count, Is.EqualTo(1));
			}
		}

		[Test]
		public void UserRemove()
		{
			var userRep = new UsersRepository();
			var count = userRep.Remove(null);

			Assert.That(count, Is.EqualTo(0));
		}
	}
}