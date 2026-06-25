using Rage;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace OpenReinforce.Utilities;

public static class Checks
{
    public static void Exists([NotNull] IHandleable? handleable,
        [CallerArgumentExpression(nameof(handleable))] string? argumentName = null)
    {
        if (handleable == null || !handleable.IsValid())
        {
            ThrowNotExists(argumentName);
        }
    }

    [DoesNotReturn]
    private static void ThrowNotExists(string? argumentName)
    {
        throw new ArgumentException("The specified entity is invalid.",
            argumentName);
    }

    public static void NotInjured([NotNull] Ped? ped,
        [CallerArgumentExpression(nameof(ped))] string? argumentName = null)
    {
        if (ped.IsInjured())
        {
            ThrowInjured(argumentName);
        }
    }

    [DoesNotReturn]
    private static void ThrowInjured(string? argumentName)
    {
        throw new ArgumentException("An invalid, dead or injured ped is not allowed.",
            argumentName);
    }
}
