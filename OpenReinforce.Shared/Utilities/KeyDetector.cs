#if GTA
using Rage;
using System.Windows.Forms;

namespace OpenReinforce.Utilities;

internal sealed class KeyDetector
{
    private static readonly TimeSpan HoldDownInterval = TimeSpan.FromMilliseconds(1500);

    private readonly Keys _key;

    private bool _isDown;
    private bool _reset;
    private DateTimeOffset _checkTime;

    public KeyDetector(Keys key)
    {
        _key = key;
    }

    public void Process()
    {
        var keyDown = Game.IsKeyDownRightNow(_key);
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