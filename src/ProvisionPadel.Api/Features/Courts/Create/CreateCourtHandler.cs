namespace ProvisionPadel.Api.Features.Courts.Create;

public record CreateCourtCommand(string Description) : ICommand<Result<bool>>;

public class CreateCourtCommandValidation : AbstractValidator<CreateCourtCommand>
{
    public CreateCourtCommandValidation()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("O nome do campo deve ser informado")
            .MinimumLength(3).WithMessage("O nome deve ter no minímo 3 caracteres");
    }
}

public class CreateCourtHandler
    (IApplicationDbContext context,
    IValidator<CreateCourtCommand> validator) : ICommandHandler<CreateCourtCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IValidator<CreateCourtCommand> _validator = validator;

    public async Task<Result<bool>> Handle(CreateCourtCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await CommandValidator.ValidateAsync(command, _validator);

        if (validationResult.Errors != null && validationResult.Errors.Any())
            return Result<bool>.Failure(validationResult.Errors);

        if (await _context.Courts.AnyAsync(x => x.Description == command.Description, cancellationToken))
            return Result<bool>.Failure(new Error("Já existe um campo cadastrado com este nome"));

        var newCourt = Court.Create(command.Description);

        _context.Courts.Add(newCourt);
        var result = await _context.SaveChangesAsync(cancellationToken) > 0;

        return Result<bool>.Success(true);
    }
}