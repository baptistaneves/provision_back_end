namespace ProvisionPadel.Api.Features.Courts.Update;

public record UpdateCourtCommand(Guid Id, string Description) : ICommand<Result<bool>>;

public class UpdateCourtCommandValidation : AbstractValidator<UpdateCourtCommand>
{
    public UpdateCourtCommandValidation()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("O Id do campo deve ser informado")
            .NotEqual(Guid.Empty).WithMessage("O Id do campo deve ser informado");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("O nome do campo deve ser informado")
            .MinimumLength(3).WithMessage("O nome deve ter no minímo 3 caracteres");
    }
}

public class UpdateCourtHandler
    (IApplicationDbContext context,
     IValidator<UpdateCourtCommand> validator)
    : ICommandHandler<UpdateCourtCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IValidator<UpdateCourtCommand> _validator = validator;

    public async Task<Result<bool>> Handle(UpdateCourtCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await CommandValidator.ValidateAsync(command, _validator);

        if (validationResult.Errors != null && validationResult.Errors.Any())
            return Result<bool>.Failure(validationResult.Errors);

        var court = await _context.Courts.SingleOrDefaultAsync(x => x.Id == command.Id);

        if(court is null)
            return Result<bool>.Failure(new Error("O campo solicitado não foi encontrado!"));

        court.Update(command.Description);
        _context.Courts.Update(court);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}