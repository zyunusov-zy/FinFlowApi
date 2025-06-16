namespace FinFlowApi.DTOs;

public class VerifyDtoRequest
{
    public int Id { get; set; } = -1;
    public string Token { get; set; } = string.Empty;
}