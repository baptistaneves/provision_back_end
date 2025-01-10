namespace ProvisionPadel.Api.Features.Courts.Remove;

public record RemoveCourtCommand(Guid Id) : ICommand<Result<bool>>;

public class RemoveCourtCommandValidation : AbstractValidator<RemoveCourtCommand>
{
    public RemoveCourtCommandValidation()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("O Id do campo deve ser informado")
            .NotEqual(Guid.Empty).WithMessage("O Id do campo deve ser informado");
    }
}

public class RemoveCourteHandler
    (IApplicationDbContext context,
    IValidator<RemoveCourtCommand> validator) 
    : ICommandHandler<RemoveCourtCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IValidator<RemoveCourtCommand> _validator = validator;

    public async Task<Result<bool>> Handle(RemoveCourtCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await CommandValidator.ValidateAsync(command, _validator);

        if (validationResult.Errors != null && validationResult.Errors.Any())
            return Result<bool>.Failure(validationResult.Errors);

        var court = await _context.Courts.SingleOrDefaultAsync(x => x.Id == command.Id);

        if (court is null)
            return Result<bool>.Failure(new Error("O campo solicitado não foi encontrado!"));

        _context.Courts.Remove(court);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}