using FinFlowApi.DTOs;

namespace FinFlowApi.Services;

public interface ICardTokenService
{
    Task<int> RemoveCardToken(CardTokenRemoveAndUnblockDto dto);
    Task<int> BlockCardTokenS(BlockCardTokenDto dto);
    Task<int> UnblockCardTokenS(CardTokenRemoveAndUnblockDto dto);
}