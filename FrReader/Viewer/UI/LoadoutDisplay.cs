// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using OpenReinforce.Engine.Data.Models.Agencies;
using Spectre.Console;

namespace OpenReinforce.Viewer.UI;

internal sealed class LoadoutDisplay : ListDisplay<FrLoadout>
{
    public LoadoutDisplay(IReadOnlyList<FrLoadout> loadoutList) : base(loadoutList)
    {
    }

    protected override void GenerateRows(Table table, FrLoadout value)
    {
        table.AddOptionalRow("Name", value.Name)
            .AddOptionalChanceRow("Chance", value.Chance)
            .AddOptionalFlagsRow("Flags", value.Flags);

        if (value.NumPeds != null)
        {
            table.AddRow("Minimum peds", value.NumPeds.Min.ToString())
                .AddRow("Maximum peds", value.NumPeds.Max.ToString());
        }
    }

    protected override void DisplayKeybinds()
    {
        AnsiConsole.MarkupLine("[black on white]q: Back | v: Vehicles | p: Peds | Left: prev | Right: next[/]");
    }

    override protected void ProcessInput(ConsoleKey key, FrLoadout currentValue)
    {
        switch (key)
        {
            case ConsoleKey.V:
                if (currentValue.Vehicles != null)
                {
                    Displays.Run(new LoadoutVehicleDisplay(currentValue.Vehicles));
                }
                break;
            case ConsoleKey.P:
                if (currentValue.Peds != null)
                {
                    Displays.Run(new LoadoutPedDisplay(currentValue.Peds));
                }
                break;
        }
        base.ProcessInput(key, currentValue);
    }
}
