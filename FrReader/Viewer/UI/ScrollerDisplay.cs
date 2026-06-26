// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using OpenReinforce.Engine.Data.Models.Agencies;
using Spectre.Console;

namespace OpenReinforce.Viewer.UI;

internal abstract class ScrollerDisplay<T> : QuittableDisplay
{
    private readonly T[] _items;
    private int _pageStart;
    private int _cursor;

    protected ScrollerDisplay(T[] items)
    {
        _items = items;
    }

    public override void Run()
    {
        var pageSize = Console.BufferHeight - 2;
        if (pageSize < 0)
        {
            AnsiConsole.WriteLine("[red on white]Please EXPAND console window![/]");
            base.Run();
            return;
        }

        if (_items.Length == 0)
        {
            AnsiConsole.WriteLine("[red on white]List is empty![/]");
            base.Run();
            return;
        }

        var actualCount = pageSize;
        if (_pageStart + pageSize >= _items.Length)
        {
            actualCount = _items.Length - _pageStart;
        }

        var pageRange = new ArraySegment<T>(_items, _pageStart, actualCount);
        var counter = 0;
        foreach (var item in pageRange)
        {
            RenderLine(item, _cursor == counter);
            counter++;
        }

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[black on white]q: Exit | Up: cursor up | Down: cursor down | Left: prev | Right: next [/]");

        var key = Console.ReadKey(true);
        switch (key.Key)
        {
            case ConsoleKey.Q:
                Quit = true;
                break;
            case ConsoleKey.LeftArrow:
                PreviousPage(pageSize);
                break;
            case ConsoleKey.RightArrow:
                NextPage(pageSize);
                break;
            case ConsoleKey.UpArrow:
                MoveUp();
                break;
            case ConsoleKey.DownArrow:
                MoveDown(counter);
                break;
        }
    }

    private void MoveDown(int numItems)
    {
        _cursor++;
        if (_cursor >= numItems)
        {
            _cursor = numItems - 1;
        }
    }

    private void MoveUp()
    {
        _cursor--;
        if (_cursor < 0)
        {
            _cursor = 0;
        }
    }

    private void NextPage(int pageSize)
    {
        _pageStart += pageSize;
        if (_pageStart >= _items.Length)
        {
            _pageStart = 0;
        }

        _cursor = 0;
    }

    private void PreviousPage(int pageSize)
    {
        _pageStart -= pageSize;
        if (_pageStart < 0)
        {
            _pageStart = 0;
        }

        _cursor = 0;
    }

    public abstract void RenderLine(T item, bool current);
}
