# Copilot Instructions

## Project Guidelines
- CBus 需要拆分为两个核心项目：一个面向客户端/服务安装的 NuGet 包，负责 HTTP/Pipe 连接与 CBus 发现；另一个为可独立运行的宿主项目，负责 HTTP/Pipe 监听并将端口或 Pipe 地址写入注册表和文件系统。
