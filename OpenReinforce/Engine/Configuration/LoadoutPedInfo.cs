// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using OpenReinforce.Engine.Configuration.Outfits;
using OpenReinforce.Engine.Data.Models.Agencies;
using OpenReinforce.Engine.Entities;
using OpenReinforce.Utilities;
using OpenReinforce.Utilities.Collections;
using System.Diagnostics.CodeAnalysis;

namespace OpenReinforce.Engine.Configuration;

internal sealed record LoadoutPedInfo : IChanced
{
    public required int Chance { get; init; }

    public required uint ModelHash { get; init; }

    public OutfitReference? Outfit { get; init; }

    public PedOutfit? OverrideOutfit { get; init; }

    public static bool TryConvert(FrLoadoutPed fr,
        [MaybeNullWhen(false)] out LoadoutPedInfo result)
    {
        if (string.IsNullOrWhiteSpace(fr.Model))
        {
            result = null;
            return false;
        }

        OutfitReference? outfitRef = null;
        if (!string.IsNullOrWhiteSpace(fr.Outfit))
        {
            _ = OutfitReference.TryParse(fr.Outfit!, out outfitRef);
        }

        var outfit = EvaluateOutfit(fr);
        result = new LoadoutPedInfo
        {
            Chance = fr.Chance,
            ModelHash = Hasher.Joaat(fr.Model!),
            OverrideOutfit = outfit,
            Outfit = outfitRef
        };
        return true;
    }

    private static PedOutfit EvaluateOutfit(FrLoadoutPed fr)
    {
        List<PedComponentInfo>? components = null;
        List<PedPropInfo>? props = null;

        void EvaluateSlot(ComponentSlot slot, int drawable, int texture)
        {
            if (drawable != -999)
            {
                components ??= [];
                components.Add(new PedComponentInfo()
                {
                    Drawable = drawable,
                    Texture = texture == -999 ? 0 : texture,
                    Slot = slot
                });
            }
        }

        void EvaluateProp(PedPropAnchor slot, int drawable, int texture)
        {
            if (drawable != -999)
            {
                props ??= [];
                props.Add(new PedPropInfo()
                {
                    Drawable = drawable,
                    Texture = texture == -999 ? 0 : texture,
                    Anchor = slot
                });
            }
        }

        EvaluateSlot(ComponentSlot.Head, fr.ComponentHead, fr.TextureHead);
        EvaluateSlot(ComponentSlot.Beard, fr.ComponentBeard, fr.TextureBeard);
        EvaluateSlot(ComponentSlot.Hair, fr.ComponentHair, fr.TextureHair);
        EvaluateSlot(ComponentSlot.Upper, fr.ComponentUpper, fr.TextureUpper);
        EvaluateSlot(ComponentSlot.Lower, fr.ComponentLower, fr.TextureLower);
        EvaluateSlot(ComponentSlot.Hand, fr.ComponentHand, fr.TextureHand);
        EvaluateSlot(ComponentSlot.Feet, fr.ComponentFeet, fr.TextureFeet);
        EvaluateSlot(ComponentSlot.Teeth, fr.ComponentTeeth, fr.TextureTeeth);
        EvaluateSlot(ComponentSlot.Accessories, fr.ComponentAccessories, fr.TextureAccessories);
        EvaluateSlot(ComponentSlot.Task, fr.ComponentTask, fr.TextureTask);
        EvaluateSlot(ComponentSlot.Decal, fr.ComponentDecal, fr.TextureDecal);
        EvaluateSlot(ComponentSlot.Jbib, fr.ComponentJbib, fr.TextureJbib);

        EvaluateProp(PedPropAnchor.Head, fr.PropHead, fr.TexturePropHead);
        EvaluateProp(PedPropAnchor.Eyes, fr.PropEyes, fr.TexturePropEyes);
        EvaluateProp(PedPropAnchor.Ears, fr.PropEars, fr.TexturePropEars);
        EvaluateProp(PedPropAnchor.LeftWrist, fr.PropLWrist, fr.TexturePropLWrist);
        EvaluateProp(PedPropAnchor.RightWrist, fr.PropRWrist, fr.TexturePropRWrist);

        return new PedOutfit()
        {
            Components = EmptyList<PedComponentInfo>.Or(components),
            Props = EmptyList<PedPropInfo>.Or(props)
        };
    }
}
