﻿using Mapster;

namespace ProvisionPadel.Api.Features.Courts.GetAll;

public record GetAllCourtsReponse(IEnumerable<CourtDto> courts);

public class GetAllCourtsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/court/get-all", async (ISender sender) =>
        {
            var query = new GetAllCourtsQuery();

            var result = await sender.Send(query);

            var response = result.Adapt<GetAllCourtsReponse>();

            return Results.Ok(response);
        })
        .WithName("GetAllCourts")
        .Produces<GetAllCourtsReponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get All Courts")
        .WithDescription("Get All Courts")
        .RequireAuthorization(options =>
        {
            options.AuthenticationSchemes = new[] { JwtBearerDefaults.AuthenticationScheme };
            options.RequireClaim(Access.Court, Access.View);
        });
    }
}