using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LMUSessionTracker.Server.Migrations
{
    /// <inheritdoc />
    public partial class Vehicles14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Marco Sorensen", "009_25_THO56EF0039" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Marco Sorensen", "009_25_THO91FE16D4" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Marco Sorensen", "009_25_THOCF55367F" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Marco Sorensen", "009_26_THO14274046" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Alex Lynn", "12_26_JOTA50003117" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Alex Palou", "2_24_CADIL8C6CDF7" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Rene Rast", "20_24_WRT_211F986E" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Rene Rast", "20_24_WRT_2E6B4FF3" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Rene Rast", "20_24_WRT_7492C369" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Rene Rast", "20_25_WRT_1827A0A9" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Rene Rast", "20_25_WRT_B511DB3E" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Rene Rast", "20_25_WRT_FCE7B289" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Rene Rast", "20_26_WRT_31540483" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "François Heriau", "21_25_AFCO36CA1D94" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "François Heriau", "21_25_AFCO72EE38BF" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Matthieu Vaxiviere", "36_24_ALPI18C9931" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Matthieu Vaxiviere", "36_24_ALPIEBC93816" });

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
                keyValues: new object[] { " Paulo Matias", "46_25_ADES51981969" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { " Stephen Haley", "46_25_ADES51981969" });

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
                keyValues: new object[] { "François Heriau", "55_24_AFCOB65363E7" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Marco Sorensen", "777_24_DST103B6012" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Marco Sorensen", "777_24_DST8B5349B6" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Răzvan Umbrărescu", "87_26_AKKO74271960" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jean-Eric Vergne", "93_24_PEUG105EB2DB" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jean-Eric Vergne", "93_24_PEUG98B27A84" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jean-Eric Vergne", "93_24_PEUGE6B75DF3" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jean-Eric Vergne", "93_25_PEUG354B9ECA" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jean-Eric Vergne", "93_25_PEUGD0A1B532" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jean-Eric Vergne", "93_PEUGEOT1713C81C" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jean-Eric Vergne", "93_PEUGEOTC47DDCEA" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jean-Eric Vergne", "93_PEUGEOTF5017513" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Marvin Kirchhöfer", "10_26_GARA63384034" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "DE", "Platinum" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "André Lotterer", "17_26_GENE34E4954B" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "DE", "Platinum" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "François Hériau", "21_26_AFCO95641716" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "FR", "Bronze" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Gray Newell", "23_26_THOR59931582" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "US", "Bronze" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jonny Adam", "23_26_THOR59931582" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "GB", "Platinum" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Peter Dempsey", "34_26_TFSP47451091" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "IE", "Bronze" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Salih Yoluç", "34_26_TFSP47451091" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "TR", "Silver" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "António Félix da Costa", "35_26_ALPI41651716" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "PT", "Platinum" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Frédéric Makowiecki", "36_26_ALPI82796866" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "FR", "Platinum" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Victor Martins", "36_26_ALPI82796866" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "FR", "Gold" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Sébastien Bourdais", "38_26_JOTA51270994" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "FR", "Platinum" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Alexander West", "58_26_GARA17941687" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "SE", "Bronze" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Benjamin Goethe", "58_26_GARA17941687" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "DE", "Gold" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Abdulla Al-Khelaifi", "62_26_IRON98036068" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "QA", "Bronze" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Giuliano Alesi", "62_26_IRON98036068" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "FR", "Gold" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Julian Hanses", "62_26_IRON98036068" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "DE", "Silver" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Anthony McIntosh", "69_26_WRT_56324638" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "US", "Bronze" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Dan Harper", "69_26_WRT_56324638" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "GB", "Platinum" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Parker Thompson", "69_26_WRT_56324638" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "CA", "Silver" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Eric Powell", "77_26_PROT51390877" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "US", "Bronze" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Sebastian Priaulx", "77_26_PROT51390877" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "GB", "Gold" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Johannes Zelger", "79_26_IRON66951108" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "IT", "Bronze" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Johannes Zelger", "79_26_IRON79382419" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "IT", "Bronze" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Sébastien Buemi", "8_26_TOYOT63257480" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "CH", "Platinum" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Petru Umbrărescu", "87_25_AKKO208CDD39" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "RO", "Silver" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Petru Umbrărescu", "87_25_AKKOA15D50DC" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "RO", "Silver" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "José María López", "87_26_AKKO74271960" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "AR", "Platinum" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Logan Sargeant", "88_26_PROT29987739" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "US", "Platinum" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Ayhancan Güven", "91_26_MANT18218509" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "TR", "Platinum" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Nick Cassidy", "93_26_PEUG26011673" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "NZ", "Platinum" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Nick Cassidy", "93_26_PEUG27100541" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "NZ", "Platinum" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Théo Pourchaire", "94_26_PEUG73522851" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "FR", "Platinum" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Théo Pourchaire", "94_26_PEUG80967432" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "FR", "Platinum" });

            migrationBuilder.InsertData(
                table: "VehicleDrivers",
                columns: new[] { "Name", "Veh", "Nationality", "Skill" },
                values: new object[,]
                {
                    { "Marco Sørensen", "009_25_THO56EF0039", "DK", "Platinum" },
                    { "Marco Sørensen", "009_25_THO91FE16D4", "DK", "Platinum" },
                    { "Marco Sørensen", "009_25_THOCF55367F", "DK", "Platinum" },
                    { "Marco Sørensen", "009_26_THO14274046", "DK", "Platinum" },
                    { "Louis Delétraz", "12_26_JOTA50003117", "CH", "Gold" },
                    { "Álex Palou", "2_24_CADIL8C6CDF7", "ES", "Platinum" },
                    { "René Rast", "20_24_WRT_211F986E", "DE", "Platinum" },
                    { "René Rast", "20_24_WRT_2E6B4FF3", "DE", "Platinum" },
                    { "René Rast", "20_24_WRT_7492C369", "DE", "Platinum" },
                    { "René Rast", "20_25_WRT_1827A0A9", "DE", "Platinum" },
                    { "René Rast", "20_25_WRT_B511DB3E", "DE", "Platinum" },
                    { "René Rast", "20_25_WRT_FCE7B289", "DE", "Platinum" },
                    { "René Rast", "20_26_WRT_31540483", "DE", "Platinum" },
                    { "François Hériau", "21_25_AFCO36CA1D94", "FR", "Bronze" },
                    { "François Hériau", "21_25_AFCO72EE38BF", "FR", "Bronze" },
                    { "Matthieu Vaxivière", "36_24_ALPI18C9931", "FR", "Gold" },
                    { "Matthieu Vaxivière", "36_24_ALPIEBC93816", "FR", "Gold" },
                    { "Alex Sawczuk", "46_25_ADES20619231", "", "" },
                    { "Will Bennett", "46_25_ADES20619231", "", "" },
                    { "Paulo Matias", "46_25_ADES51981969", "", "" },
                    { "Stephen Haley", "46_25_ADES51981969", "", "" },
                    { "Marek Lesniak", "46_25_ADES81662442", "", "" },
                    { "Michael Borda", "46_25_ADES81662442", "", "" },
                    { "François Hériau", "55_24_AFCOB65363E7", "FR", "Bronze" },
                    { "Marco Sørensen", "777_24_DST103B6012", "DK", "Platinum" },
                    { "Marco Sørensen", "777_24_DST8B5349B6", "DK", "Platinum" },
                    { "Petru Umbrărescu", "87_26_AKKO74271960", "RO", "Silver" },
                    { "Jean-Éric Vergne", "93_24_PEUG105EB2DB", "FR", "Platinum" },
                    { "Jean-Éric Vergne", "93_24_PEUG98B27A84", "FR", "Platinum" },
                    { "Jean-Éric Vergne", "93_24_PEUGE6B75DF3", "FR", "Platinum" },
                    { "Jean-Éric Vergne", "93_25_PEUG354B9ECA", "FR", "Platinum" },
                    { "Jean-Éric Vergne", "93_25_PEUGD0A1B532", "FR", "Platinum" },
                    { "Jean-Éric Vergne", "93_PEUGEOT1713C81C", "FR", "Platinum" },
                    { "Jean-Éric Vergne", "93_PEUGEOTC47DDCEA", "FR", "Platinum" },
                    { "Jean-Éric Vergne", "93_PEUGEOTF5017513", "FR", "Platinum" }
                });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "12_24_JOTAA5525C5E",
                column: "Team",
                value: "Hertz Team Jota");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "12_24_JOTAE2910C5C",
                column: "Team",
                value: "Hertz Team Jota");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "21_26_AFCO95641716",
                columns: new[] { "Name", "Team" },
                values: new object[] { "Vista AF Corse 2026 #21:WEC", "Vista AF Corse" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "37_24_COOL6B221F6",
                column: "Team",
                value: "Cool Racing");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "37_COOLBFC82CE9",
                column: "Team",
                value: "Cool Racing");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "38_24_JOTA9B8F3A36",
                column: "Team",
                value: "Hertz Team Jota");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "38_24_JOTAA127831A",
                column: "Team",
                value: "Hertz Team Jota");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "38_24_JOTAA157897C",
                column: "Team",
                value: "Hertz Team Jota");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_26_Z06GT3R",
                column: "Name",
                value: "Z06GT3R Custom Team 2026 #397:CS");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "46_25_ADES20619231",
                columns: new[] { "Name", "Team" },
                values: new object[] { "ADESS Racing Team 2025 #46:LMU1", "ADESS Racing Team 2025" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "46_25_ADES51981969",
                columns: new[] { "Name", "Team" },
                values: new object[] { "ADESS Racing Team 2025 #46:ELMS", "ADESS Racing Team 2025" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "46_25_ADES81662442",
                columns: new[] { "Name", "Team" },
                values: new object[] { "ADESS Racing Team 2025 #46:LMU2", "ADESS Racing Team 2025" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "47_24_COOL3477D827",
                column: "Team",
                value: "Cool Racing");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "47_COOLC6D667C1",
                column: "Team",
                value: "Cool Racing");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "54_26_AFCO96652607",
                columns: new[] { "Name", "Team" },
                values: new object[] { "Vista AF Corse 2026 #54:WEC", "Vista AF Corse" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "91_24_MANT4FC2B6C0",
                column: "Team",
                value: "Manthey Ema");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "91_24_MANT5728CF9F",
                column: "Team",
                value: "Manthey Ema");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "92_24_MANT5488007D",
                column: "Team",
                value: "Manthey PureRxcing");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "92_24_MANT7039D8B3",
                column: "Team",
                value: "Manthey PureRxcing");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "98_THOR437E22AC",
                column: "Team",
                value: "The Heart of Racing");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "98_THORWECF6670323",
                column: "Team",
                value: "The Heart of Racing");

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "Class", "Custom", "Livery", "Model", "Name", "Number", "Series", "Team" },
                values: new object[,]
                {
                    { "007_26_THO73564855", "Hypercar", false, "Le Mans", "AstonMartin_Valkyrie", "Aston Martin THOR Team 2026 #007:LM", "007", "WEC2026", "Aston Martin THOR Team" },
                    { "009_26_THO96632041", "Hypercar", false, "Le Mans", "AstonMartin_Valkyrie", "Aston Martin THOR Team 2026 #009:LM", "009", "WEC2026", "Aston Martin THOR Team" },
                    { "101_26_WTR51729170", "Hypercar", false, "Le Mans", "Cadillac_V_lmdh", "Cadillac WTR 2026 #101:LM", "101", "WEC2026", "Cadillac WTR" },
                    { "12_26_JOTA86515413", "Hypercar", false, "Le Mans", "Cadillac_V_lmdh", "Cadillac Hertz Team Jota 2026 #12:LM", "12", "WEC2026", "Cadillac Hertz Team Jota" },
                    { "15_26_WRT_34121376", "Hypercar", false, "Le Mans", "BMW_M_Hybrid", "BMW M Team WRT 2026 #15:LM", "15", "WEC2026", "BMW M Team WRT" },
                    { "20_26_WRT_40613364", "Hypercar", false, "Le Mans", "BMW_M_Hybrid", "BMW M Team WRT 2026 #20:LM", "20", "WEC2026", "BMW M Team WRT" },
                    { "35_26_ALPI86162428", "Hypercar", false, "Le Mans", "Alpine_A424", "Alpine Endurance Team 2026 #35:LM", "35", "WEC2026", "Alpine Endurance Team" },
                    { "36_26_ALPI20490502", "Hypercar", false, "Le Mans", "Alpine_A424", "Alpine Endurance Team 2026 #36:LM", "36", "WEC2026", "Alpine Endurance Team" },
                    { "38_26_JOTA15055593", "Hypercar", false, "Le Mans", "Cadillac_V_lmdh", "Cadillac Hertz Team Jota 2026 #38:LM", "38", "WEC2026", "Cadillac Hertz Team Jota" },
                    { "397_26_Z06GT3R1", "GT3", true, "Custom", "Corvette_Z06_LMGT3R", "Z06GT3R Custom Team 2026 #397:CS1", "397", "WEC2026", "Custom Team" },
                    { "50_26_FERR45066807", "Hypercar", false, "Le Mans", "Ferrari_499P", "Ferrari AF Corse 2026 #50:LM", "50", "WEC2026", "Ferrari AF Corse" },
                    { "51_26_FERR70540074", "Hypercar", false, "Le Mans", "Ferrari_499P", "Ferrari AF Corse 2026 #51:LM", "51", "WEC2026", "Ferrari AF Corse" },
                    { "7_26_TOYOT18793560", "Hypercar", false, "Le Mans", "Toyota_GR010", "Toyota Racing 2026 #7:LM", "7", "WEC2026", "Toyota Racing" },
                    { "8_26_TOYOT66538972", "Hypercar", false, "Le Mans", "Toyota_GR010", "Toyota Racing 2026 #8:LM", "8", "WEC2026", "Toyota Racing" },
                    { "83_26_AFCO76679772", "Hypercar", false, "Le Mans", "Ferrari_499P", "AF Corse 2026 #83:LM", "83", "WEC2026", "AF Corse" }
                });

            migrationBuilder.InsertData(
                table: "VehicleDrivers",
                columns: new[] { "Name", "Veh", "Nationality", "Skill" },
                values: new object[,]
                {
                    { "Harry Tincknell", "007_26_THO73564855", "GB", "Platinum" },
                    { "Ross Gunn", "007_26_THO73564855", "GB", "Platinum" },
                    { "Tom Gamble", "007_26_THO73564855", "GB", "Gold" },
                    { "Alex Riberas", "009_26_THO96632041", "ES", "Gold" },
                    { "Marco Sorensen", "009_26_THO96632041", "DK", "Platinum" },
                    { "Roman De Angelis", "009_26_THO96632041", "CA", "Gold" },
                    { "Filipe Albuquerque", "101_26_WTR51729170", "PT", "Platinum" },
                    { "Jordan Taylor", "101_26_WTR51729170", "US", "Platinum" },
                    { "Ricky Taylor", "101_26_WTR51729170", "US", "Platinum" },
                    { "Alex Lynn", "12_26_JOTA86515413", "GB", "Platinum" },
                    { "Norman Nato", "12_26_JOTA86515413", "FR", "Gold" },
                    { "Will Stevens", "12_26_JOTA86515413", "GB", "Platinum" },
                    { "Dries Vanthoor", "15_26_WRT_34121376", "BE", "Platinum" },
                    { "Kevin Magnussen", "15_26_WRT_34121376", "DK", "Platinum" },
                    { "Raffaele Marciello", "15_26_WRT_34121376", "IT", "Platinum" },
                    { "Rene Rast", "20_26_WRT_40613364", "DE", "Platinum" },
                    { "Robin Frijns", "20_26_WRT_40613364", "NL", "Platinum" },
                    { "Sheldon van der Linde", "20_26_WRT_40613364", "ZA", "Platinum" },
                    { "António Félix da Costa", "35_26_ALPI86162428", "PT", "Platinum" },
                    { "Charles Milesi", "35_26_ALPI86162428", "FR", "Gold" },
                    { "Ferdinand Habsburg", "35_26_ALPI86162428", "AT", "Gold" },
                    { "Frédéric Makowiecki", "36_26_ALPI20490502", "FR", "Platinum" },
                    { "Jules Gounon", "36_26_ALPI20490502", "FR", "Platinum" },
                    { "Victor Martins", "36_26_ALPI20490502", "FR", "Gold" },
                    { "Earl Bamber", "38_26_JOTA15055593", "NZ", "Platinum" },
                    { "Jack Aitken", "38_26_JOTA15055593", "GB", "Platinum" },
                    { "Sébastien Bourdais", "38_26_JOTA15055593", "FR", "Platinum" },
                    { "Antonio Fuoco", "50_26_FERR45066807", "IT", "Platinum" },
                    { "Miguel Molina", "50_26_FERR45066807", "ES", "Platinum" },
                    { "Nicklas Nielsen", "50_26_FERR45066807", "DK", "Platinum" },
                    { "Alessandro Pier Guidi", "51_26_FERR70540074", "IT", "Platinum" },
                    { "Antonio Giovinazzi", "51_26_FERR70540074", "IT", "Platinum" },
                    { "James Calado", "51_26_FERR70540074", "GB", "Platinum" },
                    { "Kamui Kobayashi", "7_26_TOYOT18793560", "JP", "Platinum" },
                    { "Mike Conway", "7_26_TOYOT18793560", "GB", "Platinum" },
                    { "Nyck De Vries", "7_26_TOYOT18793560", "NL", "Platinum" },
                    { "Brendon Hartley", "8_26_TOYOT66538972", "NZ", "Platinum" },
                    { "Ryo Hirakawa", "8_26_TOYOT66538972", "JP", "Platinum" },
                    { "Sébastien Buemi", "8_26_TOYOT66538972", "CH", "Platinum" },
                    { "Philip Hanson", "83_26_AFCO76679772", "GB", "Gold" },
                    { "Robert Kubica", "83_26_AFCO76679772", "PL", "Platinum" },
                    { "Yifei Ye", "83_26_AFCO76679772", "CN", "Gold" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Harry Tincknell", "007_26_THO73564855" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Ross Gunn", "007_26_THO73564855" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Tom Gamble", "007_26_THO73564855" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Marco Sørensen", "009_25_THO56EF0039" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Marco Sørensen", "009_25_THO91FE16D4" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Marco Sørensen", "009_25_THOCF55367F" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Marco Sørensen", "009_26_THO14274046" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Alex Riberas", "009_26_THO96632041" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Marco Sorensen", "009_26_THO96632041" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Roman De Angelis", "009_26_THO96632041" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Filipe Albuquerque", "101_26_WTR51729170" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jordan Taylor", "101_26_WTR51729170" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Ricky Taylor", "101_26_WTR51729170" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Louis Delétraz", "12_26_JOTA50003117" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Alex Lynn", "12_26_JOTA86515413" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Norman Nato", "12_26_JOTA86515413" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Will Stevens", "12_26_JOTA86515413" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Dries Vanthoor", "15_26_WRT_34121376" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Kevin Magnussen", "15_26_WRT_34121376" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Raffaele Marciello", "15_26_WRT_34121376" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Álex Palou", "2_24_CADIL8C6CDF7" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "René Rast", "20_24_WRT_211F986E" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "René Rast", "20_24_WRT_2E6B4FF3" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "René Rast", "20_24_WRT_7492C369" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "René Rast", "20_25_WRT_1827A0A9" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "René Rast", "20_25_WRT_B511DB3E" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "René Rast", "20_25_WRT_FCE7B289" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "René Rast", "20_26_WRT_31540483" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Rene Rast", "20_26_WRT_40613364" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Robin Frijns", "20_26_WRT_40613364" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Sheldon van der Linde", "20_26_WRT_40613364" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "François Hériau", "21_25_AFCO36CA1D94" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "François Hériau", "21_25_AFCO72EE38BF" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "António Félix da Costa", "35_26_ALPI86162428" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Charles Milesi", "35_26_ALPI86162428" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Ferdinand Habsburg", "35_26_ALPI86162428" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Matthieu Vaxivière", "36_24_ALPI18C9931" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Matthieu Vaxivière", "36_24_ALPIEBC93816" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Frédéric Makowiecki", "36_26_ALPI20490502" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jules Gounon", "36_26_ALPI20490502" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Victor Martins", "36_26_ALPI20490502" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Earl Bamber", "38_26_JOTA15055593" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jack Aitken", "38_26_JOTA15055593" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Sébastien Bourdais", "38_26_JOTA15055593" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Alex Sawczuk", "46_25_ADES20619231" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Will Bennett", "46_25_ADES20619231" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Paulo Matias", "46_25_ADES51981969" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Stephen Haley", "46_25_ADES51981969" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Marek Lesniak", "46_25_ADES81662442" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Michael Borda", "46_25_ADES81662442" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Antonio Fuoco", "50_26_FERR45066807" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Miguel Molina", "50_26_FERR45066807" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Nicklas Nielsen", "50_26_FERR45066807" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Alessandro Pier Guidi", "51_26_FERR70540074" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Antonio Giovinazzi", "51_26_FERR70540074" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "James Calado", "51_26_FERR70540074" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "François Hériau", "55_24_AFCOB65363E7" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Kamui Kobayashi", "7_26_TOYOT18793560" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Mike Conway", "7_26_TOYOT18793560" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Nyck De Vries", "7_26_TOYOT18793560" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Marco Sørensen", "777_24_DST103B6012" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Marco Sørensen", "777_24_DST8B5349B6" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Brendon Hartley", "8_26_TOYOT66538972" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Ryo Hirakawa", "8_26_TOYOT66538972" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Sébastien Buemi", "8_26_TOYOT66538972" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Philip Hanson", "83_26_AFCO76679772" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Robert Kubica", "83_26_AFCO76679772" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Yifei Ye", "83_26_AFCO76679772" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Petru Umbrărescu", "87_26_AKKO74271960" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jean-Éric Vergne", "93_24_PEUG105EB2DB" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jean-Éric Vergne", "93_24_PEUG98B27A84" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jean-Éric Vergne", "93_24_PEUGE6B75DF3" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jean-Éric Vergne", "93_25_PEUG354B9ECA" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jean-Éric Vergne", "93_25_PEUGD0A1B532" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jean-Éric Vergne", "93_PEUGEOT1713C81C" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jean-Éric Vergne", "93_PEUGEOTC47DDCEA" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jean-Éric Vergne", "93_PEUGEOTF5017513" });

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_26_Z06GT3R1");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "007_26_THO73564855");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "009_26_THO96632041");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "101_26_WTR51729170");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "12_26_JOTA86515413");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "15_26_WRT_34121376");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "20_26_WRT_40613364");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "35_26_ALPI86162428");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "36_26_ALPI20490502");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "38_26_JOTA15055593");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "50_26_FERR45066807");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "51_26_FERR70540074");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "7_26_TOYOT18793560");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "8_26_TOYOT66538972");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "83_26_AFCO76679772");

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Marvin Kirchhöfer", "10_26_GARA63384034" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "André Lotterer", "17_26_GENE34E4954B" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "François Hériau", "21_26_AFCO95641716" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Gray Newell", "23_26_THOR59931582" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jonny Adam", "23_26_THOR59931582" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Peter Dempsey", "34_26_TFSP47451091" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Salih Yoluç", "34_26_TFSP47451091" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "António Félix da Costa", "35_26_ALPI41651716" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Frédéric Makowiecki", "36_26_ALPI82796866" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Victor Martins", "36_26_ALPI82796866" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Sébastien Bourdais", "38_26_JOTA51270994" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Alexander West", "58_26_GARA17941687" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Benjamin Goethe", "58_26_GARA17941687" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Abdulla Al-Khelaifi", "62_26_IRON98036068" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Giuliano Alesi", "62_26_IRON98036068" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Julian Hanses", "62_26_IRON98036068" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Anthony McIntosh", "69_26_WRT_56324638" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Dan Harper", "69_26_WRT_56324638" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Parker Thompson", "69_26_WRT_56324638" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Eric Powell", "77_26_PROT51390877" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Sebastian Priaulx", "77_26_PROT51390877" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Johannes Zelger", "79_26_IRON66951108" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Johannes Zelger", "79_26_IRON79382419" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Sébastien Buemi", "8_26_TOYOT63257480" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Petru Umbrărescu", "87_25_AKKO208CDD39" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Petru Umbrărescu", "87_25_AKKOA15D50DC" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "José María López", "87_26_AKKO74271960" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Logan Sargeant", "88_26_PROT29987739" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Ayhancan Güven", "91_26_MANT18218509" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Nick Cassidy", "93_26_PEUG26011673" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Nick Cassidy", "93_26_PEUG27100541" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Théo Pourchaire", "94_26_PEUG73522851" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Théo Pourchaire", "94_26_PEUG80967432" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.InsertData(
                table: "VehicleDrivers",
                columns: new[] { "Name", "Veh", "Nationality", "Skill" },
                values: new object[,]
                {
                    { "Marco Sorensen", "009_25_THO56EF0039", "DK", "Platinum" },
                    { "Marco Sorensen", "009_25_THO91FE16D4", "DK", "Platinum" },
                    { "Marco Sorensen", "009_25_THOCF55367F", "DK", "Platinum" },
                    { "Marco Sorensen", "009_26_THO14274046", "DK", "Platinum" },
                    { "Alex Lynn", "12_26_JOTA50003117", "GB", "Platinum" },
                    { "Alex Palou", "2_24_CADIL8C6CDF7", "ES", "Platinum" },
                    { "Rene Rast", "20_24_WRT_211F986E", "DE", "Platinum" },
                    { "Rene Rast", "20_24_WRT_2E6B4FF3", "DE", "Platinum" },
                    { "Rene Rast", "20_24_WRT_7492C369", "DE", "Platinum" },
                    { "Rene Rast", "20_25_WRT_1827A0A9", "DE", "Platinum" },
                    { "Rene Rast", "20_25_WRT_B511DB3E", "DE", "Platinum" },
                    { "Rene Rast", "20_25_WRT_FCE7B289", "DE", "Platinum" },
                    { "Rene Rast", "20_26_WRT_31540483", "DE", "Platinum" },
                    { "François Heriau", "21_25_AFCO36CA1D94", "FR", "Bronze" },
                    { "François Heriau", "21_25_AFCO72EE38BF", "FR", "Bronze" },
                    { "Matthieu Vaxiviere", "36_24_ALPI18C9931", "FR", "Gold" },
                    { "Matthieu Vaxiviere", "36_24_ALPIEBC93816", "FR", "Gold" },
                    { " Alex Sawczuk", "46_25_ADES20619231", "", "" },
                    { " Will Bennett", "46_25_ADES20619231", "", "" },
                    { " Paulo Matias", "46_25_ADES51981969", "", "" },
                    { " Stephen Haley", "46_25_ADES51981969", "", "" },
                    { " Marek Lesniak", "46_25_ADES81662442", "", "" },
                    { " Michael Borda", "46_25_ADES81662442", "", "" },
                    { "François Heriau", "55_24_AFCOB65363E7", "FR", "Bronze" },
                    { "Marco Sorensen", "777_24_DST103B6012", "DK", "Platinum" },
                    { "Marco Sorensen", "777_24_DST8B5349B6", "DK", "Platinum" },
                    { "Răzvan Umbrărescu", "87_26_AKKO74271960", "", "" },
                    { "Jean-Eric Vergne", "93_24_PEUG105EB2DB", "FR", "Platinum" },
                    { "Jean-Eric Vergne", "93_24_PEUG98B27A84", "FR", "Platinum" },
                    { "Jean-Eric Vergne", "93_24_PEUGE6B75DF3", "FR", "Platinum" },
                    { "Jean-Eric Vergne", "93_25_PEUG354B9ECA", "FR", "Platinum" },
                    { "Jean-Eric Vergne", "93_25_PEUGD0A1B532", "FR", "Platinum" },
                    { "Jean-Eric Vergne", "93_PEUGEOT1713C81C", "FR", "Platinum" },
                    { "Jean-Eric Vergne", "93_PEUGEOTC47DDCEA", "FR", "Platinum" },
                    { "Jean-Eric Vergne", "93_PEUGEOTF5017513", "FR", "Platinum" }
                });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "12_24_JOTAA5525C5E",
                column: "Team",
                value: "Hertz Team JOTA");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "12_24_JOTAE2910C5C",
                column: "Team",
                value: "Hertz Team JOTA");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "21_26_AFCO95641716",
                columns: new[] { "Name", "Team" },
                values: new object[] { "Vista AF Corsa 2026 #21:WEC", "Vista AF Corsa" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "37_24_COOL6B221F6",
                column: "Team",
                value: "COOL Racing");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "37_COOLBFC82CE9",
                column: "Team",
                value: "COOL Racing");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "38_24_JOTA9B8F3A36",
                column: "Team",
                value: "Hertz Team JOTA");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "38_24_JOTAA127831A",
                column: "Team",
                value: "Hertz Team JOTA");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "38_24_JOTAA157897C",
                column: "Team",
                value: "Hertz Team JOTA");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_26_Z06GT3R",
                column: "Name",
                value: "Z06GT3R Custom Team 2026 #397");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "46_25_ADES20619231",
                columns: new[] { "Name", "Team" },
                values: new object[] { "ADESS Factory Racing Team 2025 #46:LMU1", "ADESS Factory Racing Team 2025" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "46_25_ADES51981969",
                columns: new[] { "Name", "Team" },
                values: new object[] { "ADESS Factory Racing Team 2025 #46:ELMS", "ADESS Factory Racing Team 2025" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "46_25_ADES81662442",
                columns: new[] { "Name", "Team" },
                values: new object[] { "ADESS Factory Racing Team 2025 #46:LMU2", "ADESS Factory Racing Team 2025" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "47_24_COOL3477D827",
                column: "Team",
                value: "COOL Racing");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "47_COOLC6D667C1",
                column: "Team",
                value: "COOL Racing");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "54_26_AFCO96652607",
                columns: new[] { "Name", "Team" },
                values: new object[] { "Vista AF Corsa 2026 #54:WEC", "Vista AF Corsa" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "91_24_MANT4FC2B6C0",
                column: "Team",
                value: "Manthey");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "91_24_MANT5728CF9F",
                column: "Team",
                value: "Manthey");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "92_24_MANT5488007D",
                column: "Team",
                value: "Manthey");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "92_24_MANT7039D8B3",
                column: "Team",
                value: "Manthey");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "98_THOR437E22AC",
                column: "Team",
                value: "Northwest AMR");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "98_THORWECF6670323",
                column: "Team",
                value: "Northwest AMR");
        }
    }
}
