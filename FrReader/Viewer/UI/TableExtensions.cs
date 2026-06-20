// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using Spectre.Console;
using System.Globalization;

namespace OpenReinforce.Viewer.UI;

internal static class TableExtensions
{
    private static void GetColours(bool empty, out Color keyColor, out Color valueColor)
    {
        keyColor = empty
            ? Color.DarkCyan
            : Color.Aqua;
        valueColor = empty
            ? Color.White
            : Color.Gray;
    }

    public static Table AddRow(this Table table, string key, bool value)
    {
        table.AddRow(new Text(key, new Style(Color.Aqua)),
            new Text(value ? "Yes" : "No", new Style(Color.White)));

        return table;
    }

    public static Table AddOptionalRow(this Table table, string key, int value)
    {
        var empty = value == 0;
        var displayValue = empty ? "-" : value.ToString(CultureInfo.InvariantCulture);

        GetColours(empty, out var keyColor, out var valueColor);

        table.AddRow(new Text(key, new Style(keyColor)),
            new Text(displayValue, new Style(valueColor)));

        return table;
    }

    public static Table AddOptionalRow(this Table table, string key, bool value)
    {
        if (!value)
        {
            return table;
        }

        var displayValue = value.ToString(CultureInfo.InvariantCulture);

        GetColours(false, out var keyColor, out var valueColor);

        table.AddRow(new Text(key, new Style(keyColor)),
            new Text(displayValue, new Style(valueColor)));

        return table;
    }

    public static Table AddOptionalRow(this Table table, string key, string? value = null)
    {
        var empty = string.IsNullOrWhiteSpace(value);
        if (empty)
        {
            value = "-";
        }

        GetColours(empty, out var keyColor, out var valueColor);

        table.AddRow(new Text(key, new Style(keyColor)),
            new Text(value!, new Style(valueColor)));

        return table;
    }

    public static Table AddOptionalRow<T>(this Table table,
        string key,
        Func<T, string> converter,
        T? value = default)
    {
        var empty = Equals(value, default);
        var valueDisplay = empty
            ? "-"
            : converter(value!);

        GetColours(empty, out var keyColor, out var valueColor);

        table.AddRow(new Text(key, new Style(keyColor)),
            new Text(valueDisplay, new Style(valueColor)));

        return table;
    }

    public static Table AddOptionalFlagsRow<TEnum>(this Table table, string key, TEnum[]? value = null)
        where TEnum : struct, Enum
    {
        var empty = value == null || value.Length == 0;
        var valueDisplay = empty
            ? "-"
            : string.Join(", ", value!);

        GetColours(empty, out var keyColor, out var valueColor);

        table.AddRow(new Text(key, new Style(keyColor)),
            new Text(valueDisplay, new Style(valueColor)));

        return table;
    }

    public static Table AddOptionalChanceRow(this Table table, string key, int value)
    {
        var empty = value == 0;
        var valueDisplay = empty
            ? $"{value.ToString(CultureInfo.InvariantCulture)}%"
            : "-";

        GetColours(empty, out var keyColor, out var valueColor);

        table.AddRow(new Text(key, new Style(keyColor)),
            new Text(valueDisplay, new Style(valueColor)));

        return table;
    }
}
