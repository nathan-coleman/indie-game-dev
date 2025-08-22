using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using IndieGameDev.Models;
using IndieGameDev.Utils;
using IndieGameDev.Backend;

namespace IndieGameDev.Game.UI;

public partial class CreateGameController : Node
{

    [Export] private PackedScene? _largeGridItemPrefab;

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
        var gameScopes = GameController.Instance?.DataLoader.LoadListItems<List<GameScope>>("GameScopes");
        var gameScopeSelectionContainer = gameOptionSelectionTabContainer?.GetNode<GridContainer>("ScopeSelectionBox/GridContainer");
        InstantiateListItems(gameScopes, gameScopeSelectionContainer, ScopeSelected);
    }

    private void ScopeSelected(IListItem selected)
    {
        GetNode<Button>("%OpenScopePaneButton").Text = $"Scope: {selected.Name}";
        GetNode<Button>("%OpenScopePaneButton").TooltipText = $"{selected.Name}: {selected.Description}";
    }

    private void LoadGenreOptions()
    {
        var gameGenres = GameController.Instance?.DataLoader.LoadListItems<List<GameGenre>>("GameGenres");
        var gameGenreSelectionContainer = gameOptionSelectionTabContainer?.GetNode<GridContainer>("GenreSelectionBox/GridContainer");
        InstantiateListItems(gameGenres, gameGenreSelectionContainer, GenreSelected);
    }

    private void GenreSelected(IListItem selected)
    {
        GetNode<Button>("%OpenGenrePaneButton").Text = $"Genre: {selected.Name}";
        GetNode<Button>("%OpenGenrePaneButton").TooltipText = $"{selected.Name}: {selected.Description}";
    }

    private void LoadTopicOptions()
    {
        var gameTopics = GameController.Instance?.DataLoader.LoadListItems<List<GameTopic>>("GameTopics");
        var gameTopicSelectionContainer = gameOptionSelectionTabContainer?.GetNode<GridContainer>("TopicSelectionBox/GridContainer");
        InstantiateListItems(gameTopics, gameTopicSelectionContainer, TopicSelected);
    }

    private void TopicSelected(IListItem selected)
    {
        GetNode<Button>("%OpenTopicPaneButton").Text = $"Topic: {selected.Name}";
        GetNode<Button>("%OpenTopicPaneButton").TooltipText = $"{selected.Name}: {selected.Description}";
    }

    private void LoadPlatformOptions()
    {
        var gamePlatforms = GameController.Instance?.DataLoader.LoadListItems<List<GamePlatform>>("GamePlatforms");
        var gamePlatformSelectionContainer = gameOptionSelectionTabContainer?.GetNode<GridContainer>("PlatformSelectionBox/GridContainer");
        InstantiateListItems(gamePlatforms, gamePlatformSelectionContainer, PlatformSelected);
    }

    private void PlatformSelected(IListItem selected)
    {
        GetNode<Button>("%OpenPlatformPaneButton").Text = $"Platform: {selected.Name}";
        GetNode<Button>("%OpenPlatformPaneButton").TooltipText = $"{selected.Name}: {selected.Description}";
    }

    private void LoadAudienceOptions()
    {
        var gameAudiences = GameController.Instance?.DataLoader.LoadListItems<List<GameAudience>>("GameAudiences");
        var gameAudienceSelectionContainer = gameOptionSelectionTabContainer?.GetNode<GridContainer>("AudienceSelectionBox/GridContainer");
        InstantiateListItems(gameAudiences, gameAudienceSelectionContainer, AudienceSelected);
    }

    private void AudienceSelected(IListItem selected)
    {
        GetNode<Button>("%OpenAudiencePaneButton").Text = $"Game Audience: {selected.Name}";
        GetNode<Button>("%OpenAudiencePaneButton").TooltipText = $"{selected.Name}: {selected.Description}";
    }

    private void InstantiateListItems<T>(T? listData, Control? instantiationParent, Action<IListItem>? buttonPressedCallback) where T : IEnumerable<IListItem>, new()
    {
        ArgumentNullException.ThrowIfNull(instantiationParent);
        ArgumentNullException.ThrowIfNull(_largeGridItemPrefab);

        instantiationParent.QueueFreeChildren();

        foreach (var listItem in listData ?? new())
        {
            var gridItem = _largeGridItemPrefab.Instantiate<LargeGridItem>();

            gridItem.ItemName = listItem.Name;
            gridItem.ItemDescription = listItem.Description;
            gridItem.ItemIcon = ResourceLoader.Load<Texture2D>(listItem.IconPath);
            if (buttonPressedCallback != null)
            {
                gridItem.Pressed += () => buttonPressedCallback(listItem);
            }

            instantiationParent.AddChild(gridItem);
        }
    }
}
