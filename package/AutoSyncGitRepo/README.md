# AutoSyncGitRepo

自动调用 git fetch --all 命令同步文件夹内所有 git 仓库

## 安装

```
dotnet tool install -g Lindexi.Tool.AutoSyncGitRepo
```

## 使用

```
AutoSyncGitRepo [folder]
```

调用此命令，将会找到当前文件夹内所有的 git 仓库，进行更新

其中 folder 文件夹如不写则采用当前命令的工作文件夹