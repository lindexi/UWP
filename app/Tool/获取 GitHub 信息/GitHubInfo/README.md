# GitHubInfo

`GitHubInfo` 是一个用于将 GitHub Pull Request 或 Issue 导出为 AI 友好文件的命令行工具。

- 输入 Pull Request 地址，输出 Git patch 文件。
- 输入 Issue 地址，输出包含 Issue 正文和全部评论的 Markdown 文件。
- 支持使用 GitHub Token 访问私有仓库并提高 API 请求限额。

## 环境要求

- .NET 10 SDK
- 可访问 `github.com` 和 `api.github.com` 的网络

检查 .NET SDK：

```powershell
dotnet --version
```

## 构建

在项目目录执行：

```powershell
dotnet build
```

生成的程序默认位于：

```text
bin\Debug\net10.0\GitHubInfo.exe
```

发布 Release 版本：

```powershell
dotnet publish -c Release
```

## 使用方法

命令格式：

```text
GitHubInfo <GitHub PR 或 Issue 地址> [输出文件路径]
```

第二个参数可以省略。省略时，程序会在当前工作目录自动生成输出文件。

## 导出 Pull Request Patch

```powershell
GitHubInfo.exe https://github.com/owner/repository/pull/123
```

默认输出文件名：

```text
owner-repository-pr-123.patch
```

也可以指定输出路径：

```powershell
GitHubInfo.exe https://github.com/owner/repository/pull/123 D:\GitHubExports\change.patch
```

输出内容是 GitHub 返回的完整 patch，可用于代码审查、AI 分析或 Git 工具处理。

## 导出 Issue Markdown

```powershell
GitHubInfo.exe https://github.com/owner/repository/issues/123
```

默认输出文件名：

```text
owner-repository-issue-123.md
```

也可以指定输出路径：

```powershell
GitHubInfo.exe https://github.com/owner/repository/issues/123 D:\GitHubExports\issue.md
```

Markdown 文件包含：

- Issue 标题、地址和状态。
- Issue 创建者发表的正文。
- 按时间排序的全部评论。
- 每条发言的用户名和时间。
- 发言之间使用多个 `=` 组成的分隔线。

Issue 正文和评论会直接保留原始 Markdown，不会放入代码块，也不会额外转义。这种格式主要用于提供给 AI 阅读，而不是用于安全地重新发布到网页。

输出格式大致如下：

```text
# Issue owner/repository#123: Issue title

URL: https://github.com/owner/repository/issues/123
State: open

========================================

user1 said at 2025-01-01T12:00:00.0000000+00:00:

这里是 Issue 正文，保留原始 Markdown。

========================================

user2 said at 2025-01-02T12:00:00.0000000+00:00:

这里是评论内容。
```

## 配置 GitHub Token

未认证的 GitHub API 请求通常受同一公网 IP 每小时约 60 次的限制。如果使用公司网络、代理、VPN、云主机或共享公网出口，额度可能被其他用户共同消耗。

发生限流时通常会看到：

```text
API rate limit exceeded
```

或者：

```text
Response status code does not indicate success: 403 (rate limit exceeded)
```

这不表示 PR 或 Issue 地址错误，而是当前请求没有认证，或者认证账号的限额已经耗尽。

程序从 `GITHUB_TOKEN` 环境变量读取 Token。

### 在 CMD 中临时设置

该设置只对当前 CMD 窗口有效：

```bat
set GITHUB_TOKEN=你的GitHub令牌
GitHubInfo.exe https://github.com/owner/repository/issues/123
```

### 在 PowerShell 中临时设置

该设置只对当前 PowerShell 会话有效：

```powershell
$env:GITHUB_TOKEN = "你的GitHub令牌"
.\GitHubInfo.exe https://github.com/owner/repository/issues/123
```

### 在 Windows 中持久设置

```bat
setx GITHUB_TOKEN "你的GitHub令牌"
```

执行后需要关闭并重新打开终端，新环境变量才会生效。

### 检查 Token 是否已设置

CMD：

```bat
if defined GITHUB_TOKEN (echo GITHUB_TOKEN is set) else (echo GITHUB_TOKEN is not set)
```

PowerShell：

```powershell
if ($env:GITHUB_TOKEN) { "GITHUB_TOKEN is set" } else { "GITHUB_TOKEN is not set" }
```

不建议直接输出 Token 内容，以免它被保存到终端历史、截图或日志中。

## Token 权限建议

对于公开仓库，使用只具备必要读取权限的 fine-grained personal access token 即可。

如果需要导出私有仓库的信息，Token 必须获得对应仓库的读取权限。应遵循最小权限原则，不要为本工具授予写入、管理或删除权限。

不要执行以下操作：

- 不要把 Token 写入源代码。
- 不要把 Token 提交到 Git 仓库。
- 不要在聊天记录、Issue、日志或截图中公开 Token。
- 不要把包含 Token 的配置文件发送给其他人。

如果 Token 意外泄露，应立即在 GitHub 中撤销并重新创建。

## 默认输出位置

未指定输出路径时，文件写入当前工作目录，而不是 `GitHubInfo.exe` 所在目录。

例如当前目录是 `D:\`：

```bat
D:\>C:\Tools\GitHubInfo.exe https://github.com/owner/repository/issues/123
```

则输出文件位于：

```text
D:\owner-repository-issue-123.md
```

如果希望输出到固定目录，请提供第二个参数。

## 取消操作

请求过程中按 `Ctrl+C` 可以取消。程序被取消时返回退出码 `130`。

## 常见问题

### 为什么浏览器可以打开 GitHub，但程序提示 API 限流？

浏览器打开 GitHub 页面和程序访问 GitHub API 使用不同的限额机制。浏览器已经登录 GitHub，并不意味着命令行程序自动获得浏览器登录状态。

本工具不会自动读取：

- 浏览器中的 GitHub 登录信息。
- Git Credential Manager 凭据。
- Git 命令配置的远程仓库凭据。
- GitHub CLI 的登录信息。

需要通过 `GITHUB_TOKEN` 环境变量显式提供 Token。

### 为什么设置 Token 后仍然返回 403？

可能原因包括：

1. 当前终端是在执行 `setx` 之前打开的，需要重新打开终端。
2. Token 已过期、被撤销或复制不完整。
3. Token 无权访问目标私有仓库。
4. 组织启用了 SSO，但 Token 尚未获得组织授权。
5. 认证账号自身的 API 限额也已耗尽。
6. 网络代理或安全软件移除了认证请求头。

### 支持哪些 URL？

支持标准 HTTPS GitHub 地址：

```text
https://github.com/owner/repository/pull/123
https://github.com/owner/repository/issues/123
```

当前不支持 GitHub Enterprise、自定义域名、PR diff 地址或单条评论地址。

### 为什么 Issue 内容不进行 Markdown 转义？

输出文件用于 AI 阅读，需要尽量保留 GitHub 原始内容。即使正文恰好包含与分隔线相同的内容，也不会进行特殊处理。

## 退出码

- `0`：导出成功。
- `1`：请求、权限、文件写入或其他可处理错误。
- `2`：命令参数数量错误。
- `130`：用户通过 `Ctrl+C` 取消。
