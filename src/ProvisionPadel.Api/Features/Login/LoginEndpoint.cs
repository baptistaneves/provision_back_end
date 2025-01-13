using Mapster;

namespace ProvisionPadel.Api.Features.Login;

public record LoginRequest(string Email, string Password);

public record LoginResponse(Guid Id, string Token, string Email, string Name);

public class LoginEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/login", async (LoginRequest loginRequest, ISender sender) =>
        {
            var command = loginRequest.Adapt<LoginCommand>();

            var result = await sender.Send(command);

            return Response(result);
        })
         .WithName("Login")
         .Produces<LoginResponse>(StatusCodes.Status200OK)
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .WithDescription("Login");
    }
}
