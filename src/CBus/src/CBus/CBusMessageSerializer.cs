using System.Text;

namespace CBus;

/// <summary>
/// 负责在 CBus 请求响应模型与文本协议之间进行转换。
/// </summary>
public sealed class CBusMessageSerializer
{
    /// <summary>
    /// 将请求序列化为 HTTP 1.1 风格文本。
    /// </summary>
    public string SerializeRequest(CBusRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        return Serialize(
            $"{request.Method} {request.Path} HTTP/1.1",
            request.Headers,
            request.Body);
    }

    /// <summary>
    /// 将 HTTP 1.1 风格文本反序列化为请求。
    /// </summary>
    public CBusRequest DeserializeRequest(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Content cannot be null or whitespace.", nameof(content));
        }

        var (startLine, headers, body) = Deserialize(content);
        var parts = startLine.Split(' ', 3, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2)
        {
            throw new FormatException("Invalid request start line.");
        }

        return new CBusRequest(parts[0], parts[1], headers, body);
    }

    /// <summary>
    /// 将响应序列化为 HTTP 1.1 风格文本。
    /// </summary>
    public string SerializeResponse(CBusResponse response)
    {
        ArgumentNullException.ThrowIfNull(response);

        return Serialize(
            $"HTTP/1.1 {response.StatusCode} {response.ReasonPhrase}",
            response.Headers,
            response.Body);
    }

    /// <summary>
    /// 将 HTTP 1.1 风格文本反序列化为响应。
    /// </summary>
    public CBusResponse DeserializeResponse(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Content cannot be null or whitespace.", nameof(content));
        }

        var (startLine, headers, body) = Deserialize(content);
        var parts = startLine.Split(' ', 3, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 3 || !parts[0].Equals("HTTP/1.1", StringComparison.OrdinalIgnoreCase))
        {
            throw new FormatException("Invalid response start line.");
        }

        if (!int.TryParse(parts[1], out var statusCode))
        {
            throw new FormatException("Invalid response status code.");
        }

        return new CBusResponse(statusCode, parts[2], headers, body);
    }

    private static string Serialize(string startLine, IReadOnlyDictionary<string, string> headers, byte[] body)
    {
        var builder = new StringBuilder();
        builder.AppendLine(startLine);

        foreach (var header in headers)
        {
            builder.Append(header.Key)
                .Append(": ")
                .AppendLine(header.Value);
        }

        builder.AppendLine();

        if (body.Length > 0)
        {
            builder.Append(Convert.ToBase64String(body));
        }

        return builder.ToString();
    }

    private static (string StartLine, Dictionary<string, string> Headers, byte[] Body) Deserialize(string content)
    {
        var normalized = content.Replace("\r\n", "\n", StringComparison.Ordinal);
        var separatorIndex = normalized.IndexOf("\n\n", StringComparison.Ordinal);
        var headerSection = separatorIndex >= 0 ? normalized[..separatorIndex] : normalized;
        var bodySection = separatorIndex >= 0 ? normalized[(separatorIndex + 2)..] : string.Empty;

        var lines = headerSection.Split('\n', StringSplitOptions.None);
        if (lines.Length == 0 || string.IsNullOrWhiteSpace(lines[0]))
        {
            throw new FormatException("Message start line is required.");
        }

        var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var line in lines.Skip(1))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var separator = line.IndexOf(':');
            if (separator <= 0)
            {
                throw new FormatException($"Invalid header line '{line}'.");
            }

            var key = line[..separator].Trim();
            var value = line[(separator + 1)..].Trim();
            headers[key] = value;
        }

        var trimmedBody = bodySection.Trim();
        var body = string.IsNullOrEmpty(trimmedBody)
            ? []
            : Convert.FromBase64String(trimmedBody);

        return (lines[0], headers, body);
    }
}
