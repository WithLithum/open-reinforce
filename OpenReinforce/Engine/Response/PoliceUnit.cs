// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using OpenReinforce.Engine.Configuration;
using OpenReinforce.Engine.Entities;
using OpenReinforce.Native.Interop;
using OpenReinforce.Utilities;
using Rage;
using WithLithum.NativeWrapper;

namespace OpenReinforce.Engine.Response;

internal partial class PoliceUnit : IResponseUnit
{
    private static readonly TimeSpan WrapWaitLength = TimeSpan.FromSeconds(30);
    private static readonly TimeSpan AudioLockLength = TimeSpan.FromSeconds(5);

    private const float ArrivalThreshold = 25f;
    private const float ApproachThreshold = 85f;

    private const float MinSpawnDistance = 200f;
    private const float MaxSpawnDistance = 500f;

    private readonly LoadoutInfo _loadout;

    private PoliceUnitState _state;
    private Vector3 _destination;

    private bool _wrapEnabled;
    private DateTimeOffset _wrapWait;

    private Vehicle? _vehicle;
    private List<SpawnedPedInfo>? _peds;
    private Ped? _driver;
    private Blip? _vehicleBlip;

    public PoliceUnit(LoadoutInfo loadout)
    {
        _loadout = loadout;
    }

    public bool IsRunning { get; private set; }

    public void Start(Vector3 destination)
    {
        _destination = destination;
        if (!CreateEntities(destination))
        {
            return;
        }

        // Calculate a viable point for the unit to stop.
        var streetDestination = World.GetNextPositionOnStreet(_destination);
        if (streetDestination == Vector3.Zero
            || streetDestination.DistanceTo2D(_destination) >= 10f)
        {
            streetDestination = _destination;
        }
        _destination = streetDestination;

        // Start driving towards us.
        _vehicle!.IsSirenOn = true;

        IsRunning = true;
        SwitchToState(PoliceUnitState.GoingToDestination);

        // Play report
        if (!AudioLock.IsHeld())
        {
            AudioLock.LockAudio(AudioLockLength);

            if (!OpenReinforcePlugin.IsTestPlugin)
            {
                FrFunctions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_OFFICER_IN_NEED_OF_ASSISTANCE_01 IN_OR_ON_POSITION INSERT INTRO UNITS_RESPOND_CODE_03", _destination);
            }
        }
    }

    private bool CreateEntities(Vector3 destination)
    {
        // Get pos
        var spawnPos = World.GetNextPositionOnStreet(destination.Around2D(
            MathHelper.GetRandomSingle(MinSpawnDistance, MaxSpawnDistance)));

        // Create vehicle
        var numPeds = MathHelper.GetRandomInteger(_loadout.MaximumPeds,
            _loadout.MaximumPeds);
        _peds = new List<SpawnedPedInfo>(numPeds);

        if (!LoadoutSpawner.SpawnLoadout(_loadout,
            spawnPos,
            0,
            out _vehicle,
            out _driver,
            _peds))
        {
            return false;
        }

        _vehicleBlip = _vehicle.AttachBlip();
        _vehicleBlip.IsFriendly = true;
        _vehicleBlip.Flash(1000, 0);
        _vehicleBlip.Name = "Cop Car";

        return true;
    }

    private void DetectAndRecoverVehicle()
    {
        if (Natives.IsVehicleStuckOnRoof(_vehicle!.Handle))
        {
            // Flip the vehicle. If possible, put it in the nearest street position.
            _vehicle.SetRotationRoll(0);
            _vehicle.SetRotationPitch(0);

            var nextPos = World.GetNextPositionOnStreet(_vehicle.Position);
            if (nextPos != Vector3.Zero)
            {
                _vehicle.Position = nextPos;
            }
            return;
        }
    }

    public void Cleanup(bool force)
    {
        IsRunning = false;

        if (_peds != null)
        {
            foreach (var p in _peds)
            {
                p.Ped.Cleanup(force);
            }
        }

        if (force)
        {
            _vehicle.Cleanup(true);
        }
        _vehicleBlip.Cleanup();
    }
}
