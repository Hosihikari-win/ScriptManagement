using Hosihikari.PluginManager;
using Neo.IronLua;

namespace Hosihikari.ScriptManager;

public class LuaPlugin : Plugin
{
    private LuaChunk? _luaChunk;

    public LuaPlugin(FileInfo fileInfo) : base(fileInfo)
    {
        // Nothing need to prepare
    }

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
        _luaChunk.Run(new());
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
