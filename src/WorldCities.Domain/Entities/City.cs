namespace WorldCities.Domain.Entities;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Lat { get; set; }
    public decimal Lon { get; set; }
    public int CountryId { get; set; }
    public Country? Country { get; set; } = null!;
}