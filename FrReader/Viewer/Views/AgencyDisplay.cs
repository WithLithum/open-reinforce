using OpenReinforce.Engine.Data.Models.Agencies;
using OpenReinforce.Viewer.UI;
using Spectre.Console;

namespace OpenReinforce.Viewer.Views;

internal sealed partial class AgencyDisplay : IDisplay
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