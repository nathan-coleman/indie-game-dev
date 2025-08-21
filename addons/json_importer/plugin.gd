@tool
extends EditorPlugin

var import_plugin : EditorImportPlugin = null

func _enter_tree() -> void:
	var import_plugin_path = get_script().resource_path.get_base_dir() + "/import_plugin.gd"
	import_plugin = load(import_plugin_path).new()
	add_import_plugin(import_plugin)

func _exit_tree() -> void:
	if import_plugin != null:
		remove_import_plugin(import_plugin)
		import_plugin = null
