using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
var claudeDir = Path.Combine(home, ".claude");
Directory.CreateDirectory(claudeDir);

// ── 释放内嵌的 ClaudeNotify.exe ────────────────────────────
var exePath = Path.Combine(claudeDir, "ClaudeNotify.exe");
using (var stream = Assembly.GetExecutingAssembly()
           .GetManifestResourceStream("ClaudeNotify.exe")!)
using (var file = File.Create(exePath))
{
    stream.CopyTo(file);
}
Console.WriteLine($"已创建: {exePath}");

// ── 写入 settings.json ──────────────────────────────────────
var settingsPath = Path.Combine(claudeDir, "settings.json");

JsonObject root;
if (File.Exists(settingsPath))
{
    try { root = JsonNode.Parse(File.ReadAllText(settingsPath))?.AsObject() ?? new JsonObject(); }
    catch { root = new JsonObject(); }
}
else
{
    root = new JsonObject();
}

if (!root.ContainsKey("hooks"))
    root["hooks"] = new JsonObject();

var hooks = root["hooks"]!.AsObject();

static JsonObject MakeHook(string command) => new JsonObject
{
    ["matcher"] = "",
    ["hooks"] = new JsonArray(new JsonObject
    {
        ["type"] = "command",
        ["command"] = command
    })
};

hooks["Stop"]         = new JsonArray(MakeHook("\"$HOME/.claude/ClaudeNotify.exe\" stop"));
hooks["Notification"] = new JsonArray(MakeHook("\"$HOME/.claude/ClaudeNotify.exe\" choice"));

var json = root.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
File.WriteAllText(settingsPath, json, new UTF8Encoding(false));
Console.WriteLine($"已更新: {settingsPath}");

Console.WriteLine("\n设置完成！按任意键退出。");
Console.ReadKey();
