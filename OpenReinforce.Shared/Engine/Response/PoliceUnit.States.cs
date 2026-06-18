#if GTA
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
    private enum PoliceUnitState
    {
        None,
        GoingToDestination,
        ApproachingDestination,
        Arrived,
        Engaging,
        ReturnToCar,
        EnterCar,
        Finished
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
            PoliceUnitState.Engaging => Color.Purple,
            PoliceUnitState.ReturnToCar => Color.Gray,
            PoliceUnitState.EnterCar => Color.DarkCyan,
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
                _waitUntil = DateTimeOffset.UtcNow + WaitLength;
                if (_vehicleBlip.Exists())
                {
                    _vehicleBlip!.StopFlashing();
                }
                break;
            case PoliceUnitState.Engaging:
                _engageTimerRunning = false;
                break;
            case PoliceUnitState.ReturnToCar:
                _waitUntil = DateTimeOffset.UtcNow + WrapWaitLength;
                break;
            case PoliceUnitState.EnterCar:
                _waitUntil = DateTimeOffset.UtcNow + WrapWaitLength;
                _peds!.ForEach(x =>
                {
                    if (!x.Ped.IsInjured())
                    {
                        x.Ped.Tasks.EnterVehicle(_vehicle, x.Seat);
                    }
                });
                break;
            case PoliceUnitState.Finished:
                foreach (var (ped, seat) in _peds!)
                {
                    if (ped.Exists() && !ped.IsInAnyVehicle(false))
                    {
                        Natives.SetPedIntoVehicle(ped.Handle, _vehicle!.Handle, seat);
                    }
                }

                _vehicleBlip.Cleanup();
                _vehicle!.IsSirenOn = false;
                _driver!.Tasks.CruiseWithVehicle(_vehicle, 8.3f, VehicleDrivingFlags.Normal);
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
            case PoliceUnitState.Engaging:
                Engaging();
                break;
            case PoliceUnitState.ReturnToCar:
                ReturnToCar();
                break;
            case PoliceUnitState.EnterCar:
                EnterCar();
                break;
            case PoliceUnitState.Finished:
                Finished();
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
        if (_wrapEnabled)
        {
            var isKeyDown = Game.IsKeyDownRightNow(Keys.Back);
            if (!_wrapKeyDown && isKeyDown)
            {
                _wrapKeyDown = true;
                _wrapWait += WrapKeyDownLength;
            }
            else if (!isKeyDown)
            {
                _wrapKeyDown = false;
            }

            if (_wrapKeyDown && DateTimeOffset.UtcNow >= _wrapWait)
            {
                _wrapEnabled = false;

                // Wrap
                _vehicle!.Position = _destination;
                SwitchToState(PoliceUnitState.ApproachingDestination);
                return false;
            }
        }
        else if (DateTime.UtcNow >= _wrapWait)
        {
            _wrapEnabled = true;
            Game.DisplayHelp($"If the reinforcement unit is taking too long to arrive, hold ~{Keys.Back.GetInstructionalId()}~ to spawn it immediately.");
        }

        return true;
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

            if (p.IsInAnyVehicle(false))
            {
                p.Tasks.LeaveVehicle(LeaveVehicleFlags.None);
                continue;
            }

            if (p.IsOccupied())
            {
                SwitchToState(PoliceUnitState.Engaging);
                return;
            }
        }

        if (DateTimeOffset.UtcNow >= _waitUntil)
        {
            SwitchToState(PoliceUnitState.ReturnToCar);

            if (_vehicle.Exists() && _vehicle!.IsAlive)
            {
                foreach (var (ped, _) in _peds)
                {
                    if (!ped.Exists() || ped.IsInjured)
                    {
                        continue;
                    }

                    var vehPos = _vehicle.Position;
                    Natives.TaskFollowNavMeshToCoord(ped.Handle,
                        vehPos.X,
                        vehPos.Y,
                        vehPos.Z,
                        0.35f,
                        -1,
                        7f,
                        4 /* Don't wait for entire navmesh */,
                        _vehicle.Heading);
                }
            }

            return;
        }
    }

    private void Engaging()
    {
        var hasOccupied = false;
        for (int i = 0; i < _peds!.Count; i++)
        {
            var p = _peds[i].Ped;
            if (!p.Exists())
            {
                continue;
            }

            if (p.IsOccupied())
            {
                hasOccupied = true;

                break;
            }
        }

        if (!hasOccupied)
        {
            if (!_engageTimerRunning)
            {
                _waitUntil = DateTimeOffset.UtcNow + WaitLength;
                _engageTimerRunning = true;
            }
            else
            {
                SwitchToState(PoliceUnitState.Arrived);
            }
        }
    }

    private void ReturnToCar()
    {
        if (Natives.IsEntityDead(_vehicle!.Handle, false)
            || _driver.IsInjured())
        {
            Cleanup(false);
            return;
        }

        if (_peds!.FindIndex(x => !x.Ped.IsInjured() && x.Ped.DistanceTo(_vehicle.Position) > 20.5f)
            == -1)
        {
#if DEBUG
            Game.DisplayNotification("Entering car.");
#endif
            SwitchToState(PoliceUnitState.EnterCar);
            return;
        }
        else if (DateTimeOffset.UtcNow >= _waitUntil)
        {
#if DEBUG
            Game.DisplayNotification("Timeout wrap.");
#endif
            SwitchToState(PoliceUnitState.Finished);
            return;
        }
    }

    private void EnterCar()
    {
        if (Natives.IsEntityDead(_vehicle!.Handle, false)
            || _driver.IsInjured())
        {
            Cleanup(false);
            return;
        }

#if DEBUG
        if (DateTimeOffset.UtcNow >= _waitUntil)
        {
            Game.DisplayNotification("Timeout when sitting in.");
        }
#endif

        if (_peds!.FindIndex(x => !x.Ped.IsInjured()
                && !Natives.IsPedSittingInVehicle(x.Ped.Handle, _vehicle.Handle))
            == -1
            || DateTimeOffset.UtcNow >= _waitUntil)
        {
            SwitchToState(PoliceUnitState.Finished);
        }
    }

    private void Finished()
    {
        if (!_vehicle.Exists()
            || !_vehicle!.IsOnScreen
            || _vehicle.DistanceTo2D(Game.LocalPlayer.Character) >= CleanupThreshold)
        {
            Cleanup(true);
        }

        if (!_driver.Exists() || _driver!.IsInjured)
        {
            Cleanup(false);
        }
    }
}
#endif