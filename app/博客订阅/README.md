# MatterMost 博客RSS订阅服务

使用方法，用 docker 发布

```csharp
docker build .
```

不使用 docker 发布也可以使用 `dotnet run` 运行

然后 post 内容作为 json 写明需要订阅的博客和对应的 MatterMost 链接或企业微信链接

```csharp
{
    "MatterMostUrl": "http://mattermost.lindexi.com/hooks/sd7rhrij9ty98kerzdu8pfrbcy",
    "BlogList": 
    [
      "https://blog.lindexi.com/feed.xml", 
      "https://blog.lindexi.com/feed.xml",
      "http://feed.cnblogs.com/blog/u/148394/rss/", 
      "https://blog.walterlv.com/feed.xml", 
      "https://xinyuehtx.github.io/feed.xml", 
      "http://feed.cnblogs.com/blog/u/261865/rss/", 
      "http://feed.cnblogs.com/blog/u/481512/rss/", 
      "https://blog.sdlsj.net/feed/", 
      "http://feed.cnblogs.com/blog/u/113198/rss/", 
      "http://feed.cnblogs.com/blog/u/114368/rss/", 
      "http://feed.cnblogs.com/blog/u/138780/rss/", 
      "https://blog.ultrabluefire.cn/feed/", 
      "https://codedefault.com/rss/sn.xml", 
      "http://feed.cnblogs.com/blog/u/42514/rss/", 
      "https://www.itmangoto.cn/feed/", 
      "http://feed.cnblogs.com/blog/u/325447/rss/", 
      "http://feed.cnblogs.com/blog/u/552614/rss/", 
      "https://yangshunjie.com/rss.xml", 
      "http://feed.cnblogs.com/blog/u/127175/rss/"
    ]
}
```

请将 MatterMostUrl 替换为自己的 MatterMostUrl 链接或企业微信机器人链接，将 BlogList 替换为订阅的博客，注意 json 格式最后一项不带逗号

将内容推送到 `http://ip/api/rss` 请将 ip 替换为 docker 运行的 ip 地址

每次推送有重复内容也没关系，会自动去掉重复内容

注意：我用的是内存数据库，关闭容器将会丢失订阅配置，如需要存放到文件有两个方法

1. 修改 appsettings.json 中的 SqliteFilePath 替换为本机的路径

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  // 将下面代码替换为你需要的本地数据库地址，路径的数据库文件无需存在
  "SqliteFilePath": "blog.db" 
}
```

2. 如果是 docker 不方便更改内容，可以采用环境变量方法，添加环境变量 SqliteFilePath 指定存放路径方式，可以设置为挂载的路径

使用效果如下

![](http://image.acmx.xyz/lindexi%2F2020315176118202.jpg)

