namespace ProvisionPadel.Api.Features.Courts.Update;

public record UpdateCourtResult(bool isSucces);

public record UpdateCourtCommand(Guid Id, string Description) : ICommand<UpdateCourtResult>;
public class UpdateCourtHandler
    (IApplicationDbContext context,
    INotifier notifier)
    : ICommandHandler<UpdateCourtCommand, UpdateCourtResult>
{
    private readonly IApplicationDbContext _context = context;
    private readonly INotifier _notifier = notifier;
    public async Task<UpdateCourtResult> Handle(UpdateCourtCommand command, CancellationToken cancellationToken)
    {
        var court = await _context.Courts.SingleOrDefaultAsync(x => x.Id == command.Id);

        if(court is null)
        {
            _notifier.Add("O campo solicitado não foi encontrado!");
            return new UpdateCourtResult(false);
        }

        court.Update(command.Description);
        _context.Courts.Update(court);

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateCourtResult(true);
    }
}
