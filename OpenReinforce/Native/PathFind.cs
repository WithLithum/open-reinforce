// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using Rage;
using WithLithum.NativeWrapper;

namespace OpenReinforce.Native;

internal static class PathFind
{
    internal static bool GetNearestVehicleNodeWithHeading(Vector3 position,
        out Vector3 nodePosition,
        out float nodeHeading,
        VehicleNodeFilters filters,
        float zMeasureMult,
        int zTolerance)
    {
        var outNodePosition = Vector3.Zero;
        var outNodeHeading = 0f;

        var success = Natives.GetClosestVehicleNodeWithHeading(position.X,
            position.Y,
            position.Z,
            ref outNodePosition,
            ref outNodeHeading,
            (int)filters,
            zMeasureMult,
            zTolerance);

        nodePosition = outNodePosition;
        nodeHeading = outNodeHeading;
        return success;
    }
}