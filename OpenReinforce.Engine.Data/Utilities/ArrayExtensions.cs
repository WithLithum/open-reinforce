using System;

namespace OpenReinforce.Engine.Data.Utilities
{
    public static class ArrayExtensions
    {
        public static bool ContainsAny<T>(this T[] array, Func<T, bool> predicate)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (predicate(array[i]))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
