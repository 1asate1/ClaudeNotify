# ClaudeNotify

[![Windows](https://img.shields.io/badge/platform-Windows-blue)]()
[![.NET](https://img.shields.io/badge/.NET-9.0-self--contained-purple)]()
[![Downloads](https://img.shields.io/github/downloads/你的用户名/ClaudeNotify/total)]()

Claude Code 任务完成时的 Windows 桌面通知工具。  
A Windows desktop notification tool for Claude Code task completion.

---

## 简介 / Introduction

**中文**  
当 Claude Code 完成任务或需要你做决策时，自动弹出 Windows 系统通知。
-  如果你正在看终端且没有离开 → 不打扰
-  如果你切换到其他窗口 → 弹出通知
-  如果你超过 18 秒没有操作 → 弹出通知

**English**  
Automatically sends a Windows toast notification when Claude Code finishes a task or needs your input.
-  No notification if you're actively watching the terminal
-  Notifies if you've switched away
-  Notifies if you've been idle for more than 18 seconds

---

## 安装 / Installation

**中文**  
1. 下载 [Releases](../../releases) 页面中的 `ClaudeNotifySetup.exe`
2. 运行它，自动完成所有配置  
   **不需要安装 .NET 运行时**（已全部打包）

**English**  
1. Download `ClaudeNotifySetup.exe` from the [Releases](../../releases) page
2. Run it — setup is fully automatic  
   **No .NET runtime required** (self-contained)

---

## 通知类型 / Notification Types

| 触发时机 / Trigger | 通知内容 / Message | 场景 / Scenario |
|---|---|---|
| 任务完成 / Task complete | Output Complete | Claude 代码执行完毕 |
| 需要决策 / Needs input | CHOICE! | Claude 等待你选择操作 |

---

## 手动测试 / Manual Test

安装后，可以直接运行主程序查看当前状态：

```bash
"C:\Users\%USERNAME%\.claude\ClaudeNotify.exe" debug
```

会显示当前空闲时间和窗口焦点状态。

---

## 工作原理 / How It Works

1. Claude Code 完成任务时触发 `Stop` hook
2. 程序检查当前窗口焦点和用户空闲时间
3. 根据规则决定是否弹出通知

所有逻辑都在一个独立的 exe 中，不影响 Claude Code 本身。

---

## 系统要求 / Requirements

- Windows 10 / 11
- [Claude Code](https://claude.ai/code)
- **不需要安装 .NET 运行时**（已内置）

---

## 构建 / Build

如果你想自己编译：

```bash
dotnet publish -c Release -r win-x64 --self-contained true
```

---

## 许可证 / License

MIT