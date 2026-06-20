#if GTA
using OpenReinforce.Native;
using OpenReinforce.Utilities;
using Rage;
using RAGENativeUI;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using WithLithum.NativeWrapper;

namespace OpenReinforce.Engine.Response;

partial class PoliceUnit
{
    private readonly KeyDetector _wrapKey = new(Keys.Back);

    private enum PoliceUnitState
    {
        None,
        GoingToDestination,
        ApproachingDestination,
        Arrived
    }

    [Conditional("DEBUG")]
    private void SetTestBlipColor()
    {
        if (!_vehicleBlip.Exists())
        {
            return;
        }

        _vehicleBlip!.Color = _state switch
        {
            PoliceUnitState.GoingToDestination => Color.Yellow,
            PoliceUnitState.ApproachingDestination => Color.Blue,
            PoliceUnitState.Arrived => Color.White,
            _ => Color.Blue,
        };
    }

    private void SwitchToState(PoliceUnitState state)
    {
        _state = state;

#if DEBUG
        Game.DisplaySubtitle($"SwitchToState(): {state}");
#endif

        switch (_state)
        {
            case PoliceUnitState.GoingToDestination:
                if (_vehicleBlip.Exists())
                {
                    _vehicleBlip!.Flash(1000, 0);
                }

                if (!_vehicle.Exists() || _driver.IsInjured())
                {
                    Game.LogTrivial("Open Reinforce: cop unit failed to spawn");
                    Cleanup(true);
                    return;
                }

                _driver!.Tasks.DriveToPosition(_vehicle,
                    _destination,
                    16.6f,
                    VehicleDrivingFlags.Emergency,
                    7.5f);
                _wrapWait = DateTimeOffset.UtcNow + WrapWaitLength;

                break;
            case PoliceUnitState.ApproachingDestination:
                if (_vehicleBlip.Exists())
                {
                    _vehicleBlip!.Flash(1000, 0);
                }

                _wrapEnabled = false;
                _driver!.Tasks.DriveToPosition(_vehicle,
                    _destination,
                    5.55f, /* ~20km/h */
                    VehicleDrivingFlags.Emergency,
                    7.5f);
                break;
            case PoliceUnitState.Arrived:
                if (_vehicleBlip.Exists())
                {
                    _vehicleBlip!.StopFlashing();
                }
                break;
        }

        SetTestBlipColor();
    }

    public void Process()
    {
        switch (_state)
        {
            case PoliceUnitState.GoingToDestination:
                GoingToDestination();
                break;
            case PoliceUnitState.ApproachingDestination:
                ApproachDestination();
                break;
            case PoliceUnitState.Arrived:
                Arrived();
                break;
        }
    }
    private void GoingToDestination()
    {
        if (Natives.IsEntityDead(_vehicle!.Handle, false)
            || Natives.IsPedInjured(_driver!.Handle))
        {
            // Vehicle or driver is either dead or vanished. At this point, we cannot task it to
            // the scene anymore.

            Cleanup(false);
            return;
        }

        DetectAndRecoverVehicle();

        if (!WrapRoutine())
        {
            return;
        }

        if (_vehicle.Position.DistanceTo(_destination) <= ApproachThreshold)
        {
            SwitchToState(PoliceUnitState.ApproachingDestination);
        }
        else
        {
            var from = new Vector2(_vehicle.Position.X + 50,
                _vehicle.Position.Y + 50);
            var to = new Vector2(_vehicle.Position.X - 50,
                _vehicle.Position.Y - 50);
            Natives.RequestPathNodesInAreaThisFrame(from.X, from.Y, to.X, to.Y);
        }
    }

    private bool WrapRoutine()
    {
        if (_wrapEnabled && _wrapKey.IsHeldDown())
        {
            _wrapKey.Reset();
            _wrapEnabled = false;

            // Wrap
            FindAlternativePosIfNeeded();
            Natives.SetEntityCoords(_vehicle!.Handle,
                _destination.X,
                _destination.Y,
                _destination.Z,
                false,
                false,
                false,
                true);
            SwitchToState(PoliceUnitState.ApproachingDestination);
            return false;
        }
        else if (DateTime.UtcNow >= _wrapWait)
        {
            _wrapEnabled = true;
            Game.DisplayHelp($"If the reinforcement unit is taking too long to arrive, hold ~{_wrapKey.Key.GetInstructionalId()}~ to spawn it immediately.");
        }

        return true;
    }

    private void FindAlternativePosIfNeeded()
    {
        if (Natives.IsPositionOccupied(_destination.X,
                            _destination.Y,
                            _destination.Z,
                            25f,
                            false,
                            true,
                            true,
                            false,
                            false,
                            0,
                            true
                            ))
        {
            // Look for an alternative position
            var wrapTarget = _destination.Around2D(5f, 35f);
            if (PathFind.GetNearestVehicleNodeWithHeading(wrapTarget,
                out var newTarget,
                out _,
                VehicleNodeFilters.IncludeSwitchedOffNodes,
                0.85f,
                35))
            {
                _destination = newTarget;
            }
        }
    }

    private void ApproachDestination()
    {
        if (Natives.IsEntityDead(_vehicle!.Handle, false)
            || Natives.IsPedInjured(_driver!.Handle))
        {
            // Vehicle or driver is either dead or vanished. At this point, we cannot task it to
            // the scene anymore.

            Cleanup(false);
            return;
        }

        DetectAndRecoverVehicle();

        if (_vehicle.Position.DistanceTo(_destination) > ApproachThreshold)
        {
            SwitchToState(PoliceUnitState.GoingToDestination);
            return;
        }

        if (_vehicle.Position.DistanceTo(_destination) <= ArrivalThreshold)
        {
            _peds!.ForEach(p =>
            {
                if (!p.Ped.IsInjured())
                {
                    p.Ped.BlockPermanentEvents = true;
                    p.Ped.Tasks.LeaveVehicle(LeaveVehicleFlags.None);
                }
            });

            SwitchToState(PoliceUnitState.Arrived);
        }
        else
        {
            var from = new Vector2(_vehicle.Position.X + 50,
                _vehicle.Position.Y + 50);
            var to = new Vector2(_vehicle.Position.X - 50,
                _vehicle.Position.Y - 50);
            Natives.RequestPathNodesInAreaThisFrame(from.X, from.Y, to.X, to.Y);
        }
    }

    private void Arrived()
    {
        for (int i = 0; i < _peds!.Count; i++)
        {
            var p = _peds[i].Ped;
            if (p.IsInjured())
            {
                continue;
            }

            if (p.IsSittingInVehicle(_vehicle!))
            {
                p.Tasks.LeaveVehicle(LeaveVehicleFlags.None);
                continue;
            }

            OpenReinforcePlugin.WatchManager.Add(p, _vehicle, _peds[i].Seat);
            _peds.RemoveAt(i);
        }

        if (_peds!.Count == 0)
        {
            Cleanup(false);
        }
    }
}
#endif