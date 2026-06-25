using OpenReinforce.Engine.Data;
using OpenReinforce.Engine.Data.Models.Agencies;
using OpenReinforce.Engine.Data.Models.Regions;
using OpenReinforce.Engine.Data.Models.Response;
using OpenReinforce.Engine.Data.Utilities;
using OpenReinforce.Viewer.UI;
using Spectre.Console;

Console.WriteLine("Enter the directory of the LSPDFR data directory:");
string? dataDir = Console.ReadLine();
if (string.IsNullOrWhiteSpace(dataDir) || !Directory.Exists(dataDir))
{
    Console.WriteLine("Invalid directory.");
    return;
}

AnsiConsole.MarkupLine(":office_building: [teal]Loading agencies[/]");
var loader = new FrContainerDataLoader<FrAgency, FrAgencyFile>();
var agency = loader.Load(dataDir, "agency");

AnsiConsole.MarkupLine(":map: [yellow]Loading regions[/]");
var regionLoader = new FrContainerDataLoader<FrRegion, FrRegionFile>();
var region = regionLoader.Load(dataDir, "regions");

AnsiConsole.MarkupLine(":package: [yellow]Loading backups[/]");
var backups = FrDirectory.ReadData(dataDir,
    "backup",
    static r => ResponseTableReader.ReadTable(r),
    static (a, b) => a.Merge(b));

if (agency == null || agency.Items == null ||
    region == null ||
    backups == null)
{
    Console.WriteLine("Failed to deserialize data.");
    return;
}

var display = new ViewSelectorDisplay(agency.Items, region.Items!, backups);
Displays.Run(display);
Console.Clear();