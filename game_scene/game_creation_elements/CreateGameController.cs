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
        LoadScopes();
    }

    private void LoadScopes()
    {
        var gameScopes = LoadJsonAsObject<List<GameScope>>("res://resources/data/GameScopes.json");
        var gameScopeSelectionContainer = gameOptionSelectionTabContainer?.GetNode<GridContainer>("ScopeSelectionBox/GridContainer");
        InstantiateListItems(gameScopes.Cast<IListItemModel>(), gameScopeSelectionContainer, ScopeSelected);
    }

    private void ScopeSelected(IListItemModel selected)
    {
        GetNode<Button>("%OpenScopePaneButton").Text = $"Game Scope: {selected.Name}";
        GetNode<Button>("%OpenScopePaneButton").TooltipText = $"Selected: {selected.Description}";
    }

    private void InstantiateListItems(IEnumerable<IListItemModel> listData, Control? instantiationParent, Action<IListItemModel>? buttonPressedCallback)
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
