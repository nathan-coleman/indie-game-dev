
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using Serilog.Parsing;

namespace NathanColeman.IndieGameDev.Utils.Serilog;

public class GodotTemplateRenderer : ITextFormatter
{
    private delegate void Renderer(LogEvent logEvent, TextWriter output);

    private readonly List<Renderer> _renderers;
    private readonly IFormatProvider? _formatProvider;

    public GodotTemplateRenderer(string outputTemplate, IFormatProvider? formatProvider)
    {
        _formatProvider = formatProvider;
        _renderers = new();

        MessageTemplate template = new MessageTemplateParser().Parse(outputTemplate);

        foreach (var token in template.Tokens)
        {
            if (token is TextToken textToken)
            {
                _renderers.Add((_, output) => output.Write(textToken.Text));
            }
            else if (token is PropertyToken propertyToken)
            {
                Renderer renderer = propertyToken.PropertyName switch
                {
                    OutputProperties.LevelPropertyName => (logEvent, output) => output.Write(logEvent.Level),
                    OutputProperties.MessagePropertyName => (logEvent, output) => logEvent.RenderMessage(output, _formatProvider),
                    OutputProperties.NewLinePropertyName => (_, output) => output.Write('\n'),
                    OutputProperties.TimestampPropertyName => TimestampRenderer(propertyToken.Format),
                    _ => PropertyRenderer(propertyToken.PropertyName, propertyToken.Format),
                };
                _renderers.Add(renderer);
            }
        }
    }

    public void Format(LogEvent logEvent, TextWriter output)
    {
        foreach (var renderer in _renderers)
        {
            renderer.Invoke(logEvent, output);
        }
    }

    private Renderer TimestampRenderer(string? format)
    {
        Func<LogEvent, string> f = _formatProvider?.GetFormat(typeof(ICustomFormatter)) is ICustomFormatter formatter
            ? (logEvent) => formatter.Format(format, logEvent.Timestamp, _formatProvider)
            : (logEvent) => logEvent.Timestamp.ToString(format, _formatProvider ?? CultureInfo.InvariantCulture);

        return (logEvent, output) => output.Write(f(logEvent));
    }

    private Renderer PropertyRenderer(string propertyName, string? format)
    {
        return delegate (LogEvent logEvent, TextWriter output)
        {
            if (logEvent.Properties.TryGetValue(propertyName, out var propertyValue))
                propertyValue.Render(output, format, _formatProvider);
        };
    }
}
