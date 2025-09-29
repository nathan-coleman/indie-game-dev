using System;
using Godot;
using NathanColeman.IndieGameDev.Models;

namespace NathanColeman.IndieGameDev.Backend;

public static class ScoreCalculator
{
    private const float TopicToAudienceAffinityWeight = 1;
    private const float TopicToGenreAffinityWeight = 1;
    private const float GenreToBubbleAffinityWeight = 3;

    /// <summary>
    /// Estimates a likely score for the game.
    /// </summary>
    /// <param name="gameCompletionPayload">The game to be scored.</param>
    /// <returns>A score between 0 and 1.</returns>
    public static float EstimateScore(GameCompletionPayload gameCompletionPayload)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Calculates the score for the game.
    /// </summary>
    /// <param name="gameCompletionPayload">The game to be scored.</param>
    /// <returns>A score between 0 and 1.</returns>
    public static float CalculateScore(GameCompletionPayload gameCompletionPayload)
    {
        if (gameCompletionPayload.BubbleValues is null || gameCompletionPayload.BubbleValues.Count == 0) return 0;

        float totalScore = 0f;
        float maximumScore = 0f;

        // TopicToAudience
        var topicAudienceAffinities = gameCompletionPayload.Topic.AudienceAffinities;
        var audience = gameCompletionPayload.Audience.Name;
        if (topicAudienceAffinities.TryGetValue(audience, out float topicAudienceAffinity))
        {
            totalScore += topicAudienceAffinity * TopicToAudienceAffinityWeight;
            maximumScore += TopicToAudienceAffinityWeight;
        }
        else
        {
            GD.PrintErr($"Audience '{audience}' is not in topic's audience affinity list."
                + " It will not be used in the score calculation.");
        }

        // TopicToGenre
        var topicGenreAffinities = gameCompletionPayload.Topic.GenreAffinities;
        var genre = gameCompletionPayload.Genre.Name;
        if (topicGenreAffinities.TryGetValue(genre, out float topicGenreAffinity))
        {
            totalScore += topicGenreAffinity * TopicToGenreAffinityWeight;
            maximumScore += TopicToGenreAffinityWeight;
        }
        else
        {
            GD.PrintErr($"Genre '{genre}' is not in topic's genre affinity list."
                + " It will not be used in the score calculation.");
        }

        // PlatformToGenre
        // TODO

        // GenreToBubbleValues
        var genreBubbleAffinities = gameCompletionPayload.Genre.BubbleAffinities;
        var gameBubbleValues = gameCompletionPayload.BubbleValues;

        var genreToBubbleValuesTotalScore = 0f;

        foreach (var genreBubbleAffinity in genreBubbleAffinities)
        {
            if (gameBubbleValues.TryGetValue(genreBubbleAffinity.Key, out int gameBubbleValue))
            {
                genreToBubbleValuesTotalScore += gameBubbleValue * genreBubbleAffinity.Value;
            }
            else
            {
                GD.PrintErr($"Bubble type '{genreBubbleAffinity.Key}' is not in games's bubble value list."
                    + " It will not be used in the score calculation.");
            }
        }

        totalScore += genreToBubbleValuesTotalScore * GenreToBubbleAffinityWeight;
        maximumScore += GenreToBubbleAffinityWeight;

        return maximumScore <= 0 ? 0 : totalScore / maximumScore;
    }
}
