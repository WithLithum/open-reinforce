#if GTA
#if LSPDFR
using LSPD_First_Response.Mod.API;
#else
using WithLithum.NativeWrapper;
#endif
using Rage;

namespace OpenReinforce.Utilities;

public static class EntityExtensions
{
    public static void Cleanup<T>(this T? entity, bool force)
        where T : class, IHandleable, IDeletable, IPersistable
    {
        if (!entity.Exists())
        {
            return;
        }

        if (force)
        {
            entity!.Delete();
        }
        else
        {
            entity!.Dismiss();
        }
    }

    public static void Cleanup<T>(this T? entity)
        where T : class, IHandleable, IDeletable
    {
        if (!entity.Exists())
        {
            return;
        }

        entity!.Delete();
    }
}
#endif