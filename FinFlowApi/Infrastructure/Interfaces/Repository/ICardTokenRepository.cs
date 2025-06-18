using FinFlowApi.DTOs;

namespace FinFlowApi.Repositories;

public interface ICardTokenRepository
{
    Task<int> RemoveCardTokenAsync(CardTokenRemoveAndUnblockDto dto);

    Task<int> BlockCardTokenAsync(BlockCardTokenDto dto);
    Task<int> UnblockCardTokenAsync(CardTokenRemoveAndUnblockDto dto);
}