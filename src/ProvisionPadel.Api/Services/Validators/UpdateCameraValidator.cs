namespace ProvisionPadel.Api.Services.Validators;

public class UpdateCameraValidator : AbstractValidator<UpdateCameraRequest>
{
    public UpdateCameraValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty).WithMessage("Id da camara informada não é válido.");

        RuleFor(x => x.Channel)
            .NotNull().WithMessage("O número do canal deve informado.")
            .GreaterThan(0).WithMessage("O número do canal deve maior que zero.");

        RuleFor(x => x.CourtId)
            .NotEqual(Guid.Empty).WithMessage("Informe o campo onde a camara está instalada.");
    }
}
