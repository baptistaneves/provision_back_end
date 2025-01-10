namespace ProvisionPadel.Api.Extensions;

public static class AuthorizationPolicyBuilderExtensions
{
    public static AuthorizationPolicyBuilder RequireCustomClaim(
        this AuthorizationPolicyBuilder builder,
        string claimType,
        string claimValue)
    {
        return builder.RequireAssertion(context =>
            context.User.HasClaim(claim =>
                claim.Type == claimType && claim.Value == claimValue));
    }
}
