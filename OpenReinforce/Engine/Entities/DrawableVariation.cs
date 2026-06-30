// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using OpenReinforce.Utilities;
using Rage;
using WithLithum.NativeWrapper;

namespace OpenReinforce.Engine.Entities;

public readonly struct DrawableVariation : IEquatable<DrawableVariation>
{
    public const int DefaultValue = -999;

    public DrawableVariation() : this(DefaultValue, DefaultValue)
    {
    }

    public DrawableVariation(int drawable, int texture)
    {
        Drawable = drawable;
        Texture = texture;
    }

    public int Drawable { get; }

    public int Texture { get; }

    public void Apply(Ped ped, ComponentSlot slot)
    {
        Natives.SetPedComponentVariation(ped.Handle,
            (int)slot,
            Drawable,
            Texture,
            0);
    }

    public void Apply(Ped ped, PedPropAnchor anchor, bool attach = true)
    {
        Natives.SetPedPropIndex(ped.Handle,
            (int)anchor,
            Drawable,
            Texture,
            attach,
            0);
    }

    public bool Equals(DrawableVariation other)
    {
        return other.Drawable == Drawable && other.Texture == Texture;
    }

    public override bool Equals(object obj)
    {
        return obj is DrawableVariation variation && Equals(variation);
    }

    public override int GetHashCode()
    {
        int hashCode = -985608997;
        hashCode = hashCode * -1521134295 + Drawable.GetHashCode();
        hashCode = hashCode * -1521134295 + Texture.GetHashCode();
        return hashCode;
    }
}
