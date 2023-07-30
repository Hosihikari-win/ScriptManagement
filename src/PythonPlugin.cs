using Hosihikari.PluginManager;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace Hosihikari.ScriptManager;

public class PythonPlugin : Plugin
{
    private readonly ScriptEngine _scriptEngine;

    public PythonPlugin(FileInfo fileInfo) : base(fileInfo)
    {
        _scriptEngine = Python.CreateEngine();
    }

    protected override void Initialize()
    {
        // Nothing need to prepare
    }

    protected override void Load()
    {
        _scriptEngine.ExecuteFile(_fileInfo.FullName);
    }

    protected override void Unload()
    {
        _scriptEngine.Runtime.Shutdown();
    }
}
