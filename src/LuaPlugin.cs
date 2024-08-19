using Hosihikari.PluginManagement;
using Neo.IronLua;

namespace Hosihikari.ScriptManagement;

public class LuaPlugin(FileInfo fileInfo) : Plugin(fileInfo)
{
    private LuaChunk? _luaChunk;

    protected override void Initialize()
    {
        using Lua lua = new();
        _luaChunk = lua.CompileChunk(_fileInfo.FullName, new());
    }

    protected override void Load()
    {
        if (_luaChunk is null)
        {
            throw new NullReferenceException();
        }
        _luaChunk.Run([]);
    }

    protected override void Unload()
    {
        if (_luaChunk is null)
        {
            throw new NullReferenceException();
        }
        _luaChunk.Lua.Clear();
    }
}
