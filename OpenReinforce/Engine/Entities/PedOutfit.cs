// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using OpenReinforce.Engine.Data.Models.Outfits;
using OpenReinforce.Utilities;
using Rage;

namespace OpenReinforce.Engine.Entities;

internal sealed record PedOutfit
{
    public string? BaseVariant { get; init; }

    public bool Female { get; init; }

    public required IReadOnlyDictionary<ComponentSlot, DrawableVariation> Components { get; init; }

    public required IReadOnlyDictionary<PedPropAnchor, DrawableVariation> Props { get; init; }

    public void Apply(Ped ped)
    {
        foreach (var (slot, v) in Components)
        {
            v.Apply(ped, slot);
        }

        foreach (var (anchor, v) in Props)
        {
            v.Apply(ped, anchor, true);
        }
    }

    public static PedOutfit ConvertFrom(FrOutfitVariation variation,
        FrOutfitVariation? baseVariation = null)
    {
        Dictionary<ComponentSlot, DrawableVariation> componentMap = [];
        ApplyComponents(variation, componentMap);
        ApplyComponents(baseVariation, componentMap);

        Dictionary<PedPropAnchor, DrawableVariation> propMap = [];
        ApplyProps(variation, propMap);
        ApplyProps(baseVariation, propMap);

        return new PedOutfit
        {
            Components = componentMap ?? [],
            Props = propMap ?? [],
            BaseVariant = variation.Base,
            Female = variation.Gender == FrOutfitGender.Female
        };
    }

    private static void ApplyComponents(FrOutfitVariation? variation,
        Dictionary<ComponentSlot, DrawableVariation> map)
    {
        if (variation != null && variation.Components != null)
        {
            foreach (var component in variation.Components)
            {
                if (component.Slot is > Constants.PedComponentSlotMax or < 0)
                {
                    Log.WarnFormat($"Skipping out of bounds component slot {component.Slot} from variation {variation.Name ?? variation.ScriptName}");
                    continue;
                }

                var slot = (ComponentSlot)component.Slot;

                map[slot] = new DrawableVariation(component.Drawable, component.Texture);
            }
        }
    }

    private static void ApplyProps(FrOutfitVariation? variation,
        Dictionary<PedPropAnchor, DrawableVariation> map)
    {
        if (variation != null && variation.Props != null)
        {
            foreach (var prop in variation.Props)
            {
                if (prop.Slot is > Constants.PedPropSlotMax or < 0)
                {
                    Log.WarnFormat($"Skipping out of bounds prop slot {prop.Slot} from variation {variation.Name ?? variation.ScriptName}");
                    continue;
                }

                var slot = (PedPropAnchor)prop.Slot;

                map[slot] = new DrawableVariation(prop.Drawable, prop.Texture);
            }
        }
    }
}
