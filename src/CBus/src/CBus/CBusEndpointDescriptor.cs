namespace CBus;

/// <summary>
/// 表示发现到的 CBus 终结点信息。
/// </summary>
public sealed record CBusEndpointDescriptor
{
    /// <summary>
    /// 使用 HTTP 终结点和命名管道地址创建发现结果。
    /// </summary>
    public CBusEndpointDescriptor(Uri httpEndpoint, string pipeAddress)
    {
        ArgumentNullException.ThrowIfNull(httpEndpoint);
        if (!httpEndpoint.IsAbsoluteUri)
        {
            throw new ArgumentException("HTTP endpoint must be an absolute URI.", nameof(httpEndpoint));
        }

        if (string.IsNullOrWhiteSpace(pipeAddress))
        {
            throw new ArgumentException("Pipe address cannot be null or whitespace.", nameof(pipeAddress));
        }

        HttpEndpoint = httpEndpoint;
        PipeAddress = pipeAddress;
    }

    /// <summary>
    /// 获取 HTTP 终结点。
    /// </summary>
    public Uri HttpEndpoint { get; }

    /// <summary>
    /// 获取命名管道地址。
    /// </summary>
    public string PipeAddress { get; }
}
