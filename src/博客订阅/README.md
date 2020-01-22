# MatterMost 博客RSS订阅服务

使用方法，用 docker 发布

```csharp
docker build .
```

不使用 docker 发布也可以使用 `dotnet run` 运行

然后 post 内容作为 json 写明需要订阅的博客和对应的 MatterMost 链接

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

将内容推送到 `http://ip/api/rss` 请将 ip 替换为 docker 运行的 ip 地址

多次推送内容会添加订阅博客

注意：我用的是内存数据库，关闭容器将会丢失订阅配置
