// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace OpenReinforce.Engine.Data.Models.Regions
{
    public sealed class FrRegionFile : IFrDataRoot<FrRegion>
    {
        public FrRegion[]? Items { get; set; }
    }
}
