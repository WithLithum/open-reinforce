// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using OpenReinforce.Engine.Entities;
using OpenReinforce.Utilities;

namespace OpenReinforce.Engine.Configuration.Outfits;

internal sealed class OutfitManager : IOutfitManager
{
    private readonly Dictionary<string, Dictionary<string, PedOutfit>> _outfits = [];

    public void Add(string id, string groupId, PedOutfit outfit)
    {
        var dict = GetGroupDict(groupId);
        dict.Add(id, outfit);
    }

    public PedOutfit? PickByRef(OutfitReference reference, bool female)
    {
        var groupDict = GetGroupDict(reference.GroupName);
        if (string.IsNullOrWhiteSpace(reference.VariantName))
        {
            return PickByRefInternalUniform(groupDict, female);
        }

        // Pick by name. Return null if not found.
        _ = groupDict.TryGetValue(reference.VariantName!, out var variant);
        return variant;
    }

    private static PedOutfit? PickByRefInternalUniform(Dictionary<string, PedOutfit> groupDict,
        bool female)
    {
        if (groupDict.Count == 0)
        {
            return null;
        }

        return ItemSelector.PickByUniform(groupDict.Values,
            x => x.Female == female);
    }

    private Dictionary<string, PedOutfit> GetGroupDict(string groupName)
    {
        if (_outfits.TryGetValue(groupName, out var result))
        {
            return result;
        }

        var dict = new Dictionary<string, PedOutfit>();
        _outfits.Add(groupName, dict);
        return dict;
    }
}
