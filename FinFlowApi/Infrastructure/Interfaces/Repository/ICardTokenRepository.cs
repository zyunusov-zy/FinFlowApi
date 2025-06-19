using FinFlowApi.DTOs;

namespace FinFlowApi.Repositories;

public interface ICardTokenRepository
{
    Task<int> RemoveCardTokenAsync(CardTokenRemoveAndUnblockDto dto);

    Task<int> BlockCardTokenAsync(BlockCardTokenDto dto);
    Task<int> UnblockCardTokenAsync(CardTokenRemoveAndUnblockDto dto);

    Task<(string?, decimal)> CardBalanceCheckAsync(CardTokenRemoveAndUnblockDto dto);

    Task<Dictionary<string, string>> GetCardInfoAsync(CardTokenRemoveAndUnblockDto dto, string username);
}