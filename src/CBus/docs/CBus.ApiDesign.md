# CBus API 设计

## 1. 总体结构

CBus 现在拆分为两个核心项目：

- `CBus`：客户端/NuGet 类库。
- `CBus.Host`：可独立运行的宿主项目。

这样拆分后，协议层与宿主实现层职责明确分离。

## 2. `CBus` 项目 API

### 2.1 协议模型

- `CBusRequest`
- `CBusResponse`
- `CBusRouteDefinition`
- `CBusMessageSerializer`
- `CBusDefaults`

职责：

- 描述 HTTP 1.1 风格请求响应。
- 统一 Header、Body、状态码与路由路径。
- 提供文本消息编解码能力。

### 2.2 连接能力

- `HttpConnector`
- `PipeConnector`
- `ICBusHttpTransport`
- `ICBusPipeTransport`

职责：

- 对外提供 HTTP/Pipe 两种访问入口。
- 将真实传输细节下沉到传输抽象。
- 保持客户端调用方式稳定。

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
- 从文件系统读取 Pipe 地址。
- 组合成统一的可发现终结点信息。

## 3. `CBus.Host` 项目 API

### 3.1 宿主核心

- `CBusHost`
- `CBusHostOptions`
- `CBusAddressPublisher`

职责：

- 管理宿主运行配置。
- 发布 HTTP/Pipe 监听地址。
- 提供可执行入口。

### 3.2 服务注册与分发

- `CBusServiceRegistration`
- `CBusServiceRegistry`
- `CBusDispatcher`
- `CBusDispatchResult`

职责：

- 管理服务元数据与路由注册。
- 保证路径根段唯一。
- 根据请求路径完成服务分发。

### 3.3 监听适配

- `CBusHttpListener`
- `CBusPipeListener`

职责：

- 作为宿主侧的 HTTP/Pipe 监听入口。
- 将请求交给 `CBusDispatcher`。
- 返回统一的 `CBusResponse`。

## 4. 终结点发布规则

- HTTP 端口写入注册表键：`HKCU\Software\CBus\HttpPort`
- Pipe 地址写入文件：`%LocalAppData%\CBus\cbus.pipe`
- 客户端通过 `CBusEndpointDiscovery` 聚合这两部分信息

## 5. 测试策略

### 5.1 `CBus`

- 连接器测试验证传输抽象调用。
- 发现测试验证注册表与文件系统读取。
- 序列化测试验证协议文本往返一致性。

### 5.2 `CBus.Host`

- 服务注册测试验证根路径冲突约束。
- 分发测试验证命中与未命中行为。
- 宿主测试验证地址发布与监听适配。
