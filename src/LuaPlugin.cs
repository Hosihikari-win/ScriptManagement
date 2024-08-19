using Hosihikari.PluginManagement;
using Neo.IronLua;

namespace Hosihikari.ScriptManagement;

public class LuaPlugin(FileInfo fileInfo) : Plugin(fileInfo)
{
    private readonly Lua _lua = new();

    protected override void Initialize()
    {
        // Nothing need to prepare
    }

    protected override void Load()
    {
        LuaChunk luaChunk = _lua.CompileChunk(_fileInfo.FullName, new());
        luaChunk.Run([]);
    }

    protected override void Unload()
    {
        _lua.Dispose();
    }
}
