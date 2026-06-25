using OpenReinforce.Utilities;
using Rage;

namespace OpenReinforce.Engine;

internal sealed class DismissManager
{
    private const float VehicleDismissDistance = 150f;

    private readonly List<Vehicle> _pendingVehicles = [];

    public void Process()
    {
        var playerPos = Game.LocalPlayer.Character.Position;
        for (int i = 0; i < _pendingVehicles.Count; i++)
        {
            var veh = _pendingVehicles[i];
            if (!veh.IsValid())
            {
                _pendingVehicles.RemoveAt(i);
                continue;
            }

            if (veh.Position.DistanceTo(playerPos) >= VehicleDismissDistance
                || !veh.IsOnScreen)
            {
                DeleteVehicleProperly(veh);
                _pendingVehicles.RemoveAt(i);
            }
        }
    }

    private static void DeleteVehicleProperly(Vehicle veh)
    {
        var contraindicated = false;
        if (veh.HasOccupants)
        {
            foreach (var passenger in veh.Occupants)
            {
                if (passenger.IsPlayer || (!passenger.IsInjured() && passenger.IsOccupied()))
                {
                    contraindicated = true;
                    continue;
                }

                passenger.Delete();
            }
        }

        veh.Cleanup(!contraindicated);
    }

    public void Add(Vehicle vehicle)
    {

        if (!vehicle.Exists())
        {
            return;
        }

        _pendingVehicles.Add(vehicle);
    }

    public void Cleanup()
    {
        foreach (var vehicle in _pendingVehicles)
        {
            if (!vehicle.Exists())
            {
                continue;
            }

            DeleteVehicleProperly(vehicle);
        }
    }
}
