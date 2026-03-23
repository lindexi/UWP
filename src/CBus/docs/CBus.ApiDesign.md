# CBus API 设计

## 1. 设计目标

本设计聚焦于 CBus 核心类库的最小可用 API，覆盖以下能力：

- 使用统一的 HTTP 1.1 风格消息模型表达请求与响应。
- 支持服务注册与基于路径第一节的路由分发。
- 为 `HttpConnector` 与 `PipeConnector` 提供一致的调用抽象。
- 将真实网络监听、进程拉起、注册表读写等基础设施能力留作后续扩展。

## 2. 命名空间

所有类型统一放在 `CBus` 命名空间下，保持现有项目风格。

## 3. 核心类型设计

### 3.1 `CBusRequest`

表示客户端到 CBus 的请求。

建议成员：

- `string Method`
- `string Path`
- `IReadOnlyDictionary<string, string> Headers`
- `byte[] Body`

职责：

- 校验方法与路径。
- 保存 Header 与 Body。
- 提供从文本创建 Body 的辅助方法。

### 3.2 `CBusResponse`

表示 CBus 或服务返回的响应。

建议成员：

- `int StatusCode`
- `string ReasonPhrase`
- `IReadOnlyDictionary<string, string> Headers`
- `byte[] Body`

职责：

- 表达标准响应。
- 提供创建成功、未找到、错误响应的辅助方法。

### 3.3 `CBusRouteDefinition`

表示一个服务声明的可路由路径。

建议成员：

- `string Path`
- `string RouteRoot`

职责：

- 校验路径格式。
- 解析第一节路径作为全局唯一根段。

### 3.4 `CBusServiceRegistration`

表示一个服务的注册信息。

建议成员：

- `string ServiceName`
- `string ExecutablePath`
- `IReadOnlyList<string> Arguments`
- `IReadOnlyList<CBusRouteDefinition> Routes`

职责：

- 聚合服务基础信息。
- 校验服务名、可执行路径与路由集合。

### 3.5 `CBusDispatchResult`

表示分发处理结果。

建议成员：

- `bool IsSuccess`
- `string? MatchedServiceName`
- `CBusResponse Response`

职责：

- 表达成功、未命中、处理失败等分发结果。

### 3.6 `CBusServiceRegistry`

表示服务注册表。

建议成员：

- `void Register(CBusServiceRegistration registration, Func<CBusRequest, CancellationToken, Task<CBusResponse>> handler)`
- `bool TryGetByPath(string path, out RegisteredService service)`
- `IReadOnlyCollection<CBusServiceRegistration> GetRegistrations()`

职责：

- 维护已注册服务。
- 校验路径根段唯一性。
- 按请求路径定位服务。

说明：`RegisteredService` 可作为内部类型，持有注册信息与处理委托。

### 3.7 `CBusDispatcher`

表示请求分发器。

建议成员：

- `Task<CBusDispatchResult> DispatchAsync(CBusRequest request, CancellationToken cancellationToken = default)`

职责：

- 根据请求路径查找服务。
- 调用注册处理委托。
- 未命中时返回 `404` 响应。

### 3.8 `CBusMessageSerializer`

表示 HTTP 1.1 风格消息编解码器。

建议成员：

- `string SerializeRequest(CBusRequest request)`
- `CBusRequest DeserializeRequest(string content)`
- `string SerializeResponse(CBusResponse response)`
- `CBusResponse DeserializeResponse(string content)`

职责：

- 将请求与响应转成文本消息。
- 让 HTTP 与 Pipe 连接器共享同一编解码逻辑。

约束：

- Header 行使用 `Key: Value` 格式。
- Header 与 Body 之间使用空行分隔。
- Body 以 Base64 文本承载，以避免二进制丢失。

### 3.9 `HttpConnector`

定位：面向 HTTP 风格传输的轻量连接器。

建议成员：

- 构造函数接收 `CBusDispatcher` 或处理委托。
- `Task<CBusResponse> SendAsync(CBusRequest request, CancellationToken cancellationToken = default)`
- `string Serialize(CBusRequest request)`
- `CBusRequest Deserialize(string content)`

职责：

- 暴露 HTTP 连接概念。
- 复用统一消息编解码。
- 将请求提交给分发器。

### 3.10 `PipeConnector`

定位：面向 Pipe 风格传输的轻量连接器。

建议成员与 `HttpConnector` 保持一致。

职责：

- 暴露 Pipe 连接概念。
- 保持与 HTTP 连接器相同的调用方式。
- 通过相同编解码器保证协议一致性。

## 4. 请求路径规则

### 4.1 路径格式

- 必须以 `/` 开始。
- 必须至少包含一个非空路径段。
- 根路径段使用路径去掉起始 `/` 后的第一节。
- `/Foo` 与 `/Foo/Bar` 的根路径段均为 `Foo`。

### 4.2 唯一性规则

- `CBusServiceRegistry` 内所有已注册路由的根路径段必须唯一。
- 若新服务与旧服务存在相同根路径段，注册失败并抛出明确异常。

## 5. 错误处理约定

- 参数为空：抛出 `ArgumentNullException`。
- 字符串为空白：抛出 `ArgumentException`。
- 路径格式错误：抛出 `ArgumentException`。
- 路由冲突：抛出 `InvalidOperationException`。
- 请求未命中：返回 `404 Not Found` 响应，不抛异常。

## 6. 测试点映射

### 6.1 `CBusRouteDefinition`

- 非法路径抛错。
- 能正确提取根路径段。

### 6.2 `CBusServiceRegistry`

- 单个服务可成功注册。
- 不同根路径段可同时注册。
- 相同根路径段注册失败。

### 6.3 `CBusDispatcher`

- 命中服务时返回服务处理结果。
- 未命中服务时返回 `404`。

### 6.4 `CBusMessageSerializer`

- 请求序列化后可无损反序列化。
- 响应序列化后可无损反序列化。

### 6.5 `HttpConnector` 与 `PipeConnector`

- 使用同一分发器时行为一致。
- 输入同一请求时输出相同响应。

## 7. 扩展点

本设计为后续扩展预留以下位置：

- 端口发现服务。
- 服务配置文件加载器。
- Windows 注册表端口记录器。
- 真实 `HttpListener` 或 ASP.NET Core 托管层。
- 真实命名管道传输层。
- 调用统计与禁用策略模块。
