namespace Models;


public class Card
{
    public string Pan { get; set; } = string.Empty;
    public string ExpiryData { get; set; } = string.Empty;
    public string PhoneNum { get; set; } = string.Empty;
    public int Otp { get; set; } = 0;
}