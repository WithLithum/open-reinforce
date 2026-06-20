// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace OpenReinforce.Engine.Data.Models
{
    public interface IFrDataRoot<T>
    {
        public T[]? Items { get; set; }
    }
}
