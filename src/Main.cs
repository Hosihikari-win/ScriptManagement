using Hosihikari.PluginManagement;
using Hosihikari.ScriptManagement;
using System.Text.Json;

[assembly: EntryPoint<Main>]

namespace Hosihikari.ScriptManagement;

internal class Main : IEntryPoint
{
    internal static readonly List<Plugin> s_plugins;

    static Main()
    {
        s_plugins = [];
    }

    public void Initialize(AssemblyPlugin _plugin)
    {
        DirectoryInfo directoryInfo = new("plugins");
        foreach (FileInfo file in directoryInfo.EnumerateFiles("*.py"))
        {
            PythonPlugin plugin = new(file);
            Manager.Load(plugin);
            s_plugins.Add(plugin);
        }

        foreach (FileInfo file in directoryInfo.EnumerateFiles("*.lua"))
        {
            LuaPlugin plugin = new(file);
            Manager.Load(plugin);
            s_plugins.Add(plugin);
        }

        string nodePackagesPath = Path.Combine("plugins", "package.json");
        if (File.Exists(nodePackagesPath))
        {
            string nodePackagesText = File.ReadAllText(nodePackagesPath);
            Dictionary<string, object> nodePackages =
                JsonSerializer.Deserialize<Dictionary<string, object>>(nodePackagesText)!;
            if (nodePackages.TryGetValue("dependencies", out object? value))
            {
                foreach ((string name, _) in (value as Dictionary<string, string>)!)
                {
                    string nodePluginPath = Path.Combine("plugins", "node_modules", name);
                    FileInfo nodePluginFileInfo = new(nodePluginPath);
                    NodePlugin plugin = new(nodePluginFileInfo);
                    Manager.Load(plugin);
                    s_plugins.Add(plugin);
                }
            }
        }

        foreach (Plugin plugin in s_plugins)
        {
            if (string.IsNullOrWhiteSpace(plugin.Name))
            {
                throw new NullReferenceException();
            }

            Manager.Initialize(plugin.Name);
        }
    }
}
