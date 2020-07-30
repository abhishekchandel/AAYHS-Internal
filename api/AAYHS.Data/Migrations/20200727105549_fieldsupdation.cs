using Microsoft.EntityFrameworkCore.Migrations;

namespace AAYHS.Data.Migrations
{
    public partial class fieldsupdation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ClassSponsor",
                table: "ClassSponsor");

            migrationBuilder.DropColumn(
                name: "AgeFrom",
                table: "ClassSponsor");

            migrationBuilder.DropColumn(
                name: "AgeTo",
                table: "ClassSponsor");

            migrationBuilder.RenameTable(
                name: "ClassSponsor",
                newName: "ClassSponsors");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "ExhibitorSponser",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HorseId",
                table: "ExhibitorClass",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsScratch",
                table: "ExhibitorClass",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AgeGroup",
                table: "ClassSponsors",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClassSponsors",
                table: "ClassSponsors",
                column: "ClassSponsorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ClassSponsors",
                table: "ClassSponsors");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "ExhibitorSponser");

            migrationBuilder.DropColumn(
                name: "HorseId",
                table: "ExhibitorClass");

            migrationBuilder.DropColumn(
                name: "IsScratch",
                table: "ExhibitorClass");

            migrationBuilder.DropColumn(
                name: "AgeGroup",
                table: "ClassSponsors");

            migrationBuilder.RenameTable(
                name: "ClassSponsors",
                newName: "ClassSponsor");

            migrationBuilder.AddColumn<int>(
                name: "AgeFrom",
                table: "ClassSponsor",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AgeTo",
                table: "ClassSponsor",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClassSponsor",
                table: "ClassSponsor",
                column: "ClassSponsorId");
        }
    }
}
