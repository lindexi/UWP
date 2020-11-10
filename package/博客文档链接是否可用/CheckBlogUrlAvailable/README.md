# 博客文档链接是否可用

尝试访问文档的每个连接，如果访问不上，就输出

## 安装

```
dotnet tool install -g Lindexi.Tool.CheckBlogUrlAvailable
```

## 使用

```
CheckBlogUrlAvailable [folder]
```

调用此命令，将会找到当前文件夹内所有的 md 文档，读取里面的内容，找到里面的连接，尝试访问链接

其中 folder 文件夹如不写则采用当前命令的工作文件夹
