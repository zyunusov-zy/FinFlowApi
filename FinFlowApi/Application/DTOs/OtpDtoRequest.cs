namespace FinFlowApi.DTOs;

public class OtpDtoRequest
{
    public string Pan { get; set; } = string.Empty;
    public string Expiry { get; set; } = string.Empty;
}