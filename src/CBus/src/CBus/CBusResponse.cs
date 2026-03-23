using System.Collections.ObjectModel;
using System.Text;

namespace CBus;

public sealed class CBusResponse
{
    public CBusResponse(int statusCode, string reasonPhrase, IReadOnlyDictionary<string, string>? headers = null, byte[]? body = null)
    {
        if (string.IsNullOrWhiteSpace(reasonPhrase))
        {
            throw new ArgumentException("Reason phrase cannot be null or whitespace.", nameof(reasonPhrase));
        }

        StatusCode = statusCode;
        ReasonPhrase = reasonPhrase;
        Headers = CreateHeaders(headers);
        Body = body?.ToArray() ?? [];
    }

    public int StatusCode { get; }

    public string ReasonPhrase { get; }

    public IReadOnlyDictionary<string, string> Headers { get; }

    public byte[] Body { get; }

    /// <summary>
    /// 创建成功响应。
    /// </summary>
    public static CBusResponse Ok(string? body = null, IReadOnlyDictionary<string, string>? headers = null, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        return new CBusResponse(200, "OK", headers, body is null ? null : encoding.GetBytes(body));
    }

    /// <summary>
    /// 创建未找到响应。
    /// </summary>
    public static CBusResponse NotFound(string? body = null, IReadOnlyDictionary<string, string>? headers = null, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        return new CBusResponse(404, "Not Found", headers, body is null ? null : encoding.GetBytes(body));
    }

    /// <summary>
    /// 按指定编码读取响应正文。
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
