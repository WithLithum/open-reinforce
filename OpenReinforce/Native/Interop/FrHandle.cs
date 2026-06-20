#if GTA
#if LSPDFR
using LSPD_First_Response.Mod.API;
#endif

namespace OpenReinforce.Native.Interop;

internal readonly struct FrHandle
{
#if LSPDFR
    private readonly LHandle? _inner;
#else
    private readonly object? _inner;
#endif

#if LSPDFR
    internal FrHandle(LHandle? inner)
#else
    internal FrHandle(object? inner)
#endif
    {
        _inner = inner;
    }

#if LSPDFR
    internal LHandle? Inner => _inner;
#else
    internal object? Inner => _inner;
#endif

    public bool IsNull => Inner == null;
}
#endif