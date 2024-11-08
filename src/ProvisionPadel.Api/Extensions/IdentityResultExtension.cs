namespace ProvisionPadel.Api.Extensions;

public static class IdentityResultExtension
{
    public static void ValidateOperation(this IdentityResult result, INotifier notifier)
    {
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                notifier.Add(error.Description);
            }
        }
    }
}
