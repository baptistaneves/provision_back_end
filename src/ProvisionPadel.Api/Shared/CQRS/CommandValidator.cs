namespace ProvisionPadel.Api.Shared.CQRS;

public static class CommandValidator
{
    public static async Task<Result<T>> ValidateAsync<T>(T command, IValidator<T> validator)
    {
        var validationResult = await validator.ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(e => new Error(e.ErrorMessage))
                .ToList();
            return Result<T>.Failure(errors);
        }

        return Result<T>.Success(command);
    }
}
