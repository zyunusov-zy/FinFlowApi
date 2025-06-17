using System.Text;
using System.Security.Cryptography;


namespace FinFlowApi.Services;




public class OtpService
{
    public static string GenerateOtp(int length = 6)
    {
        var random = new Random();
        var otp = string.Concat(Enumerable.Range(1, length).Select(_ => random.Next(0, 10)));
        return otp;
    }


}