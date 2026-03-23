using System.Net.Http;

namespace CBus;

/// <summary>
/// 使用系统 HTTP 客户端将请求发送到指定终结点。
/// </summary>
public sealed class SystemNetHttpTransport : ICBusHttpTransport, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly bool _disposeHttpClient;

    /// <summary>
    /// 使用默认 <see cref="HttpClient"/> 创建传输实现。
    /// </summary>
    public SystemNetHttpTransport()
        : this(new HttpClient(), true)
    {
    }

    /// <summary>
    /// 使用指定的 <see cref="HttpClient"/> 创建传输实现。
    /// </summary>
    public SystemNetHttpTransport(HttpClient httpClient)
        : this(httpClient, false)
    {
    }

    private SystemNetHttpTransport(HttpClient httpClient, bool disposeHttpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _disposeHttpClient = disposeHttpClient;
    }

    /// <summary>
    /// 使用系统 HTTP 客户端将请求发送到指定终结点。
    /// </summary>
    public async Task<CBusResponse> SendAsync(Uri endpoint, CBusRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(endpoint);
        ArgumentNullException.ThrowIfNull(request);

        using var message = new HttpRequestMessage(new HttpMethod(request.Method), new Uri(endpoint, request.Path));
        PopulateRequest(message, request);

        using var response = await _httpClient
            .SendAsync(message, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);

        var body = await response.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
        var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var header in response.Headers)
        {
            headers[header.Key] = string.Join(", ", header.Value);
        }

        foreach (var header in response.Content.Headers)
        {
            headers[header.Key] = string.Join(", ", header.Value);
        }

        return new CBusResponse((int)response.StatusCode, response.ReasonPhrase ?? response.StatusCode.ToString(), headers, body);
    }

    /// <summary>
    /// 释放由当前实例创建的 <see cref="HttpClient"/>。
    /// </summary>
    public void Dispose()
    {
        if (_disposeHttpClient)
        {
            _httpClient.Dispose();
        }
    }

    private static void PopulateRequest(HttpRequestMessage message, CBusRequest request)
    {
        ByteArrayContent? content = null;
        if (request.Body.Length > 0)
        {
            content = new ByteArrayContent(request.Body);
            message.Content = content;
        }

        foreach (var header in request.Headers)
        {
            if (message.Headers.TryAddWithoutValidation(header.Key, header.Value))
            {
                continue;
            }

            content ??= new ByteArrayContent(request.Body);
            message.Content ??= content;

            if (!content.Headers.TryAddWithoutValidation(header.Key, header.Value))
            {
                throw new InvalidOperationException($"Header '{header.Key}' cannot be added to the HTTP request.");
            }
        }
    }
}
