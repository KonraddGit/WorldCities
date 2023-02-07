using System.ComponentModel.DataAnnotations;

namespace WorldCities.Domain.Entities;

public class Country
{
    [Key]
    [Required]
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string ISO2 { get; set; } = null!;

    public string ISO3 { get; set; } = null!;

    public ICollection<City> Cities { get; set; } = null!;
}