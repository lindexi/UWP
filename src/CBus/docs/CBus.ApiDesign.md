# CBus API 设计

## 1. 总体结构

当前代码库由两个核心项目组成：

- `CBus`：客户端类库。
- `CBus.Host`：宿主进程。

两者通过共享的消息模型与发现约定协作，但宿主实现细节保持在 `CBus.Host` 内部。

## 2. `CBus` 项目 API

### 2.1 协议模型

- `CBusRequest`
- `CBusResponse`
- `CBusRouteDefinition`
- `CBusMessageSerializer`
- `CBusDefaults`

职责：

- 描述统一的请求/响应结构。
- 约束路由路径格式。
- 提供 Pipe 传输使用的文本协议编解码能力。

### 2.2 客户端连接能力

- `HttpConnector`
- `PipeConnector`
- `ICBusHttpTransport`
- `ICBusPipeTransport`
- `SystemNetHttpTransport`
- `SystemNamedPipeTransport`

职责：

- 提供稳定的客户端调用入口。
- 将具体网络传输细节封装在 transport 实现中。
- 允许测试使用替身 transport，运行时使用系统 transport。

### 2.3 发现能力

- `CBusEndpointDiscovery`
- `CBusEndpointDescriptor`
- `CBusDiscoveryOptions`
- `ICBusRegistryStore`
- `ICBusFileStore`
- `WindowsRegistryStore`
- `SystemFileStore`

职责：

- 从注册表读取 HTTP 端口。
- 从文件系统读取命名管道地址。
- 聚合成统一的发现结果。

## 3. `CBus.Host` 项目 API

### 3.1 宿主核心

- `CBusHost`
- `CBusHostOptions`
- `CBusAddressPublisher`

职责：

- 管理宿主配置与生命周期。
- 启动和停止 HTTP/命名管道监听器。
- 发布发现信息。

### 3.2 服务注册与分发

- `CBusServiceRegistration`
- `CBusServiceRegistry`
- `CBusDispatcher`
- `CBusDispatchResult`

职责：

- 维护服务元数据与路由映射。
- 保证路由根段唯一。
- 将请求分发到正确服务。

### 3.3 监听实现

- `CBusHttpListener`
- `CBusPipeListener`

职责：

- HTTP 监听器负责接收真实 HTTP 请求，并映射为 `CBusRequest`。
- Pipe 监听器负责接收真实命名管道消息，并通过 `CBusMessageSerializer` 解析请求。
- 两者都将结果统一回写为 `CBusResponse`。

## 4. 协议与边界

### 4.1 HTTP

- 客户端通过 `SystemNetHttpTransport` 使用 `HttpClient` 发送真实 HTTP 请求。
- 宿主通过 `CBusHttpListener` 监听 TCP/HTTP，并将请求方法、路径、头和正文映射为 `CBusRequest`。

### 4.2 Named Pipe

- 客户端通过 `SystemNamedPipeTransport` 使用命名管道发送请求。
- Pipe 消息使用长度前缀 + UTF-8 文本载荷。
- 文本载荷由 `CBusMessageSerializer` 负责请求/响应编解码。

## 5. 终结点发布规则

- HTTP 端口写入注册表值：`HKCU\Software\CBus\HttpPort`
- Pipe 地址写入文件：`%LocalAppData%\CBus\cbus.pipe`
- 客户端通过 `CBusEndpointDiscovery` 聚合这两部分信息。

## 6. 测试策略

- 客户端测试覆盖连接器、发现与协议序列化。
- 宿主测试覆盖注册、分发、地址发布以及真实 HTTP/Pipe 监听链路。
- 测试中的注册表与文件系统使用内存替身；监听链路使用真实 loopback HTTP 和真实命名管道。
