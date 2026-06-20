#if GTA
using OpenReinforce.Native;
using OpenReinforce.Utilities;
using Rage;
using RAGENativeUI;
using System.Windows.Forms;
using WithLithum.NativeWrapper;

namespace OpenReinforce.Engine.Scene;

internal sealed class WatchManager
{
    private const float FaceToFaceThreshold = 10f;
    private const float FollowThreshold = 10.5f;

    private static readonly TimeSpan FaceToFaceTimeThreshold = TimeSpan.FromMilliseconds(500);
    private static readonly TimeSpan OccupiedTimeout = TimeSpan.FromSeconds(2);
    private static readonly TimeSpan DismissTimeout = TimeSpan.FromSeconds(20);

    private readonly List<WatchPedInfo> _peds = [];

    private readonly KeyDetector _dismissCurrentKey = new(Keys.U);
    private readonly KeyDetector _dismissEveryoneKey = new(Keys.Enter);

    private DateTimeOffset _faceToFaceTimer;
    private uint _currentlyFacingPed;

    public void Add(Ped ped, Vehicle? vehicle, int seat)
    {
        Checks.NotInjured(ped);

        if (!ped.IsPersistent)
        {
            ped.IsPersistent = true;
        }

        if (vehicle.Exists() && !vehicle!.IsPersistent)
        {
            vehicle.IsPersistent = true;
        }

        ped.BlockPermanentEvents = false;

        var pedBlip = ped.AttachBlip();
        if (pedBlip.IsValid())
        {
            pedBlip.Sprite = BlipSprite.Enemy;
            pedBlip.SetColour(BlipColour.Blue);
            pedBlip.SetLocalizedName(0x38302786);
            pedBlip.IsFriendly = true;
            pedBlip.Scale = 0.5f;
        }

        _peds.Add(new WatchPedInfo
        {
            Ped = ped,
            Vehicle = vehicle,
            VehicleSeat = seat,
            Blip = pedBlip,
            State = WatchPedState.None
        });
    }

    public void Process()
    {
        if (_peds.Count == 0)
        {
            return;
        }

        _dismissCurrentKey.Process();
        _dismissEveryoneKey.Process();

        var isFacingAnybody = false;
        for (int i = 0; i < _peds.Count; i++)
        {
            ProcessPed(i, ref isFacingAnybody);
        }

        if (!isFacingAnybody)
        {
            _currentlyFacingPed = 0;
            _dismissCurrentKey.ResetTimer();
        }

        if (_dismissEveryoneKey.IsHeldDown()
            && _peds.FindIndex(x => x.State == WatchPedState.None) != -1)
        {
            Game.DisplayHelp("All idle collagues are being dismissed.");
            DismissEveryone();
            _dismissEveryoneKey.Reset();
        }
    }

    private void DismissEveryone()
    {
        for (int i = 0; i < _peds.Count; i++)
        {
            WatchPedInfo? p = _peds[i];
            if (!p.Ped.IsInjured() && p.State == WatchPedState.None)
            {
                DismissCollague(p, i);
            }
        }
    }

    private void ProcessPed(int index, ref bool isFacingAnybody)
    {
        var info = _peds[index];

        if (info.Ped.IsInjured() || info.State == WatchPedState.Dismissed)
        {
            info.Ped.Cleanup();
            _peds.RemoveAt(index);
            return;
        }

        if (info.Vehicle.Exists()
            && ProcessVehicleDismiss(info))
        {
            return;
        }

        if (info.Ped.IsOccupied() && info.State != WatchPedState.Engaged)
        {
            info.State = WatchPedState.Engaged;
            return;
        }

        if (info.State == WatchPedState.Engaged
            && !info.Ped.IsOccupied())
        {
            if (!info.Timeout.HasValue)
            {
                info.Timeout = DateTimeOffset.UtcNow + OccupiedTimeout;
            }
            else if (DateTimeOffset.UtcNow >= info.Timeout)
            {
                info.State = WatchPedState.None;
                info.Timeout = null;
                Natives.TaskSwapWeapon(info.Ped.Handle, false);
            }

            return;
        }

        if (info.State == WatchPedState.None)
        {
            ProcessPedInteraction(info, index, ref isFacingAnybody);
        }
    }

    private bool ProcessVehicleDismiss(WatchPedInfo info)
    {
        var veh = info.Vehicle!;

        // Checks for timeout or unsatisfactory conditions with the car.
        if (info.State is WatchPedState.EnteringVehicle)
        {
            if (DateTimeOffset.UtcNow >= info.Timeout)
            {
                info.Ped.WarpIntoVehicle(veh, info.VehicleSeat);
                info.State = WatchPedState.AwaitingDismiss;
                info.Timeout = null;
                return true;
            }

            // Check for when it is simply not worth getting into the car.
            // If these happens, the ped will simply walk off.
            if (!veh.Exists()
                || veh.IsDead
                || Natives.IsVehicleStuckOnRoof(veh.Handle))
            {
                info.State = WatchPedState.Dismissed;
                return true;
            }
        }

        if (info.State == WatchPedState.EnteringVehicle
            && info.Ped.IsSittingInVehicle(veh))
        {
            // Off we go to Awaiting Dismiss state state.

            info.State = WatchPedState.AwaitingDismiss;
            info.Timeout = DateTimeOffset.UtcNow + DismissTimeout;

            return true;
        }
        else if (info.State == WatchPedState.AwaitingDismiss
            && _peds.FindIndex(x =>
                x.Vehicle?.Handle == veh.Handle
                && !x.Ped.IsInjured()
                && !x.Ped.IsSittingInVehicle(veh)) == -1)
        {
            // Tell driver to get away.
            var driver = veh.Driver;
            if (!driver.IsInjured() && !driver.IsPlayer)
            {
                driver.Tasks.CruiseWithVehicle(Constants.CruiseSpeed,
                    VehicleDrivingFlags.Normal);
            }

            _peds.RemoveAll(x => x.Vehicle?.Handle == veh.Handle);
            veh.IsSirenOn = false;

            OpenReinforcePlugin.DismissManager.Add(veh);

            return true;
        }

        return false;
    }

    private void ProcessPedInteraction(WatchPedInfo info, int index, ref bool facingAnybody)
    {
        var ped = info.Ped;
        var pc = Game.LocalPlayer.Character;
        if (ped.IsInjured() || ped.IsOccupied())
        {
            info.TaskedToFollow = false;
            return;
        }

        // Check for ped distance and tell them to follow.
        var pedDistance = ped.DistanceTo(pc);
        if (pedDistance >= FollowThreshold && !info.TaskedToFollow)
        {
            ped.Tasks.Clear();
            // Tell the ped to get over to the player.
            Natives.TaskFollowToOffsetOfEntity(ped.Handle,
                pc.Handle,
                Vector3.RelativeFront.X,
                Vector3.RelativeFront.Y,
                Vector3.RelativeFront.Z,
                2.0f,
                -1,
                FollowThreshold,
                true
                );
            ped.Tasks.FollowToOffsetFromEntity(pc, Vector3.RelativeFront);
            info.TaskedToFollow = true;
        }
        else if (pedDistance < FollowThreshold && info.TaskedToFollow)
        {
            ped.Tasks.Clear();
            ped.Tasks.StandStill(-1);
            info.TaskedToFollow = false;
        }

        if (!facingAnybody
            && ped.DistanceTo(pc) <= 6.5f
            && Natives.IsPedFacingPed(pc.Handle, ped.Handle, FaceToFaceThreshold))
        {
            if (_currentlyFacingPed != info.Ped.Handle)
            {
                _currentlyFacingPed = info.Ped.Handle;
                _faceToFaceTimer = DateTimeOffset.UtcNow + FaceToFaceTimeThreshold;
                _dismissCurrentKey.ResetTimer();
            }

            facingAnybody = true;
        }

        if (facingAnybody && DateTimeOffset.UtcNow >= _faceToFaceTimer)
        {
            Game.DisplayHelp($"Hold down ~{Keys.U.GetInstructionalId()}~ to dismiss this collague.",
                500);

            if (_dismissCurrentKey.IsHeldDown() && _currentlyFacingPed == info.Ped.Handle)
            {
                Game.HideHelp();
                DismissCollague(info, index);
            }
        }
    }

    private void DismissCollague(WatchPedInfo info, int index)
    {
        info.Ped.Tasks.ClearImmediately();

        if (info.Ped.IsInjured())
        {
            info.State = WatchPedState.Dismissed;
            _peds.RemoveAt(index);
            return;
        }

        if (info.Vehicle.Exists() && info.Vehicle!.IsAlive)
        {
            info.State = WatchPedState.EnteringVehicle;
            Natives.TaskEnterVehicle(info.Ped.Handle,
                info.Vehicle.Handle,
                15000,
                info.VehicleSeat,
                2.0f,
                (int)(EnterExitVehicleOptions.BlockSeatShuffling | EnterExitVehicleOptions.WrapIfDoorIsBlocked),
                null!);
            info.Blip.Cleanup();
            info.Timeout = DateTimeOffset.UtcNow + DismissTimeout;
            return;
        }

        info.Ped.Dismiss();
        _peds.RemoveAt(index);
    }

    public void Cleanup()
    {
        foreach (var p in _peds)
        {
            p.Ped.Cleanup(true);
            p.Vehicle.Cleanup(true);
        }
    }
}
#endif
