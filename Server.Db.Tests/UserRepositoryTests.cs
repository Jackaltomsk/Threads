namespace Server.Db.Tests
{
	using NUnit.Framework;

	using Server.Db.Infrastructure;

	[TestFixture]
	public class CoorditanesrRepositoryTests
	{
		[Test]
		public void UserCreation()
		{
			var userRep = new UsersRepository();
			var user = userRep.Create(1);

			try
			{
				Assert.That(user.Id, Is.GreaterThan(0));
				Assert.That(user.Name, Is.EqualTo(1));
				var changedUser = userRep.Create(user.Name);
				
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
		public void UserRemove()
		{
			var userRep = new UsersRepository();
			var count = userRep.Remove(null);

			Assert.That(count, Is.EqualTo(0));
		}
	}
}