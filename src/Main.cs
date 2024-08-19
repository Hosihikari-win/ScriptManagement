using Hosihikari.PluginManagement;
using Hosihikari.ScriptManagement;

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
