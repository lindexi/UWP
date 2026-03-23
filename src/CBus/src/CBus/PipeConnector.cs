namespace CBus;

public sealed class PipeConnector
{
    private readonly string _pipeAddress;
    private readonly ICBusPipeTransport _transport;

    public PipeConnector(string pipeAddress, ICBusPipeTransport transport)
    {
        if (string.IsNullOrWhiteSpace(pipeAddress))
        {
            throw new ArgumentException("Pipe address cannot be null or whitespace.", nameof(pipeAddress));
        }

        _pipeAddress = pipeAddress;
        _transport = transport ?? throw new ArgumentNullException(nameof(transport));
    }

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
