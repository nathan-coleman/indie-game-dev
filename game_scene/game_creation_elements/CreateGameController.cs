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
    [Export] PackedScene _largeGridItemPrefab;

    private TabContainer gameOptionSelectionTabContainer;

    public override void _Ready()
    {
        gameOptionSelectionTabContainer = GetNode<TabContainer>("%GameOptionSelectionTabContainer");

        ConnectButtons();
        LoadData();
    }

    private void ConnectButtons()
    {
        GetNode<Button>("%OpenScopePaneButton").Pressed += OpenScopePane;
        GetNode<Button>("%OpenGenrePaneButton").Pressed += OpenGenrePane;
        GetNode<Button>("%OpenTopicPaneButton").Pressed += OpenTopicPane;
        GetNode<Button>("%OpenPlatformPaneButton").Pressed += OpenPlatformPane;
        GetNode<Button>("%OpenGraphicsPaneButton").Pressed += OpenGraphicsPane;
        GetNode<Button>("%OpenTargetAudiencePaneButton").Pressed += OpenTargetAudiencePane;
    }

    private void OpenScopePane() => gameOptionSelectionTabContainer.CurrentTab = 0;
    private void OpenGenrePane() => gameOptionSelectionTabContainer.CurrentTab = 1;
    private void OpenTopicPane() => gameOptionSelectionTabContainer.CurrentTab = 2;
    private void OpenPlatformPane() => gameOptionSelectionTabContainer.CurrentTab = 3;
    private void OpenGraphicsPane() => gameOptionSelectionTabContainer.CurrentTab = 4;
    private void OpenTargetAudiencePane() => gameOptionSelectionTabContainer.CurrentTab = 5;

    private void LoadData()
    {
        LoadScopes();
    }

    private void LoadScopes()
    {
        var gameScopes = LoadJsonAsObject<List<GameScope>>("res://resources/data/GameScopes.json");
        var gameScopeSelectionContainer = gameOptionSelectionTabContainer.GetNode<GridContainer>("ScopeSelectionBox/GridContainer");
        InstantiateListItems(gameScopes.Cast<IListItemModel>(), gameScopeSelectionContainer, ScopeSelected);
    }

    private void ScopeSelected(IListItemModel selected)
    {
        GetNode<Button>("%OpenScopePaneButton").Text = $"Game Scope: {selected.Name}";
        GetNode<Button>("%OpenScopePaneButton").TooltipText = $"Selected: {selected.Description}";
    }

    private void InstantiateListItems(IEnumerable<IListItemModel> listData, Control instantiationParent, Action<IListItemModel> buttonPressedCallback)
    {
        instantiationParent.QueueFreeChildren();

        foreach (var listItem in listData)
        {
            var gridItem = _largeGridItemPrefab.Instantiate<LargeGridItem>();

            gridItem.ItemName = listItem.Name;
            gridItem.ItemDescription = listItem.Description;
            gridItem.ItemIcon = ResourceLoader.Load<Texture2D>(listItem.IconPath);
            gridItem.Pressed += () => buttonPressedCallback(listItem);

            instantiationParent.AddChild(gridItem);
        }
    }

    private static T LoadJsonAsObject<T>(string filePath)
    {
        using var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
        if (file == null)
        {
            GD.PrintErr($"Error loading '{filePath}': {FileAccess.GetOpenError()}");
            return default;
        }
        var rawJson = file.GetAsText();
        var deserializedObject = JsonConvert.DeserializeObject<T>(rawJson);
        if (deserializedObject == null) throw new JsonException($"Error deserializing json in '{filePath}' to {typeof(T)}");

        return deserializedObject;
    }
}
