using CBus;

namespace CBus.Hosting;

/// <summary>
/// 表示一次请求分发的结果。
/// </summary>
public sealed class CBusDispatchResult
{
    private CBusDispatchResult(bool isSuccess, string? matchedServiceName, CBusResponse response)
    {
        IsSuccess = isSuccess;
        MatchedServiceName = matchedServiceName;
        Response = response;
    }

    /// <summary>
    /// 获取本次分发是否命中了服务。
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// 获取命中的服务名称；未命中时为 <see langword="null"/>。
    /// </summary>
    public string? MatchedServiceName { get; }

    /// <summary>
    /// 获取最终返回的响应。
    /// </summary>
    public CBusResponse Response { get; }

    /// <summary>
    /// 创建成功分发结果。
    /// </summary>
    public static CBusDispatchResult Success(string matchedServiceName, CBusResponse response)
    {
        if (string.IsNullOrWhiteSpace(matchedServiceName))
        {
            throw new ArgumentException("Matched service name cannot be null or whitespace.", nameof(matchedServiceName));
        }

        ArgumentNullException.ThrowIfNull(response);
        return new CBusDispatchResult(true, matchedServiceName, response);
    }

    /// <summary>
    /// 创建未命中的分发结果。
    /// </summary>
    public static CBusDispatchResult NotFound(CBusResponse response)
    {
        ArgumentNullException.ThrowIfNull(response);
        return new CBusDispatchResult(false, null, response);
    }
}
