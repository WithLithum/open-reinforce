// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using OpenReinforce.Engine.Configuration;
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
    private List<PedInfo>? _peds;
    private Ped? _driver;
    private Blip? _vehicleBlip;

    private readonly record struct PedInfo(Ped Ped, int Seat);

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

        var vehicleModel = ItemSelector.PickByChance(_loadout.Vehicles);

        // Create vehicle
        // TODO replace models with actual vehicles & peds
        _vehicle = new Vehicle(vehicleModel.ModelHash, spawnPos);
        if (!_vehicle.IsValid())
        {
            Game.LogTrivial("OpenReinforce: Cop car spawn failed");
            Game.LogTrivial("OpenReinforce: Check your configured vehicle model name");
            return false;
        }
        _vehicle.IsPersistent = true;
        vehicleModel.Apply(_vehicle);

        _vehicleBlip = _vehicle.AttachBlip();
        _vehicleBlip.IsFriendly = true;
        _vehicleBlip.Flash(1000, 0);
        _vehicleBlip.Name = "Cop Car";

        var maxSeats = Natives.GetVehicleMaxNumberOfPassengers(_vehicle.Handle)
             - 1;

        var seatIndex = -1;
        var numPeds = MathHelper.GetRandomInteger(_loadout.MaximumPeds,
            _loadout.MaximumPeds);
        _peds = new List<PedInfo>(numPeds);
        for (int i = 0; i < numPeds; i++)
        {
            if (seatIndex > maxSeats)
            {
                Log.Warn("NumPeds greater than vehicle seats! Not spawning more.");
                break;
            }

            var model = new Model("S_M_Y_COP_01");
            model.LoadAndWait();
            if (!model.IsLoaded)
            {
                Log.Warn("Cop model load failed. Does it exist?");
                return false;
            }

            var pedHandle = Natives.CreatePedInsideVehicle(_vehicle.Handle,
                0,
                model.Hash,
                seatIndex,
                false,
                false);
            if (!Natives.DoesEntityExist(pedHandle))
            {
                Game.LogTrivial($"OpenReinforce: Failed to spawn ped on seat {i}");
                continue;
            }

            var ped = World.GetEntityByHandle<Ped>(pedHandle);
            ped.IsPersistent = true;
            ped.BlockPermanentEvents = true;

            if (!OpenReinforcePlugin.IsTestPlugin)
            {
                FrFunctions.SetPedAsCop(ped);
            }
            else
            {
                ped.RelationshipGroup = RelationshipGroup.Cop;
            }

            // TODO properly assign configured weapon!
            if (seatIndex == -1)
            {
                _driver = ped;
                _driver.KeepTasks = true;
                ped.Inventory.GiveNewWeapon(WeaponHash.CombatPistol, short.MaxValue, true);
            }
            else
            {
                ped.Inventory.GiveNewWeapon(WeaponHash.PumpShotgun, short.MaxValue, true);
            }

            _peds.Add(new PedInfo(ped, seatIndex));

            seatIndex++;
            model.Dismiss();
        }

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
