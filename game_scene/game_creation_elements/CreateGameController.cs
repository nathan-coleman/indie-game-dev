using System;
using System.Collections.Generic;
using Godot;
using NathanColeman.IndieGameDev.Models;
using NathanColeman.IndieGameDev.Utils;
using NathanColeman.IndieGameDev.Backend;

namespace NathanColeman.IndieGameDev.Ui;

public partial class CreateGameController : Node
{
    [Export] private PackedScene? _largeGridItemPrefab;

    private TabContainer? _optionsContainer;
    private GameCreationPayload _gameCreationPayload = new();

    public override void _Ready()
    {
        _optionsContainer = GetNode<TabContainer>("%GameOptionSelectionTabContainer");

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
        GetNode<Button>("%ConfirmGameCreationButton").Pressed += ConfirmGame;
    }

    private void ConfirmGame()
    {
        _gameCreationPayload.Name = GetNode<LineEdit>("%GameTitle").Text;

        if (!_gameCreationPayload.IsValid)
        {
            GD.PrintErr("Game is not complete!");
            return;
        }

        GetNode<LineEdit>("%GameTitle").Text = null;

        GameController.Instance.StartGame(_gameCreationPayload);
        GetParent().GetParent<CenterContentController>().CloseUi();
    }

    private void SetTab(int tabIndex)
    {
        ArgumentNullException.ThrowIfNull(_optionsContainer);
        _optionsContainer.CurrentTab = tabIndex;
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
        var gameScopes = GameController.Instance.DataLoader.LoadListItems<List<GameScope>>("GameScopes");
        var gameScopeSelectionContainer = _optionsContainer?.GetNode<GridContainer>("ScopeSelectionBox/MarginContainer/GridContainer");
        InstantiateListItems(gameScopes, gameScopeSelectionContainer, ScopeSelected);
    }

    private void ScopeSelected(GameScope selected)
    {
        _gameCreationPayload.Scope = selected;
        GetNode<Button>("%OpenScopePaneButton").Text = $"Scope: {selected.Name}";
        GetNode<Button>("%OpenScopePaneButton").TooltipText = $"{selected.Name}: {selected.Description}";
    }

    private void LoadGenreOptions()
    {
        var gameGenres = GameController.Instance.DataLoader.LoadListItems<List<GameGenre>>("GameGenres");
        var gameGenreSelectionContainer = _optionsContainer?.GetNode<GridContainer>("GenreSelectionBox/MarginContainer/GridContainer");
        InstantiateListItems(gameGenres, gameGenreSelectionContainer, GenreSelected);
    }

    private void GenreSelected(GameGenre selected)
    {
        _gameCreationPayload.Genre = selected;
        GetNode<Button>("%OpenGenrePaneButton").Text = $"Genre: {selected.Name}";
        GetNode<Button>("%OpenGenrePaneButton").TooltipText = $"{selected.Name}: {selected.Description}";
    }

    private void LoadTopicOptions()
    {
        var gameTopics = GameController.Instance.DataLoader.LoadListItems<List<GameTopic>>("GameTopics");
        var gameTopicSelectionContainer = _optionsContainer?.GetNode<GridContainer>("TopicSelectionBox/MarginContainer/GridContainer");
        InstantiateListItems(gameTopics, gameTopicSelectionContainer, TopicSelected);
    }

    private void TopicSelected(GameTopic selected)
    {
        _gameCreationPayload.Topic = selected;
        GetNode<Button>("%OpenTopicPaneButton").Text = $"Topic: {selected.Name}";
        GetNode<Button>("%OpenTopicPaneButton").TooltipText = $"{selected.Name}: {selected.Description}";
    }

    private void LoadPlatformOptions()
    {
        var gamePlatforms = GameController.Instance.DataLoader.LoadListItems<List<GamePlatform>>("GamePlatforms");
        var gamePlatformSelectionContainer = _optionsContainer?.GetNode<GridContainer>("PlatformSelectionBox/MarginContainer/GridContainer");
        InstantiateListItems(gamePlatforms, gamePlatformSelectionContainer, PlatformSelected);
    }

    private void PlatformSelected(GamePlatform selected)
    {
        _gameCreationPayload.Platform = selected;
        GetNode<Button>("%OpenPlatformPaneButton").Text = $"Platform: {selected.Name}";
        GetNode<Button>("%OpenPlatformPaneButton").TooltipText = $"{selected.Name}: {selected.Description}";
    }

    private void LoadAudienceOptions()
    {
        var gameAudiences = GameController.Instance.DataLoader.LoadListItems<List<GameAudience>>("GameAudiences");
        var gameAudienceSelectionContainer = _optionsContainer?.GetNode<GridContainer>("AudienceSelectionBox/MarginContainer/GridContainer");
        InstantiateListItems(gameAudiences, gameAudienceSelectionContainer, AudienceSelected);
    }

    private void AudienceSelected(GameAudience selected)
    {
        _gameCreationPayload.Audience = selected;
        GetNode<Button>("%OpenAudiencePaneButton").Text = $"Game Audience: {selected.Name}";
        GetNode<Button>("%OpenAudiencePaneButton").TooltipText = $"{selected.Name}: {selected.Description}";
    }

    private void InstantiateListItems<T>(IEnumerable<T>? listData, Control? instantiationParent, Action<T>? buttonPressedCallback) where T : IListItem
    {
        ArgumentNullException.ThrowIfNull(listData);
        ArgumentNullException.ThrowIfNull(instantiationParent);
        ArgumentNullException.ThrowIfNull(_largeGridItemPrefab);

        instantiationParent.QueueFreeChildren();

        foreach (var listItem in listData)
        {
            var gridItem = _largeGridItemPrefab.Instantiate<LargeGridItem>();
            gridItem.ItemName = listItem.Name;
            gridItem.ItemDescription = listItem.Description;
            gridItem.ItemIcon = ResourceLoader.Load<Texture2D>(listItem.IconPath);
            instantiationParent.AddChild(gridItem);

            if (buttonPressedCallback != null)
            {
                gridItem.Pressed += () => buttonPressedCallback(listItem);
            }
        }
    }
}
