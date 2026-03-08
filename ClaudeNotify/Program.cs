using Microsoft.Win32;
using System.Reflection;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

const string AppId = "Anthropic.ClaudeNotify";

// ── 注册 App ID ──────────────────────────────────────────────
using (var key = Registry.CurrentUser.CreateSubKey(@"Software\Classes\AppUserModelId\" + AppId))
    key.SetValue("DisplayName", "Claude Code");

bool debugMode = args.Length > 0 && args[0].ToLower() == "debug";

// ── 判断是否通知 ────────────────────────────────────────────
string type = args.Length > 0 ? args[0].ToLower() : "stop";

if (debugMode) { Console.WriteLine($"模式: {type} → 无条件通知"); Console.ReadKey(); return; }

// ── 提取嵌入图标到临时目录 ───────────────────────────────────
string iconUri = "";
try
{
    string tempDir = Path.Combine(Path.GetTempPath(), "ClaudeNotify");
    Directory.CreateDirectory(tempDir);
    string iconPath = Path.Combine(tempDir, "claudeLOGOPHOTO.png");

    using var stream = Assembly.GetExecutingAssembly()
        .GetManifestResourceStream("ClaudeNotify.claudeLOGOPHOTO.png");
    if (stream != null)
    {
        using var fs = File.Create(iconPath);
        stream.CopyTo(fs);
        iconUri = "file:///" + iconPath.Replace('\\', '/');
    }
}
catch { }

// ── 弹出通知 ────────────────────────────────────────────────
string body  = type == "choice" ? "CHOICE!"        : "Output Complete";
string audio = type == "choice" ? "ms-winsoundevent:Notification.Reminder"
                                 : "ms-winsoundevent:Notification.Default";
string logoXml = iconUri != "" ? $"""<image placement="appLogoOverride" src="{iconUri}" hint-crop="circle"/>""" : "";

string xmlContent = $"""
    <toast>
      <visual>
        <binding template="ToastGeneric">
          <text>Claude Code</text>
          <text>{body}</text>
          {logoXml}
        </binding>
      </visual>
      <audio src="{audio}"/>
    </toast>
    """;

var doc = new XmlDocument();
doc.LoadXml(xmlContent);
var toast = new ToastNotification(doc);
ToastNotificationManager
    .CreateToastNotifier(AppId)
    .Show(toast);
