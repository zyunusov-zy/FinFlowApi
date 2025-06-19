using FinFlowApi.DTOs;
using FinFlowApi.Repositories;

namespace FinFlowApi.Services;


public class CardTokenService : ICardTokenService
{
    private readonly ICardTokenRepository _repo;

    public CardTokenService(ICardTokenRepository repo)
    {
        _repo = repo;
    }
    public async Task<int> RemoveCardToken(CardTokenRemoveAndUnblockDto dto)
    {
        var removed = await _repo.RemoveCardTokenAsync(dto);

        if (removed == -1)
            throw new KeyNotFoundException("Card token not found.");
        return 0;
    }

    public async Task<int> BlockCardTokenS(BlockCardTokenDto dto)
    {
        var blocked = await _repo.BlockCardTokenAsync(dto);
        if (blocked == -1)
            throw new KeyNotFoundException("Card token not found.");
        return 0;
    }
    public async Task<int> UnblockCardTokenS(CardTokenRemoveAndUnblockDto dto)
    {
        var unblocked = await _repo.UnblockCardTokenAsync(dto);
        if (unblocked == -1)
            throw new KeyNotFoundException("Card token not found.");
        return 0;
    }

    public async Task<(string?, decimal)> CardBalanceCheck(CardTokenRemoveAndUnblockDto dto)
    {
        var (pan, balance) = await _repo.CardBalanceCheckAsync(dto);
        if (pan == null && balance == -1)
            throw new KeyNotFoundException("Card token not found.");
        return (pan, balance);
    }
}