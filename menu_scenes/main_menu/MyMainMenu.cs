using System;
using Godot;

namespace IndieGameDev.Menus;

public partial class MyMainMenu : Control
{
	[Export(PropertyHint.File, "*.tscn")] private string? _gameScenePath;

	[Export] private PackedScene? _optionsPackedScene;

	[Export] private PackedScene? _creditsPackedScene;

	private Control? _optionsScene;
	private Control? _creditsScene;
	private Control? _subMenu;

	public override void _Ready()
	{
		HideExitForWeb();
		AddOrHideOptions();
		AddOrHideCredits();
		HideNewGameIfUnset();
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent.IsActionReleased("ui_cancel") && _subMenu != null)
		{
			CloseSubMenu();
		}

		if (inputEvent.IsActionReleased("ui_accept") && GetViewport().GuiGetFocusOwner() == null)
		{
			GetNode<Container>("%MenuButtonsBoxContainer").FocusMode = FocusModeEnum.All;
			GetNode<Container>("%MenuButtonsBoxContainer").GrabFocus();
		}
	}

	public void OnNewGameButtonPressed() => LoadGameScene();

    public void OnOptionsButtonPressed() => OpenSubMenu(_optionsScene);

    public void OnCreditsButtonPressed() => OpenSubMenu(_creditsScene);

    public void OnExitButtonPressed() => GetTree().Quit();

    public void OnBackButtonPressed() => CloseSubMenu();

    public void OnCreditsEndReached() => CloseCreditsScene();

    private void LoadGameScene()
	{
        ArgumentNullException.ThrowIfNullOrWhiteSpace(_gameScenePath);
		var sceneLoader = GetNode("/root/SceneLoader");
		sceneLoader.Call("load_scene", _gameScenePath);
	}

	private void OpenSubMenu(Control? menu)
	{
		if (menu == null) return;

		_subMenu = menu;
		_subMenu.Show();
		GetNode<Button>("%BackButton").Show();
		GetNode<Control>("%MenuContainer").Hide();
	}

	private void CloseSubMenu()
	{
		if (_subMenu == null) return;

		_subMenu.Hide();
		_subMenu = null;
		GetNode<Button>("%BackButton").Hide();
		GetNode<Control>("%MenuContainer").Show();
	}

    private void CloseCreditsScene()
    {
        if (_subMenu == _creditsScene)
        {
            CloseSubMenu();
        }
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
		if (string.IsNullOrEmpty(_gameScenePath))
		{
			GetNode<Button>("%NewGameButton").Hide();
		}
	}

	private void AddOrHideOptions()
	{
		if (_optionsPackedScene == null)
		{
			GetNode<Button>("%OptionsButton").Hide();
		}
		else
		{
			_optionsScene = _optionsPackedScene.Instantiate<Control>();
			_optionsScene.Hide();
			GetNode<Control>("%OptionsContainer").CallDeferred("add_child", _optionsScene);
		}
	}

	private void AddOrHideCredits()
	{
		if (_creditsPackedScene == null)
		{
			GetNode<Button>("%CreditsButton").Hide();
		}
		else
		{
			_creditsScene = _creditsPackedScene.Instantiate<Control>();
			_creditsScene.Hide();
			if (_creditsScene.HasSignal("end_reached"))
			{
				_creditsScene.Connect("end_reached", new Callable(this, nameof(OnCreditsEndReached)));
			}
			GetNode<Control>("%CreditsContainer").CallDeferred("add_child", _creditsScene);
		}
	}
}
