using System;
using Godot;
using NathanColeman.IndieGameDev.Models;
using NathanColeman.IndieGameDev.Ui;

namespace NathanColeman.IndieGameDev.Backend;

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
            GameUiController.SetMenuBar(_currentProject);
        }
    }

    private GameSpeed _currentSpeed = GameSpeed.Paused;
    public GameSpeed CurrentSpeed
    {
        get => _currentSpeed;
        set => _currentSpeed = value;
    }

    private GameClock? _gameClock;
    public GameClock GameClock
    {
        get
        {
            _gameClock ??= new GameClock(new(1, 1, 1), false);
            return _gameClock;
        }
        set => _gameClock = value;
    }

    private GameUiController? _gameUiController;
    public GameUiController GameUiController
    {
        get
        {
            _gameUiController ??= GetTree().Root.GetNode<GameUiController>("GameUi/GameUiController");
            return _gameUiController;
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

    public override void _Process(double delta)
    {
        GameClock.Process(delta);
    }

    public void StartGame(GameCreationPayload gameCreationPayload)
    {
        if (gameCreationPayload.IsValid == false) return;

        CurrentProject = ProjectType.Game;
        GameUiController.SetGameName(gameCreationPayload.Name!);
        GameUiController.InitializeBubblesBox();
    }

    public void AddBubbles(string bubbleName, int amount)
    {
        GameUiController.AddBubbles(bubbleName, amount);
    }
}
