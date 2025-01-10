namespace ProvisionPadel.Api.Features.BaseEndpoints;

public abstract class BaseEndpoint
{
    protected IResult Response<T>(Result<T> result = null)
    {
        if(result != null)
        {
            return result.Map<IResult>(
                onSuccess: value => Results.Ok(value),
                onSingleFailure: error => Results.BadRequest(new { Error = error.Message }),
                onMultipleFailures: errors => Results.BadRequest(new { Errors = errors.Select(e => e.Message).ToList() })
            );
        }

        return Results.BadRequest();
    }
}
