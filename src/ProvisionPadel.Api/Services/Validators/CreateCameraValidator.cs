namespace ProvisionPadel.Api.Services.Validators;

public class CreateCameraValidator : AbstractValidator<CreateCameraRequest>
{
    public CreateCameraValidator()
    {
        RuleFor(x => x.Channel)
            .NotNull().WithMessage("O número do canal deve informado.")
            .GreaterThan(0).WithMessage("O número do canal deve maior que zero.");

        RuleFor(x => x.CourtId)
            .NotEqual(Guid.Empty).WithMessage("Informe o campo onde a camara está instalada.");
    }
}