using FinFlowApi.DTOs;

namespace FinFlowApi.Services;

public interface ICardService
{
    Task<(string?, string?)> CardVerification(OtpDtoRequest dto);
    Task<string?> VerifyOtp(VerifyDtoRequest dto);
}