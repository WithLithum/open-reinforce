// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using Spectre.Console;

namespace OpenReinforce.Viewer.UI;

internal abstract class ListDisplay<T> : IDisplay
{
    private readonly IReadOnlyList<T> _values;
    private int _currentIndex;

    protected ListDisplay(IReadOnlyList<T> values)
    {
        _values = values;
    }

    public bool Quit { get; private set; }

    public void Run()
    {
        if (_values.Count == 0)
        {
            RunEmptyLoadouts();
            return;
        }

        var value = _values[_currentIndex];

        AnsiConsole.MarkupLineInterpolated($"[black on white]Showing {_currentIndex + 1}/{_values.Count}[/]");

        if (value is null)
        {
            AnsiConsole.MarkupLine("[red]Invalid value[/]");
        }
        else
        {
            var table = new Table()
               .AddColumn("K")
               .AddColumn("V")
               .HideHeaders();

            GenerateRows(table, value);

            AnsiConsole.Write(table);
            AnsiConsole.WriteLine();
            DisplayKeybinds();
        }

        ProcessInput(Console.ReadKey(true).Key, value);
    }

    protected abstract void GenerateRows(Table table, T value);

    protected virtual void DisplayKeybinds()
    {
        AnsiConsole.MarkupLine("[black on white]q: Back | Left: prev | Right: next[/]");
    }

    protected virtual void ProcessInput(ConsoleKey key, T currentValue)
    {
        switch (key)
        {
            case ConsoleKey.Q:
                Quit = true;
                break;
            case ConsoleKey.LeftArrow:
                Previous();
                break;
            case ConsoleKey.RightArrow:
                Next();
                break;
        }
    }

    private void Next()
    {
        _currentIndex++;

        if (_currentIndex >= _values.Count)
        {
            _currentIndex = _values.Count - 1;
        }
    }

    private void Previous()
    {
        _currentIndex--;

        if (_currentIndex <= 0)
        {
            _currentIndex = 0;
        }
    }

    private void RunEmptyLoadouts()
    {
        AnsiConsole.MarkupLine("[red]List is empty[/]");
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[black on white]q: Back[/]");
        if (Console.ReadKey(true).Key == ConsoleKey.Q)
        {
            Quit = true;
        }
    }
}
