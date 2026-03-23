using CBus;

namespace CBus.Hosting;

public sealed class CBusDispatchResult
{
    private CBusDispatchResult(bool isSuccess, string? matchedServiceName, CBusResponse response)
    {
        IsSuccess = isSuccess;
        MatchedServiceName = matchedServiceName;
        Response = response;
    }

    public bool IsSuccess { get; }

    public string? MatchedServiceName { get; }

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
