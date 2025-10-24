using NathanColeman.IndieGameDev.Models;
using NathanColeman.IndieGameDev.Ui;
using NathanColeman.IndieGameDev.Utils.Serilog;
using Godot;
using Serilog;
using System;
using System.Collections.Generic;

namespace NathanColeman.IndieGameDev.Backend;

public partial class GameController : Node
{
#region Singleton-likes
    private static GameController? _gameControllerInstance;
    public static GameController Instance
    {
        get => _gameControllerInstance ?? throw new InvalidOperationException($"GameController Instance is null!");
        private set => _gameControllerInstance = value;
    }

    private ILogger? _logger;
    public ILogger Logger
    {
        get
        {
            if (_logger == null)
            {
                var configuration = new LoggerConfiguration();
                configuration.WriteTo.Sink(new GodotSink());

                if (OS.IsDebugBuild()) configuration.MinimumLevel.Debug();
                else configuration.MinimumLevel.Information();

                _logger = configuration.CreateLogger();
                _logger.Information("Logger initiallized");
            }

            return _logger;
        }
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
#endregion

    private GameCompletionPayload? _activeGame;
    public GameCompletionPayload CurrentGame
    {
        get
        {
            if (CurrentProject != ProjectType.Game) GD.PushError("ActiveGame should be accessed when current project is not a game");
            if (_activeGame == null) throw new NullReferenceException("ActiveGame can not be be accessed when _activeGame is null");
            return _activeGame;
        }
        set
        {
            _activeGame = value;
        }
    }

    public override void _Ready()
    {
        if (_gameControllerInstance == null)
        {
            GD.Print("Setting GameController instance.");
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
        CurrentGame = new GameCompletionPayload(gameCreationPayload);

        var bubbleTypes = DataLoader.LoadData<List<BubbleType>>("BubbleTypes");
        foreach (var bubbleType in bubbleTypes)
        {
            CurrentGame.BubbleValues.Add(bubbleType.Name, 0);
        }

        GameUiController.SetGameName(gameCreationPayload.Name!);
        GameUiController.InitializeBubblesBox();
    }

    public void AddBubbles(string bubbleName, int amount)
    {
        ArgumentNullException.ThrowIfNull(CurrentGame.BubbleValues);

        CurrentGame.BubbleValues[bubbleName] += amount;
        GameUiController.AddBubbles(bubbleName, amount);
    }

    public void CompleteGame()
    {
        if (CurrentProject != ProjectType.Game) throw new InvalidOperationException("Cannot complete game when current project is not a game");
        // if (CurrentGame.IsValid == false) return;

        var gameScore = ScoreCalculator.CalculateScore(CurrentGame);
        GameUiController.ShowCompleteGameBox(gameScore);

        CurrentProject = ProjectType.None;
        // ArchivedGames += CurrentGame;

        GameUiController.DeinitializeBubblesBox();
    }
}
