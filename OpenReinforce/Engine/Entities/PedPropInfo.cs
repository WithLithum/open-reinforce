// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using OpenReinforce.Engine.Data.Models;
using OpenReinforce.Utilities;
using Rage;
using WithLithum.NativeWrapper;

namespace OpenReinforce.Engine.Entities;

internal readonly record struct PedPropInfo
{
    public required PedPropAnchor Anchor { get; init; }

    public required int Drawable { get; init; }

    public required int Texture { get; init; }

    public void Apply(Ped ped)
    {
        Checks.Exists(ped);
        Natives.SetPedPropIndex(ped.Handle,
            (int)Anchor,
            Drawable,
            Texture,
            true,
            0);
    }

    public static PedPropInfo ConvertFrom(PedVariation variation)
    {
        if (variation.Slot is < 0 or > Constants.PedPropSlotMax)
        {
            throw new ArgumentException("Invalid component slot.",
                nameof(variation));
        }

        return new PedPropInfo
        {
            Anchor = (PedPropAnchor)variation.Slot,
            Drawable = variation.Drawable,
            Texture = variation.Texture,
        };
    }
}
