using Godot;

namespace IndieGameDev.Backend;

public partial class GameController : Node
{
    // Follow Singleton pattern
    public static GameController? Instance { get; private set; }

    private GameDataLoader _dataLoader = new GameDataLoader("res://resources/data");
    public GameDataLoader DataLoader
    {
        get => _dataLoader;
        private set => _dataLoader = value;
    }

    public override void _Ready()
    {
        if (Instance == null) Instance = this;
        else
        {
            GD.PrintErr("Two instances of GameController cannot coexist! Exiting game...");
            GetTree().Quit();
        }
    }
}
