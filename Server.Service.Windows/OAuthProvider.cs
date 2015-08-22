namespace Server.Service.Windows
{
	using System;
	using System.Security.Claims;
	using System.Threading.Tasks;

	using Microsoft.Owin.Security.OAuth;

	using Server.Db.Infrastructure;

	internal class OAuthProvider : OAuthAuthorizationServerProvider
	{
		public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
		{
			// This call is required...
			// but we're not using client authentication, so validate and move on...
			await Task.FromResult(context.Validated());
		}

		public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
		{
			// DEMO ONLY: Pretend we are doing some sort of REAL checking here:
			var userRep = new UsersRepository();

			if (!userRep.IsUserValid(int.Parse(context.UserName), Guid.Parse(context.Password)))
			{
				context.SetError("invalid_grant", "Имя пользователя или пароль неправильны.");
				context.Rejected();
				return;
			}

			// Create or retrieve a ClaimsIdentity to represent the 
			// Authenticated user:
			var identity = new ClaimsIdentity(context.Options.AuthenticationType);
			identity.AddClaim(new Claim("user_name", context.UserName));

			// Identity info will ultimately be encoded into an Access Token
			// as a result of this call:
			context.Validated(identity);
		}
	}
}