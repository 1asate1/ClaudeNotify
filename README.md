# ClaudeNotify

[![Windows](https://img.shields.io/badge/platform-Windows%2010%2F11-blue)]()
[![.NET](https://img.shields.io/badge/.NET-9.0%20NativeAOT-purple)]()
[![Downloads](https://img.shields.io/github/downloads/1asate1/ClaudeNotify/total)]()

Claude Code 任务完成时的 Windows 桌面通知工具。  
A Windows desktop notification tool for Claude Code.

---

## 简介 / Introduction

**中文**  
当 Claude Code 完成任务或需要你做决策时，自动弹出 Windows 系统通知，带有 Claude 图标和提示音。即使你切换到了其他窗口，也不会错过任何消息。

**English**  
Automatically sends a Windows toast notification when Claude Code finishes a task or needs your input. Features the Claude icon and distinct sounds so you never miss a message, even when working in another window.

---

## 通知类型 / Notification Types

| 触发时机 / Trigger | 通知内容 / Message | 提示音 / Sound |
|---|---|---|
| Claude 输出完毕 / Output complete | Output Complete | 默认提示音 / Default |
| 需要你做决策 / Needs your input | CHOICE! | 提醒音 / Reminder |

---

## 安装 / Installation

**中文**  
1. 前往 [Releases](../../releases) 页面下载 `ClaudeNotifySetup.exe`
2. 双击运行，自动完成所有配置
3. 无需安装 .NET 运行时

**English**  
1. Download `ClaudeNotifySetup.exe` from the [Releases](../../releases) page
2. Double-click to run — setup is fully automatic
3. No .NET runtime required

---

## 更新 / Update

**中文**  
下载新版 `ClaudeNotifySetup.exe` 重新运行一次即可，会自动覆盖旧版本。

**English**  
Download the new `ClaudeNotifySetup.exe` and run it again. It will automatically replace the old version.

---

## 系统要求 / Requirements

- Windows 10 / 11
- [Claude Code](https://claude.ai/code)

---

## 工作原理 / How It Works

Setup 安装时会做两件事：

1. 将 `ClaudeNotify.exe` 释放到 `%USERPROFILE%\.claude\`
2. 在 `%USERPROFILE%\.claude\settings.json` 中写入以下 hooks：

```json
{
  "hooks": {
    "Stop": "ClaudeNotify.exe stop",
    "Notification (permission_prompt)": "ClaudeNotify.exe choice",
    "Notification (elicitation_dialog)": "ClaudeNotify.exe choice"
  }
}
```

每次 hook 触发时，`ClaudeNotify.exe` 启动、弹出通知、立即退出，不常驻后台。

---

## 构建 / Build

需要 Visual Studio 2022（含 C++ 桌面开发工具）。

```bash
dotnet publish ClaudeNotify -c Release -o dist
dotnet publish ClaudeNotifySetup -c Release -o dist
```

---

## 许可证 / License

MIT
