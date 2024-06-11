namespace Portfolio_Box.Models.Files
{
    public record class UpdatedRemoteFileDto : RemoteFileDto
    {
        public string? PreviousPath { get; init; }
    }
}
