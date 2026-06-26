#if LSPDFR
using LSPD_First_Response.Mod.API;
#endif

namespace OpenReinforce.Native.Interop;

internal readonly struct FrHandle : IEquatable<FrHandle>
{
    private readonly object? _inner;

    internal FrHandle(object? inner)
    {
#if LSPDFR
        if (inner != null && inner is not LHandle)
        {
            throw new ArgumentException("The specified handle is not LHandle.");
        }
#endif

        _inner = inner;
    }

    internal object? Inner => _inner;

    public bool IsNull => Inner == null;

    public static FrHandle Null => new(null);

    public bool Equals(FrHandle other)
    {
        return _inner == other._inner;
    }

    public override bool Equals(object? obj)
    {
        return obj is FrHandle other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _inner?.GetHashCode() ?? 0;
    }
}
