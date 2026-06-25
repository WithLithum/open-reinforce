// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using OpenReinforce.Engine.Response;
using OpenReinforce.UI;
using OpenReinforce.Utilities;
using Rage;
using WithLithum.NativeWrapper;

namespace OpenReinforce.Engine;

public static class ResponseManager
{
    private static readonly List<IResponseUnit> Units = [];

    public static void CreateResponse(ReinforceCategory category,
        ReinforceType type)
    {
        // Pick a loadout.
        var xyz = Game.LocalPlayer.Character.Position;
        var zone = Natives.GetNameOfZone(xyz.X, xyz.Y, xyz.Z);
        var loadout = OpenReinforcePlugin.LoadoutManager.PickLoadout(zone, type);
        if (loadout == null)
        {
            Log.Warn("Failed to pick a loadout");
            return;
        }

        // TODO properly implement this
        PoliceUnit? p = null;
        try
        {
            p = new PoliceUnit(loadout);
            p.Start(Game.LocalPlayer.Character.FrontPosition);
        }
        catch (Exception ex)
        {
            p?.Cleanup(true);
            Game.LogTrivial($"OpenReinforce: Unit failed to start: {ex}");
            return;
        }

        if (!p.IsRunning)
        {
            p.Cleanup(true);
            Game.LogTrivial("OpenReinforce: Unit gracefully failed to start");
        }
        Units.Add(p);
    }

    public static void Process()
    {
        for (int i = 0; i < Units.Count; i++)
        {
            var u = Units[i];
            if (u.IsRunning)
            {
                try
                {
                    u.Process();
                }
                catch (Exception ex)
                {
                    Game.LogTrivial("OpenReinforce: Whilst ticking a unit: ");
                    Game.LogTrivial(ex.ToString());
                    u.Cleanup(true);
                }
            }
            else
            {
                u.Cleanup(false);
                Units.RemoveAt(i);
            }
        }
    }

    public static void Cleanup()
    {
        foreach (var unit in Units)
        {
            unit.Cleanup(true);
        }
    }
}
