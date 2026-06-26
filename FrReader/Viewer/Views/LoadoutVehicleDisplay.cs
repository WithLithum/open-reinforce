// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using OpenReinforce.Engine.Data.Models.Agencies;
using OpenReinforce.Viewer.UI;
using Spectre.Console;

namespace OpenReinforce.Viewer.Views;

internal sealed class LoadoutVehicleDisplay : ListDisplay<FrLoadoutVehicle>
{
    public LoadoutVehicleDisplay(IReadOnlyList<FrLoadoutVehicle> values) : base(values)
    {
    }

    protected override void GenerateRows(Table table, FrLoadoutVehicle value)
    {
        table.AddOptionalRow("Model", value.Model)
            .AddOptionalChanceRow("Chance", value.Chance)
            .AddRow("MP vehicle?", value.IsMp)
            .AddRow("Use modkit liveries?", value.UseModKitLiveries)
            .AddOptionalRow("Livery (Single)", value.Livery)
            .AddOptionalRow("Livery (One of)", value.LiveryMulti)
            .AddOptionalRow("Extra 1", value.Extra1)
            .AddOptionalRow("Extra 2", value.Extra2)
            .AddOptionalRow("Extra 3", value.Extra3)
            .AddOptionalRow("Extra 4", value.Extra4)
            .AddOptionalRow("Extra 5", value.Extra5)
            .AddOptionalRow("Extra 6", value.Extra6)
            .AddOptionalRow("Extra 7", value.Extra7)
            .AddOptionalRow("Extra 8", value.Extra8)
            .AddOptionalRow("Extra 9", value.Extra9)
            .AddOptionalRow("Extra 10", value.Extra10)
            .AddOptionalRow("Extra 11", value.Extra11)
            .AddOptionalRow("Extra 12", value.Extra12)
            .AddOptionalRow("Extra 13", value.Extra13)
            .AddOptionalRow("Extra 14", value.Extra14)
            .AddOptionalRow("Extra 15", value.Extra15);
    }
}
