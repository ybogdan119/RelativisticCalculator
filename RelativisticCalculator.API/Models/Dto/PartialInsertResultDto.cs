namespace RelativisticCalculator.API.Models.Dto;

public class PartialInsertResultDto
{
    public string Message { get; set; } = null!;
    public int StatusCode { get; set; }

    public List<StarDto> Inserted { get; set; } = new();
    public List<string> Conflicts { get; set; } = new();
}