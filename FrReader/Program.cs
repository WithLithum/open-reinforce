using OpenReinforce.Engine.Data;
using OpenReinforce.Engine.Data.Models.Agencies;
using OpenReinforce.Viewer.UI;
using Spectre.Console;

Console.WriteLine("Enter the directory of the LSPDFR data directory:");
string? dataDir = Console.ReadLine();
if (string.IsNullOrWhiteSpace(dataDir) || !Directory.Exists(dataDir))
{
    Console.WriteLine("Invalid directory.");
    return;
}

var loader = new FrContainerDataLoader<FrAgency, FrAgencyFile>();
var agency = loader.Load(dataDir, "agency");

if (agency == null || agency.Items == null)
{
    Console.WriteLine("Failed to deserialize agency.");
    return;
}

var display = new AgencyDisplay(agency.Items);
while (!display.Quit)
{
    display.Run();
}