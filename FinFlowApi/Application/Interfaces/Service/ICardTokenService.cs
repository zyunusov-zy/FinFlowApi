using FinFlowApi.DTOs;

namespace FinFlowApi.Services;

public interface ICardTokenService
{
    Task<int> RemoveCardToken(CardTokenRemoveDto dto);
}