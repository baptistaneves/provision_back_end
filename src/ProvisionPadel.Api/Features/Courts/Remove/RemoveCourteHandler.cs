
using ProvisionPadel.Api.Features.Courts.Update;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ProvisionPadel.Api.Features.Courts.Remove;

public record RemoveCourtResult(bool IsSuccess);

public record RemoveCourtCommand(Guid Id) : ICommand<RemoveCourtResult>;

public class RemoveCourteHandler
    (IApplicationDbContext context,
    INotifier notifier) 
    : ICommandHandler<RemoveCourtCommand, RemoveCourtResult>
{
    private readonly IApplicationDbContext _context = context;
    private readonly INotifier _notifier = notifier;
    public async Task<RemoveCourtResult> Handle(RemoveCourtCommand command, CancellationToken cancellationToken)
    {
        var court = await _context.Courts.SingleOrDefaultAsync(x => x.Id == command.Id);

        if (court is null)
        {
            _notifier.Add("O campo solicitado não foi encontrado!");
            return new RemoveCourtResult(false);
        }

        _context.Courts.Remove(court);
        await _context.SaveChangesAsync(cancellationToken);

        return new RemoveCourtResult(true);
    }
}
