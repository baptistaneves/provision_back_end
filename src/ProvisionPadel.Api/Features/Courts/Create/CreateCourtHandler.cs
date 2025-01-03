﻿namespace ProvisionPadel.Api.Features.Courts.Create;

public record CreateCourtResult(bool IsSuccess);

public record CreateCourtCommand(string Description) : ICommand<CreateCourtResult>;

public class CreateCourtHandler(IApplicationDbContext context) : ICommandHandler<CreateCourtCommand, CreateCourtResult>
{
    private readonly IApplicationDbContext _context = context;
    public async Task<CreateCourtResult> Handle(CreateCourtCommand command, CancellationToken cancellationToken)
    {
        var newCourt = Court.Create(command.Description);

        _context.Courts.Add(newCourt);
        var result = await _context.SaveChangesAsync(cancellationToken) > 0;

        return new CreateCourtResult(result);
    }
}
