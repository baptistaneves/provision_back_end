namespace ProvisionPadel.Api.Dtos;

public record CameraDto(Guid Id, Guid CourtId, string CourtDescription, int Channel, bool IsRecording);
