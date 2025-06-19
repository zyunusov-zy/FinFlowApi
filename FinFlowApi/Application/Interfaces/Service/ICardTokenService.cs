using FinFlowApi.DTOs;

namespace FinFlowApi.Services;

public interface ICardTokenService
{
    Task<int> RemoveCardToken(CardTokenRemoveAndUnblockDto dto);
    Task<int> BlockCardTokenS(BlockCardTokenDto dto);
    Task<int> UnblockCardTokenS(CardTokenRemoveAndUnblockDto dto);
    Task<(string?, decimal)> CardBalanceCheck(CardTokenRemoveAndUnblockDto dto);

    Task<Dictionary<string, string>> GetCardInfo(CardTokenRemoveAndUnblockDto dto, string username);
}