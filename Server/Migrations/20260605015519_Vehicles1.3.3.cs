using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LMUSessionTracker.Server.Migrations
{
    /// <inheritdoc />
    public partial class Vehicles133 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jens Reno Moller", "55_GMB2DFFBF6F" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "DK", "Bronze" });

            migrationBuilder.InsertData(
                table: "VehicleModels",
                columns: new[] { "Id", "Engine", "Manufacturer", "Name" },
                values: new object[] { "ADESS_AD25_LMP3", "Toyota V35A-FTS 3500 cc V6 twin-turbocharged", "ADESS", "ADESS AD25 LMP3" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "19_26_GENE467AE018",
                column: "Series",
                value: "WEC2026");

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "Class", "Custom", "Livery", "Model", "Name", "Number", "Series", "Team" },
                values: new object[,]
                {
                    { "007_26_THO22699059", "Hypercar", false, "WEC", "AstonMartin_Valkyrie", "Aston Martin THOR Team 2026 #007:WEC", "007", "WEC2026", "Aston Martin THOR Team" },
                    { "009_26_THO14274046", "Hypercar", false, "WEC", "AstonMartin_Valkyrie", "Aston Martin THOR Team 2026 #009:WEC", "009", "WEC2026", "Aston Martin THOR Team" },
                    { "1_26_GCHAL73279592", "GT3", false, "Logitech", "McLaren_720S_LMGT3_Evo", "Logitech G Challenge #1:LGC", "1", "Logitech2026", "Logitech G Challenge" },
                    { "10_26_GARA63384034", "GT3", false, "WEC", "McLaren_720S_LMGT3_Evo", "Garage 59 2026 #10:WEC", "10", "WEC2026", "Garage 59" },
                    { "10_26_GCHA17031780", "GT3", false, "Logitech", "McLaren_720S_LMGT3_Evo", "Logitech G Challenge #10:LGC", "10", "Logitech2026", "Logitech G Challenge" },
                    { "11_26_GCHA47509705", "GT3", false, "Logitech", "McLaren_720S_LMGT3_Evo", "Logitech G Challenge #11:LGC", "11", "Logitech2026", "Logitech G Challenge" },
                    { "12_26_GCHA18747807", "GT3", false, "Logitech", "McLaren_720S_LMGT3_Evo", "Logitech G Challenge #12:LGC", "12", "Logitech2026", "Logitech G Challenge" },
                    { "12_26_JOTA50003117", "Hypercar", false, "WEC", "Cadillac_V_lmdh", "Cadillac Hertz Team Jota 2026 #12:WEC", "12", "WEC2026", "Cadillac Hertz Team Jota" },
                    { "15_26_WRT_37164931", "Hypercar", false, "WEC", "BMW_M_Hybrid", "BMW M Team WRT 2026 #15:WEC", "15", "WEC2026", "BMW M Team WRT" },
                    { "17_26_GENE34E4954B", "Hypercar", false, "Le Mans", "Genesis_GMR001", "Genesis Magma Racing 2026 #17:LM", "17", "WEC2026", "Genesis Magma Racing" },
                    { "19_26_GENE5D7465E6", "Hypercar", false, "Le Mans", "Genesis_GMR001", "Genesis Magma Racing 2026 #19:LM", "19", "WEC2026", "Genesis Magma Racing" },
                    { "2_26_GCHAL98500904", "GT3", false, "Logitech", "McLaren_720S_LMGT3_Evo", "Logitech G Challenge #2:LGC", "2", "Logitech2026", "Logitech G Challenge" },
                    { "20_26_WRT_31540483", "Hypercar", false, "WEC", "BMW_M_Hybrid", "BMW M Team WRT 2026 #20:WEC", "20", "WEC2026", "BMW M Team WRT" },
                    { "21_26_AFCO95641716", "GT3", false, "WEC", "Ferrari_296_LMGT3", "Vista AF Corsa 2026 #21:WEC", "21", "WEC2026", "Vista AF Corsa" },
                    { "23_26_THOR59931582", "GT3", false, "WEC", "AMR_LMGT3", "Heart of Racing Team 2026 #23:WEC", "23", "WEC2026", "Heart of Racing Team" },
                    { "27_26_THOR22130173", "GT3", false, "WEC", "AMR_LMGT3", "Heart of Racing Team 2026 #27:WEC", "27", "WEC2026", "Heart of Racing Team" },
                    { "3_26_GCHAL41540517", "GT3", false, "Logitech", "McLaren_720S_LMGT3_Evo", "Logitech G Challenge #3:LGC", "3", "Logitech2026", "Logitech G Challenge" },
                    { "32_26_WRT_83524148", "GT3", false, "WEC", "BMW_M4_LMGT3", "Team WRT 2026 #32:WEC", "32", "WEC2026", "Team WRT" },
                    { "33_26_TFSP28162365", "GT3", false, "WEC", "Corvette_Z06_LMGT3R", "TF Sport 2026 #33:WEC", "33", "WEC2026", "TF Sport" },
                    { "34_26_TFSP47451091", "GT3", false, "WEC", "Corvette_Z06_LMGT3R", "Racing Team Turkey by TF 2026 #34:WEC", "34", "WEC2026", "Racing Team Turkey by TF" },
                    { "35_26_ALPI41651716", "Hypercar", false, "WEC", "Alpine_A424", "Alpine Endurance Team 2026 #35:WEC", "35", "WEC2026", "Alpine Endurance Team" },
                    { "36_26_ALPI82796866", "Hypercar", false, "WEC", "Alpine_A424", "Alpine Endurance Team 2026 #36:WEC", "36", "WEC2026", "Alpine Endurance Team" },
                    { "38_26_JOTA51270994", "Hypercar", false, "WEC", "Cadillac_V_lmdh", "Cadillac Hertz Team Jota 2026 #38:WEC", "38", "WEC2026", "Cadillac Hertz Team Jota" },
                    { "397_26_296GT3", "GT3", true, "Custom", "Ferrari_296_LMGT3", "296GT3 Custom Team 2026 #397", "397", "WEC2026", "Custom Team" },
                    { "397_26_499P", "Hypercar", true, "Custom", "Ferrari_499P", "499P Custom Team 2026 #397", "397", "WEC2026", "Custom Team" },
                    { "397_26_911GT3R", "GT3", true, "Custom", "Porsche_911_GT3_R_LMGT3", "911GT3R Custom Team 2026 #397", "397", "WEC2026", "Custom Team" },
                    { "397_26_9X8W", "Hypercar", true, "Custom", "Peugeot_9x8_Wing", "9x8 Wing Custom Team 2026 #397", "397", "WEC2026", "Custom Team" },
                    { "397_26_ALPINE", "Hypercar", true, "Custom", "Alpine_A424", "Alpine Custom Team 2026 #397", "397", "WEC2026", "Custom Team" },
                    { "397_26_AMG", "GT3", true, "Custom", "Mercedes_AMG_GT3", "Mercedes AMG Custom Team 2026 #397", "397", "WEC2026", "Custom Team" },
                    { "397_26_AMV", "GT3", true, "Custom", "AMR_LMGT3", "AMR GT3 Custom Team 2026 #397", "397", "WEC2026", "Custom Team" },
                    { "397_26_AMVALK", "Hypercar", true, "Custom", "AstonMartin_Valkyrie", "AM Valkyrie Custom Team 2026 #397", "397", "WEC2026", "Custom Team" },
                    { "397_26_BMW", "GT3", true, "Custom", "BMW_M4_LMGT3", "BMW GT3 Custom Team 2026 #397", "397", "WEC2026", "Custom Team" },
                    { "397_26_BMWMH", "Hypercar", true, "Custom", "BMW_M_Hybrid", "BMWMH Custom Team 2026 #397", "397", "WEC2026", "Custom Team" },
                    { "397_26_GR010", "Hypercar", true, "Custom", "Toyota_GR010", "Toyota TR010 Custom Team 2026 #397", "397", "WEC2026", "Custom Team" },
                    { "397_26_LEXUS", "GT3", true, "Custom", "Lexus_RCF_GT3", "Lexus Custom Team 2026 #397", "397", "WEC2026", "Custom Team" },
                    { "397_26_MCLAREN", "GT3", true, "Custom", "McLaren_720S_LMGT3_Evo", "McLaren Custom Team 2026 #397", "397", "WEC2026", "Custom Team" },
                    { "397_26_MUSTANG", "GT3", true, "Custom", "Ford_Mustang_LMGT3", "Mustang Custom Team 2026 #397", "397", "WEC2026", "Custom Team" },
                    { "397_26_VLMDH", "Hypercar", true, "Custom", "Cadillac_V_lmdh", "VLMDH Custom Team 2026 #397", "397", "WEC2026", "Custom Team" },
                    { "397_26_Z06GT3R", "GT3", true, "Custom", "Corvette_Z06_LMGT3R", "Z06GT3R Custom Team 2026 #397", "397", "WEC2026", "Custom Team" },
                    { "4_26_GCHAL72398716", "GT3", false, "Logitech", "McLaren_720S_LMGT3_Evo", "Logitech G Challenge #4:LGC", "4", "Logitech2026", "Logitech G Challenge" },
                    { "5_26_GCHAL67256536", "GT3", false, "Logitech", "McLaren_720S_LMGT3_Evo", "Logitech G Challenge #5:LGC", "5", "Logitech2026", "Logitech G Challenge" },
                    { "50_26_FERR90502958", "Hypercar", false, "WEC", "Ferrari_499P", "Ferrari AF Corse 2026 #50:WEC", "50", "WEC2026", "Ferrari AF Corse" },
                    { "51_26_FERR16908376", "Hypercar", false, "WEC", "Ferrari_499P", "Ferrari AF Corse 2026 #51:WEC", "51", "WEC2026", "Ferrari AF Corse" },
                    { "54_26_AFCO96652607", "GT3", false, "WEC", "Ferrari_296_LMGT3", "Vista AF Corsa 2026 #54:WEC", "54", "WEC2026", "Vista AF Corsa" },
                    { "58_26_GARA17941687", "GT3", false, "WEC", "McLaren_720S_LMGT3_Evo", "Garage 59 2026 #58:WEC", "58", "WEC2026", "Garage 59" },
                    { "6_26_GCHAL89066091", "GT3", false, "Logitech", "McLaren_720S_LMGT3_Evo", "Logitech G Challenge #6:LGC", "6", "Logitech2026", "Logitech G Challenge" },
                    { "61_26_IRON17109319", "GT3", false, "Le Mans", "Mercedes_AMG_GT3", "Iron Lynx 2026 #61:LM", "61", "WEC2026", "Iron Lynx" },
                    { "61_26_IRON57024276", "GT3", false, "WEC", "Mercedes_AMG_GT3", "Iron Lynx 2026 #61:WEC", "61", "WEC2026", "Iron Lynx" },
                    { "62_26_IRON98036068", "GT3", false, "Le Mans", "Mercedes_AMG_GT3", "Team Qatar by Iron Lynx 2026 #62:LM", "62", "WEC2026", "Team Qatar by Iron Lynx" },
                    { "69_26_WRT_56324638", "GT3", false, "WEC", "BMW_M4_LMGT3", "Team WRT 2026 #69:WEC", "69", "WEC2026", "Team WRT" },
                    { "7_26_GCHAL34731486", "GT3", false, "Logitech", "McLaren_720S_LMGT3_Evo", "Logitech G Challenge #7:LGC", "7", "Logitech2026", "Logitech G Challenge" },
                    { "7_26_TOYOT80734264", "Hypercar", false, "WEC", "Toyota_GR010", "Toyota Racing 2026 #7:WEC", "7", "WEC2026", "Toyota Racing" },
                    { "77_26_PROT51390877", "GT3", false, "WEC", "Ford_Mustang_LMGT3", "Proton Competition 2026 #77:WEC", "77", "WEC2026", "Proton Competition" },
                    { "78_26_AKKO79996909", "GT3", false, "WEC", "Lexus_RCF_GT3", "Akkodis ASP Team 2026 #78:WEC", "78", "WEC2026", "Akkodis ASP Team" },
                    { "79_26_IRON66951108", "GT3", false, "Le Mans", "Mercedes_AMG_GT3", "Iron Lynx 2026 #79:LM", "79", "WEC2026", "Iron Lynx" },
                    { "79_26_IRON79382419", "GT3", false, "WEC", "Mercedes_AMG_GT3", "Iron Lynx 2026 #79:WEC", "79", "WEC2026", "Iron Lynx" },
                    { "8_26_GCHAL79481284", "GT3", false, "Logitech", "McLaren_720S_LMGT3_Evo", "Logitech G Challenge #8:LGC", "8", "Logitech2026", "Logitech G Challenge" },
                    { "8_26_TOYOT63257480", "Hypercar", false, "WEC", "Toyota_GR010", "Toyota Racing 2026 #8:WEC", "8", "WEC2026", "Toyota Racing" },
                    { "83_26_AFCO45368276", "Hypercar", false, "WEC", "Ferrari_499P", "AF Corse 2026 #83:WEC", "83", "WEC2026", "AF Corse" },
                    { "87_26_AKKO74271960", "GT3", false, "WEC", "Lexus_RCF_GT3", "Akkodis ASP Team 2026 #87:WEC", "87", "WEC2026", "Akkodis ASP Team" },
                    { "88_26_PROT29987739", "GT3", false, "WEC", "Ford_Mustang_LMGT3", "Proton Competition 2026 #88:WEC", "88", "WEC2026", "Proton Competition" },
                    { "9_26_GCHAL61794112", "GT3", false, "Logitech", "McLaren_720S_LMGT3_Evo", "Logitech G Challenge #9:LGC", "9", "Logitech2026", "Logitech G Challenge" },
                    { "91_26_MANT18218509", "GT3", false, "WEC", "Porsche_911_GT3_R_LMGT3", "Manthey DK Engineering 2026 #91:WEC", "91", "WEC2026", "Manthey DK Engineering" },
                    { "92_26_MANT22466058", "GT3", false, "WEC", "Porsche_911_GT3_R_LMGT3", "The Bend Manthey 2026 #92:WEC", "92", "WEC2026", "The Bend Manthey" },
                    { "93_26_PEUG26011673", "Hypercar", false, "WEC", "Peugeot_9x8", "Peugeot TotalEnergies 2026 #93:WEC", "93", "WEC2026", "Peugeot TotalEnergies" },
                    { "93_26_PEUG27100541", "Hypercar", false, "Le Mans", "Peugeot_9x8", "Peugeot TotalEnergies 2026 #93:LM", "93", "WEC2026", "Peugeot TotalEnergies" },
                    { "94_26_PEUG73522851", "Hypercar", false, "Le Mans", "Peugeot_9x8", "Peugeot TotalEnergies 2026 #94:LM", "94", "WEC2026", "Peugeot TotalEnergies" },
                    { "94_26_PEUG80967432", "Hypercar", false, "WEC", "Peugeot_9x8", "Peugeot TotalEnergies 2026 #94:WEC", "94", "WEC2026", "Peugeot TotalEnergies" }
                });

            migrationBuilder.InsertData(
                table: "VehicleDrivers",
                columns: new[] { "Name", "Veh", "Nationality", "Skill" },
                values: new object[,]
                {
                    { "Harry Tincknell", "007_26_THO22699059", "GB", "Platinum" },
                    { "Ross Gunn", "007_26_THO22699059", "GB", "Platinum" },
                    { "Tom Gamble", "007_26_THO22699059", "GB", "Gold" },
                    { "Alex Riberas", "009_26_THO14274046", "ES", "Gold" },
                    { "Marco Sorensen", "009_26_THO14274046", "DK", "Platinum" },
                    { "Roman De Angelis", "009_26_THO14274046", "CA", "Gold" },
                    { "Antares Au", "10_26_GARA63384034", "", "Bronze" },
                    { "Marvin Kirchhöfer", "10_26_GARA63384034", "", "" },
                    { "Tom Fleming", "10_26_GARA63384034", "GB", "Silver" },
                    { "Alex Lynn", "12_26_JOTA50003117", "GB", "Platinum" },
                    { "Norman Nato", "12_26_JOTA50003117", "FR", "Gold" },
                    { "Will Stevens", "12_26_JOTA50003117", "GB", "Platinum" },
                    { "Dries Vanthoor", "15_26_WRT_37164931", "BE", "Platinum" },
                    { "Kevin Magnussen", "15_26_WRT_37164931", "DK", "Platinum" },
                    { "Raffaele Marciello", "15_26_WRT_37164931", "IT", "Platinum" },
                    { "André Lotterer", "17_26_GENE34E4954B", "", "" },
                    { "Mathys Jaubert", "17_26_GENE34E4954B", "FR", "Silver" },
                    { "Pipo Derani", "17_26_GENE34E4954B", "BR", "Platinum" },
                    { "Daniel Juncadella", "19_26_GENE5D7465E6", "ES", "Platinum" },
                    { "Mathieu Jaminet", "19_26_GENE5D7465E6", "FR", "Platinum" },
                    { "Paul-Loup Chatin", "19_26_GENE5D7465E6", "FR", "Gold" },
                    { "Rene Rast", "20_26_WRT_31540483", "DE", "Platinum" },
                    { "Robin Frijns", "20_26_WRT_31540483", "NL", "Platinum" },
                    { "Sheldon van der Linde", "20_26_WRT_31540483", "ZA", "Platinum" },
                    { "Alessio Rovera", "21_26_AFCO95641716", "IT", "Platinum" },
                    { "François Hériau", "21_26_AFCO95641716", "", "" },
                    { "Simon Mann", "21_26_AFCO95641716", "GB", "Silver" },
                    { "Eduardo Barrichello", "23_26_THOR59931582", "BR", "Silver" },
                    { "Gray Newell", "23_26_THOR59931582", "", "" },
                    { "Jonny Adam", "23_26_THOR59931582", "", "" },
                    { "Ian James", "27_26_THOR22130173", "US", "Bronze" },
                    { "Mattia Drudi", "27_26_THOR22130173", "IT", "Platinum" },
                    { "Zacharie Robichon", "27_26_THOR22130173", "CA", "Silver" },
                    { "Augusto Farfus", "32_26_WRT_83524148", "BR", "Platinum" },
                    { "Darren Leung", "32_26_WRT_83524148", "GB", "Bronze" },
                    { "Sean Gelael", "32_26_WRT_83524148", "ID", "Silver" },
                    { "Ben Keating", "33_26_TFSP28162365", "US", "Bronze" },
                    { "Jonny Edgar", "33_26_TFSP28162365", "GB", "Silver" },
                    { "Nicky Catsburg", "33_26_TFSP28162365", "NL", "Platinum" },
                    { "Charlie Eastwood", "34_26_TFSP47451091", "IE", "Gold" },
                    { "Peter Dempsey", "34_26_TFSP47451091", "", "" },
                    { "Salih Yoluç", "34_26_TFSP47451091", "", "" },
                    { "António Félix da Costa", "35_26_ALPI41651716", "", "" },
                    { "Charles Milesi", "35_26_ALPI41651716", "FR", "Gold" },
                    { "Ferdinand Habsburg", "35_26_ALPI41651716", "AT", "Gold" },
                    { "Frédéric Makowiecki", "36_26_ALPI82796866", "", "" },
                    { "Jules Gounon", "36_26_ALPI82796866", "FR", "Platinum" },
                    { "Victor Martins", "36_26_ALPI82796866", "", "" },
                    { "Earl Bamber", "38_26_JOTA51270994", "NZ", "Platinum" },
                    { "Jack Aitken", "38_26_JOTA51270994", "GB", "Platinum" },
                    { "Sébastien Bourdais", "38_26_JOTA51270994", "", "" },
                    { "Antonio Fuoco", "50_26_FERR90502958", "IT", "Platinum" },
                    { "Miguel Molina", "50_26_FERR90502958", "ES", "Platinum" },
                    { "Nicklas Nielsen", "50_26_FERR90502958", "DK", "Platinum" },
                    { "Alessandro Pier Guidi", "51_26_FERR16908376", "IT", "Platinum" },
                    { "Antonio Giovinazzi", "51_26_FERR16908376", "IT", "Platinum" },
                    { "James Calado", "51_26_FERR16908376", "GB", "Platinum" },
                    { "Davide Rigon", "54_26_AFCO96652607", "IT", "Platinum" },
                    { "Francesco Castellacci", "54_26_AFCO96652607", "IT", "Silver" },
                    { "Thomas Flohr", "54_26_AFCO96652607", "CH", "Bronze" },
                    { "Alexander West", "58_26_GARA17941687", "", "" },
                    { "Benjamin Goethe", "58_26_GARA17941687", "", "" },
                    { "Finn Gehrsitz", "58_26_GARA17941687", "DE", "Silver" },
                    { "Martin Berry", "61_26_IRON17109319", "AU", "Bronze" },
                    { "Maxime Martin", "61_26_IRON17109319", "BE", "Platinum" },
                    { "Rui Andrade", "61_26_IRON17109319", "AO", "Silver" },
                    { "Martin Berry", "61_26_IRON57024276", "AU", "Bronze" },
                    { "Maxime Martin", "61_26_IRON57024276", "BE", "Platinum" },
                    { "Rui Andrade", "61_26_IRON57024276", "AO", "Silver" },
                    { "Abdulla Al-Khelaifi", "62_26_IRON98036068", "", "" },
                    { "Giuliano Alesi", "62_26_IRON98036068", "", "" },
                    { "Julian Hanses", "62_26_IRON98036068", "", "" },
                    { "Anthony McIntosh", "69_26_WRT_56324638", "", "" },
                    { "Dan Harper", "69_26_WRT_56324638", "", "" },
                    { "Parker Thompson", "69_26_WRT_56324638", "", "" },
                    { "Kamui Kobayashi", "7_26_TOYOT80734264", "JP", "Platinum" },
                    { "Mike Conway", "7_26_TOYOT80734264", "GB", "Platinum" },
                    { "Nyck De Vries", "7_26_TOYOT80734264", "NL", "Platinum" },
                    { "Ben Tuck", "77_26_PROT51390877", "GB", "Silver" },
                    { "Eric Powell", "77_26_PROT51390877", "", "" },
                    { "Sebastian Priaulx", "77_26_PROT51390877", "", "" },
                    { "Esteban Masson", "78_26_AKKO79996909", "FR", "Silver" },
                    { "Jack Hawksworth", "78_26_AKKO79996909", "GB", "Gold" },
                    { "Tom Van Rompuy", "78_26_AKKO79996909", "BE", "Bronze" },
                    { "Johannes Zelger", "79_26_IRON66951108", "", "" },
                    { "Lin Hodenius", "79_26_IRON66951108", "NL", "Silver" },
                    { "Matteo Cressoni", "79_26_IRON66951108", "IT", "Silver" },
                    { "Johannes Zelger", "79_26_IRON79382419", "", "" },
                    { "Lin Hodenius", "79_26_IRON79382419", "NL", "Silver" },
                    { "Matteo Cressoni", "79_26_IRON79382419", "IT", "Silver" },
                    { "Brendon Hartley", "8_26_TOYOT63257480", "NZ", "Platinum" },
                    { "Ryo Hirakawa", "8_26_TOYOT63257480", "JP", "Platinum" },
                    { "Sébastien Buemi", "8_26_TOYOT63257480", "", "" },
                    { "Philip Hanson", "83_26_AFCO45368276", "GB", "Gold" },
                    { "Robert Kubica", "83_26_AFCO45368276", "PL", "Platinum" },
                    { "Yifei Ye", "83_26_AFCO45368276", "CN", "Gold" },
                    { "Clemens Schmid", "87_26_AKKO74271960", "AT", "Silver" },
                    { "José María López", "87_26_AKKO74271960", "", "" },
                    { "Răzvan Umbrărescu", "87_26_AKKO74271960", "", "" },
                    { "Giammarco Levorato", "88_26_PROT29987739", "IT", "Silver" },
                    { "Logan Sargeant", "88_26_PROT29987739", "", "" },
                    { "Stefano Gattuso", "88_26_PROT29987739", "IT", "Bronze" },
                    { "Ayhancan Güven", "91_26_MANT18218509", "", "" },
                    { "James Cottingham", "91_26_MANT18218509", "GB", "Bronze" },
                    { "Timur Boguslavskiy", "91_26_MANT18218509", "RU", "Silver" },
                    { "Riccardo Pera", "92_26_MANT22466058", "IT", "Silver" },
                    { "Richard Lietz", "92_26_MANT22466058", "AT", "Platinum" },
                    { "Yasser Shahin", "92_26_MANT22466058", "AU", "Bronze" },
                    { "Nick Cassidy", "93_26_PEUG26011673", "", "" },
                    { "Paul di Resta", "93_26_PEUG26011673", "GB", "Platinum" },
                    { "Stoffel Vandoorne", "93_26_PEUG26011673", "BE", "Platinum" },
                    { "Nick Cassidy", "93_26_PEUG27100541", "", "" },
                    { "Paul di Resta", "93_26_PEUG27100541", "GB", "Platinum" },
                    { "Stoffel Vandoorne", "93_26_PEUG27100541", "BE", "Platinum" },
                    { "Loïc Duval", "94_26_PEUG73522851", "FR", "Platinum" },
                    { "Malthe Jakobsen", "94_26_PEUG73522851", "DK", "Gold" },
                    { "Théo Pourchaire", "94_26_PEUG73522851", "", "" },
                    { "Loïc Duval", "94_26_PEUG80967432", "FR", "Platinum" },
                    { "Malthe Jakobsen", "94_26_PEUG80967432", "DK", "Gold" },
                    { "Théo Pourchaire", "94_26_PEUG80967432", "", "" }
                });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "Class", "Custom", "Livery", "Model", "Name", "Number", "Series", "Team" },
                values: new object[,]
                {
                    { "397_25_AD25", "LMP3", true, "Custom", "ADESS_AD25_LMP3", "ADESS AD25 LMP3 Custom Team 2025 #397", "397", "ELMS2025", "Custom Team" },
                    { "46_25_ADES20619231", "LMP3", false, "LMU1", "ADESS_AD25_LMP3", "ADESS Factory Racing Team 2025 #46:LMU1", "46", "ELMS2025", "ADESS Factory Racing Team 2025" },
                    { "46_25_ADES51981969", "LMP3", false, "ELMS", "ADESS_AD25_LMP3", "ADESS Factory Racing Team 2025 #46:ELMS", "46", "ELMS2025", "ADESS Factory Racing Team 2025" },
                    { "46_25_ADES81662442", "LMP3", false, "LMU2", "ADESS_AD25_LMP3", "ADESS Factory Racing Team 2025 #46:LMU2", "46", "ELMS2025", "ADESS Factory Racing Team 2025" }
                });

            migrationBuilder.InsertData(
                table: "VehicleDrivers",
                columns: new[] { "Name", "Veh", "Nationality", "Skill" },
                values: new object[,]
                {
                    { " Alex Sawczuk", "46_25_ADES20619231", "", "" },
                    { " Will Bennett", "46_25_ADES20619231", "", "" },
                    { "Mirza Rustemović", "46_25_ADES20619231", "", "" },
                    { " Paulo Matias", "46_25_ADES51981969", "", "" },
                    { " Stephen Haley", "46_25_ADES51981969", "", "" },
                    { "Alex Coutie", "46_25_ADES51981969", "", "" },
                    { " Marek Lesniak", "46_25_ADES81662442", "", "" },
                    { " Michael Borda", "46_25_ADES81662442", "", "" },
                    { "Dennis Jordan", "46_25_ADES81662442", "", "" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Harry Tincknell", "007_26_THO22699059" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Ross Gunn", "007_26_THO22699059" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Tom Gamble", "007_26_THO22699059" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Alex Riberas", "009_26_THO14274046" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Marco Sorensen", "009_26_THO14274046" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Roman De Angelis", "009_26_THO14274046" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Antares Au", "10_26_GARA63384034" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Marvin Kirchhöfer", "10_26_GARA63384034" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Tom Fleming", "10_26_GARA63384034" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Alex Lynn", "12_26_JOTA50003117" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Norman Nato", "12_26_JOTA50003117" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Will Stevens", "12_26_JOTA50003117" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Dries Vanthoor", "15_26_WRT_37164931" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Kevin Magnussen", "15_26_WRT_37164931" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Raffaele Marciello", "15_26_WRT_37164931" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "André Lotterer", "17_26_GENE34E4954B" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Mathys Jaubert", "17_26_GENE34E4954B" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Pipo Derani", "17_26_GENE34E4954B" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Daniel Juncadella", "19_26_GENE5D7465E6" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Mathieu Jaminet", "19_26_GENE5D7465E6" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Paul-Loup Chatin", "19_26_GENE5D7465E6" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Rene Rast", "20_26_WRT_31540483" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Robin Frijns", "20_26_WRT_31540483" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Sheldon van der Linde", "20_26_WRT_31540483" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Alessio Rovera", "21_26_AFCO95641716" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "François Hériau", "21_26_AFCO95641716" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Simon Mann", "21_26_AFCO95641716" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Eduardo Barrichello", "23_26_THOR59931582" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Gray Newell", "23_26_THOR59931582" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jonny Adam", "23_26_THOR59931582" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Ian James", "27_26_THOR22130173" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Mattia Drudi", "27_26_THOR22130173" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Zacharie Robichon", "27_26_THOR22130173" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Augusto Farfus", "32_26_WRT_83524148" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Darren Leung", "32_26_WRT_83524148" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Sean Gelael", "32_26_WRT_83524148" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Ben Keating", "33_26_TFSP28162365" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jonny Edgar", "33_26_TFSP28162365" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Nicky Catsburg", "33_26_TFSP28162365" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Charlie Eastwood", "34_26_TFSP47451091" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Peter Dempsey", "34_26_TFSP47451091" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Salih Yoluç", "34_26_TFSP47451091" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "António Félix da Costa", "35_26_ALPI41651716" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Charles Milesi", "35_26_ALPI41651716" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Ferdinand Habsburg", "35_26_ALPI41651716" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Frédéric Makowiecki", "36_26_ALPI82796866" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jules Gounon", "36_26_ALPI82796866" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Victor Martins", "36_26_ALPI82796866" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Earl Bamber", "38_26_JOTA51270994" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jack Aitken", "38_26_JOTA51270994" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Sébastien Bourdais", "38_26_JOTA51270994" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { " Alex Sawczuk", "46_25_ADES20619231" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { " Will Bennett", "46_25_ADES20619231" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Mirza Rustemović", "46_25_ADES20619231" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { " Paulo Matias", "46_25_ADES51981969" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { " Stephen Haley", "46_25_ADES51981969" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Alex Coutie", "46_25_ADES51981969" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { " Marek Lesniak", "46_25_ADES81662442" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { " Michael Borda", "46_25_ADES81662442" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Dennis Jordan", "46_25_ADES81662442" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Antonio Fuoco", "50_26_FERR90502958" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Miguel Molina", "50_26_FERR90502958" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Nicklas Nielsen", "50_26_FERR90502958" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Alessandro Pier Guidi", "51_26_FERR16908376" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Antonio Giovinazzi", "51_26_FERR16908376" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "James Calado", "51_26_FERR16908376" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Davide Rigon", "54_26_AFCO96652607" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Francesco Castellacci", "54_26_AFCO96652607" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Thomas Flohr", "54_26_AFCO96652607" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Alexander West", "58_26_GARA17941687" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Benjamin Goethe", "58_26_GARA17941687" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Finn Gehrsitz", "58_26_GARA17941687" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Martin Berry", "61_26_IRON17109319" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Maxime Martin", "61_26_IRON17109319" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Rui Andrade", "61_26_IRON17109319" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Martin Berry", "61_26_IRON57024276" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Maxime Martin", "61_26_IRON57024276" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Rui Andrade", "61_26_IRON57024276" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Abdulla Al-Khelaifi", "62_26_IRON98036068" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Giuliano Alesi", "62_26_IRON98036068" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Julian Hanses", "62_26_IRON98036068" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Anthony McIntosh", "69_26_WRT_56324638" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Dan Harper", "69_26_WRT_56324638" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Parker Thompson", "69_26_WRT_56324638" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Kamui Kobayashi", "7_26_TOYOT80734264" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Mike Conway", "7_26_TOYOT80734264" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Nyck De Vries", "7_26_TOYOT80734264" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Ben Tuck", "77_26_PROT51390877" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Eric Powell", "77_26_PROT51390877" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Sebastian Priaulx", "77_26_PROT51390877" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Esteban Masson", "78_26_AKKO79996909" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jack Hawksworth", "78_26_AKKO79996909" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Tom Van Rompuy", "78_26_AKKO79996909" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Johannes Zelger", "79_26_IRON66951108" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Lin Hodenius", "79_26_IRON66951108" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Matteo Cressoni", "79_26_IRON66951108" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Johannes Zelger", "79_26_IRON79382419" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Lin Hodenius", "79_26_IRON79382419" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Matteo Cressoni", "79_26_IRON79382419" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Brendon Hartley", "8_26_TOYOT63257480" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Ryo Hirakawa", "8_26_TOYOT63257480" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Sébastien Buemi", "8_26_TOYOT63257480" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Philip Hanson", "83_26_AFCO45368276" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Robert Kubica", "83_26_AFCO45368276" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Yifei Ye", "83_26_AFCO45368276" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Clemens Schmid", "87_26_AKKO74271960" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "José María López", "87_26_AKKO74271960" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Răzvan Umbrărescu", "87_26_AKKO74271960" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Giammarco Levorato", "88_26_PROT29987739" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Logan Sargeant", "88_26_PROT29987739" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Stefano Gattuso", "88_26_PROT29987739" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Ayhancan Güven", "91_26_MANT18218509" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "James Cottingham", "91_26_MANT18218509" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Timur Boguslavskiy", "91_26_MANT18218509" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Riccardo Pera", "92_26_MANT22466058" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Richard Lietz", "92_26_MANT22466058" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Yasser Shahin", "92_26_MANT22466058" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Nick Cassidy", "93_26_PEUG26011673" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Paul di Resta", "93_26_PEUG26011673" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Stoffel Vandoorne", "93_26_PEUG26011673" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Nick Cassidy", "93_26_PEUG27100541" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Paul di Resta", "93_26_PEUG27100541" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Stoffel Vandoorne", "93_26_PEUG27100541" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Loïc Duval", "94_26_PEUG73522851" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Malthe Jakobsen", "94_26_PEUG73522851" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Théo Pourchaire", "94_26_PEUG73522851" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Loïc Duval", "94_26_PEUG80967432" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Malthe Jakobsen", "94_26_PEUG80967432" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Théo Pourchaire", "94_26_PEUG80967432" });

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "1_26_GCHAL73279592");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "10_26_GCHA17031780");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "11_26_GCHA47509705");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "12_26_GCHA18747807");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "2_26_GCHAL98500904");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "3_26_GCHAL41540517");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_25_AD25");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_26_296GT3");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_26_499P");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_26_911GT3R");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_26_9X8W");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_26_ALPINE");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_26_AMG");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_26_AMV");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_26_AMVALK");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_26_BMW");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_26_BMWMH");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_26_GR010");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_26_LEXUS");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_26_MCLAREN");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_26_MUSTANG");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_26_VLMDH");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_26_Z06GT3R");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "4_26_GCHAL72398716");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "5_26_GCHAL67256536");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "6_26_GCHAL89066091");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "7_26_GCHAL34731486");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "8_26_GCHAL79481284");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "9_26_GCHAL61794112");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "007_26_THO22699059");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "009_26_THO14274046");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "10_26_GARA63384034");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "12_26_JOTA50003117");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "15_26_WRT_37164931");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "17_26_GENE34E4954B");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "19_26_GENE5D7465E6");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "20_26_WRT_31540483");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "21_26_AFCO95641716");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "23_26_THOR59931582");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "27_26_THOR22130173");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "32_26_WRT_83524148");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "33_26_TFSP28162365");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "34_26_TFSP47451091");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "35_26_ALPI41651716");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "36_26_ALPI82796866");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "38_26_JOTA51270994");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "46_25_ADES20619231");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "46_25_ADES51981969");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "46_25_ADES81662442");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "50_26_FERR90502958");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "51_26_FERR16908376");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "54_26_AFCO96652607");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "58_26_GARA17941687");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "61_26_IRON17109319");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "61_26_IRON57024276");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "62_26_IRON98036068");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "69_26_WRT_56324638");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "7_26_TOYOT80734264");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "77_26_PROT51390877");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "78_26_AKKO79996909");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "79_26_IRON66951108");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "79_26_IRON79382419");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "8_26_TOYOT63257480");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "83_26_AFCO45368276");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "87_26_AKKO74271960");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "88_26_PROT29987739");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "91_26_MANT18218509");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "92_26_MANT22466058");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "93_26_PEUG26011673");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "93_26_PEUG27100541");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "94_26_PEUG73522851");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "94_26_PEUG80967432");

            migrationBuilder.DeleteData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "ADESS_AD25_LMP3");

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jens Reno Moller", "55_GMB2DFFBF6F" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "19_26_GENE467AE018",
                column: "Series",
                value: "WEC2025");
        }
    }
}
