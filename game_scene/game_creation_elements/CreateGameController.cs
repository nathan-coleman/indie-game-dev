using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using IndieGameDev.Models;
using IndieGameDev.Utils;
using Newtonsoft.Json;

namespace IndieGameDev.Game.UI;

public partial class CreateGameController : Node
{
    private const string DEFAULT_ICON_PATH = "res://resources/assets/game_logo/icon.png";

    [Export] PackedScene? _largeGridItemPrefab;

    private TabContainer? gameOptionSelectionTabContainer;

    public override void _Ready()
    {
        gameOptionSelectionTabContainer = GetNode<TabContainer>("%GameOptionSelectionTabContainer");

        ConnectButtons();
        LoadData();
    }

    private void ConnectButtons()
    {
        GetNode<Button>("%OpenScopePaneButton").Pressed += () => SetTab(0);
        GetNode<Button>("%OpenGenrePaneButton").Pressed += () => SetTab(1);
        GetNode<Button>("%OpenTopicPaneButton").Pressed += () => SetTab(2);
        GetNode<Button>("%OpenPlatformPaneButton").Pressed += () => SetTab(3);
        GetNode<Button>("%OpenGraphicsPaneButton").Pressed += () => SetTab(4);
        GetNode<Button>("%OpenAudiencePaneButton").Pressed += () => SetTab(5);
    }

    private void SetTab(int tabIndex)
    {
        ArgumentNullException.ThrowIfNull(gameOptionSelectionTabContainer);
        gameOptionSelectionTabContainer.CurrentTab = tabIndex;
    }

    private void LoadData()
    {
        LoadScopeOptions();
        LoadGenreOptions();
        LoadTopicOptions();
        LoadPlatformOptions();
        LoadAudienceOptions();
    }

    private void LoadScopeOptions()
    {
        var gameScopes = LoadJsonAsObject<List<GameScope>>("res://resources/data/GameScopes.json");
        var gameScopeSelectionContainer = gameOptionSelectionTabContainer?.GetNode<GridContainer>("ScopeSelectionBox/GridContainer");
        InstantiateListItems(gameScopes.Cast<IListItem>(), gameScopeSelectionContainer, ScopeSelected);
    }

    private void ScopeSelected(IListItem selected)
    {
        GetNode<Button>("%OpenScopePaneButton").Text = $"Scope: {selected.Name}";
        GetNode<Button>("%OpenScopePaneButton").TooltipText = $"{selected.Name}: {selected.Description}";
    }

    private void LoadGenreOptions()
    {
        var gameGenres = LoadJsonAsObject<List<GameGenre>>("res://resources/data/GameGenres.json");
        var gameGenreSelectionContainer = gameOptionSelectionTabContainer?.GetNode<GridContainer>("GenreSelectionBox/GridContainer");
        InstantiateListItems(gameGenres.Cast<IListItem>(), gameGenreSelectionContainer, GenreSelected);
    }

    private void GenreSelected(IListItem selected)
    {
        GetNode<Button>("%OpenGenrePaneButton").Text = $"Genre: {selected.Name}";
        GetNode<Button>("%OpenGenrePaneButton").TooltipText = $"{selected.Name}: {selected.Description}";
    }

    private void LoadTopicOptions()
    {
        var gameTopics = LoadJsonAsObject<List<GameTopic>>("res://resources/data/GameTopics.json");
        var gameTopicSelectionContainer = gameOptionSelectionTabContainer?.GetNode<GridContainer>("TopicSelectionBox/GridContainer");
        InstantiateListItems(gameTopics.Cast<IListItem>(), gameTopicSelectionContainer, TopicSelected);
    }

    private void TopicSelected(IListItem selected)
    {
        GetNode<Button>("%OpenTopicPaneButton").Text = $"Topic: {selected.Name}";
        GetNode<Button>("%OpenTopicPaneButton").TooltipText = $"{selected.Name}: {selected.Description}";
    }

    private void LoadPlatformOptions()
    {
        var gamePlatforms = LoadJsonAsObject<List<GamePlatform>>("res://resources/data/GamePlatforms.json");
        var gamePlatformSelectionContainer = gameOptionSelectionTabContainer?.GetNode<GridContainer>("PlatformSelectionBox/GridContainer");
        InstantiateListItems(gamePlatforms.Cast<IListItem>(), gamePlatformSelectionContainer, PlatformSelected);
    }

    private void PlatformSelected(IListItem selected)
    {
        GetNode<Button>("%OpenPlatformPaneButton").Text = $"Platform: {selected.Name}";
        GetNode<Button>("%OpenPlatformPaneButton").TooltipText = $"{selected.Name}: {selected.Description}";
    }

    private void LoadAudienceOptions()
    {
        var gameAudiences = LoadJsonAsObject<List<GameAudience>>("res://resources/data/GameAudiences.json");
        var gameAudienceSelectionContainer = gameOptionSelectionTabContainer?.GetNode<GridContainer>("AudienceSelectionBox/GridContainer");
        InstantiateListItems(gameAudiences.Cast<IListItem>(), gameAudienceSelectionContainer, AudienceSelected);
    }

    private void AudienceSelected(IListItem selected)
    {
        GetNode<Button>("%OpenAudiencePaneButton").Text = $"Game Audience: {selected.Name}";
        GetNode<Button>("%OpenAudiencePaneButton").TooltipText = $"{selected.Name}: {selected.Description}";
    }

    private void InstantiateListItems(IEnumerable<IListItem> listData, Control? instantiationParent, Action<IListItem>? buttonPressedCallback)
    {
        ArgumentNullException.ThrowIfNull(instantiationParent);
        ArgumentNullException.ThrowIfNull(_largeGridItemPrefab);

        instantiationParent.QueueFreeChildren();

        foreach (var listItem in listData)
        {
            var gridItem = _largeGridItemPrefab.Instantiate<LargeGridItem>();

            gridItem.ItemName = listItem.Name;
            gridItem.ItemDescription = listItem.Description;
            gridItem.ItemIcon = ResourceLoader.Load<Texture2D>(listItem.IconPath ?? DEFAULT_ICON_PATH);
            if (buttonPressedCallback != null)
            {
                gridItem.Pressed += () => buttonPressedCallback(listItem);
            }

            instantiationParent.AddChild(gridItem);
        }
    }

    private static T LoadJsonAsObject<T>(string filePath)
    {
        using var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
        if (file == null) throw new JsonException($"Error loading '{filePath}': {FileAccess.GetOpenError()}");

        var rawJson = file.GetAsText();
        var deserializedObject = JsonConvert.DeserializeObject<T>(rawJson);
        if (deserializedObject == null) throw new JsonException($"Error deserializing json in '{filePath}' to {typeof(T)}");

        return deserializedObject;
    }
}
