using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace RelativisticCalculator.API.Models.Entities;

public class Star
{
    [Key]
    [Required]
    public int Id { get; set; }

    [MaxLength(100)]
    public required string Name { get; set; }
    
    [Required]
    public double DistanceLy { get; set; }
}