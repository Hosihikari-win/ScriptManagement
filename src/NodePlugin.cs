using Hosihikari.PluginManagement;
using Microsoft.JavaScript.NodeApi;
using Microsoft.JavaScript.NodeApi.Runtime;
using System.Text.Json;

namespace Hosihikari.ScriptManagement;

public class NodePlugin : Plugin
{
    private readonly NodejsPlatform _nodejsPlatform;

    public NodePlugin(FileInfo fileInfo) : base(fileInfo)
    {
        if (OperatingSystem.IsWindows())
        {
            _nodejsPlatform = new("libnode.dll");
            return;
        }

        if (OperatingSystem.IsMacOS())
        {
            _nodejsPlatform = new("libnode.dylib");
            return;
        }

        _nodejsPlatform = new("libnode");
    }

    protected override void Initialize()
    {
        string packagePath = Path.Combine(_fileInfo.FullName, "package.json");
        if (!File.Exists(packagePath))
        {
            throw new FileNotFoundException();
        }

        string packageString = File.ReadAllText(packagePath);
        Dictionary<string, object> package = JsonSerializer.Deserialize<Dictionary<string, object>>(packageString)!;
        if (package.TryGetValue("name", out object? name))
        {
            Name = (name as string)!;
        }

        if (package.TryGetValue("version", out object? version))
        {
            Version = Version.Parse((version as string)!);
        }
    }

    protected override void Load()
    {
        NodejsEnvironment env = _nodejsPlatform.CreateEnvironment();
        env.Run(() =>
        {
            JSValue main = env.Import(Name);
            // TODO
        });
    }

    protected override void Unload()
    {
        _nodejsPlatform.Dispose();
    }
}
