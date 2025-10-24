using Godot;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using System;
using System.IO;

namespace NathanColeman.IndieGameDev.Utils.Serilog;

public class GodotSink : ILogEventSink
{
    private const string DefaultOutputTemplate = "[{Timestamp:HH:mm:ss}] {Message:lj}";

    private readonly ITextFormatter _formatter;

    public GodotSink(string outputTemplate = DefaultOutputTemplate, IFormatProvider? formatProvider = null)
    {
        _formatter = new GodotTemplateRenderer(outputTemplate, formatProvider);
    }

    public void Emit(LogEvent logEvent)
    {
        using TextWriter writer = new StringWriter();
        _formatter.Format(logEvent, writer);
        writer.Flush();

        string color = logEvent.Level switch
        {
            LogEventLevel.Debug => Colors.WebGray.ToHtml(),
            LogEventLevel.Information => Colors.LightGray.ToHtml(),
            LogEventLevel.Warning => Colors.Yellow.ToHtml(),
            LogEventLevel.Error => Colors.Red.ToHtml(),
            LogEventLevel.Fatal => Colors.Purple.ToHtml(),
            _ => Colors.LightGray.ToHtml(),
        };

        foreach (string line in writer.ToString()?.Split('\n') ?? [])
        {
            GD.PrintRich($"[color=#{color}]{line}[/color]");
        }

        if (logEvent.Exception is null) return;

        if (logEvent.Level >= LogEventLevel.Error)
            GD.PushError(logEvent.Exception);
        else
            GD.PushWarning(logEvent.Exception);
    }
}
