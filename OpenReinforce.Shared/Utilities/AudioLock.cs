namespace OpenReinforce.Utilities;

public static class AudioLock
{
    private static DateTimeOffset UnlockAt;

    public static void LockAudio(TimeSpan length)
    {
        UnlockAt = DateTimeOffset.UtcNow + length;
    }

    public static bool IsHeld()
    {
        return DateTimeOffset.UtcNow >= UnlockAt;
    }
}