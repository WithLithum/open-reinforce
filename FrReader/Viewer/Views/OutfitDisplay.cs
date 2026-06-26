// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using OpenReinforce.Engine.Data.Models.Outfits;
using OpenReinforce.Viewer.UI;
using Spectre.Console;

namespace OpenReinforce.Viewer.Views;

internal class OutfitDisplay : ScrollerDisplay<FrOutfit>
{
    public OutfitDisplay(FrOutfit[] items) : base(items)
    {
    }

    public override void RenderLine(FrOutfit item, bool current)
    {
        var nameStyle = current
            ? new Style(Color.DarkBlue, Color.White)
            : new Style(Color.Aqua);
        var idStyle = current
            ? new Style(Color.DarkBlue, Color.White)
            : new Style(Color.Aqua);

        var nameText = new Text(item.Name ?? "(Unnamed)",
            nameStyle);
        var idText = new Text($" [{item.ScriptName ?? "<no script name>"}]",
            idStyle);

        AnsiConsole.Write(nameText);
        AnsiConsole.Write(idText);
        AnsiConsole.WriteLine();
    }
}
