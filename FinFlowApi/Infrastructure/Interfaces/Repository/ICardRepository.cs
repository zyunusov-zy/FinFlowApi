using FinFlowApi.DTOs;

namespace FinFlowApi.Repositories;

public interface ICardRepository
{
    Task<(string?, string?)> VerifyCardAsync(OtpDtoRequest dto);
    Task<int> VerifyOtpAsync(VerifyDtoRequest dto);

    Task SaveRefNumAsync(VerifyDtoRequest dto, string refNum);
}