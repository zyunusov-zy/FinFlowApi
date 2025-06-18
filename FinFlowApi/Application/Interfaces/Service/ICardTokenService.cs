using FinFlowApi.DTOs;

namespace FinFlowApi.Services;

public interface ICardTokenService
{
    Task<int> RemoveCardToken(CardTokenRemoveDto dto);
    Task<int> BlockCardTokenS(BlockCardTokenDto dto);
}