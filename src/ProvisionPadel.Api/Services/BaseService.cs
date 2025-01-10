namespace ProvisionPadel.Api.Services;

public abstract class BaseService
{
    public List<Error> Validate<TV, TEntity>(TV validator, TEntity entity) where TV : AbstractValidator<TEntity> where TEntity : notnull
    {
        var validatorResult = validator.Validate(entity);

        if (!validatorResult.IsValid)
            return validatorResult.Errors.Select(x => new Error(x.ErrorMessage)).ToList();

        return new List<Error>();
    }
}