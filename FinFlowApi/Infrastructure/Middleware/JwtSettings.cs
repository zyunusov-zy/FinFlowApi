namespace FinFlowApi.Middleware;

public class JwtSettings
{
    public string Secret { get; set; } = string.Empty;
    public int ExpiryMinutes { get; set; } = 60;
    public string Issuer { get; set; } = "FinFlowApi";
    public string Audience { get; set; } = "FinFlowApiUsers";

    
}
