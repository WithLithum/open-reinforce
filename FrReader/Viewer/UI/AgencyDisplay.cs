using OpenReinforce.Engine.Data.Models.Agencies;
using Spectre.Console;

namespace OpenReinforce.Viewer.UI;

internal sealed partial class AgencyDisplay
{
    private readonly FrAgency[] _agencies;
    private Page _currentPage;
    private FrAgency? _selectedAgency;
    private string? _status;

    private enum Page
    {
        Home,
        Info
    }

    public AgencyDisplay(FrAgency[] agencies)
    {
        _agencies = agencies;
    }

    public bool Quit { get; private set; }

    public void Run()
    {
        AnsiConsole.Clear();
        if (_status == null)
        {
            AnsiConsole.WriteLine();
        }
        else
        {
            AnsiConsole.MarkupLineInterpolated($"[white on red] {"["} {_status} {"]"} [/]");
            _status = null;
        }

        switch (_currentPage)
        {
            case Page.Home:
                RunHomePage();
                break;
            case Page.Info:
                RunInfoPage();
                break;
        }
    }
}