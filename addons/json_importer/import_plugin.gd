@tool
extends EditorImportPlugin

func _get_importer_name() -> String:
	return "the_grand_jaggard.json"

func _get_visible_name() -> String:
	return "JSON Resource"

func _get_recognized_extensions() -> PackedStringArray:
	return ["json"]

func _get_save_extension() -> String:
	return "tres"

func _get_resource_type() -> String:
	# must match the class_name from json_resource.gd
	return "JsonResource"

func _get_preset_count() -> int:
	return 0

func _get_import_options(path: String, preset_index: int) -> Array:
	# No options in this simple plugin, return empty array (required).
	return []

func _get_option_visibility(path: String, option_name: StringName, options: Dictionary) -> bool:
	return true

func _get_priority() -> float:
	return 0.5

func _import(source_file: String, save_path: String, options: Dictionary, r_platform_variants: Array, r_gen_files: Array) -> int:
	# Open source JSON file
	var file := FileAccess.open(source_file, FileAccess.READ)
	if file == null:
		return FileAccess.get_open_error()

	# Read whole file to a string
	var text := ""
	while not file.eof_reached():
		text += file.get_line()
		if not file.eof_reached():
			text += "\n"
	file.close()

	# Parse JSON (using JSON API)
	var json := JSON.new()
	var err := json.parse_string(text)
	if err != OK:
		push_error("JSON parse error (%d) in %s" % [err, source_file])
		return err

	var parsed := json.get_data()

	# Create resource and set parsed data
	var resource := preload("res://addons/json_importer/json_resource.gd").new()
	resource.data = parsed

	# Save resource (save_path comes WITHOUT the extension per docs)
	var save_file := "%s.%s" % [save_path, _get_save_extension()]
	var save_err := ResourceSaver.save(resource, save_file)
	if save_err != OK:
		return save_err

	# Tell the editor about generated file(s)
	r_gen_files.push_back(save_file)

	return OK
