using OpenReinforce.Engine.Data.Models.Agencies;
using Spectre.Console;

namespace OpenReinforce.Viewer.Views;

partial class AgencyDisplay
{
    private const int HomePageSize = 10;

    private int _homePageStart;

    private void RunHomePage()
    {
        var actualCount = HomePageSize;
        if (_homePageStart + HomePageSize >= _agencies.Length)
        {
            actualCount = _agencies.Length - _homePageStart;
        }

        AnsiConsole.MarkupLineInterpolated($"[black on aqua]Showing {actualCount} of {_agencies.Length} from {_homePageStart}[/]");


        var homePageRange = new ArraySegment<FrAgency>(_agencies, _homePageStart, actualCount);
        var counter = 0;
        foreach (var item in homePageRange)
        {
            AnsiConsole.MarkupLineInterpolated($"#[bold DarkOrange]{counter}:[/] [DarkBlue]{item.ScriptName}[/] - {item.Name} ({item.ShortName})");
            counter++;
        }

        AnsiConsole.MarkupLine("[black on white]q: Exit | 0-9: Select Agency | Left: prev | Right: next [/]");
        var key = Console.ReadKey(true);

        switch (key.Key)
        {
            case ConsoleKey.Q:
                Quit = true;
                break;
            case ConsoleKey.LeftArrow:
                HomePreviousPage();
                break;
            case ConsoleKey.RightArrow:
                HomeNextPage();
                break;
            case ConsoleKey.D0:
            case ConsoleKey.D1:
            case ConsoleKey.D2:
            case ConsoleKey.D3:
            case ConsoleKey.D4:
            case ConsoleKey.D5:
            case ConsoleKey.D6:
            case ConsoleKey.D7:
            case ConsoleKey.D8:
            case ConsoleKey.D9:
                SelectAgency(key.Key, homePageRange);
                break;
        }

    }

    private void HomeNextPage()
    {
        _homePageStart += HomePageSize;
        if (_homePageStart >= _agencies.Length)
        {
            _homePageStart = 0;
        }
    }

    private void HomePreviousPage()
    {
        _homePageStart -= HomePageSize;
        if (_homePageStart < 0)
        {
            _homePageStart = 0;
        }
    }

    private void SelectAgency(ConsoleKey key, ArraySegment<FrAgency> selectFrom)
    {
        var index = key switch
        {
            ConsoleKey.D0 => 0,
            ConsoleKey.D1 => 1,
            ConsoleKey.D2 => 2,
            ConsoleKey.D3 => 3,
            ConsoleKey.D4 => 4,
            ConsoleKey.D5 => 5,
            ConsoleKey.D6 => 6,
            ConsoleKey.D7 => 7,
            ConsoleKey.D8 => 8,
            ConsoleKey.D9 => 9,
            _ => -1
        };

        if (index == -1 || selectFrom.Count <= index)
        {
            return;
        }

        _selectedAgency = selectFrom[index];
        _currentPage = Page.Info;
    }
}