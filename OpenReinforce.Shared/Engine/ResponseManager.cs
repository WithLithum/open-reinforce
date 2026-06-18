// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

#if GTA

using OpenReinforce.Engine.Response;
using OpenReinforce.UI;
using Rage;

#if LSPDFR
using LSPD_First_Response.Mod.API;
#endif

namespace OpenReinforce.Engine;

public static class ResponseManager
{
    private static readonly List<IResponseUnit> Units = [];

    public static void CreateResponse(ReinforceCategory category,
        ReinforceType type)
    {
        // TODO properly implement this
        PoliceUnit? p = null;
        try
        {
            p = new PoliceUnit();
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

#endif