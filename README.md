# ClaudeNotify

Claude Code 任务完成时的 Windows 桌面通知工具。

A Windows desktop notification tool for Claude Code task completion.

---

## 简介 / Introduction

**中文**

当 Claude Code 完成任务或需要你做决策时，自动弹出 Windows 系统通知。
- 如果你正在看终端且没有离开，则不打扰
- 如果你切换到其他窗口，或超过 18 秒没有操作，则弹出通知

**English**

Automatically sends a Windows toast notification when Claude Code finishes a task or needs your input.
- No notification if you're actively watching the terminal
- Notifies if you've switched away or been idle for more than 18 seconds

---

## 安装 / Installation

**中文**

1. 下载 [Releases](../../releases) 页面中的 `ClaudeNotifySetup.exe`
2. 运行它，自动完成所有配置

**English**

1. Download `ClaudeNotifySetup.exe` from the [Releases](../../releases) page
2. Run it — setup is fully automatic

---

## 通知类型 / Notification Types

| 触发时机 / Trigger | 通知内容 / Message | 音效 / Sound |
|---|---|---|
| 任务完成 / Task complete | Output Complete | Default |
| 需要决策 / Needs input | CHOICE! | Reminder |

---

## 系统要求 / Requirements

- Windows 10 / 11
- [Claude Code](https://claude.ai/code)
