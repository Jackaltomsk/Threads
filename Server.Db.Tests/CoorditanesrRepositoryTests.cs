namespace Server.Db.Tests
{
	using NUnit.Framework;

	using Server.Db.Infrastructure;

	[TestFixture]
	public class UserRepositoryTests
	{
		[Test]
		public void UserCreation()
		{
			var userRep = new UsersRepository();
			var user = userRep.Create();

			try
			{
				Assert.That(user.Id, Is.GreaterThan(0));
				var changedUser = userRep.Create(user.Id);
				
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