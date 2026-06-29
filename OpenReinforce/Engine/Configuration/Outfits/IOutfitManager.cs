// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using OpenReinforce.Engine.Entities;

namespace OpenReinforce.Engine.Configuration.Outfits;

internal interface IOutfitManager
{
    PedOutfit? PickByRef(OutfitReference reference, bool female);

    void Add(string id, string groupId, PedOutfit outfit);
}
