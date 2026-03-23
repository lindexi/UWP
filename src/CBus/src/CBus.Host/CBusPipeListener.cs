using CBus;

namespace CBus.Hosting;

public sealed class CBusPipeListener : ICBusPipeTransport
{
    private readonly string _pipeAddress;
    private readonly CBusDispatcher _dispatcher;

    public CBusPipeListener(string pipeAddress, CBusDispatcher dispatcher)
    {
        if (string.IsNullOrWhiteSpace(pipeAddress))
        {
            throw new ArgumentException("Pipe address cannot be null or whitespace.", nameof(pipeAddress));
        }

        _pipeAddress = pipeAddress;
        _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
    }

    public string PipeAddress => _pipeAddress;

    /// <summary>
    /// 处理发往监听 Pipe 地址的请求。
    /// </summary>
    public async Task<CBusResponse> SendAsync(string pipeAddress, CBusRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(pipeAddress))
        {
            throw new ArgumentException("Pipe address cannot be null or whitespace.", nameof(pipeAddress));
        }

        ArgumentNullException.ThrowIfNull(request);

        if (!string.Equals(_pipeAddress, pipeAddress, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"Listener pipe address '{_pipeAddress}' does not match '{pipeAddress}'.");
        }

        var result = await _dispatcher.DispatchAsync(request, cancellationToken).ConfigureAwait(false);
        return result.Response;
    }
}
