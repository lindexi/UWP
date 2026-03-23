namespace CBus;

/// <summary>
/// 定义通过命名管道发送请求到 CBus 的传输能力。
/// </summary>
public interface ICBusPipeTransport
{
    /// <summary>
    /// 将请求发送到指定的 Pipe 地址。
    /// </summary>
    Task<CBusResponse> SendAsync(string pipeAddress, CBusRequest request, CancellationToken cancellationToken = default);
}
