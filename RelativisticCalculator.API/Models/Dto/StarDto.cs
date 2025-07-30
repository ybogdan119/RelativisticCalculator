namespace RelativisticCalculator.API.Models.Dto;

public class StarDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public double DistanceLy { get; set; }
}