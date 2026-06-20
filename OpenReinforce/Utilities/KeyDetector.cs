#if GTA
using Rage;
using System.Windows.Forms;

namespace OpenReinforce.Utilities;

internal sealed class KeyDetector
{
    private static readonly TimeSpan HoldDownInterval = TimeSpan.FromMilliseconds(1500);

    private bool _isDown;
    private bool _reset;
    private DateTimeOffset _checkTime;

    public KeyDetector(Keys key)
    {
        Key = key;
    }

    public Keys Key { get; }

    public void Process()
    {
        var keyDown = Game.IsKeyDownRightNow(Key);
        if (_isDown != keyDown)
        {
            _reset = false;
            _isDown = keyDown;
            ResetTimer();
        }
    }

    public void Reset()
    {
        _reset = true;
    }

    public void ResetTimer()
    {
        _checkTime = DateTimeOffset.UtcNow + HoldDownInterval;
    }

    public bool IsHeldDown()
    {
        return !_reset && _isDown && DateTimeOffset.UtcNow >= _checkTime;
    }
}
#endif