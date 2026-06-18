// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

#if GTA
using Rage;

namespace OpenReinforce.Engine.Response;

public interface IResponseUnit
{
    bool IsRunning { get; }

    void Start(Vector3 destination);

    void Process();

    void Cleanup(bool force);
}

#endif