using Rage;

namespace OpenReinforce.Utilities;

internal static class ItemSelector
{
    internal static T PickByUniform<T>(IReadOnlyList<T> items)
    {
        if (items.Count == 0)
        {
            throw new ArgumentException("Attempting to pick from an empty list.", nameof(items));
        }

        return items[MathHelper.GetRandomInteger(0, items.Count - 1)];
    }

    internal static T PickByUniform<T>(IEnumerable<T> items,
        Predicate<T>? predicate = null)
    {
        var col = items;

        if (predicate != null)
        {
            col = col.Where(x => predicate(x));
        }

        return col.OrderBy(o => MathHelper.GetRandomInteger(0, 150)).First();
    }

    internal static T PickByChance<T>(IReadOnlyList<T> items) where T : IChanced
    {
        if (items.Count == 0)
        {
            throw new ArgumentException("Attempting to pick from an empty list.", nameof(items));
        }

        if (items.Count == 1)
        {
            // No point to run pick logic.
            return items[0];
        }

        var randomNumber = MathHelper.GetRandomInteger(1, 100);
        var totalSum = 0;
        foreach (var item in items)
        {
            totalSum += item.Chance;
            if (totalSum > 100)
            {
                Log.Warn("Picking a list with sum exceeding 100");
                return item;
            }

            if (randomNumber <= totalSum)
            {
                return item;
            }
        }

        if (totalSum != 0)
        {
            Game.LogTrivial(
                "Open Reinforce: Warning: distribution fallback triggered for non-zero sum.");
        }

        // Try using distribution.
        return items[MathHelper.GetRandomInteger(0, items.Count - 1)];
    }
}
