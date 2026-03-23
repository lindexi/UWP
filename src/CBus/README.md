# CBus

`CBus` 是一个面向本机进程间通信的消息总线实验项目，当前基于 `.NET 10` 维护。

仓库当前包含两个核心项目：

- `src/CBus`：客户端类库，提供请求/响应模型、消息序列化、终结点发现、连接器以及默认的 HTTP 与命名管道传输实现。
- `src/CBus.Host`：宿主进程，提供服务注册、路由分发、真实 HTTP 监听、真实命名管道监听以及地址发布能力。

## 当前能力

- 使用真实 HTTP 监听接收请求，并将方法、路径、头和正文映射为 `CBusRequest`。
- 使用真实命名管道接收请求，并通过 `CBusMessageSerializer` 进行请求/响应编解码。
- 通过 `CBusEndpointDiscovery` 从注册表与文件系统发现宿主终结点。
- 基于路由根段进行服务注册与请求分发。

## 快速开始

1. 运行 `src/CBus.Host`。
2. 使用 `CBusEndpointDiscovery` 获取当前宿主的 `HttpEndpoint` 和 `PipeAddress`。
3. 通过 `HttpConnector` + `SystemNetHttpTransport` 或 `PipeConnector` + `SystemNamedPipeTransport` 发起调用。

## 文档

- `docs/CBus.Requirements.md`
- `docs/CBus.ApiDesign.md`
- `docs/CBus.Usage.md`
