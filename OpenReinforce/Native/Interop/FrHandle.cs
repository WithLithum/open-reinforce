using LSPD_First_Response.Mod.API;

namespace OpenReinforce.Native.Interop;

internal readonly struct FrHandle : IEquatable<FrHandle>
{
    private readonly object? _inner;

    internal FrHandle(object? inner)
    {
        if (inner != null && inner is not LHandle)
        {
            throw new ArgumentException("The specified handle is not LHandle.");
        }

        _inner = inner;
    }

    internal object? Inner => _inner;

    public bool IsNull => Inner == null;

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
