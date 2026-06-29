// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using OpenReinforce.Utilities;
using Rage;
using System.Diagnostics.CodeAnalysis;
using WithLithum.NativeWrapper;

namespace OpenReinforce.Engine.Entities;

internal static class EntitySpawn
{
    internal static bool TryCreatePedInVehicle(Vehicle vehicle,
        int vehicleSeat,
        Model model,
        [MaybeNullWhen(false)] out Ped ped)
    {
        Checks.Exists(vehicle);

        model.LoadAndWait();
        if (!model.IsLoaded)
        {
            ped = null;
            return false;
        }

        var pedHandle = Natives.CreatePedInsideVehicle(vehicle.Handle,
            0,
            model.Hash,
            vehicleSeat,
            false,
            false);
        if (!Natives.DoesEntityExist(pedHandle))
        {
            ped = null;
            return false;
        }

        ped = World.GetEntityByHandle<Ped>(pedHandle);
        return true;
    }
}
