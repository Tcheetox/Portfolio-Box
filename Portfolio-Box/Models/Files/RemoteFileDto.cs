namespace Portfolio_Box.Models.Files;

public record class RemoteFileDto
{
	public required string Path { get; init; }
	public long Length { get; init; }
}