// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using OpenReinforce.Engine.Data.Models.Response;
using OpenReinforce.Engine.Data.Utilities;
using OpenReinforce.Viewer.UI;
using Spectre.Console;

namespace OpenReinforce.Viewer.Views;

internal sealed class BackupsDisplay : ListDisplay<BackupsDisplay.Shrimp>
{
    public readonly struct Shrimp
    {
        internal Shrimp(string name,
            IReadOnlyDictionary<string, string>? value)
        {
            Name = name;
            Value = value;
        }

        internal string Name { get; }
        internal IReadOnlyDictionary<string, string>? Value { get; }
    }

    public BackupsDisplay(ResponseTable responseTable)
        : base([
            new("Local Patrol", responseTable.LocalPatrol),
            new("State Patrol", responseTable.StatePatrol),
            new("Local SWAT", responseTable.LocalSWAT),
            new("NOOSE SWAT", responseTable.NooseSWAT),
            new("Local Air", responseTable.LocalAir),
            new("NOOSE Air", responseTable.NooseAir),
            new("Ambulance", responseTable.Ambulance),
            new("Fire Service", responseTable.Firetruck)])
    {
    }

    protected override void GenerateRows(Table table, Shrimp value)
    {
        if (value.Value == null)
        {
            table.AddRow("!!!", "No Data");
            return;
        }

        table.Title(value.Name)
            .ShowHeaders();
        foreach (var pair in value.Value)
        {
            table.AddRow(pair.Key, pair.Value);
        }
    }
}
