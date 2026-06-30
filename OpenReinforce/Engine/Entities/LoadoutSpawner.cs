// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using OpenReinforce.Engine.Configuration;
using OpenReinforce.Engine.Configuration.Outfits;
using OpenReinforce.Engine.Data.Models.Agencies;
using OpenReinforce.Native.Interop;
using OpenReinforce.Utilities;
using Rage;
using System.Diagnostics.CodeAnalysis;
using WithLithum.NativeWrapper;

namespace OpenReinforce.Engine.Entities;

internal static class LoadoutSpawner
{
    internal static bool SpawnLoadout(LoadoutInfo loadout,
        Vector3 position,
        float heading,
        [NotNullWhen(true)] out Vehicle? vehicle,
        [NotNullWhen(true)] out Ped? driver,
        IList<SpawnedPedInfo> storePedsIn,
        int numPeds = 0)
    {
        var vehicleInfo = ItemSelector.PickByChance(loadout.Vehicles);

        vehicle = new Vehicle(vehicleInfo.ModelHash, position, heading);
        if (!vehicle.Exists())
        {
            driver = null;
            return false;
        }
        vehicleInfo.Apply(vehicle);

        if (numPeds == 0)
        {
            numPeds = MathHelper.GetRandomInteger(loadout.MaximumPeds,
                loadout.MaximumPeds);
        }

        var maxSeats = Natives.GetVehicleMaxNumberOfPassengers(vehicle.Handle)
             - 1;
        var seatIndex = -1;
        Ped? driverBuf = null;

        for (int i = 0; i < numPeds; i++)
        {
            if (seatIndex > maxSeats)
            {
                Log.Warn("NumPeds greater than vehicle seats! Not spawning more.");
                break;
            }

            var ped = CreatePed(loadout, vehicle, seatIndex);
            if (ped == null)
            {
                continue;
            }

            if (seatIndex == -1)
            {
                driverBuf = ped;
                driverBuf.KeepTasks = true;
            }

            storePedsIn.Add(new SpawnedPedInfo(ped, seatIndex));

            seatIndex++;
        }

        if (driverBuf == null)
        {
            driver = null;
            return false;
        }

        driver = driverBuf;
        return true;
    }

    private static Ped? CreatePed(LoadoutInfo loadout,
        Vehicle vehicle,
        int seatIndex)
    {
        var pedLoadout = ItemSelector.PickByChance(loadout.Peds);
        var female = PedExtensions.IsFemale(pedLoadout.ModelHash);
        PedOutfit? outfit = pedLoadout.Outfit == null
            ? null
            : OpenReinforcePlugin.OutfitManager.PickByRef(pedLoadout.Outfit,
            female);

        if (!EntitySpawn.TryCreatePedInVehicle(vehicle,
            seatIndex,
            pedLoadout.ModelHash,
            out var ped)
            || !ped.IsValid())
        {
            Log.Warn($"Failed to spawn ped on seat {seatIndex}");
            return null;
        }

        ped.BlockPermanentEvents = true;

        outfit?.Apply(ped);
        pedLoadout.OverrideOutfit?.Apply(ped);

        if (ped.IsFreemodePed())
        {
            ped.RandomizeMpAppearance();
        }

        if (!OpenReinforcePlugin.IsTestPlugin)
        {
            FrFunctions.SetPedAsCop(ped);
        }
        else
        {
            ped.RelationshipGroup = RelationshipGroup.Cop;
        }

        // TODO properly assign configured weapon!
        if (seatIndex == -1)
        {
            ped.Inventory.GiveNewWeapon(WeaponHash.CombatPistol, short.MaxValue, true);
        }
        else
        {
            ped.Inventory.GiveNewWeapon(WeaponHash.PumpShotgun, short.MaxValue, true);
        }

        return ped;
    }
}
