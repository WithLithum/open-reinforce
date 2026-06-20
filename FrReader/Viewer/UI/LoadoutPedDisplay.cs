// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using OpenReinforce.Engine.Data.Models.Agencies;
using Spectre.Console;

namespace OpenReinforce.Viewer.UI;

internal sealed class LoadoutPedDisplay : ListDisplay<FrLoadoutPed>
{
    public LoadoutPedDisplay(IReadOnlyList<FrLoadoutPed> values) : base(values)
    {
    }

    protected override void GenerateRows(Table table, FrLoadoutPed value)
    {
        table.AddOptionalRow("Model", value.Model)
            .AddOptionalRow("Outfit", value.Outfit)
            .AddOptionalRow("Inventory", value.Inventory)
            .AddOptionalChanceRow("Chance", value.Chance)
            .AddRow("Randomize props?", value.RandomizeProps);
    }
}
