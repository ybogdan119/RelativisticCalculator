namespace RelativisticCalculator.API.Models.Dto;

public class InsertConflictDto : ErrorResponseDto
{
    public IEnumerable<string>? Conflicts { get; set; }
}