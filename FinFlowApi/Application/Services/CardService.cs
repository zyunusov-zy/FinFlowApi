using System.Security.Cryptography;
using System.Text;

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
        if (code == -2) throw new Exception("OTP Expiried");
        var refNum = GenerateRefNum();
        await _repo.SaveRefNumAsync(dto ,refNum);
        return refNum;
    }


    private static string GenerateRefNum(int length = 32)
    {
        const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var result = new StringBuilder(length);
        var buffer = new byte[sizeof(uint)];

        for (int i = 0; i < length; i++)
        {
            RandomNumberGenerator.Fill(buffer);
            uint num = BitConverter.ToUInt32(buffer, 0);
            result.Append(AllowedChars[(int)(num % (uint)AllowedChars.Length)]);
        }

        return result.ToString();
    }
}