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

    public required IReadOnlyList<PedComponentInfo> Components { get; init; }

    public required IReadOnlyList<PedPropInfo> Props { get; init; }

    public void Apply(Ped ped)
    {
        foreach (var component in Components)
        {
#if DEBUG
            Log.Info($"{component.Slot}: component model #{component.Drawable} | tex #{component.Texture}");
#endif
            component.Apply(ped);
        }

        foreach (var prop in Props)
        {
#if DEBUG
            Log.Info($"{prop.Anchor}: prop model #{prop.Drawable} | tex #{prop.Texture}");
#endif

            prop.Apply(ped);
        }
    }

    public static PedOutfit ConvertFrom(FrOutfitVariation variation)
    {
        List<PedComponentInfo> componentList = [];
        ApplyComponents(variation, componentList);

        List<PedPropInfo> propList = [];
        ApplyProps(variation, propList);

        return new PedOutfit
        {
            Components = componentList ?? [],
            Props = propList ?? [],
            BaseVariant = variation.Base,
            Female = variation.Gender == FrOutfitGender.Female
        };
    }

    private static void ApplyComponents(FrOutfitVariation variation, List<PedComponentInfo> componentList)
    {
        if (variation.Components != null)
        {
            componentList.Capacity = variation.Components.Length;

            foreach (var component in variation.Components)
            {
                componentList.Add(PedComponentInfo.ConvertFrom(component));
            }
        }
    }

    private static void ApplyProps(FrOutfitVariation variation, List<PedPropInfo> propList)
    {
        if (variation.Props != null)
        {
            propList.Capacity = variation.Props.Length;
            foreach (var component in variation.Props)
            {
                propList.Add(PedPropInfo.ConvertFrom(component));
            }
        }
    }
}
