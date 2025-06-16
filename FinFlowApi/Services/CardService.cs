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

    public async Task<string?> VerifyOtp(VerifyDtoRequest dto)
    {
        var code = await _repo.VerifyOtpAsync(dto);
        if (code == -1) throw new Exception("Invalid Credentials");
        // mb needed otp card and expiry number idk
        var refNum = "string"; // create function to generate refNum;
        return refNum;
    }
}