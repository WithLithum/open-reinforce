// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Diagnostics.CodeAnalysis;

namespace OpenReinforce.Engine.Configuration.Outfits;

internal sealed class OutfitReference
{
    [SetsRequiredMembers]
    public OutfitReference(string groupName, string? variantName = null)
    {
        GroupName = groupName;
        VariantName = variantName;
    }

    public required string GroupName { get; init; }

    public string? VariantName { get; init; }

    internal static bool TryParse(string input,
        [MaybeNullWhen(false)] out OutfitReference result)
    {
        if (!input.Contains('.'))
        {
            result = new OutfitReference(input);
            return true;
        }

        var splitted = input.Split('.');
        if (splitted.Length > 2)
        {
            result = null;
            return false;
        }

        result = new OutfitReference(splitted[0], splitted[1]);
        return true;
    }

    internal static bool IsOutfitNameValid(string name)
    {
        for (int i = 0; i < name.Length; i++)
        {
            var c = name[i];
            if (c == '.' || c == ' ' || c == '\0')
            {
                return false;
            }
        }

        return true;
    }
}
