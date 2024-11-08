namespace ProvisionPadel.Api.Features.Courts.GetAll;

public record GetAllCourtsResult(IEnumerable<Court> Courts);

public record GetAllCourtsQuery : IQuery<GetAllCourtsResult>;
public class GetAllCourtsHandler(IApplicationDbContext context) : IQueryHandler<GetAllCourtsQuery, GetAllCourtsResult>
{
    private readonly IApplicationDbContext _context = context;
    public async Task<GetAllCourtsResult> Handle(GetAllCourtsQuery request, CancellationToken cancellationToken)
    {
       var courts = await _context.Courts.AsNoTracking().ToListAsync(cancellationToken);

        return new GetAllCourtsResult(courts);
    }
}
