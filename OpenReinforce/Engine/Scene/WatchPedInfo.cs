// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace OpenReinforce.Engine.Scene;

using Rage;

internal sealed class WatchPedInfo
{
    public required WatchPedState State { get; set; }

    public int VehicleSeat { get; set; }
    public Vehicle? Vehicle { get; set; }
    public Blip? Blip { get; set; }

    public required Ped Ped { get; set; }

    public DateTimeOffset? Timeout { get; set; }

    public bool TaskedToFollow { get; set; }
}
