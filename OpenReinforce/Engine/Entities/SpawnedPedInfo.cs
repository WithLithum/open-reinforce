// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using Rage;

namespace OpenReinforce.Engine.Entities;

internal readonly record struct SpawnedPedInfo(Ped Ped,
    int SeatIndex);