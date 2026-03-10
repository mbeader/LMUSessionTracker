using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMUSessionTracker.Server.Migrations
{
    /// <inheritdoc />
    public partial class EngineManufacturerSwap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Alpine_A424",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Alpine-modified Mecachrome V634 3.4L V6 turbocharged", "Alpine" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "AMR_LMGT3",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Twin-turbo 4.0-litre V8", "Aston Martin" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Aston_Martin_Vantage_AMR",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Mercedes AMG M177 4.0L V8, Turbocharged", "Aston Martin" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "AstonMartin_Valkyrie",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Aston Martin RA 6.5L V12", "Aston Martin" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "BMW_M_Hybrid",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "BMW P66/3 3,999 cc 90 degree V8 twin-turbocharged", "BMW" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "BMW_M4_LMGT3",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "BMW Six Cylinder, M TwinPower turbo technology, 2993cc", "BMW" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Cadillac_V_lmdh",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Cadillac LMC55R 5.5 L 90 V8 NA", "Cadillac" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Corvette_Z06_LMGT3R",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "LT6.R 5.5L 90° V8", "Corvette" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Ferrari_296_LMGT3",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Ferrari F163CE V6", "Ferrari" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Ferrari_488_GTE_EVO",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Ferrari 3996cc V8, Turbocharged", "Ferrari" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Ferrari_499P",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Ferrari F163 2,992 cc 120° V6 twin-turbocharged", "Ferrari" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Ford_Mustang_LMGT3",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Ford Coyote 5.4 L V8", "Ford" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Ginetta_G61LTP3_Evo",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Toyota V35A-FTS 3500 cc V6 twin-turbocharged", "Ginetta" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Glickenhaus_SGC007",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Glickenhaus by Pipo Moteurs P21 3.5 litre V8", "Glickenhaus" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "GTE_Corvette_C8.R",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "LT6.R 5.5L 90 V8 Naturally Aspirated", "Corvette" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Isotta_TIPO6",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "HWA 3.0 V6", "Isotta Fraschini" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Lamborghini_Huracan_GT3_Evo2",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Lamborghini DGF 5.2 L V10", "Lamborghini" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Lamborghini_SC63",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Lamborghini 3.8 L V8", "Lamborghini" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Lexus_RCF_GT3",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Lexus 2UR-GSE 5.0 L V8", "Lexus" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Ligier_JS_P325",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Toyota V35A-FTS 3500 cc V6 twin-turbocharged", "Ligier" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "McLaren_720S_LMGT3_Evo",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "McLaren M840T 4.0L Twin Turbo V8", "McLaren" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Mercedes_AMG_GT3",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "AMG 6.3 Litre V8", "Mercedes-AMG" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Oreca_07",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Gibson GK-428 4.2 litre V8 naturally aspirated", "Oreca" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Peugeot_9x8",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Peugeot X6H 2.6 litre V6 90 degree twin-turbo", "Peugeot" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Peugeot_9x8_Wing",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Peugeot X6H 2.6 litre V6 90 degree twin-turbo", "Peugeot" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Porsche_911_GT3_R_LMGT3",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Porsche six-cylinder boxer engine 4,194 cm³", "Porsche" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Porsche_911_RSR-19",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Porsche M98/80 Flat-6 NA", "Porsche" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Porsche_963",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Porsche 9RD 4,593 cc V8 twin-turbocharged", "Porsche" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Toyota_GR010",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Toyota H8909 3.5L V6 Twin-turbo", "Toyota" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Vanwall_680",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Gibson GL458 4.5 litre V8 naturally-aspirated", "Vanwall" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Alpine_A424",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Alpine", "Alpine-modified Mecachrome V634 3.4L V6 turbocharged" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "AMR_LMGT3",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Aston Martin", "Twin-turbo 4.0-litre V8" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Aston_Martin_Vantage_AMR",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Aston Martin", "Mercedes AMG M177 4.0L V8, Turbocharged" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "AstonMartin_Valkyrie",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Aston Martin", "Aston Martin RA 6.5L V12" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "BMW_M_Hybrid",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "BMW", "BMW P66/3 3,999 cc 90 degree V8 twin-turbocharged" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "BMW_M4_LMGT3",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "BMW", "BMW Six Cylinder, M TwinPower turbo technology, 2993cc" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Cadillac_V_lmdh",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Cadillac", "Cadillac LMC55R 5.5 L 90 V8 NA" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Corvette_Z06_LMGT3R",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Corvette", "LT6.R 5.5L 90° V8" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Ferrari_296_LMGT3",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Ferrari", "Ferrari F163CE V6" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Ferrari_488_GTE_EVO",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Ferrari", "Ferrari 3996cc V8, Turbocharged" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Ferrari_499P",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Ferrari", "Ferrari F163 2,992 cc 120° V6 twin-turbocharged" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Ford_Mustang_LMGT3",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Ford", "Ford Coyote 5.4 L V8" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Ginetta_G61LTP3_Evo",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Ginetta", "Toyota V35A-FTS 3500 cc V6 twin-turbocharged" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Glickenhaus_SGC007",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Glickenhaus", "Glickenhaus by Pipo Moteurs P21 3.5 litre V8" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "GTE_Corvette_C8.R",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Corvette", "LT6.R 5.5L 90 V8 Naturally Aspirated" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Isotta_TIPO6",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Isotta Fraschini", "HWA 3.0 V6" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Lamborghini_Huracan_GT3_Evo2",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Lamborghini", "Lamborghini DGF 5.2 L V10" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Lamborghini_SC63",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Lamborghini", "Lamborghini 3.8 L V8" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Lexus_RCF_GT3",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Lexus", "Lexus 2UR-GSE 5.0 L V8" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Ligier_JS_P325",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Ligier", "Toyota V35A-FTS 3500 cc V6 twin-turbocharged" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "McLaren_720S_LMGT3_Evo",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "McLaren", "McLaren M840T 4.0L Twin Turbo V8" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Mercedes_AMG_GT3",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Mercedes-AMG", "AMG 6.3 Litre V8" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Oreca_07",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Oreca", "Gibson GK-428 4.2 litre V8 naturally aspirated" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Peugeot_9x8",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Peugeot", "Peugeot X6H 2.6 litre V6 90 degree twin-turbo" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Peugeot_9x8_Wing",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Peugeot", "Peugeot X6H 2.6 litre V6 90 degree twin-turbo" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Porsche_911_GT3_R_LMGT3",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Porsche", "Porsche six-cylinder boxer engine 4,194 cm³" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Porsche_911_RSR-19",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Porsche", "Porsche M98/80 Flat-6 NA" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Porsche_963",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Porsche", "Porsche 9RD 4,593 cc V8 twin-turbocharged" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Toyota_GR010",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Toyota", "Toyota H8909 3.5L V6 Twin-turbo" });

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Vanwall_680",
                columns: new[] { "Engine", "Manufacturer" },
                values: new object[] { "Vanwall", "Gibson GL458 4.5 litre V8 naturally-aspirated" });
        }
    }
}
