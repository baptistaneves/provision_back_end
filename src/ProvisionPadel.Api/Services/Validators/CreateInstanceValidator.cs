namespace ProvisionPadel.Api.Services.Validators;

public class CreateInstanceValidator : AbstractValidator<string>
{
    public CreateInstanceValidator()
    {
        RuleFor(x => x)
            .NotEmpty().WithMessage("O nome não pode ser vazio.")
            .Length(3, 50).WithMessage("O nome deve ter entre 3 e 50 caracteres.");
    }
}