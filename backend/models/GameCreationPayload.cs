namespace NathanColeman.IndieGameDev.Models
{
    public record GameCreationPayload
    {
        public string? Name { get; set; }
        public GameScope? Scope { get; set; }
        public GameGenre? Genre { get; set; }
        public GameTopic? Topic { get; set; }
        public GamePlatform? Platform { get; set; }
        public GameAudience? Audience { get; set; }

        public bool IsValid
        {
            get => !string.IsNullOrWhiteSpace(Name)
                && Scope != null
                && Genre != null
                && Topic != null
                && Platform != null
                && Audience != null;
        }
    }
}
