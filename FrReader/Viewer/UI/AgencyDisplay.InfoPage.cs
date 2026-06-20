using Spectre.Console;

namespace OpenReinforce.Viewer.UI;

partial class AgencyDisplay
{
    private void RunInfoPage()
    {
        RenderInfoPage();

        var key = Console.ReadKey(true);
        switch (key.Key)
        {
            case ConsoleKey.Q:
                _selectedAgency = null;
                _currentPage = Page.Home;
                break;
            case ConsoleKey.L:
                if (_selectedAgency!.Loadouts != null)
                {
                    Displays.Run(new LoadoutDisplay(_selectedAgency.Loadouts));
                }
                break;
            case ConsoleKey.P:
                NavigateToParent();
                break;
        }
    }

    private void NavigateToParent()
    {
        var parentId = _selectedAgency?.Parent;
        if (string.IsNullOrWhiteSpace(parentId))
        {
            _status = "No parent";
            return;
        }

        var parent = _agencies.FirstOrDefault(x => x.ScriptName == _selectedAgency?.Parent);
        if (parent == null)
        {
            _status = "Parent doesn't exist";
            return;
        }

        _selectedAgency = parent;
    }

    private void RenderInfoPage()
    {
        if (_selectedAgency == null)
        {
            AnsiConsole.MarkupLine("[white on red]No selected agency[/]");
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[black on white]q: Exit[/]");
            return;
        }

        var table = new Table()
            .AddColumn("K")
            .AddColumn("V")
            .HideHeaders();

        table.AddRow("Name", _selectedAgency.Name ?? "None");
        table.AddRow("Short name", _selectedAgency.ShortName ?? "None");
        table.AddRow("Script name", _selectedAgency.ScriptName ?? "None");
        table.AddRow("Parent", _selectedAgency.Parent ?? "None");
        table.AddRow("Texture dict", _selectedAgency.TextureDictionary ?? "None");
        table.AddRow("Texture name", _selectedAgency.TextureName ?? "None");
        table.AddRow("Shield model", _selectedAgency.ShieldModel ?? "None");
        table.AddRow("Evidence marker model", _selectedAgency.EvidenceMarkerModel ?? "None");
        table.AddRow("Exclude from backup menu", _selectedAgency.ExcludeFromBackupMenu.ToString());

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[black on white]q: Exit | p: Parent | l: Loadouts[/]");
    }
}