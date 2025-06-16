using FinFlowApi.DTOs;

namespace FinFlowApi.Repositories;

public interface ICardRepository
{
    Task<(string?, string?)> VerifyCardAsync(OtpDtoRequest dto);
}