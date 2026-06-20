namespace OpenReinforce.Native;

[Flags]
public enum VehicleNodeFilters
{
    IncludeSwitchedOffNodes = 1,
    IncludeBoatNodes = 2,
    IgnoreSlipLanes = 4,
    IgnoreSwitchedOffDeadEnds = 8,
    GetHeading = 256,
    FavourFacing = 512
}