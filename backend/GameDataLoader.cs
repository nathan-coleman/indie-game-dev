using Godot;
using Newtonsoft.Json;
using IndieGameDev.Models;
using System.Collections.Generic;

namespace IndieGameDev.Backend;

public class GameDataLoader(
    string _rootDataPath,
    string _fileExtension = "json",
    string _defaultIconPath = "res://resources/assets/game_logo/icon.png")
{
    private readonly Dictionary<string, object> _cache = new();

    public T LoadListItems<T>(string fileName, string? subfolder = null) where T : IEnumerable<IListItem>
    {
        IEnumerable<IListItem> data = LoadData<T>(fileName, subfolder);

        foreach (var listItem in data)
        {
            listItem.IconPath ??= _defaultIconPath;
        }

        return (T)data;
    }

    public T LoadData<T>(string fileName, string? subfolder = null)
    {
        if (_cache.TryGetValue($"{subfolder}/{fileName}", out object? cachedValue))
        {
            return (T)cachedValue;
        }

        var fullResourcePath = $"{_rootDataPath}/{subfolder}/{fileName}.{_fileExtension}";
        using var file = FileAccess.Open(fullResourcePath, FileAccess.ModeFlags.Read);
        if (file == null) throw new JsonException($"Error loading '{fileName}': {FileAccess.GetOpenError()}");

        var rawJson = file.GetAsText();
        var deserializedObject = JsonConvert.DeserializeObject<T>(rawJson);
        if (deserializedObject == null) throw new JsonException($"Error deserializing json in '{fileName}' to {typeof(T)}");

        _cache.Add($"{subfolder}/{fileName}", deserializedObject);
        return deserializedObject;
    }
}
