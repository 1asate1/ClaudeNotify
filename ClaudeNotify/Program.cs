using System.Diagnostics;
using System.Runtime.InteropServices;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

bool debugMode = args.Length > 0 && args[0].ToLower() == "debug";

// ── 空闲时间 ────────────────────────────────────────────────
var lastInput = new LASTINPUTINFO { cbSize = (uint)Marshal.SizeOf<LASTINPUTINFO>() };
Win32.GetLastInputInfo(ref lastInput);
uint idleSeconds = (Win32.GetTickCount() - lastInput.dwTime) / 1000;

// ── 前台窗口是不是终端进程 ──────────────────────────────────
var foregroundWindow = Win32.GetForegroundWindow();
Win32.GetWindowThreadProcessId(foregroundWindow, out uint foregroundPid);
bool isForegroundTerminal = IsTerminalProcess((int)foregroundPid);

if (debugMode)
{
    Console.WriteLine($"空闲时间:       {idleSeconds}s");
    Console.WriteLine($"前台窗口句柄:   {foregroundWindow}");
    Console.WriteLine($"前台进程PID:    {foregroundPid}");
    try {
        using var fp = Process.GetProcessById((int)foregroundPid);
        Console.WriteLine($"前台进程名:     {fp.ProcessName}");
    } catch { Console.WriteLine("前台进程名:     (无法获取)"); }
    Console.WriteLine($"是终端:         {isForegroundTerminal}");
}

// ── 判断是否通知 ────────────────────────────────────────────
string type = args.Length > 0 ? args[0].ToLower() : "stop";

bool shouldNotify;
if (type == "choice")
{
    // choice 需要用户决策 → 无条件通知
    shouldNotify = true;
    if (debugMode) Console.WriteLine("模式: choice → 无条件通知");
}
else if (!isForegroundTerminal)
{
    // stop + 非终端窗口 → 肯定没看 Claude → 通知
    shouldNotify = true;
    if (debugMode) Console.WriteLine("模式: 非终端窗口 → 通知");
}
else
{
    // stop + 终端窗口 → 用空闲时间判断
    shouldNotify = idleSeconds > 18;
    if (debugMode) Console.WriteLine($"模式: 终端窗口 + 空闲{idleSeconds}s → shouldNotify={shouldNotify}");
}

if (debugMode) { Console.ReadKey(); return; }
if (!shouldNotify) return;

// ── 弹出通知 ────────────────────────────────────────────────
string body  = type == "choice" ? "CHOICE!"        : "Output Complete";
string audio = type == "choice" ? "ms-winsoundevent:Notification.Reminder"
                                 : "ms-winsoundevent:Notification.Default";

string xmlContent = $"""
    <toast>
      <visual>
        <binding template="ToastGeneric">
          <text>Claude Code</text>
          <text>{body}</text>
        </binding>
      </visual>
      <audio src="{audio}"/>
    </toast>
    """;

var doc = new XmlDocument();
doc.LoadXml(xmlContent);
var toast = new ToastNotification(doc);
ToastNotificationManager
    .CreateToastNotifier("{1AC14E77-02E7-4E5D-B744-2EB1AE5198B7}\\WindowsPowerShell\\v1.0\\powershell.exe")
    .Show(toast);

// ── 本地函数 ────────────────────────────────────────────────
static bool IsTerminalProcess(int pid)
{
    var terminalNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "WindowsTerminal", "powershell", "pwsh", "cmd", "mintty", "conhost", "bash"
    };
    try
    {
        using var p = Process.GetProcessById(pid);
        return terminalNames.Contains(p.ProcessName);
    }
    catch { return false; }
}

// ── Win32 API 声明 ──────────────────────────────────────────
static class Win32
{
    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    [DllImport("user32.dll")]
    public static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

    [DllImport("kernel32.dll")]
    public static extern uint GetTickCount();
}

// ── 结构体 ──────────────────────────────────────────────────
[StructLayout(LayoutKind.Sequential)]
struct LASTINPUTINFO { public uint cbSize; public uint dwTime; }
