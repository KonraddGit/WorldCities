using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorldCities.Domain.Entities;

public class City
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    [Column(TypeName = "decimal(7,4)")]
    public decimal Lat { get; set; }

    [Required]
    [Column(TypeName = "decimal(7,4)")]
    public decimal Lon { get; set; }

    [ForeignKey(nameof(Country))]
    public int CountryId { get; set; }

    public Country? Country { get; set; } = null!;
}