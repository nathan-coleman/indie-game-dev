using Godot;

namespace IndieGameDev.Game.UI;

public partial class CenterContentController : TabContainer
{
    public override void _Ready()
    {
        CloseUi();
    }

    public void CloseUi() => OpenUiAtTabIndex(-1);
    public void OpenCreateGameUi() => OpenUiAtTabIndex(0);

    private void OpenUiAtTabIndex(int index)
    {
        var uiActive = index != -1;
        GetParentControl().GetNode<Control>("InputBlocker").Visible = uiActive;
        GetParentControl().GetNode<Button>("CloseButton").Visible = uiActive;

        CurrentTab = index;
    }
}
