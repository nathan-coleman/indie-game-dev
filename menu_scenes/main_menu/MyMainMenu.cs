using Godot;

namespace IndieGameDev.Menus;

public partial class MyMainMenu : Control
{
	[Export(PropertyHint.File, "*.tscn")]
	public string GameScenePath;

	[Export]
	public PackedScene OptionsPackedScene;

	[Export]
	public PackedScene CreditsPackedScene;

	private Control optionsScene;
	private Control creditsScene;
	private Control subMenu;

	public override void _Ready()
	{
		HideExitForWeb();
		AddOrHideOptions();
		AddOrHideCredits();
		HideNewGameIfUnset();
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent.IsActionReleased("ui_cancel"))
		{
			if (subMenu != null)
				CloseSubMenu();
			else
				GetTree().Quit();
		}

		if (inputEvent.IsActionReleased("ui_accept") && GetViewport().GuiGetFocusOwner() == null)
		{
			GetNode<Container>("%MenuButtonsBoxContainer").FocusMode = FocusModeEnum.All;
			GetNode<Container>("%MenuButtonsBoxContainer").GrabFocus();
		}
	}

	public void OnNewGameButtonPressed()
	{
		NewGame();
	}

	public void OnOptionsButtonPressed()
	{
		OpenSubMenu(optionsScene);
	}

	public void OnCreditsButtonPressed()
	{
		OpenSubMenu(creditsScene);
	}

	public void OnExitButtonPressed()
	{
		GetTree().Quit();
	}

	public void OnCreditsEndReached()
	{
		if (subMenu == creditsScene)
		{
			CloseSubMenu();
		}
	}

	public void OnBackButtonPressed()
	{
		CloseSubMenu();
	}

	private void LoadGameScene()
	{
		var sceneLoader = GetNode("/root/SceneLoader");
		sceneLoader.Call("LoadScene", GameScenePath);
	}

	private void NewGame()
	{
		LoadGameScene();
	}

	private void OpenSubMenu(Control menu)
	{
		subMenu = menu;
		subMenu.Show();
		GetNode<Button>("%BackButton").Show();
		GetNode<Control>("%MenuContainer").Hide();
	}

	private void CloseSubMenu()
	{
		if (subMenu == null)
			return;

		subMenu.Hide();
		subMenu = null;
		GetNode<Button>("%BackButton").Hide();
		GetNode<Control>("%MenuContainer").Show();
	}

	private static bool EventIsMouseButtonReleased(InputEvent inputEvent)
	{
		return inputEvent is InputEventMouseButton mouseEvent
			&& !mouseEvent.Pressed;
	}

	private void HideExitForWeb()
	{
		if (OS.HasFeature("web"))
		{
			GetNode<Button>("%ExitButton").Hide();
		}
	}

	private void HideNewGameIfUnset()
	{
		if (string.IsNullOrEmpty(GameScenePath))
		{
			GetNode<Button>("%NewGameButton").Hide();
		}
	}

	private void AddOrHideOptions()
	{
		if (OptionsPackedScene == null)
		{
			GetNode<Button>("%OptionsButton").Hide();
		}
		else
		{
			optionsScene = OptionsPackedScene.Instantiate<Control>();
			optionsScene.Hide();
			GetNode<Control>("%OptionsContainer").CallDeferred("add_child", optionsScene);
		}
	}

	private void AddOrHideCredits()
	{
		if (CreditsPackedScene == null)
		{
			GetNode<Button>("%CreditsButton").Hide();
		}
		else
		{
			creditsScene = CreditsPackedScene.Instantiate<Control>();
			creditsScene.Hide();
			if (creditsScene.HasSignal("end_reached"))
			{
				creditsScene.Connect("end_reached", new Callable(this, nameof(OnCreditsEndReached)));
			}
			GetNode<Control>("%CreditsContainer").CallDeferred("add_child", creditsScene);
		}
	}
}
