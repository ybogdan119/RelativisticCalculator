namespace RelativisticCalculator.API.Models.Dto;

public class ErrorResponseDto
{
    public string Message { get; set; } = null!;
    public string? Details { get; set; }
    public int StatusCode { get; set; }
}