using Godot;

namespace NathanColeman.IndieGameDev.Ui;

public partial class CenterContentController : TabContainer
{
    public override void _Ready()
    {
        CloseUi();
    }

    public void CloseUi() => OpenUiAtTabIndex(-1);

    public CreateGameController OpenCreateGameUi()
    {
        Control tab = OpenUiAtTabIndex(0);
        return tab.GetNode<CreateGameController>("CreateGameController");
    }

    public CompleteGameController OpenCompleteGameUi()
    {
        Control tab = OpenUiAtTabIndex(1);
        return tab.GetNode<CompleteGameController>("CompleteGameController");
    }

    private Control OpenUiAtTabIndex(int index)
    {
        var uiActive = index != -1;
        GetParentControl().GetNode<Control>("InputBlocker").Visible = uiActive;
        GetParentControl().GetNode<Button>("CloseButton").Visible = uiActive;

        CurrentTab = index;
        return GetCurrentTabControl();
    }
}
