#if GTA
#if LSPDFR
using LSPD_First_Response.Mod.API;
#endif
using Rage;
using System.Diagnostics.CodeAnalysis;
using WithLithum.NativeWrapper;

namespace OpenReinforce.Utilities;

public static class PedExtensions
{
#if !LSPDFR
    private static int NumberOfMaleHairComponents = -1;
    private static int NumberOfFemaleHairComponents = -1;
    private const int VanillaParentsCount = 45;
#endif

    public const uint FreemodeMaleHash = 0x705E61F2;
    public const uint FreemodeFemaleHash = 0x9C9EFFD8;

    /// <summary>
    /// Determines whether the specified ped is valid and can be given tasks.
    /// </summary>
    /// <param name="ped">
    /// The ped to check.
    /// </param>
    /// <returns>
    /// <para><see langword="true"/> if all of the following are met:</para>
    /// <list type="bullet">
    /// <item>The ped is not <see langword="null"/>.</item>
    /// <item>
    /// The ped is valid (<see cref="IHandleable.IsValid"/> returns <see langword="true"/>).
    /// </item>
    /// <item>The ped is not dead.</item>
    /// <item>The ped's health is above <see cref="Ped.InjuryHealthThreshold"/>.</item>
    /// </list>
    /// </returns>
    public static bool IsInjured([NotNullWhen(false)] this Ped? ped)
    {
        return ped == null
            || Natives.IsPedInjured(ped.Handle);
    }

    public static bool IsSittingInVehicle(this Ped ped, Vehicle vehicle)
    {
        Checks.Exists(ped);
        Checks.Exists(vehicle);

        return Natives.IsPedSittingInVehicle(ped.Handle, vehicle.Handle);
    }

    public static bool IsOccupied(this Ped ped)
    {
        return
#if LSPDFR
            Functions.IsPedInPursuit(ped) ||
#endif
               ped.IsAiming
            || ped.IsInCover
            || ped.IsInMeleeCombat
            || ped.IsInCombat
            || ped.IsShooting;
    }


#if !LSPDFR
    private static void PopulateVariables()
    {
        if (NumberOfMaleHairComponents == -1
            || NumberOfFemaleHairComponents == -1)
        {
            NumberOfMaleHairComponents = Natives.GetNumberOfPedDrawableVariations(FreemodeMaleHash,
                (int)ComponentSlot.Hair);
            NumberOfFemaleHairComponents = Natives.GetNumberOfPedDrawableVariations(FreemodeFemaleHash,
                (int)ComponentSlot.Hair);
        }
    }
#endif

    public static void RandomizeMpAppearance(this Ped ped)
    {
        var hash = ped.Model.Hash;
        if (hash != FreemodeMaleHash && hash != FreemodeFemaleHash)
        {
            throw new ArgumentException("The specified ped is not a freemode male nor freemode female.");
        }

#if LSPDFR
        Functions.RandomizeMultiplayerModelAppearance(ped);
#else
        PopulateVariables();
        Natives.SetPedHeadBlendData(ped.Handle,
            MathHelper.GetRandomInteger(VanillaParentsCount),
            MathHelper.GetRandomInteger(VanillaParentsCount),
            MathHelper.GetRandomInteger(VanillaParentsCount),
            MathHelper.GetRandomInteger(VanillaParentsCount),
            MathHelper.GetRandomInteger(VanillaParentsCount),
            MathHelper.GetRandomInteger(VanillaParentsCount),
            MathHelper.GetRandomSingle(0f, 1f),
            MathHelper.GetRandomSingle(0f, 1f),
            MathHelper.GetRandomSingle(0f, 1f),
            false);
        Natives.SetPedComponentVariation(ped.Handle, (int)ComponentSlot.Hair,
            MathHelper.GetRandomInteger(ped.IsMale
                ? NumberOfMaleHairComponents
                : NumberOfFemaleHairComponents),
            0,
            0);
        Natives.SetPedHeadOverlay(ped.Handle,
            2, MathHelper.GetRandomInteger(33), 1f);
        Natives.SetPedHeadOverlayTint(ped.Handle, 2, 1, 1, 1);
#endif
    }
}
#endif