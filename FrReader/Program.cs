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


foreach (var item in agency.Items)
{
    var table = new Table
    {
        Title = new TableTitle($"Agency: {item.Name} ({item.ShortName}) - '{item.ScriptName}'")
    };
    table.AddColumn("Key");
    table.AddColumn("Value");
    table.HideHeaders();

    table.AddRow("Icon", $"{item.TextureDictionary ?? "N/A"} -> {item.TextureName ?? "N/A"}");
    table.AddRow("Inventory", $"{item.Inventory ?? "N/A"}");
    table.AddRow("Parent", $"{item.Parent ?? "N/A"}");

    if (item.Loadouts != null)
    {
        var loadoutMainTable = new Table()
            .HideHeaders()
            .AddColumn("Name")
            .AddColumn("Options")
            .ShowRowSeparators();

        foreach (var loadout in item.Loadouts)
        {
            var loadoutTable = new Table()
                .HideHeaders()
                .AddColumn("K")
                .AddColumn("V")
                .Border(TableBorder.None);
            loadoutTable.AddRow("Chance", $"{loadout.Chance}%");
            loadoutTable.AddRow("NumPeds", $"{loadout.NumPeds}");

            if (loadout.Flags != null)
            {
                loadoutTable.AddRow("Flags", string.Join(", ", loadout.Flags));
            }

            if (loadout.Vehicles != null)
            {
                var vehicleTable = new Table()
                    .HideHeaders()
                    .ShowRowSeparators()
                    .AddColumn("Model")
                    .AddColumn("Options");

                foreach (var vehicle in loadout.Vehicles)
                {
                    var vehicleItemTable = new Table()
                        .HideHeaders()
                        .Border(TableBorder.None)
                        .AddColumn("Key")
                        .AddColumn("Value");

                    vehicleItemTable.AddRow("Spawn chance", vehicle.Chance.ToString());
                    vehicleItemTable.AddRow("Weapon", vehicle.Weapon ?? "None");
                    vehicleItemTable.AddRow("Livery", vehicle.LiveryMulti ?? vehicle.Livery.ToString());
                    vehicleItemTable.AddRow("Modkit liveries", vehicle.UseModKitLiveries.ToString());

                    vehicleTable.AddRow(new Markup(vehicle.Model ?? "No Model"), vehicleItemTable);
                }

                loadoutTable.AddRow(new Markup("Vehicles"), vehicleTable);
            }

            loadoutMainTable.AddRow(new Markup(loadout.Name ?? "(Unnamed)"), loadoutTable);
        }

        table.AddRow(new Markup("Loadouts"), loadoutMainTable);
    }

    AnsiConsole.Write(table);
}
