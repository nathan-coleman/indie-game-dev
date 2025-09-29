using System;
using System.Collections.Generic;

namespace NathanColeman.IndieGameDev.Models
{
    public record GameCompletionPayload
    {
        public GameCompletionPayload(GameCreationPayload gameCreationPayload)
        {
            if (!gameCreationPayload.IsValid) throw new ArgumentException("GameCreationPayload must be valid to create GameCompletionPayload", nameof(gameCreationPayload));

            Name = gameCreationPayload.Name!;
            Scope = gameCreationPayload.Scope!;
            Genre = gameCreationPayload.Genre!;
            Topic = gameCreationPayload.Topic!;
            Platform = gameCreationPayload.Platform!;
            Audience = gameCreationPayload.Audience!;
            BubbleValues = new Dictionary<string, int>();
        }

        public string Name { get; set; }
        public GameScope Scope { get; set; }
        public GameGenre Genre { get; set; }
        public GameTopic Topic { get; set; }
        public GamePlatform Platform { get; set; }
        public GameAudience Audience { get; set; }
        public Dictionary<string, int> BubbleValues { get; set; }
    }
}
