# 01: Watch system

> [!NOTE]
> The key words "MUST", "MUST NOT", "REQUIRED", "SHALL", "SHALL NOT", "SHOULD", "SHOULD NOT", "RECOMMENDED", "NOT RECOMMENDED", "MAY", and "OPTIONAL" in this document are to be interpreted as described in BCP 14 [RFC2119] [RFC8174] when, and only when, they appear in all capitals, as shown here.

Police backup units, on arrival, will exit their vehicle and be transferred to the Watch Manager.

## 0. Definitions

- _collague ped_: a police ped managed by Watch Manager
- _occupied_: when `PedExtensions.IsOccupied` returns `true`

## 1. Behaviour

The default behaviour for collague peds is to follow the player. If the player is not moving for more than 5 seconds, all collague peds will remain still.

Collague peds will also remain still if they are within 10 metres of distance from player.

They will also switch to their assigned primary weapon (handgun or long gun) whenever player selected an active weapon that is a firearm.

### Occupied peds

Whenever a ped becomes occupied, the Watch Manager MUST stop assiging tasks to it until it is no longer occupied.

A currently occupied collague ped cannot be interacted with and cannot be dismissed by holding down the Dismiss All key bind.

## 2. Interactions

Player can interact with a _still_ and _unoccupied_ collague ped by standing still to that ped face to face.

### Dismiss

A collague ped can be dismissed by pressing the _Dismiss Collague_ key.

When dismissed, and the original vehicle of that collague ped still exists and is alive, the collague ped will return to their seat. If the driver of a vehicle is dismissed, all passengers from that vehicle will be dismissed as well.

If a collague ped does not have an original vehicle or the vehicle is not alive, the collague ped will simply walk away - `Dismiss()` is called then and there.

Script command [`IS_PED_FACING_PED`](https://docs.fivem.net/natives/?_0xD71649DB0A545AA3), with the angle range specified as 10 degrees, is used to to determine whether the ped is standing face to face with a collague ped.

[RFC2119]: https://www.rfc-editor.org/info/rfc2119/
[RFC8174]: https://www.rfc-editor.org/info/rfc8174/
