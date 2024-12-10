namespace ProvisionPadel.Api.Dtos;

public record InstanceDto(Guid Id, string Name, string Integration, string Token, string ConnectionStatus,
    string Number, bool Qrcode, DateTime CreatedAt, DateTime UpdatedAt);