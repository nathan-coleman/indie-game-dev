using System;
using System.Collections.Generic;
using System.Linq;
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
            GameController.Instance.Logger.Debug($"TopicToAudience: adding {topicAudienceAffinity * TopicToAudienceAffinityWeight} to {totalScore}, adding {TopicToAudienceAffinityWeight} to {maximumScore}.");
            totalScore += topicAudienceAffinity * TopicToAudienceAffinityWeight;
            maximumScore += TopicToAudienceAffinityWeight;
        }
        else
        {
            GameController.Instance.Logger.Error($"Audience '{audience}' is not in topic's audience affinity list."
                + " It will not be used in the score calculation.");
        }

        // TopicToGenre
        var topicGenreAffinities = gameCompletionPayload.Topic.GenreAffinities;
        var genre = gameCompletionPayload.Genre.Name;
        if (topicGenreAffinities.TryGetValue(genre, out float topicGenreAffinity))
        {
            GameController.Instance.Logger.Debug($"TopicToGenre: adding {topicGenreAffinity * TopicToGenreAffinityWeight} to {totalScore}, adding {TopicToGenreAffinityWeight} to {maximumScore}.");
            totalScore += topicGenreAffinity * TopicToGenreAffinityWeight;
            maximumScore += TopicToGenreAffinityWeight;
        }
        else
        {
            GameController.Instance.Logger.Error($"Genre '{genre}' is not in topic's genre affinity list."
                + " It will not be used in the score calculation.");
        }

        // PlatformToGenre
        // TODO

        // GenreToBubbleValues
        var genreExpectedBubbles = gameCompletionPayload.Genre.ExpectedBubbles;
        var gameBubbleValues = gameCompletionPayload.BubbleValues;

        var genreBubblesSuccess = new Dictionary<string, double>();

        foreach (var genreExpectedBubble in genreExpectedBubbles)
        {
            if (gameBubbleValues.TryGetValue(genreExpectedBubble.Key, out int gameBubbleValue))
            {
                GameController.Instance.Logger.Debug($"GenreToBubbleValues: adding {BubbleClosenessMultiplier(gameBubbleValue, genreExpectedBubble.Value)}, for {genreExpectedBubble.Key}.");
                genreBubblesSuccess[genreExpectedBubble.Key] = BubbleClosenessMultiplier(gameBubbleValue, genreExpectedBubble.Value);
            }
            else
            {
                GameController.Instance.Logger.Error($"Bubble type '{genreExpectedBubble.Key}' is not in games's bubble value list."
                    + " It will not be used in the score calculation.");
            }
        }

        GameController.Instance.Logger.Debug($"GenreToBubbleValues: adding {genreBubblesSuccess.Values.Sum() / genreBubblesSuccess.Count * GenreToBubbleAffinityWeight} to {totalScore}, adding {GenreToBubbleAffinityWeight} to {maximumScore}.");

        totalScore += (float)genreBubblesSuccess.Values.Sum() / genreBubblesSuccess.Count * GenreToBubbleAffinityWeight;
        maximumScore += GenreToBubbleAffinityWeight;

        GameController.Instance.Logger.Information($"Total game score: {totalScore}/{maximumScore}.");

        return maximumScore <= 0 ? 0 : totalScore / maximumScore;
    }

    /// <summary>
    /// Given BubbleValue/BubbleExpectedValue,
    /// returns the score out of 1 for that bubble.
    /// Exceptional bubble values can return up to 1.1.
    /// </summary>
    /// <returns>A value between 0 and 1.1.</returns>
    private static double BubbleClosenessMultiplier(double bubbleValue, double bubbleExpectedValue)
    {
        // Combines two sigmoid curves to score closeness of bubble value to expected value.
        // Exceptional bubble values can score slightly above 1.0 (up to 1.1).
        if (bubbleExpectedValue == 0) return 1.1;
        double longSigmoid = 1.0 / (1.0 + Math.Exp(-4.0 * ((bubbleValue / bubbleExpectedValue) - 0.2)));
        double shortSigmoid = 1.0 / (1.0 + Math.Exp(-(6.9 * (bubbleValue / bubbleExpectedValue) - 4.0)));
        return longSigmoid * shortSigmoid * 1.1;
    }
}
