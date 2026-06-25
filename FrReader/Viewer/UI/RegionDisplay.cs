// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using OpenReinforce.Engine.Data.Models.Regions;
using Spectre.Console;

namespace OpenReinforce.Viewer.UI;

internal class RegionDisplay : ListDisplay<FrRegion>
{
    public RegionDisplay(IReadOnlyList<FrRegion> values) : base(values)
    {
    }

    protected override void GenerateRows(Table table, FrRegion value)
    {
        table.AddColumns("Name", "Zones");
        table.AddRow(value.Name ?? "None", "--");

        if (value.Zones == null)
        {
            return;
        }
        foreach (var zone in value.Zones)
        {
            table.AddRow("-", zone);
        }
    }
}
