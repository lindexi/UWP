# CBus 用法文档

## 1. 项目拆分

当前 CBus 被拆分为两个核心项目：

- `src/CBus`：面向客户端与服务安装方的 NuGet 类库，提供消息模型、HTTP/Pipe 连接器与 CBus 发现能力。
- `src/CBus.Host`：可独立运行的宿主项目，提供服务注册、请求分发、HTTP/Pipe 监听适配以及地址发布能力。

## 2. 启动宿主

宿主项目默认：

- 监听 HTTP 端口：`3323`
- 发布 Pipe 地址：`cbus.pipe`
- 将 HTTP 端口写入注册表：`HKCU\\Software\\CBus\\HttpPort`
- 将 Pipe 地址写入文件：`%LocalAppData%\\CBus\\cbus.pipe`

直接运行 `CBus.Host` 后，宿主会注册内置的 `/CBus/Ping` 路由，并将发现信息发布出去。

## 3. 在宿主中注册服务

```csharp
using CBus;
using CBus.Hosting;

var host = new CBusHost(
    new CBusHostOptions(3323, "cbus.pipe", CBusDefaults.DefaultPublishDirectory, CBusDefaults.DefaultRegistrySubKey),
    new WindowsRegistryStore(),
    new SystemFileStore());

host.RegisterService(
    new CBusServiceRegistration("FooService", "FooService.exe", [], [new CBusRouteDefinition("/Foo/Path1")]),
    static (_, _) => Task.FromResult(CBusResponse.Ok("handled")));

await host.StartAsync();
```

## 4. 在客户端发现 CBus

```csharp
using CBus;

var discovery = new CBusEndpointDiscovery(
    new WindowsRegistryStore(),
    new SystemFileStore());

var endpoints = await discovery.DiscoverAsync();
```

发现结果包含：

- `HttpEndpoint`
- `PipeAddress`

## 5. 使用 HTTP 连接器访问 CBus

```csharp
using CBus;

ICBusHttpTransport transport = GetTransport();
var connector = new HttpConnector(new Uri("http://127.0.0.1:3323/"), transport);
var response = await connector.SendAsync(new CBusRequest("GET", "/Foo/Path1"));
```

## 6. 使用 Pipe 连接器访问 CBus

```csharp
using CBus;

ICBusPipeTransport transport = GetTransport();
var connector = new PipeConnector("cbus.pipe", transport);
var response = await connector.SendAsync(new CBusRequest("GET", "/Foo/Path1"));
```

## 7. 推荐集成方式

### 7.1 面向客户端

- 启动时先调用 `CBusEndpointDiscovery`。
- 根据运行环境选择 `HttpConnector` 或 `PipeConnector`。
- 将真实网络或管道能力封装为 `ICBusHttpTransport` / `ICBusPipeTransport`。

### 7.2 面向宿主

- 使用 `CBusHost` 管理服务注册与地址发布。
- 通过 `CBusHttpListener` 与 `CBusPipeListener` 承载监听入口。
- 若后续接入真实 `HttpListener`、ASP.NET Core 或命名管道，只需在宿主项目内替换监听适配层。
