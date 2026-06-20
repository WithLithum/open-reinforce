#if GTA
using Rage;
using WithLithum.NativeWrapper;

namespace OpenReinforce.Utilities;

public static class EntityExtensions
{
    public static void SetColour(this Blip blip, BlipColour colour)
    {
        Checks.Exists(blip);
        Natives.SetBlipColour(blip.Handle, (int)colour);
    }

    public static void SetLocalizedName(this Blip blip, string key)
    {
        Checks.Exists(blip);
        Natives.BeginTextCommandSetBlipName(key);
        Natives.EndTextCommandSetBlipName(blip.Handle);
    }

    public static void SetLocalizedName(this Blip blip, uint keyHash)
    {
        Checks.Exists(blip);
        Natives.BeginTextCommandSetBlipName("STRING");
        Natives.AddTextComponentSubstringTextLabelHashKey(keyHash);
        Natives.EndTextCommandSetBlipName(blip.Handle);
    }

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