// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using OpenReinforce.Engine.Data.Models.Agencies;
using OpenReinforce.Engine.Data.Models.Outfits;
using OpenReinforce.Engine.Data.Models.Regions;
using OpenReinforce.Engine.Data.Models.Response;
using OpenReinforce.Viewer.Views;
using Spectre.Console;

namespace OpenReinforce.Viewer.UI;

internal sealed class ViewSelectorDisplay : IDisplay
{
    private readonly FrAgency[] _agencies;
    private readonly FrRegion[] _regions;
    private readonly ResponseTable _backups;
    private readonly FrOutfit[] _outfits;

    public ViewSelectorDisplay(FrAgency[] agencies,
        FrRegion[] regions,
        ResponseTable backups,
        FrOutfit[] outfits)
    {
        _agencies = agencies;
        _regions = regions;
        _backups = backups;
        _outfits = outfits;
    }

    public bool Quit { get; private set; }

    public void Run()
    {
        AnsiConsole.MarkupLine("[black on aqua]Select one of the below things:[/]");
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[bold aqua]1[/]: [white]Agency Viewer[/]");
        AnsiConsole.MarkupLine("[bold aqua]2[/]: [white]Region Viewer[/]");
        AnsiConsole.MarkupLine("[bold aqua]3[/]: [white]Backup Viewer[/]");
        AnsiConsole.MarkupLine("[bold aqua]4[/]: [white]Outfit Viewer[/]");
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[bold aqua]q[/]: [white]Exit[/]");

        var keyInfo = Console.ReadKey(true);
        if (keyInfo.Key == ConsoleKey.Q)
        {
            Quit = true;
            return;
        }

        IDisplay? screen = keyInfo.KeyChar switch
        {
            '1' => new AgencyDisplay(_agencies),
            '2' => new RegionDisplay(_regions),
            '3' => new BackupsDisplay(_backups),
            '4' => new OutfitDisplay(_outfits),
            _ => null,
        };
        if (screen != null)
        {
            Displays.Run(screen);
        }
    }
}
