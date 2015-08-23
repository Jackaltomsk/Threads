namespace Server.Service.Windows
{
	using System;
	using System.Security.Claims;
	using System.Threading.Tasks;

	using Microsoft.Owin.Security.OAuth;

	using Server.Db.Infrastructure;

	/// <summary>
	/// Провайдер аутентификации.
	/// </summary>
	internal class OAuthProvider : OAuthAuthorizationServerProvider
	{
		public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
		{
			// Аутентификация на сервере, так что просто продолжаем.
			await Task.FromResult(context.Validated());
		}

		public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
		{
			var userRep = new UsersRepository();

			if (!await userRep.IsUserValid(int.Parse(context.UserName), Guid.Parse(context.Password)))
			{
				context.SetError("invalid_grant", "Имя пользователя или пароль неправильны.");
				context.Rejected();
				return;
			}
			
			var identity = new ClaimsIdentity(context.Options.AuthenticationType);
			identity.AddClaim(new Claim("user_name", context.UserName));
			
			context.Validated(identity);
		}
	}
}