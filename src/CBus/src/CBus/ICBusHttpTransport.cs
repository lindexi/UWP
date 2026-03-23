namespace CBus;

public interface ICBusHttpTransport
{
    /// <summary>
    /// 将请求发送到指定的 HTTP 终结点。
    /// </summary>
    Task<CBusResponse> SendAsync(Uri endpoint, CBusRequest request, CancellationToken cancellationToken = default);
}
