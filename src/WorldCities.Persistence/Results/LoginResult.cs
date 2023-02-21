namespace WorldCities.Persistence.Results;

public class LoginResult
{
    public bool Succes { get; set; }
    public string Message { get; set; } = null!;
    public string? Token { get; set; }
}
