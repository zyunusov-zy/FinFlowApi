using FinFlowApi.DTOs;

namespace FinFlowApi.Repositories;

public interface ICardTokenRepository
{
    Task<int> RemoveCardTokenAsync(CardTokenRemoveDto dto);

    Task<int> BlockCardTokenAsync(BlockCardTokenDto dto);
}