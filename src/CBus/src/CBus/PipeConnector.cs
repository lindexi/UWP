namespace CBus;

/// <summary>
/// 提供面向命名管道的客户端调用入口。
/// </summary>
public sealed class PipeConnector
{
    private readonly string _pipeAddress;
    private readonly ICBusPipeTransport _transport;

    /// <summary>
    /// 使用目标管道地址和 Pipe 传输实现创建连接器。
    /// </summary>
    public PipeConnector(string pipeAddress, ICBusPipeTransport transport)
    {
        if (string.IsNullOrWhiteSpace(pipeAddress))
        {
            throw new ArgumentException("Pipe address cannot be null or whitespace.", nameof(pipeAddress));
        }

        _pipeAddress = pipeAddress;
        _transport = transport ?? throw new ArgumentNullException(nameof(transport));
    }

    /// <summary>
    /// 获取目标管道地址。
    /// </summary>
    public string PipeAddress => _pipeAddress;

    /// <summary>
    /// 使用 Pipe 风格连接发送请求。
    /// </summary>
    public Task<CBusResponse> SendAsync(CBusRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return _transport.SendAsync(_pipeAddress, request, cancellationToken);
    }
}
