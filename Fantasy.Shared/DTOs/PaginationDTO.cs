namespace Fantasy.Shared.DTOs;

public class PaginationDTO
{
    public int Id { get; set; }

    public int Page { get; set; }

    public int RecordsNumber { get; set; } = 25;

    public string? Filter { get; set; }
}