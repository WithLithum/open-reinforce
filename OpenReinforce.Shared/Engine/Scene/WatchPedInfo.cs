#if GTA
namespace OpenReinforce.Shared.Engine.Scene;

using Rage;

internal sealed class WatchPedInfo
{
    public required WatchPedState State { get; set; }

    public int VehicleSeat { get; set; }
    public Vehicle? Vehicle { get; set; }
    public Blip? Blip { get; set; }

    public required Ped Ped { get; set; }

    public DateTimeOffset? Timeout { get; set; }
}

#endif
