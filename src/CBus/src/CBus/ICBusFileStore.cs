namespace CBus;

public interface ICBusFileStore
{
    /// <summary>
    /// 读取文本文件内容。
    /// </summary>
    Task<string?> ReadAllTextAsync(string path, CancellationToken cancellationToken = default);

    /// <summary>
    /// 写入文本文件内容。
    /// </summary>
    Task WriteAllTextAsync(string path, string content, CancellationToken cancellationToken = default);
}
