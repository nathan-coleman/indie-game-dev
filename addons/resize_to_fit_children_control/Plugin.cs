#if TOOLS
using System.Linq;
using Godot;

namespace TheGrandJaggard.Addons.ResizeToFitChildrenControl;

[Tool]
public partial class Plugin : EditorPlugin
{
	public override void _EnterTree()
	{
		CallDeferred(nameof(SetupPlugin));
	}

	public override void _ExitTree()
	{
		RemoveCustomType(nameof(ResizeToFitChildrenControl));
	}

	private void SetupPlugin()
	{
		var allPluginPaths = ProjectSettings.GetSetting("editor_plugins/enabled").AsStringArray();
		var pathToConfigFile = allPluginPaths.FirstOrDefault(path => "resize_to_fit_children_control".IsSubsequenceOf(path));
		var pluginRootPath = pathToConfigFile?.GetBaseDir();

		if (pluginRootPath == null)
		{
			GD.PrintErr("resize_to_fit_children_control could not be found in enabled plugins list."
				+ " Plugin will not work as expected.");
			return;
		}

		var controlScript = GD.Load<Script>($"{pluginRootPath}/{nameof(ResizeToFitChildrenControl)}.cs");
		var controlIcon = GD.Load<Texture2D>($"{pluginRootPath}/Control.svg");

		AddCustomType(nameof(ResizeToFitChildrenControl), nameof(Control), controlScript, controlIcon);
	}
}
#endif
