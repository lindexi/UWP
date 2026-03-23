namespace CBus;

public interface ICBusPipeTransport
{
    /// <summary>
    /// 将请求发送到指定的 Pipe 地址。
    /// </summary>
    Task<CBusResponse> SendAsync(string pipeAddress, CBusRequest request, CancellationToken cancellationToken = default);
}
