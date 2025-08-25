using System;
using Godot;
using IndieGameDev.Models;

namespace IndieGameDev.Backend;

public partial class GameController : Node
{
    private static GameController? _gameControllerInstance;
    public static GameController Instance
    {
        get => _gameControllerInstance ?? throw new InvalidOperationException($"GameController Instance is null!");
        private set => _gameControllerInstance = value;
    }

    private GameDataLoader _dataLoader = new GameDataLoader("res://resources/data");
    public GameDataLoader DataLoader
    {
        get => _dataLoader;
        private set => _dataLoader = value;
    }

    private ProjectType _currentProject = ProjectType.None;
    public ProjectType CurrentProject
    {
        get => _currentProject;
        set
        {
            _currentProject = value;
            SetMenuBar(_currentProject);
        }
    }

    private GameSpeed _currentSpeed = GameSpeed.Paused;
    public GameSpeed CurrentSpeed
    {
        get => _currentSpeed;
        set => _currentSpeed = value;
    }

    private Control? _uiRoot;
    public Control UiRoot
    {
        get
        {
            _uiRoot ??= GetTree().Root.GetNode<Control>("GameUI");
            return _uiRoot;
        }
    }

    public override void _Ready()
    {
        if (_gameControllerInstance == null)
        {
            GD.Print("Setting Instance to this.");
            _gameControllerInstance = this;
        }
        else
        {
            GD.PrintErr("Two instances of GameController cannot coexist! Aborting game.");
            GetTree().Quit();
        }
    }

    public void StartGame(GameCreationPayload gameCreationPayload)
    {
        CurrentProject = ProjectType.Game;
    }

    private void SetMenuBar(ProjectType currentProject)
    {
        var topLayoutBox = UiRoot.GetNode<TabContainer>("TopLayoutBox");
        topLayoutBox.CurrentTab = (int)currentProject;
    }
}
