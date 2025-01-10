namespace ProvisionPadel.Api.Services.Requests;

public record UpdateCameraRequest(Guid Id, int Channel, Guid CourtId);
