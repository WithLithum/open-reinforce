// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

#if LSPDFR

using LSPD_First_Response.Mod.API;

namespace OpenReinforce;

public class Main : Plugin
{
    public override void Initialize()
    {
        OpenReinforcePlugin.Initialize();
    }

    public override void Finally()
    {
        OpenReinforcePlugin.Finally();
    }
}

#endif