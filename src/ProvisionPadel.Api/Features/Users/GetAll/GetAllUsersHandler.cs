namespace ProvisionPadel.Api.Features.Users.GetAll;

public record GetAllUsersResult(IEnumerable<User> users);

public record GetAllUsersQuery() : ICommand<GetAllUsersResult>;

public class GetAllUsersHandler(IApplicationDbContext context) : ICommandHandler<GetAllUsersQuery, GetAllUsersResult>
{
    private readonly IApplicationDbContext _context = context;
    public async Task<GetAllUsersResult> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _context.Users.AsNoTracking().ToListAsync(cancellationToken);

        return new GetAllUsersResult(users);
    }
}