using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LMUSessionTracker.Server.Migrations
{
    /// <inheritdoc />
    public partial class Vehicles142 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Genesis_GMR001",
                columns: new[] { "Engine", "Name" },
                values: new object[] { "Genesis GMR-001 3.2L V8 Twin Turbocharged", "Genesis GMR-001" });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "Class", "Custom", "Livery", "Model", "Name", "Number", "Series", "Team" },
                values: new object[,]
                {
                    { "10_26_GARA16926271", "GT3", false, "Le Mans", "McLaren_720S_LMGT3_Evo", "Garage 59 2026 #10:LM", "10", "WEC2026", "Garage 59" },
                    { "10_26_VECT30391627", "LMP2_ELMS", false, "ELMS", "Oreca_07", "Vector Sport #10:ELMS26", "10", "ELMS2026", "Vector Sport" },
                    { "10_26_VECT83155033", "LMP2_ELMS", false, "ELMS 2", "Oreca_07", "Vector Sport #10:ELMS2", "10", "ELMS2026", "Vector Sport" },
                    { "11_26_EI_E99190308", "LMP3", false, "ELMS", "Ligier_JS_P325", "EuroInternational #11:ELMS26", "11", "ELMS2026", "EuroInternational" },
                    { "13_26_13AU64749108", "GT3", false, "Le Mans", "Corvette_Z06_LMGT3R", "13 Autosport 2026 #13:LM", "13", "WEC2026", "13 Autosport" },
                    { "13_26_INTE97740060", "LMP3", false, "ELMS", "Ligier_JS_P325", "Inter Europol Competition #13:ELMS26", "13", "ELMS2026", "Inter Europol Competition" },
                    { "14_26_TDS_89261174", "LMP2_ELMS", false, "ELMS", "Oreca_07", "TDS Racing #14:ELMS26", "14", "ELMS2026", "TDS Racing" },
                    { "14_26_TDS_B596C72F", "LMP2", false, "Le Mans", "Oreca_07", "TDS Racing 2026 #14:LM", "14", "WEC2026", "TDS Racing" },
                    { "150_26_AFC71744116", "GT3", false, "Le Mans", "Ferrari_296_LMGT3", "Richard Mille AF Corse 2026 #150:LM", "150", "WEC2026", "Richard Mille AF Corse" },
                    { "17_26_CLX_32009151", "LMP3", false, "ELMS", "Ligier_JS_P325", "CLX Motorsport #17:ELMS26", "17", "ELMS2026", "CLX Motorsport" },
                    { "18_26_IDEC78043241", "LMP2_ELMS", false, "ELMS", "Oreca_07", "IDEC Sport #18:ELMS26", "18", "ELMS2026", "IDEC Sport" },
                    { "183_26_AFCCFF6FD80", "LMP2", false, "Le Mans", "Oreca_07", "AF Corse 2026 #183:LM", "183", "WEC2026", "AF Corse" },
                    { "19_26_VIRA82136742", "LMP2_ELMS", false, "ELMS", "Oreca_07", "Rossa Racing by Virage #19:ELMS26", "19", "ELMS2026", "Rossa Racing by Virage" },
                    { "2_26_TFSPO46601482", "GT3", false, "Le Mans", "Corvette_Z06_LMGT3R", "TF Sport 2026 #2:LM", "2", "WEC2026", "TF Sport" },
                    { "20_26_APR_16693969", "LMP2_ELMS", false, "ELMS", "Oreca_07", "Algarve Pro Racing #20:ELMS26", "20", "ELMS2026", "Algarve Pro Racing" },
                    { "21_26_AFCO31910023", "GT3", false, "Le Mans", "Ferrari_296_LMGT3", "Vista AF Corsa 2026 #21:LM", "21", "WEC2026", "Vista AF Corsa" },
                    { "21_26_UNIT55791918", "LMP2_ELMS", false, "ELMS", "Oreca_07", "United Autosports #21:ELMS26", "21", "ELMS2026", "United Autosports" },
                    { "22_26_UNIT48490510", "LMP2_ELMS", false, "ELMS", "Oreca_07", "United Autosports #22:ELMS26", "22", "ELMS2026", "United Autosports" },
                    { "22_26_UNIT7683F817", "LMP2", false, "Le Mans", "Oreca_07", "United Autosports 2026 #22:LM", "22", "WEC2026", "United Autosports" },
                    { "222_26_UNI72EE9E12", "LMP2", false, "Le Mans", "Oreca_07", "United Autosports 2026 #222:LM", "222", "WEC2026", "United Autosports" },
                    { "23_26_THOR64591293", "GT3", false, "Le Mans", "AMR_LMGT3", "Heart of Racing Team 2026 #23:LM", "23", "WEC2026", "Heart of Racing Team" },
                    { "23_26_UNIT14272908", "GT3", false, "ELMS", "McLaren_720S_LMGT3_Evo", "United Autosports 2026 #23:ELMS", "23", "ELMS2026", "United Autosports" },
                    { "24_26_NIEL37A85D27", "LMP2", false, "Le Mans", "Oreca_07", "Nielsen Racing 2026 #24:LM", "24", "WEC2026", "Nielsen Racing" },
                    { "24_26_NIEL86328564", "LMP2_ELMS", false, "ELMS", "Oreca_07", "Nielsen Racing #24:ELMS26", "24", "ELMS2026", "Nielsen Racing" },
                    { "25_26_APR_14077374", "LMP2_ELMS", false, "ELMS", "Oreca_07", "Algarve Pro Racing #25:ELMS26", "25", "ELMS2026", "Algarve Pro Racing" },
                    { "25_26_APR_AE921C40", "LMP2", false, "Le Mans", "Oreca_07", "Algarve Pro Racing 2026 #25:LM", "25", "WEC2026", "Algarve Pro Racing" },
                    { "26_26_VECT606F72C9", "LMP2", false, "Le Mans", "Oreca_07", "Vector Sport 2026 #26:LM", "26", "WEC2026", "Vector Sport" },
                    { "27_26_NIEL63263732", "LMP2_ELMS", false, "ELMS", "Oreca_07", "Nielsen Racing #27:ELMS26", "27", "ELMS2026", "Nielsen Racing" },
                    { "27_26_THOR94783089", "GT3", false, "Le Mans", "AMR_LMGT3", "Heart of Racing Team 2026 #27:LM", "27", "WEC2026", "Heart of Racing Team" },
                    { "28_26_IDEC52704256", "LMP2_ELMS", false, "ELMS", "Oreca_07", "IDEC Sport #28:ELMS26", "28", "ELMS2026", "IDEC Sport" },
                    { "28_26_IDECF85586AD", "LMP2", false, "Le Mans", "Oreca_07", "IDEC Sport 2026 #28:LM", "28", "WEC2026", "IDEC Sport" },
                    { "29_26_PANI67136124", "LMP2_ELMS", false, "ELMS", "Oreca_07", "Forestier Racing by Panis #29:ELMS26", "29", "ELMS2026", "Forestier Racing by Panis" },
                    { "29_26_PANIC8E05983", "LMP2", false, "Le Mans", "Oreca_07", "Forestier Racing by Panis 2026 #29:LM", "29", "WEC2026", "Forestier Racing by Panis" },
                    { "3_26_DKR_E72469578", "LMP2_ELMS", false, "ELMS", "Oreca_07", "DKR Engineering #3:ELMS26", "3", "ELMS2026", "DKR Engineering" },
                    { "3_26_DKR_LA8DF8C9A", "LMP2", false, "Le Mans", "Oreca_07", "DKR Engineering 2026 #3:LM", "3", "WEC2026", "DKR Engineering" },
                    { "30_26_DUQU13852371", "LMP2_ELMS", false, "ELMS", "Oreca_07", "Duqueine Team #30:ELMS26", "30", "ELMS2026", "Duqueine Team" },
                    { "30_26_DUQUFE1ABCFD", "LMP2", false, "Le Mans", "Oreca_07", "Duqueine Team 2026 #30:LM", "30", "WEC2026", "Duqueine Team" },
                    { "31_26_RSL_79686428", "LMP3", false, "ELMS", "Ligier_JS_P325", "Racing Spirit of Léman #31:ELMS26", "31", "ELMS2026", "Racing Spirit of Léman" },
                    { "32_26_WRT_65186446", "GT3", false, "Le Mans", "BMW_M4_LMGT3", "Team WRT 2026 #32:LM", "32", "WEC2026", "Team WRT" },
                    { "33_26_TFSP74306766", "GT3", false, "ELMS", "Corvette_Z06_LMGT3R", "TF Sport 2026 #33:ELMS", "33", "ELMS2026", "TF Sport" },
                    { "33_26_TFSP96347225", "GT3", false, "Le Mans", "Corvette_Z06_LMGT3R", "TF Sport 2026 #33:LM", "33", "WEC2026", "TF Sport" },
                    { "34_26_INTE45896526", "LMP2_ELMS", false, "ELMS", "Oreca_07", "Inter Europol Competition #34:ELMS26", "34", "ELMS2026", "Inter Europol Competition" },
                    { "34_26_TFSP28117283", "GT3", false, "Le Mans", "Corvette_Z06_LMGT3R", "Racing Team Turkey by TF 2026 #34:LM", "34", "WEC2026", "Racing Team Turkey by TF" },
                    { "343_26_INT8DE791EE", "LMP2", false, "Le Mans", "Oreca_07", "Inter Europol Competition 2026 #343:LM", "343", "WEC2026", "Inter Europol Competition" },
                    { "35_26_ULTI72230997", "LMP3", false, "ELMS", "Ligier_JS_P325", "Ultimate #35:ELMS26", "35", "ELMS2026", "Ultimate" },
                    { "37_26_CLX_55395349", "LMP2_ELMS", false, "ELMS", "Oreca_07", "CLX Motorsport #37:ELMS26", "37", "ELMS2026", "CLX Motorsport" },
                    { "37_26_CLX_8047943F", "LMP2", false, "Le Mans", "Oreca_07", "CLX Motorsport 2026 #37:LM", "37", "WEC2026", "CLX Motorsport" },
                    { "397_26_AD25", "LMP3", true, "Custom", "ADESS_AD25_LMP3", "ADESS AD25 LMP3 Custom Team 2026 #397", "397", "ELMS2026", "Custom Team" },
                    { "397_26_D09P3", "LMP3", true, "Custom", "Duqueine_D09_P3", "Duqueine D09 P3 Custom Team 2026 #397", "397", "ELMS2026", "Custom Team" },
                    { "397_26_G61LTP3EVO", "LMP3", true, "Custom", "Ginetta_G61LTP3_Evo", "Ginetta G61-LT-P3 Custom Team #397", "397", "ELMS2026", "Custom Team" },
                    { "397_26_JSP325", "LMP3", true, "Custom", "Ligier_JS_P325", "Ligier JS P325 Custom Team 2026 #397", "397", "ELMS2026", "Custom Team" },
                    { "397_26_ORECA07", "LMP2", true, "Custom", "Oreca_07", "Oreca 07 Custom Team 2026 #397", "397", "WEC2026", "Custom Team" },
                    { "397_26ELMS_296GT3", "GT3", true, "Custom", "Ferrari_296_LMGT3", "296GT3 ELMS Custom Team 2026 #397", "397", "ELMS2026", "Custom Team" },
                    { "397_26ELMS_911GT3R", "GT3", true, "Custom", "Porsche_911_GT3_R_LMGT3", "911GT3R ELMS Custom Team 2026 #397", "397", "ELMS2026", "Custom Team" },
                    { "397_26ELMS_AMG", "GT3", true, "Custom", "Mercedes_AMG_GT3", "Mercedes AMG ELMS Custom Team 2026 #397", "397", "ELMS2026", "Custom Team" },
                    { "397_26ELMS_AMV", "GT3", true, "Custom", "AMR_LMGT3", "AMR GT3 ELMS Custom Team 2026 #397", "397", "ELMS2026", "Custom Team" },
                    { "397_26ELMS_MCLAREN", "GT3", true, "Custom", "McLaren_720S_LMGT3_Evo", "McLaren ELMS Custom Team 2026 #397", "397", "ELMS2026", "Custom Team" },
                    { "397_26ELMS_ORECA07", "LMP2_ELMS", true, "Custom", "Oreca_07", "Oreca 07 ELMS Custom Team 2026 #397", "397", "ELMS2026", "Custom Team" },
                    { "397_26ELMS_Z06GT3R", "GT3", true, "Custom", "Corvette_Z06_LMGT3R", "Z06GT3R ELMS Custom Team 2026 #397", "397", "ELMS2026", "Custom Team" },
                    { "4_26_CROWDC65437AD", "LMP2", false, "Le Mans", "Oreca_07", "CrowdStrike Racing by APR 2026 #4:LM", "4", "WEC2026", "CrowdStrike Racing by APR" },
                    { "4_26_DKR_E89369431", "LMP3", false, "ELMS", "Ligier_JS_P325", "DKR Engineering #4:ELMS26", "4", "ELMS2026", "DKR Engineering" },
                    { "43_25_INTE69722561", "LMP2_ELMS", false, "ELMS", "Oreca_07", "Inter Europol Competition #43:ELMS26", "43", "ELMS2026", "Inter Europol Competition" },
                    { "43_26_INTEBFEF5F1A", "LMP2", false, "Le Mans", "Oreca_07", "Inter Europol Competition 2026 #43:LM", "43", "WEC2026", "Inter Europol Competition" },
                    { "44_26_PROTCA5057AF", "LMP2", false, "Le Mans", "Oreca_07", "Proton Competition 2026 #44:LM", "44", "WEC2026", "Proton Competition" },
                    { "47_26_CLX_33830875", "LMP2_ELMS", false, "ELMS", "Oreca_07", "CLX Motorsport #47:ELMS26", "47", "ELMS2026", "CLX Motorsport" },
                    { "48_26_RDLI80123CF7", "LMP2", false, "Le Mans", "Oreca_07", "RD Limited 2026 #48:LM", "48", "WEC2026", "RD Limited" },
                    { "5_26_RINAL21482192", "LMP3", false, "ELMS", "Ligier_JS_P325", "Rinaldi Racing #5:ELMS26", "5", "ELMS2026", "Rinaldi Racing" },
                    { "50_26_AFCO65459354", "GT3", false, "ELMS", "Ferrari_296_LMGT3", "Richard Mille AF Corse 2026 #50:ELMS", "50", "ELMS2026", "Richard Mille AF Corse" },
                    { "51_26_AFCO83755194", "GT3", false, "ELMS", "Ferrari_296_LMGT3", "AF Corse 2026 #51:ELMS", "51", "ELMS2026", "AF Corse" },
                    { "54_26_AFCO54087704", "GT3", false, "Le Mans", "Ferrari_296_LMGT3", "Vista AF Corsa 2026 #54:LM", "54", "WEC2026", "Vista AF Corsa" },
                    { "54_26_HIGH62031041", "GT3", false, "ELMS", "Porsche_911_GT3_R_LMGT3", "High Class Racing 2026 #54:ELMS", "54", "ELMS2026", "High Class Racing" },
                    { "54_26_HIGH91914076", "GT3", false, "ELMS 2", "Porsche_911_GT3_R_LMGT3", "High Class Racing 2026 #54:ELMS2", "54", "ELMS2026", "High Class Racing" },
                    { "55_26_SPIR84723860", "GT3", false, "ELMS", "Ferrari_296_LMGT3", "Spirit of Race 2026 #55:ELMS", "55", "ELMS2026", "Spirit of Race" },
                    { "57_26_KESS47090593", "GT3", false, "Le Mans", "Ferrari_296_LMGT3", "Kessel Racing 2026 #57:LM", "57", "WEC2026", "Kessel Racing" },
                    { "57_26_KESS75581141", "GT3", false, "ELMS", "Ferrari_296_LMGT3", "Kessel Racing 2026 #57:ELMS", "57", "ELMS2026", "Kessel Racing" },
                    { "58_26_GARA79992578", "GT3", false, "Le Mans", "McLaren_720S_LMGT3_Evo", "Garage 59 2026 #58:LM", "58", "WEC2026", "Garage 59" },
                    { "59_26_RSL_16524330", "GT3", false, "Le Mans", "AMR_LMGT3", "Racing Spirit of Léman  2026 #59:LM", "59", "WEC2026", "Racing Spirit of Léman" },
                    { "59_26_RSL_80183129", "GT3", false, "ELMS", "AMR_LMGT3", "Racing Spirit of Léman 2026 #59:ELMS", "59", "ELMS2026", "Racing Spirit of Léman" },
                    { "62_26_IRON33398475", "GT3", false, "Barcelona", "Mercedes_AMG_GT3", "Team Qatar by Iron Lynx 2026 #62:ELMS2", "62", "ELMS2026", "Team Qatar by Iron Lynx" },
                    { "62_26_IRON72127902", "GT3", false, "ELMS", "Mercedes_AMG_GT3", "Team Qatar by Iron Lynx 2026 #62:ELMS", "62", "ELMS2026", "Team Qatar by Iron Lynx" },
                    { "63_26_IRON64614378", "GT3", false, "ELMS", "Mercedes_AMG_GT3", "Iron Lynx 2026 #63:ELMS", "63", "ELMS2026", "Iron Lynx" },
                    { "68_26_MRAC58337914", "LMP3", false, "ELMS", "Ligier_JS_P325", "M Racing #68:ELMS26", "68", "ELMS2026", "M Racing" },
                    { "69_26_WRT_29233637", "GT3", false, "Spa", "BMW_M4_LMGT3", "Team WRT 2026 #69:Spa", "69", "WEC2026", "Team WRT" },
                    { "69_26_WRT_74157247", "GT3", false, "Le Pessage", "BMW_M4_LMGT3", "Team WRT 2026 #69:LP", "69", "WEC2026", "Team WRT" },
                    { "69_26_WRT_87701724", "GT3", false, "Le Mans", "BMW_M4_LMGT3", "Team WRT 2026 #69:LM", "69", "WEC2026", "Team WRT" },
                    { "7_26_VECTO34030044", "LMP2_ELMS", false, "ELMS", "Oreca_07", "Vector Sport #7:ELMS26", "7", "ELMS2026", "Vector Sport" },
                    { "74_26_KESS40986092", "GT3", false, "ELMS", "Ferrari_296_LMGT3", "Kessel Racing 2026 #74:ELMS", "74", "ELMS2026", "Kessel Racing" },
                    { "74_26_KESS60006998", "GT3", false, "Le Mans", "Ferrari_296_LMGT3", "Kessel Racing 2026 #74:LM", "74", "WEC2026", "Kessel Racing" },
                    { "75_26_PROT92649671", "GT3", false, "ELMS", "Porsche_911_GT3_R_LMGT3", "Proton Competition 2026 #75:ELMS", "75", "ELMS2026", "Proton Competition" },
                    { "77_26_PROT34038378", "GT3", false, "Le Mans", "Ford_Mustang_LMGT3", "Proton Competition 2026 #77:LM", "77", "WEC2026", "Proton Competition" },
                    { "77_26_PROT43675534", "GT3", false, "ELMS", "Porsche_911_GT3_R_LMGT3", "Proton Competition 2026 #77:ELMS", "77", "ELMS2026", "Proton Competition" },
                    { "77_26_PROT58898221", "GT3", false, "ELMS 2", "Porsche_911_GT3_R_LMGT3", "Proton Competition 2026 #77:ELMS2", "77", "ELMS2026", "Proton Competition" },
                    { "78_26_AKKO55813576", "GT3", false, "Le Mans", "Lexus_RCF_GT3", "Akkodis ASP Team 2026 #78:LM", "78", "WEC2026", "Akkodis ASP Team" },
                    { "79_26_IRON66566848", "GT3", false, "Spa", "Mercedes_AMG_GT3", "Iron Lynx 2026 #79:Spa", "79", "WEC2026", "Iron Lynx" },
                    { "8_26_VIRAG56791600", "LMP3", false, "ELMS", "Ligier_JS_P325", "Team Virage #8:ELMS26", "8", "ELMS2026", "Team Virage" },
                    { "83_26_AFCO19042478", "LMP2_ELMS", false, "ELMS", "Oreca_07", "AF Corse #83:ELMS26", "83", "ELMS2026", "AF Corse" },
                    { "85_26_RACE58223536", "LMP3", false, "ELMS", "Duqueine_D09_P3", "R-ace GP 2026 #85:ELMS", "85", "ELMS2026", "R-ace GP" },
                    { "86_26_GRRA35987849", "GT3", false, "ELMS", "Ferrari_296_LMGT3", "GR Racing 2026 #86:ELMS", "86", "ELMS2026", "GR Racing" },
                    { "87_26_AKKO35783817", "GT3", false, "Le Mans", "Lexus_RCF_GT3", "Akkodis ASP Team 2026 #87:LM", "87", "WEC2026", "Akkodis ASP Team" },
                    { "88_26_PROT11517799", "GT3", false, "Le Mans", "Ford_Mustang_LMGT3", "Proton Competition 2026 #88:LM", "88", "WEC2026", "Proton Competition" },
                    { "88_26_PROT40617040", "LMP2_ELMS", false, "ELMS", "Oreca_07", "Proton Competition #88:ELMS26", "88", "ELMS2026", "Proton Competition" },
                    { "9_26_PROTO22218488", "LMP2_ELMS", false, "ELMS", "Oreca_07", "Proton Competition #9:ELMS26", "9", "ELMS2026", "Proton Competition" },
                    { "9_26_PROTOE6B77FB3", "LMP2", false, "Le Mans", "Oreca_07", "Proton Competition 2026 #9:LM", "9", "WEC2026", "Proton Competition" },
                    { "91_26_MANT93210200", "GT3", false, "Le Mans", "Porsche_911_GT3_R_LMGT3", "Manthey DK Engineering 2026 #91:LM", "91", "WEC2026", "Manthey DK Engineering" },
                    { "92_26_MANT71544075", "GT3", false, "Le Mans", "Porsche_911_GT3_R_LMGT3", "The Bend Manthey 2026 #92:LM", "92", "WEC2026", "The Bend Manthey" },
                    { "99_26_AO_E34612493", "LMP2_ELMS", false, "ELMS", "Oreca_07", "AO by TF #99:ELMS26", "99", "ELMS2026", "AO by TF" },
                    { "99_26_AO_LFAE5C0D5", "LMP2", false, "Le Mans", "Oreca_07", "AO by TF 2026 #99:LM", "99", "WEC2026", "AO by TF" }
                });

            migrationBuilder.InsertData(
                table: "VehicleDrivers",
                columns: new[] { "Name", "Veh", "Nationality", "Skill" },
                values: new object[,]
                {
                    { "Antares Au", "10_26_GARA16926271", "", "Bronze" },
                    { "Marvin Kirchhöfer", "10_26_GARA16926271", "DE", "Platinum" },
                    { "Tom Fleming", "10_26_GARA16926271", "GB", "Silver" },
                    { "Pietro Fittipaldi", "10_26_VECT30391627", "BR", "Gold" },
                    { "Ryan Cullen", "10_26_VECT30391627", "IE", "Silver" },
                    { "Vladislav Lomko", "10_26_VECT30391627", "RU", "Gold" },
                    { "Pietro Fittipaldi", "10_26_VECT83155033", "BR", "Gold" },
                    { "Ryan Cullen", "10_26_VECT83155033", "IE", "Silver" },
                    { "Vladislav Lomko", "10_26_VECT83155033", "RU", "Gold" },
                    { "Douwe Dedecker", "11_26_EI_E99190308", "BE", "Bronze" },
                    { "Matthew Richard Bell", "11_26_EI_E99190308", "GB", "Bronze" },
                    { "Max van der Snel", "11_26_EI_E99190308", "NL", "Silver" },
                    { "Lars Kern", "13_26_13AU64749108", "DE", "Silver" },
                    { "Matt Bell", "13_26_13AU64749108", "GB", "Gold" },
                    { "Orey Fidani", "13_26_13AU64749108", "CA", "Bronze" },
                    { "Alexander Bukhantsov", "13_26_INTE97740060", "", "Bronze" },
                    { "Chun-Ting Chou", "13_26_INTE97740060", "", "Silver" },
                    { "Henry Cubides Olarte", "13_26_INTE97740060", "CO", "Silver" },
                    { "Sami Meguetounif", "14_26_TDS_89261174", "FR", "Silver" },
                    { "Scott Huffaker", "14_26_TDS_89261174", "US", "Gold" },
                    { "Steven Thomas", "14_26_TDS_89261174", "US", "Bronze" },
                    { "Kévin Estre", "14_26_TDS_B596C72F", "FR", "Platinum" },
                    { "Mathias Beche", "14_26_TDS_B596C72F", "CH", "Gold" },
                    { "Tobias Lütke", "14_26_TDS_B596C72F", "CA", "Bronze" },
                    { "Custodio Toledo", "150_26_AFC71744116", "BR", "Bronze" },
                    { "Lilou Wadoux", "150_26_AFC71744116", "FR", "Silver" },
                    { "Riccardo Agostini", "150_26_AFC71744116", "IT", "Gold" },
                    { "Alexander Jacoby", "17_26_CLX_32009151", "BR", "Silver" },
                    { "Bruno Ribeiro", "17_26_CLX_32009151", "BR", "Silver" },
                    { "Paul Lanchère", "17_26_CLX_32009151", "FR", "Bronze" },
                    { "Jamie Chadwick", "18_26_IDEC78043241", "GB", "Silver" },
                    { "Laurents Hörr", "18_26_IDEC78043241", "DE", "Gold" },
                    { "Valerio Rinicella", "18_26_IDEC78043241", "IT", "Silver" },
                    { "Ben Barnicoat", "183_26_AFCCFF6FD80", "GB", "Platinum" },
                    { "François Perrodo", "183_26_AFCCFF6FD80", "FR", "Bronze" },
                    { "Matthieu Vaxivière", "183_26_AFCCFF6FD80", "FR", "Gold" },
                    { "John Falb", "19_26_VIRA82136742", "US", "Bronze" },
                    { "Manuel Espírito Santo", "19_26_VIRA82136742", "PT", "Silver" },
                    { "Rik Koen", "19_26_VIRA82136742", "NL", "Silver" },
                    { "Ben Green", "2_26_TFSPO46601482", "GB", "Platinum" },
                    { "Jefri Ibrahim", "2_26_TFSPO46601482", "MY", "Bronze" },
                    { "Lorcan Hanafin", "2_26_TFSPO46601482", "GB", "Silver" },
                    { "Enzo Trulli", "20_26_APR_16693969", "IT", "Silver" },
                    { "Malthe Jakobsen", "20_26_APR_16693969", "DK", "Gold" },
                    { "Michael Jensen", "20_26_APR_16693969", "DK", "Bronze" },
                    { "Alessio Rovera", "21_26_AFCO31910023", "IT", "Platinum" },
                    { "François Hériau", "21_26_AFCO31910023", "FR", "Bronze" },
                    { "Simon Mann", "21_26_AFCO31910023", "GB", "Silver" },
                    { "Daniel Schneider", "21_26_UNIT55791918", "BR", "Bronze" },
                    { "Marino Sato", "21_26_UNIT55791918", "JP", "Gold" },
                    { "Oliver Jarvis", "21_26_UNIT55791918", "GB", "Platinum" },
                    { "Ben Hanley", "22_26_UNIT48490510", "GB", "Gold" },
                    { "Grégoire Saucy", "22_26_UNIT48490510", "CH", "Gold" },
                    { "Griffin Peebles", "22_26_UNIT48490510", "AU", "Silver" },
                    { "Grégoire Saucy", "22_26_UNIT7683F817", "CH", "Gold" },
                    { "Mikkel Jensen", "22_26_UNIT7683F817", "DK", "Platinum" },
                    { "Rasmus Lindh", "22_26_UNIT7683F817", "SE", "Silver" },
                    { "Ben Hanley", "222_26_UNI72EE9E12", "GB", "Gold" },
                    { "Daniel Schneider", "222_26_UNI72EE9E12", "BR", "Bronze" },
                    { "Oliver Jarvis", "222_26_UNI72EE9E12", "GB", "Platinum" },
                    { "Eduardo Barrichello", "23_26_THOR64591293", "BR", "Silver" },
                    { "Gray Newell", "23_26_THOR64591293", "US", "Bronze" },
                    { "Jonny Adam", "23_26_THOR64591293", "GB", "Platinum" },
                    { "Garnet Patterson", "23_26_UNIT14272908", "AU", "Silver" },
                    { "Michael Birch", "23_26_UNIT14272908", "GB", "Bronze" },
                    { "Wayne Boyd", "23_26_UNIT14272908", "GB", "Gold" },
                    { "David Heinemeier Hansson", "24_26_NIEL37A85D27", "DK", "Silver" },
                    { "Edward Pearson", "24_26_NIEL37A85D27", "GB", "Silver" },
                    { "Jack Doohan", "24_26_NIEL37A85D27", "AU", "Platinum" },
                    { "Edward Pearson", "24_26_NIEL86328564", "GB", "Silver" },
                    { "Jack Doohan", "24_26_NIEL86328564", "AU", "Platinum" },
                    { "Roy Nissany", "24_26_NIEL86328564", "IL", "Gold" },
                    { "Jake Hughes", "25_26_APR_14077374", "GB", "Gold" },
                    { "Matthias Kaiser", "25_26_APR_14077374", "LI", "Silver" },
                    { "Tristan Vautier", "25_26_APR_14077374", "FR", "Gold" },
                    { "Enzo Trulli", "25_26_APR_AE921C40", "IT", "Silver" },
                    { "Jake Hughes", "25_26_APR_AE921C40", "GB", "Gold" },
                    { "Michael Jensen", "25_26_APR_AE921C40", "DK", "Bronze" },
                    { "Pietro Fittipaldi", "26_26_VECT606F72C9", "BR", "Gold" },
                    { "Ryan Cullen", "26_26_VECT606F72C9", "IE", "Silver" },
                    { "Vladislav Lomko", "26_26_VECT606F72C9", "RU", "Gold" },
                    { "Alex Quinn", "27_26_NIEL63263732", "GB", "Gold" },
                    { "James Allen", "27_26_NIEL63263732", "AU", "Gold" },
                    { "Kriton Lendoudis", "27_26_NIEL63263732", "GR", "Bronze" },
                    { "Ian James", "27_26_THOR94783089", "US", "Bronze" },
                    { "Mattia Drudi", "27_26_THOR94783089", "IT", "Platinum" },
                    { "Zacharie Robichon", "27_26_THOR94783089", "CA", "Silver" },
                    { "Job van Uitert", "28_26_IDEC52704256", "NL", "Gold" },
                    { "Paul Lafargue", "28_26_IDEC52704256", "FR", "Silver" },
                    { "Paul-Loup Chatin", "28_26_IDEC52704256", "FR", "Gold" },
                    { "Job van Uitert", "28_26_IDECF85586AD", "NL", "Gold" },
                    { "Paul Lafargue", "28_26_IDECF85586AD", "FR", "Silver" },
                    { "Valerio Rinicella", "28_26_IDECF85586AD", "IT", "Silver" },
                    { "Esteban Masson", "29_26_PANI67136124", "FR", "Silver" },
                    { "Louis Rousset", "29_26_PANI67136124", "FR", "Silver" },
                    { "Oliver Gray", "29_26_PANI67136124", "GB", "Silver" },
                    { "Esteban Masson", "29_26_PANIC8E05983", "FR", "Silver" },
                    { "Louis Rousset", "29_26_PANIC8E05983", "FR", "Silver" },
                    { "Oliver Gray", "29_26_PANIC8E05983", "GB", "Silver" },
                    { "Jean Glorieux", "3_26_DKR_E72469578", "BE", "Bronze" },
                    { "Marlon Hernandez", "3_26_DKR_E72469578", "FR", "Silver" },
                    { "Sebastián Álvarez", "3_26_DKR_E72469578", "MX", "Silver" },
                    { "John Farano", "3_26_DKR_LA8DF8C9A", "CA", "Bronze" },
                    { "Renger van der Zande", "3_26_DKR_LA8DF8C9A", "NL", "Platinum" },
                    { "Sebastián Álvarez", "3_26_DKR_LA8DF8C9A", "MX", "Silver" },
                    { "Doriane Pin", "30_26_DUQU13852371", "FR", "Silver" },
                    { "Giorgio Roda", "30_26_DUQU13852371", "IT", "Bronze" },
                    { "Richard Verschoor", "30_26_DUQU13852371", "NL", "Gold" },
                    { "Doriane Pin", "30_26_DUQUFE1ABCFD", "FR", "Silver" },
                    { "Julien Andlauer", "30_26_DUQUFE1ABCFD", "FR", "Gold" },
                    { "Richard Verschoor", "30_26_DUQUFE1ABCFD", "NL", "Gold" },
                    { "Grégory de Sybourg", "31_26_RSL_79686428", "CH", "Silver" },
                    { "Lenny Ried", "31_26_RSL_79686428", "DE", "Silver" },
                    { "Ralph Meichtry", "31_26_RSL_79686428", "CH", "Bronze" },
                    { "Augusto Farfus", "32_26_WRT_65186446", "BR", "Platinum" },
                    { "Darren Leung", "32_26_WRT_65186446", "GB", "Bronze" },
                    { "Sean Gelael", "32_26_WRT_65186446", "ID", "Silver" },
                    { "Alec Udell", "33_26_TFSP74306766", "US", "Silver" },
                    { "Blake McDonald", "33_26_TFSP74306766", "US", "Bronze" },
                    { "Charlie Eastwood", "33_26_TFSP74306766", "IE", "Gold" },
                    { "Ben Keating", "33_26_TFSP96347225", "US", "Bronze" },
                    { "Jonny Edgar", "33_26_TFSP96347225", "GB", "Silver" },
                    { "Nicky Catsburg", "33_26_TFSP96347225", "NL", "Platinum" },
                    { "Bijoy Garg", "34_26_INTE45896526", "US", "Silver" },
                    { "Reshad de Gerus", "34_26_INTE45896526", "FR", "Gold" },
                    { "Charlie Eastwood", "34_26_TFSP28117283", "IE", "Gold" },
                    { "Peter Dempsey", "34_26_TFSP28117283", "IE", "Bronze" },
                    { "Salih Yoluç", "34_26_TFSP28117283", "TR", "Silver" },
                    { "Bijoy Garg", "343_26_INT8DE791EE", "US", "Silver" },
                    { "Nico Müller", "343_26_INT8DE791EE", "CH", "Platinum" },
                    { "Reshad de Gerus", "343_26_INT8DE791EE", "FR", "Gold" },
                    { "Lucas Fecury", "35_26_ULTI72230997", "BR", "Silver" },
                    { "Sebastian Gravlund", "35_26_ULTI72230997", "DK", "Silver" },
                    { "Terrence Woodward", "35_26_ULTI72230997", "GB", "Bronze" },
                    { "Adrien Closmenil", "37_26_CLX_55395349", "FR", "Silver" },
                    { "Ian Aguilera", "37_26_CLX_55395349", "MX", "Silver" },
                    { "Theodor Jensen", "37_26_CLX_55395349", "DK", "Silver" },
                    { "Adrien Closmenil", "37_26_CLX_8047943F", "FR", "Silver" },
                    { "Ian Aguilera", "37_26_CLX_8047943F", "MX", "Silver" },
                    { "Theodor Jensen", "37_26_CLX_8047943F", "DK", "Silver" },
                    { "Alex Quinn", "4_26_CROWDC65437AD", "GB", "Gold" },
                    { "George Kurtz", "4_26_CROWDC65437AD", "US", "Bronze" },
                    { "Laurin Heinrich", "4_26_CROWDC65437AD", "DE", "Gold" },
                    { "Antti Rammo", "4_26_DKR_E89369431", "EE", "Bronze" },
                    { "Romain Favre", "4_26_DKR_E89369431", "FR", "Silver" },
                    { "Wyatt Brichacek", "4_26_DKR_E89369431", "US", "Silver" },
                    { "Jakub Śmiechowski", "43_25_INTE69722561", "PL", "Silver" },
                    { "Nick Yelloly", "43_25_INTE69722561", "GB", "Platinum" },
                    { "Tom Dillmann", "43_25_INTE69722561", "FR", "Gold" },
                    { "Jakub Śmiechowski", "43_26_INTEBFEF5F1A", "PL", "Silver" },
                    { "Nick Yelloly", "43_26_INTEBFEF5F1A", "GB", "Platinum" },
                    { "Tom Dillmann", "43_26_INTEBFEF5F1A", "FR", "Gold" },
                    { "Horst Felbermayr", "44_26_PROTCA5057AF", "AT", "Bronze" },
                    { "Horst Felix Felbermayr", "44_26_PROTCA5057AF", "AT", "Silver" },
                    { "Lorenzo Fluxá", "44_26_PROTCA5057AF", "ES", "Silver" },
                    { "Charles Milesi", "47_26_CLX_33830875", "FR", "Gold" },
                    { "Ferdinand Habsburg", "47_26_CLX_33830875", "AT", "Gold" },
                    { "Georgios Kolovos", "47_26_CLX_33830875", "GR", "Bronze" },
                    { "Fred Poordad", "48_26_RDLI80123CF7", "US", "Bronze" },
                    { "Romain Dumas", "48_26_RDLI80123CF7", "FR", "Platinum" },
                    { "Tristan Vautier", "48_26_RDLI80123CF7", "FR", "Gold" },
                    { "Alvise Rodella", "5_26_RINAL21482192", "IT", "Silver" },
                    { "José Cautela", "5_26_RINAL21482192", "", "Bronze" },
                    { "Mikkel Gaarde Pedersen", "5_26_RINAL21482192", "DK", "Silver" },
                    { "Custodio Toledo", "50_26_AFCO65459354", "BR", "Bronze" },
                    { "Lilou Wadoux", "50_26_AFCO65459354", "FR", "Silver" },
                    { "Riccardo Agostini", "50_26_AFCO65459354", "IT", "Gold" },
                    { "Charles-Henri Samani", "51_26_AFCO83755194", "FR", "Bronze" },
                    { "Conrad Laursen", "51_26_AFCO83755194", "DK", "Silver" },
                    { "Davide Rigon", "51_26_AFCO83755194", "IT", "Platinum" },
                    { "Davide Rigon", "54_26_AFCO54087704", "IT", "Platinum" },
                    { "Francesco Castellacci", "54_26_AFCO54087704", "IT", "Silver" },
                    { "Thomas Flohr", "54_26_AFCO54087704", "CH", "Bronze" },
                    { "Anders Fjordbach", "54_26_HIGH62031041", "DK", "Silver" },
                    { "Klaus Bachler", "54_26_HIGH62031041", "AT", "Platinum" },
                    { "Max Moritz", "54_26_HIGH62031041", "DE", "Bronze" },
                    { "Anders Fjordbach", "54_26_HIGH91914076", "DK", "Silver" },
                    { "Dennis Andersen", "54_26_HIGH91914076", "DK", "Bronze" },
                    { "Laurin Heinrich", "54_26_HIGH91914076", "DE", "Gold" },
                    { "David Perel", "55_26_SPIR84723860", "ZA", "Silver" },
                    { "Duncan Cameron", "55_26_SPIR84723860", "GB", "Bronze" },
                    { "Matt Griffin", "55_26_SPIR84723860", "IE", "Gold" },
                    { "Conrad Laursen", "57_26_KESS47090593", "DK", "Silver" },
                    { "Daniel Serra", "57_26_KESS47090593", "BR", "Platinum" },
                    { "Takeshi Kimura", "57_26_KESS47090593", "JP", "Bronze" },
                    { "Daniel Serra", "57_26_KESS75581141", "BR", "Platinum" },
                    { "Mathys Jaubert", "57_26_KESS75581141", "FR", "Silver" },
                    { "Takeshi Kimura", "57_26_KESS75581141", "JP", "Bronze" },
                    { "Alexander West", "58_26_GARA79992578", "SE", "Bronze" },
                    { "Benjamin Goethe", "58_26_GARA79992578", "DE", "Gold" },
                    { "Finn Gehrsitz", "58_26_GARA79992578", "DE", "Silver" },
                    { "Clément Mateu", "59_26_RSL_16524330", "FR", "Bronze" },
                    { "Sébastien Baud", "59_26_RSL_16524330", "FR", "Silver" },
                    { "Valentin Hasse-Clot", "59_26_RSL_16524330", "FR", "Gold" },
                    { "Clément Mateu", "59_26_RSL_80183129", "FR", "Bronze" },
                    { "Marius Fossard", "59_26_RSL_80183129", "FR", "Silver" },
                    { "Valentin Hasse-Clot", "59_26_RSL_80183129", "FR", "Gold" },
                    { "Abdulla Al-Khelaifi", "62_26_IRON33398475", "QA", "Bronze" },
                    { "Adam Christodoulou", "62_26_IRON33398475", "GB", "Gold" },
                    { "Julian Hanses", "62_26_IRON33398475", "DE", "Silver" },
                    { "Abdulla Al-Khelaifi", "62_26_IRON72127902", "QA", "Bronze" },
                    { "Julian Hanses", "62_26_IRON72127902", "DE", "Silver" },
                    { "Maxime Martin", "62_26_IRON72127902", "BE", "Platinum" },
                    { "Ameerh Naran", "63_26_IRON64614378", "ZW", "Bronze" },
                    { "Rui Andrade", "63_26_IRON64614378", "AO", "Silver" },
                    { "Sérgio Sette Câmara", "63_26_IRON64614378", "BR", "Gold" },
                    { "Nick Adcock", "68_26_MRAC58337914", "GB", "Bronze" },
                    { "Quentin Antonel", "68_26_MRAC58337914", "FR", "Silver" },
                    { "Thomas Imbourg", "68_26_MRAC58337914", "FR", "Silver" },
                    { "Anthony McIntosh", "69_26_WRT_29233637", "US", "Bronze" },
                    { "Dan Harper", "69_26_WRT_29233637", "GB", "Platinum" },
                    { "Parker Thompson", "69_26_WRT_29233637", "CA", "Silver" },
                    { "Anthony McIntosh", "69_26_WRT_74157247", "US", "Bronze" },
                    { "Dan Harper", "69_26_WRT_74157247", "GB", "Platinum" },
                    { "Parker Thompson", "69_26_WRT_74157247", "CA", "Silver" },
                    { "Anthony McIntosh", "69_26_WRT_87701724", "US", "Bronze" },
                    { "Dan Harper", "69_26_WRT_87701724", "GB", "Platinum" },
                    { "Parker Thompson", "69_26_WRT_87701724", "CA", "Silver" },
                    { "Cem Bölükbaşı", "7_26_VECTO34030044", "TR", "Silver" },
                    { "Jens Reno Møller", "7_26_VECTO34030044", "DK", "Bronze" },
                    { "Lorenzo Fluxá", "7_26_VECTO34030044", "ES", "Silver" },
                    { "Andrew Gilbert", "74_26_KESS40986092", "GB", "Bronze" },
                    { "Fran Rueda", "74_26_KESS40986092", "ES", "Silver" },
                    { "Romain Leroux", "74_26_KESS40986092", "FR", "Silver" },
                    { "Dennis Marschall", "74_26_KESS60006998", "DE", "Platinum" },
                    { "Dustin Blattner", "74_26_KESS60006998", "US", "Bronze" },
                    { "Lorenzo Patrese", "74_26_KESS60006998", "IT", "Silver" },
                    { "Matt Kurzejewski", "75_26_PROT92649671", "US", "Bronze" },
                    { "Richard Lietz", "75_26_PROT92649671", "AT", "Platinum" },
                    { "Tom Sargent", "75_26_PROT92649671", "AU", "Silver" },
                    { "Ben Tuck", "77_26_PROT34038378", "GB", "Silver" },
                    { "Eric Powell", "77_26_PROT34038378", "US", "Bronze" },
                    { "Sebastian Priaulx", "77_26_PROT34038378", "GB", "Gold" },
                    { "Harry King", "77_26_PROT43675534", "GB", "Gold" },
                    { "Huub van Eijndhoven", "77_26_PROT43675534", "NL", "Silver" },
                    { "Martin Berry", "77_26_PROT43675534", "AU", "Bronze" },
                    { "Harry King", "77_26_PROT58898221", "GB", "Gold" },
                    { "Huub van Eijndhoven", "77_26_PROT58898221", "NL", "Silver" },
                    { "Martin Berry", "77_26_PROT58898221", "AU", "Bronze" },
                    { "Hadrien David", "78_26_AKKO55813576", "FR", "Silver" },
                    { "Jack Hawksworth", "78_26_AKKO55813576", "GB", "Gold" },
                    { "Tom Van Rompuy", "78_26_AKKO55813576", "BE", "Bronze" },
                    { "Johannes Zelger", "79_26_IRON66566848", "IT", "Bronze" },
                    { "Lin Hodenius", "79_26_IRON66566848", "NL", "Silver" },
                    { "Matteo Cressoni", "79_26_IRON66566848", "IT", "Silver" },
                    { "Daniel Nogales", "8_26_VIRAG56791600", "ES", "Bronze" },
                    { "Louis Stern", "8_26_VIRAG56791600", "FR", "Bronze" },
                    { "Matteo Quintarelli", "8_26_VIRAG56791600", "IT", "Silver" },
                    { "Antonio Fuoco", "83_26_AFCO19042478", "IT", "Platinum" },
                    { "François Perrodo", "83_26_AFCO19042478", "FR", "Bronze" },
                    { "Matthieu Vaxivière", "83_26_AFCO19042478", "FR", "Gold" },
                    { "Fabien Michal", "85_26_RACE58223536", "FR", "Bronze" },
                    { "Hugo Schwarze", "85_26_RACE58223536", "DE", "Silver" },
                    { "Pierre-Alexandre Provost", "85_26_RACE58223536", "", "Silver" },
                    { "Lorcan Hanafin", "86_26_GRRA35987849", "GB", "Silver" },
                    { "Mex Jansen", "86_26_GRRA35987849", "NL", "Silver" },
                    { "Michael Wainwright", "86_26_GRRA35987849", "GB", "Bronze" },
                    { "Clemens Schmid", "87_26_AKKO35783817", "AT", "Silver" },
                    { "José María López", "87_26_AKKO35783817", "AR", "Platinum" },
                    { "Petru Umbrărescu", "87_26_AKKO35783817", "RO", "Silver" },
                    { "Giammarco Levorato", "88_26_PROT11517799", "IT", "Silver" },
                    { "Logan Sargeant", "88_26_PROT11517799", "US", "Platinum" },
                    { "Stefano Gattuso", "88_26_PROT11517799", "IT", "Bronze" },
                    { "Horst Felbermayr", "88_26_PROT40617040", "AT", "Bronze" },
                    { "Horst Felix Felbermayr", "88_26_PROT40617040", "AT", "Silver" },
                    { "René Binder", "88_26_PROT40617040", "AT", "Gold" },
                    { "Jonas Ried", "9_26_PROTO22218488", "DE", "Silver" },
                    { "Mike Rockenfeller", "9_26_PROTO22218488", "DE", "Platinum" },
                    { "Sebastian Priaulx", "9_26_PROTO22218488", "GB", "Gold" },
                    { "Harry King", "9_26_PROTOE6B77FB3", "GB", "Gold" },
                    { "Jonas Ried", "9_26_PROTOE6B77FB3", "DE", "Silver" },
                    { "Kakunoshin Ohta", "9_26_PROTOE6B77FB3", "JP", "Gold" },
                    { "Ayhancan Güven", "91_26_MANT93210200", "TR", "Platinum" },
                    { "James Cottingham", "91_26_MANT93210200", "GB", "Bronze" },
                    { "Timur Boguslavskiy", "91_26_MANT93210200", "RU", "Silver" },
                    { "Riccardo Pera", "92_26_MANT71544075", "IT", "Silver" },
                    { "Richard Lietz", "92_26_MANT71544075", "AT", "Platinum" },
                    { "Yasser Shahin", "92_26_MANT71544075", "AU", "Bronze" },
                    { "Dane Cameron", "99_26_AO_E34612493", "US", "Platinum" },
                    { "Louis Delétraz", "99_26_AO_E34612493", "CH", "Gold" },
                    { "PJ Hyett", "99_26_AO_E34612493", "US", "Bronze" },
                    { "Dane Cameron", "99_26_AO_LFAE5C0D5", "US", "Platinum" },
                    { "James Allen", "99_26_AO_LFAE5C0D5", "AU", "Gold" },
                    { "P. J. Hyett", "99_26_AO_LFAE5C0D5", "US", "Bronze" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Antares Au", "10_26_GARA16926271" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Marvin Kirchhöfer", "10_26_GARA16926271" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Tom Fleming", "10_26_GARA16926271" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Pietro Fittipaldi", "10_26_VECT30391627" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Ryan Cullen", "10_26_VECT30391627" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Vladislav Lomko", "10_26_VECT30391627" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Pietro Fittipaldi", "10_26_VECT83155033" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Ryan Cullen", "10_26_VECT83155033" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Vladislav Lomko", "10_26_VECT83155033" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Douwe Dedecker", "11_26_EI_E99190308" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Matthew Richard Bell", "11_26_EI_E99190308" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Max van der Snel", "11_26_EI_E99190308" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Lars Kern", "13_26_13AU64749108" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Matt Bell", "13_26_13AU64749108" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Orey Fidani", "13_26_13AU64749108" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Alexander Bukhantsov", "13_26_INTE97740060" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Chun-Ting Chou", "13_26_INTE97740060" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Henry Cubides Olarte", "13_26_INTE97740060" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Sami Meguetounif", "14_26_TDS_89261174" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Scott Huffaker", "14_26_TDS_89261174" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Steven Thomas", "14_26_TDS_89261174" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Kévin Estre", "14_26_TDS_B596C72F" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Mathias Beche", "14_26_TDS_B596C72F" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Tobias Lütke", "14_26_TDS_B596C72F" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Custodio Toledo", "150_26_AFC71744116" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Lilou Wadoux", "150_26_AFC71744116" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Riccardo Agostini", "150_26_AFC71744116" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Alexander Jacoby", "17_26_CLX_32009151" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Bruno Ribeiro", "17_26_CLX_32009151" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Paul Lanchère", "17_26_CLX_32009151" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jamie Chadwick", "18_26_IDEC78043241" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Laurents Hörr", "18_26_IDEC78043241" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Valerio Rinicella", "18_26_IDEC78043241" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Ben Barnicoat", "183_26_AFCCFF6FD80" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "François Perrodo", "183_26_AFCCFF6FD80" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Matthieu Vaxivière", "183_26_AFCCFF6FD80" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "John Falb", "19_26_VIRA82136742" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Manuel Espírito Santo", "19_26_VIRA82136742" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Rik Koen", "19_26_VIRA82136742" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Ben Green", "2_26_TFSPO46601482" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jefri Ibrahim", "2_26_TFSPO46601482" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Lorcan Hanafin", "2_26_TFSPO46601482" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Enzo Trulli", "20_26_APR_16693969" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Malthe Jakobsen", "20_26_APR_16693969" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Michael Jensen", "20_26_APR_16693969" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Alessio Rovera", "21_26_AFCO31910023" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "François Hériau", "21_26_AFCO31910023" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Simon Mann", "21_26_AFCO31910023" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Daniel Schneider", "21_26_UNIT55791918" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Marino Sato", "21_26_UNIT55791918" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Oliver Jarvis", "21_26_UNIT55791918" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Ben Hanley", "22_26_UNIT48490510" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Grégoire Saucy", "22_26_UNIT48490510" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Griffin Peebles", "22_26_UNIT48490510" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Grégoire Saucy", "22_26_UNIT7683F817" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Mikkel Jensen", "22_26_UNIT7683F817" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Rasmus Lindh", "22_26_UNIT7683F817" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Ben Hanley", "222_26_UNI72EE9E12" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Daniel Schneider", "222_26_UNI72EE9E12" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Oliver Jarvis", "222_26_UNI72EE9E12" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Eduardo Barrichello", "23_26_THOR64591293" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Gray Newell", "23_26_THOR64591293" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jonny Adam", "23_26_THOR64591293" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Garnet Patterson", "23_26_UNIT14272908" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Michael Birch", "23_26_UNIT14272908" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Wayne Boyd", "23_26_UNIT14272908" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "David Heinemeier Hansson", "24_26_NIEL37A85D27" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Edward Pearson", "24_26_NIEL37A85D27" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jack Doohan", "24_26_NIEL37A85D27" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Edward Pearson", "24_26_NIEL86328564" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jack Doohan", "24_26_NIEL86328564" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Roy Nissany", "24_26_NIEL86328564" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jake Hughes", "25_26_APR_14077374" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Matthias Kaiser", "25_26_APR_14077374" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Tristan Vautier", "25_26_APR_14077374" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Enzo Trulli", "25_26_APR_AE921C40" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jake Hughes", "25_26_APR_AE921C40" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Michael Jensen", "25_26_APR_AE921C40" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Pietro Fittipaldi", "26_26_VECT606F72C9" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Ryan Cullen", "26_26_VECT606F72C9" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Vladislav Lomko", "26_26_VECT606F72C9" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Alex Quinn", "27_26_NIEL63263732" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "James Allen", "27_26_NIEL63263732" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Kriton Lendoudis", "27_26_NIEL63263732" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Ian James", "27_26_THOR94783089" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Mattia Drudi", "27_26_THOR94783089" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Zacharie Robichon", "27_26_THOR94783089" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Job van Uitert", "28_26_IDEC52704256" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Paul Lafargue", "28_26_IDEC52704256" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Paul-Loup Chatin", "28_26_IDEC52704256" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Job van Uitert", "28_26_IDECF85586AD" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Paul Lafargue", "28_26_IDECF85586AD" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Valerio Rinicella", "28_26_IDECF85586AD" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Esteban Masson", "29_26_PANI67136124" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Louis Rousset", "29_26_PANI67136124" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Oliver Gray", "29_26_PANI67136124" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Esteban Masson", "29_26_PANIC8E05983" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Louis Rousset", "29_26_PANIC8E05983" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Oliver Gray", "29_26_PANIC8E05983" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jean Glorieux", "3_26_DKR_E72469578" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Marlon Hernandez", "3_26_DKR_E72469578" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Sebastián Álvarez", "3_26_DKR_E72469578" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "John Farano", "3_26_DKR_LA8DF8C9A" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Renger van der Zande", "3_26_DKR_LA8DF8C9A" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Sebastián Álvarez", "3_26_DKR_LA8DF8C9A" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Doriane Pin", "30_26_DUQU13852371" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Giorgio Roda", "30_26_DUQU13852371" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Richard Verschoor", "30_26_DUQU13852371" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Doriane Pin", "30_26_DUQUFE1ABCFD" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Julien Andlauer", "30_26_DUQUFE1ABCFD" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Richard Verschoor", "30_26_DUQUFE1ABCFD" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Grégory de Sybourg", "31_26_RSL_79686428" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Lenny Ried", "31_26_RSL_79686428" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Ralph Meichtry", "31_26_RSL_79686428" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Augusto Farfus", "32_26_WRT_65186446" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Darren Leung", "32_26_WRT_65186446" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Sean Gelael", "32_26_WRT_65186446" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Alec Udell", "33_26_TFSP74306766" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Blake McDonald", "33_26_TFSP74306766" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Charlie Eastwood", "33_26_TFSP74306766" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Ben Keating", "33_26_TFSP96347225" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jonny Edgar", "33_26_TFSP96347225" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Nicky Catsburg", "33_26_TFSP96347225" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Bijoy Garg", "34_26_INTE45896526" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Reshad de Gerus", "34_26_INTE45896526" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Charlie Eastwood", "34_26_TFSP28117283" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Peter Dempsey", "34_26_TFSP28117283" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Salih Yoluç", "34_26_TFSP28117283" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Bijoy Garg", "343_26_INT8DE791EE" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Nico Müller", "343_26_INT8DE791EE" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Reshad de Gerus", "343_26_INT8DE791EE" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Lucas Fecury", "35_26_ULTI72230997" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Sebastian Gravlund", "35_26_ULTI72230997" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Terrence Woodward", "35_26_ULTI72230997" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Adrien Closmenil", "37_26_CLX_55395349" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Ian Aguilera", "37_26_CLX_55395349" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Theodor Jensen", "37_26_CLX_55395349" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Adrien Closmenil", "37_26_CLX_8047943F" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Ian Aguilera", "37_26_CLX_8047943F" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Theodor Jensen", "37_26_CLX_8047943F" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Alex Quinn", "4_26_CROWDC65437AD" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "George Kurtz", "4_26_CROWDC65437AD" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Laurin Heinrich", "4_26_CROWDC65437AD" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Antti Rammo", "4_26_DKR_E89369431" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Romain Favre", "4_26_DKR_E89369431" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Wyatt Brichacek", "4_26_DKR_E89369431" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jakub Śmiechowski", "43_25_INTE69722561" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Nick Yelloly", "43_25_INTE69722561" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Tom Dillmann", "43_25_INTE69722561" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jakub Śmiechowski", "43_26_INTEBFEF5F1A" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Nick Yelloly", "43_26_INTEBFEF5F1A" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Tom Dillmann", "43_26_INTEBFEF5F1A" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Horst Felbermayr", "44_26_PROTCA5057AF" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Horst Felix Felbermayr", "44_26_PROTCA5057AF" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Lorenzo Fluxá", "44_26_PROTCA5057AF" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Charles Milesi", "47_26_CLX_33830875" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Ferdinand Habsburg", "47_26_CLX_33830875" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Georgios Kolovos", "47_26_CLX_33830875" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Fred Poordad", "48_26_RDLI80123CF7" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Romain Dumas", "48_26_RDLI80123CF7" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Tristan Vautier", "48_26_RDLI80123CF7" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Alvise Rodella", "5_26_RINAL21482192" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "José Cautela", "5_26_RINAL21482192" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Mikkel Gaarde Pedersen", "5_26_RINAL21482192" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Custodio Toledo", "50_26_AFCO65459354" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Lilou Wadoux", "50_26_AFCO65459354" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Riccardo Agostini", "50_26_AFCO65459354" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Charles-Henri Samani", "51_26_AFCO83755194" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Conrad Laursen", "51_26_AFCO83755194" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Davide Rigon", "51_26_AFCO83755194" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Davide Rigon", "54_26_AFCO54087704" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Francesco Castellacci", "54_26_AFCO54087704" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Thomas Flohr", "54_26_AFCO54087704" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Anders Fjordbach", "54_26_HIGH62031041" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Klaus Bachler", "54_26_HIGH62031041" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Max Moritz", "54_26_HIGH62031041" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Anders Fjordbach", "54_26_HIGH91914076" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Dennis Andersen", "54_26_HIGH91914076" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Laurin Heinrich", "54_26_HIGH91914076" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "David Perel", "55_26_SPIR84723860" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Duncan Cameron", "55_26_SPIR84723860" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Matt Griffin", "55_26_SPIR84723860" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Conrad Laursen", "57_26_KESS47090593" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Daniel Serra", "57_26_KESS47090593" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Takeshi Kimura", "57_26_KESS47090593" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Daniel Serra", "57_26_KESS75581141" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Mathys Jaubert", "57_26_KESS75581141" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Takeshi Kimura", "57_26_KESS75581141" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Alexander West", "58_26_GARA79992578" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Benjamin Goethe", "58_26_GARA79992578" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Finn Gehrsitz", "58_26_GARA79992578" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Clément Mateu", "59_26_RSL_16524330" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Sébastien Baud", "59_26_RSL_16524330" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Valentin Hasse-Clot", "59_26_RSL_16524330" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Clément Mateu", "59_26_RSL_80183129" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Marius Fossard", "59_26_RSL_80183129" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Valentin Hasse-Clot", "59_26_RSL_80183129" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Abdulla Al-Khelaifi", "62_26_IRON33398475" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Adam Christodoulou", "62_26_IRON33398475" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Julian Hanses", "62_26_IRON33398475" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Abdulla Al-Khelaifi", "62_26_IRON72127902" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Julian Hanses", "62_26_IRON72127902" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Maxime Martin", "62_26_IRON72127902" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Ameerh Naran", "63_26_IRON64614378" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Rui Andrade", "63_26_IRON64614378" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Sérgio Sette Câmara", "63_26_IRON64614378" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Nick Adcock", "68_26_MRAC58337914" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Quentin Antonel", "68_26_MRAC58337914" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Thomas Imbourg", "68_26_MRAC58337914" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Anthony McIntosh", "69_26_WRT_29233637" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Dan Harper", "69_26_WRT_29233637" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Parker Thompson", "69_26_WRT_29233637" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Anthony McIntosh", "69_26_WRT_74157247" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Dan Harper", "69_26_WRT_74157247" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Parker Thompson", "69_26_WRT_74157247" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Anthony McIntosh", "69_26_WRT_87701724" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Dan Harper", "69_26_WRT_87701724" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Parker Thompson", "69_26_WRT_87701724" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Cem Bölükbaşı", "7_26_VECTO34030044" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jens Reno Møller", "7_26_VECTO34030044" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Lorenzo Fluxá", "7_26_VECTO34030044" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Andrew Gilbert", "74_26_KESS40986092" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Fran Rueda", "74_26_KESS40986092" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Romain Leroux", "74_26_KESS40986092" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Dennis Marschall", "74_26_KESS60006998" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Dustin Blattner", "74_26_KESS60006998" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Lorenzo Patrese", "74_26_KESS60006998" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Matt Kurzejewski", "75_26_PROT92649671" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Richard Lietz", "75_26_PROT92649671" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Tom Sargent", "75_26_PROT92649671" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Ben Tuck", "77_26_PROT34038378" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Eric Powell", "77_26_PROT34038378" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Sebastian Priaulx", "77_26_PROT34038378" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Harry King", "77_26_PROT43675534" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Huub van Eijndhoven", "77_26_PROT43675534" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Martin Berry", "77_26_PROT43675534" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Harry King", "77_26_PROT58898221" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Huub van Eijndhoven", "77_26_PROT58898221" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Martin Berry", "77_26_PROT58898221" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Hadrien David", "78_26_AKKO55813576" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jack Hawksworth", "78_26_AKKO55813576" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Tom Van Rompuy", "78_26_AKKO55813576" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Johannes Zelger", "79_26_IRON66566848" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Lin Hodenius", "79_26_IRON66566848" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Matteo Cressoni", "79_26_IRON66566848" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Daniel Nogales", "8_26_VIRAG56791600" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Louis Stern", "8_26_VIRAG56791600" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Matteo Quintarelli", "8_26_VIRAG56791600" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Antonio Fuoco", "83_26_AFCO19042478" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "François Perrodo", "83_26_AFCO19042478" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Matthieu Vaxivière", "83_26_AFCO19042478" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Fabien Michal", "85_26_RACE58223536" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Hugo Schwarze", "85_26_RACE58223536" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Pierre-Alexandre Provost", "85_26_RACE58223536" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Lorcan Hanafin", "86_26_GRRA35987849" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Mex Jansen", "86_26_GRRA35987849" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Michael Wainwright", "86_26_GRRA35987849" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Clemens Schmid", "87_26_AKKO35783817" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "José María López", "87_26_AKKO35783817" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Petru Umbrărescu", "87_26_AKKO35783817" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Giammarco Levorato", "88_26_PROT11517799" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Logan Sargeant", "88_26_PROT11517799" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Stefano Gattuso", "88_26_PROT11517799" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Horst Felbermayr", "88_26_PROT40617040" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Horst Felix Felbermayr", "88_26_PROT40617040" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "René Binder", "88_26_PROT40617040" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jonas Ried", "9_26_PROTO22218488" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Mike Rockenfeller", "9_26_PROTO22218488" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Sebastian Priaulx", "9_26_PROTO22218488" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Harry King", "9_26_PROTOE6B77FB3" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Jonas Ried", "9_26_PROTOE6B77FB3" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Kakunoshin Ohta", "9_26_PROTOE6B77FB3" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Ayhancan Güven", "91_26_MANT93210200" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "James Cottingham", "91_26_MANT93210200" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Timur Boguslavskiy", "91_26_MANT93210200" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Riccardo Pera", "92_26_MANT71544075" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Richard Lietz", "92_26_MANT71544075" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Yasser Shahin", "92_26_MANT71544075" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Dane Cameron", "99_26_AO_E34612493" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Louis Delétraz", "99_26_AO_E34612493" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "PJ Hyett", "99_26_AO_E34612493" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Dane Cameron", "99_26_AO_LFAE5C0D5" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "James Allen", "99_26_AO_LFAE5C0D5" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "P. J. Hyett", "99_26_AO_LFAE5C0D5" });

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_26_AD25");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_26_D09P3");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_26_G61LTP3EVO");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_26_JSP325");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_26_ORECA07");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_26ELMS_296GT3");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_26ELMS_911GT3R");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_26ELMS_AMG");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_26ELMS_AMV");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_26ELMS_MCLAREN");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_26ELMS_ORECA07");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_26ELMS_Z06GT3R");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "10_26_GARA16926271");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "10_26_VECT30391627");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "10_26_VECT83155033");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "11_26_EI_E99190308");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "13_26_13AU64749108");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "13_26_INTE97740060");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "14_26_TDS_89261174");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "14_26_TDS_B596C72F");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "150_26_AFC71744116");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "17_26_CLX_32009151");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "18_26_IDEC78043241");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "183_26_AFCCFF6FD80");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "19_26_VIRA82136742");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "2_26_TFSPO46601482");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "20_26_APR_16693969");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "21_26_AFCO31910023");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "21_26_UNIT55791918");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "22_26_UNIT48490510");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "22_26_UNIT7683F817");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "222_26_UNI72EE9E12");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "23_26_THOR64591293");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "23_26_UNIT14272908");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "24_26_NIEL37A85D27");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "24_26_NIEL86328564");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "25_26_APR_14077374");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "25_26_APR_AE921C40");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "26_26_VECT606F72C9");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "27_26_NIEL63263732");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "27_26_THOR94783089");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "28_26_IDEC52704256");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "28_26_IDECF85586AD");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "29_26_PANI67136124");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "29_26_PANIC8E05983");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "3_26_DKR_E72469578");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "3_26_DKR_LA8DF8C9A");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "30_26_DUQU13852371");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "30_26_DUQUFE1ABCFD");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "31_26_RSL_79686428");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "32_26_WRT_65186446");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "33_26_TFSP74306766");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "33_26_TFSP96347225");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "34_26_INTE45896526");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "34_26_TFSP28117283");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "343_26_INT8DE791EE");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "35_26_ULTI72230997");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "37_26_CLX_55395349");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "37_26_CLX_8047943F");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "4_26_CROWDC65437AD");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "4_26_DKR_E89369431");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "43_25_INTE69722561");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "43_26_INTEBFEF5F1A");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "44_26_PROTCA5057AF");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "47_26_CLX_33830875");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "48_26_RDLI80123CF7");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "5_26_RINAL21482192");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "50_26_AFCO65459354");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "51_26_AFCO83755194");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "54_26_AFCO54087704");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "54_26_HIGH62031041");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "54_26_HIGH91914076");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "55_26_SPIR84723860");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "57_26_KESS47090593");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "57_26_KESS75581141");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "58_26_GARA79992578");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "59_26_RSL_16524330");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "59_26_RSL_80183129");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "62_26_IRON33398475");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "62_26_IRON72127902");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "63_26_IRON64614378");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "68_26_MRAC58337914");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "69_26_WRT_29233637");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "69_26_WRT_74157247");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "69_26_WRT_87701724");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "7_26_VECTO34030044");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "74_26_KESS40986092");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "74_26_KESS60006998");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "75_26_PROT92649671");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "77_26_PROT34038378");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "77_26_PROT43675534");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "77_26_PROT58898221");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "78_26_AKKO55813576");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "79_26_IRON66566848");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "8_26_VIRAG56791600");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "83_26_AFCO19042478");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "85_26_RACE58223536");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "86_26_GRRA35987849");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "87_26_AKKO35783817");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "88_26_PROT11517799");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "88_26_PROT40617040");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "9_26_PROTO22218488");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "9_26_PROTOE6B77FB3");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "91_26_MANT93210200");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "92_26_MANT71544075");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "99_26_AO_E34612493");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "99_26_AO_LFAE5C0D5");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Genesis_GMR001",
                columns: new[] { "Engine", "Name" },
                values: new object[] { "Genesis G8MR 3.2L V8 twin turbocharged", "Genesis GMR001" });
        }
    }
}
