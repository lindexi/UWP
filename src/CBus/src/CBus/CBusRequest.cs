using System.Collections.ObjectModel;
using System.Text;

namespace CBus;

public sealed class CBusRequest
{
    public CBusRequest(string method, string path, IReadOnlyDictionary<string, string>? headers = null, byte[]? body = null)
    {
        if (string.IsNullOrWhiteSpace(method))
        {
            throw new ArgumentException("Method cannot be null or whitespace.", nameof(method));
        }

        Method = method;
        Path = CBusRouteDefinition.ValidateAndReturnPath(path);
        Headers = CreateHeaders(headers);
        Body = body?.ToArray() ?? [];
    }

    public string Method { get; }

    public string Path { get; }

    public IReadOnlyDictionary<string, string> Headers { get; }

    public byte[] Body { get; }

    /// <summary>
    /// 使用文本内容创建请求。
    /// </summary>
    public static CBusRequest CreateText(string method, string path, string? body, IReadOnlyDictionary<string, string>? headers = null, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        return new CBusRequest(method, path, headers, body is null ? null : encoding.GetBytes(body));
    }

    /// <summary>
    /// 按指定编码读取请求正文。
    /// </summary>
    public string GetBodyAsString(Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        return encoding.GetString(Body);
    }

    private static IReadOnlyDictionary<string, string> CreateHeaders(IReadOnlyDictionary<string, string>? headers)
    {
        var dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        if (headers is not null)
        {
            foreach (var pair in headers)
            {
                dictionary[pair.Key] = pair.Value;
            }
        }

        return new ReadOnlyDictionary<string, string>(dictionary);
    }
}
