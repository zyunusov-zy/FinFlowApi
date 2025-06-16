using FinFlowApi.DTOs;
using FinFlowApi.Repositories;

namespace FinFlowApi.Services;

public class CardService : ICardService
{
    private readonly ICardRepository _repo;

    public CardService(ICardRepository repo)
    {
        _repo = repo;
    }
    public async Task<(string?, string?)> CardVerification(OtpDtoRequest dto)
    {
        var (otp, otpId) = await _repo.VerifyCardAsync(dto);

        if (otp == null) throw new Exception("Card not found");

        return (otp, otpId);
    }
}