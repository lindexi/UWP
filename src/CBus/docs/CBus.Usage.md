# CBus 用法文档

## 1. 启动宿主

`CBus.Host` 是一个控制台宿主，负责：

- 监听 HTTP 终结点。
- 监听命名管道。
- 发布发现信息。
- 承载服务注册与请求分发。

示例：

```csharp
using CBus;
using CBus.Hosting;

await using var host = new CBusHost(
    new CBusHostOptions(
        CBusDefaults.DefaultPort,
        CBusDefaults.DefaultPipeAddress,
        CBusDefaults.DefaultPublishDirectory,
        CBusDefaults.DefaultRegistrySubKey),
    new WindowsRegistryStore(),
    new SystemFileStore());

host.RegisterService(
    new CBusServiceRegistration("FooService", "FooService.exe", [], [new CBusRouteDefinition("/Foo/Path1")]),
    static (request, _) => Task.FromResult(CBusResponse.Ok(request.GetBodyAsString())));

await host.StartAsync();
```

## 2. 发现宿主终结点

```csharp
using CBus;

var discovery = new CBusEndpointDiscovery(
    new WindowsRegistryStore(),
    new SystemFileStore());

var endpoints = await discovery.DiscoverAsync();
```

返回结果包含：

- `HttpEndpoint`
- `PipeAddress`

## 3. 使用 HTTP 连接器调用

```csharp
using CBus;

var endpoints = await new CBusEndpointDiscovery(
    new WindowsRegistryStore(),
    new SystemFileStore())
    .DiscoverAsync();

using var transport = new SystemNetHttpTransport();
var connector = new HttpConnector(endpoints.HttpEndpoint, transport);
var response = await connector.SendAsync(
    CBusRequest.CreateText("POST", "/Foo/Path1", "from-http"));
```

## 4. 使用命名管道连接器调用

```csharp
using CBus;

var endpoints = await new CBusEndpointDiscovery(
    new WindowsRegistryStore(),
    new SystemFileStore())
    .DiscoverAsync();

var connector = new PipeConnector(endpoints.PipeAddress, new SystemNamedPipeTransport());
var response = await connector.SendAsync(
    CBusRequest.CreateText("POST", "/Foo/Path1", "from-pipe"));
```

## 5. 集成建议

- 客户端优先通过 `CBusEndpointDiscovery` 获取终结点。
- HTTP 场景使用 `SystemNetHttpTransport`。
- 本机 IPC 场景使用 `SystemNamedPipeTransport`。
- 宿主生命周期建议使用 `await using`，确保退出时自动停止监听器。
