// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using OpenReinforce.Engine.Data.Models;
using OpenReinforce.Utilities;
using Rage;
using WithLithum.NativeWrapper;

namespace OpenReinforce.Engine.Entities;

internal readonly record struct PedComponentInfo
{
    public required ComponentSlot Slot { get; init; }

    public required int Drawable { get; init; }

    public required int Texture { get; init; }

    public void Apply(Ped ped)
    {
        Checks.Exists(ped);
        Natives.SetPedComponentVariation(ped.Handle,
            (int)Slot,
            Drawable,
            Texture,
            0);
    }

    public static PedComponentInfo ConvertFrom(PedVariation variation)
    {
        if (variation.Slot is < 0 or > Constants.PedComponentSlotMax)
        {
            throw new ArgumentException("Invalid component slot.",
                nameof(variation));
        }

        return new PedComponentInfo
        {
            Slot = (ComponentSlot)variation.Slot,
            Drawable = variation.Drawable,
            Texture = variation.Texture,
        };
    }
}
