// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Collections.Generic;
using OpenReinforce.Engine.Data.Models;

namespace OpenReinforce.Engine.Data
{
    public class FrContainerDataLoader<T, TContainer> : FrSimpleDataLoader<T, TContainer>
        where T : class, IFrKeyed
        where TContainer : class, IFrDataRoot<T>, new()
    {
        protected override void FilterItem(T item, List<T> list)
        {
            list.RemoveAll(existing => existing.GetKey() == item.GetKey());
        }
    }
}
