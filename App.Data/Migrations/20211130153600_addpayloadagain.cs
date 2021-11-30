using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Data.Migrations
{
    public partial class addpayloadagain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BattleSamurai_Battles_BattlesBattleId",
                table: "BattleSamurai");

            migrationBuilder.DropForeignKey(
                name: "FK_BattleSamurai_Samurais_SamuraisId",
                table: "BattleSamurai");

            migrationBuilder.DropForeignKey(
                name: "FK_Horse_Samurais_SamuraiId",
                table: "Horse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Horse",
                table: "Horse");

            migrationBuilder.RenameTable(
                name: "Horse",
                newName: "Horses");

            migrationBuilder.RenameColumn(
                name: "SamuraisId",
                table: "BattleSamurai",
                newName: "SamuraiId");

            migrationBuilder.RenameColumn(
                name: "BattlesBattleId",
                table: "BattleSamurai",
                newName: "BattleId");

            migrationBuilder.RenameIndex(
                name: "IX_BattleSamurai_SamuraisId",
                table: "BattleSamurai",
                newName: "IX_BattleSamurai_SamuraiId");

            migrationBuilder.RenameIndex(
                name: "IX_Horse_SamuraiId",
                table: "Horses",
                newName: "IX_Horses_SamuraiId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateJoined",
                table: "BattleSamurai",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Horses",
                table: "Horses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BattleSamurai_Battles_BattleId",
                table: "BattleSamurai",
                column: "BattleId",
                principalTable: "Battles",
                principalColumn: "BattleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BattleSamurai_Samurais_SamuraiId",
                table: "BattleSamurai",
                column: "SamuraiId",
                principalTable: "Samurais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Horses_Samurais_SamuraiId",
                table: "Horses",
                column: "SamuraiId",
                principalTable: "Samurais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BattleSamurai_Battles_BattleId",
                table: "BattleSamurai");

            migrationBuilder.DropForeignKey(
                name: "FK_BattleSamurai_Samurais_SamuraiId",
                table: "BattleSamurai");

            migrationBuilder.DropForeignKey(
                name: "FK_Horses_Samurais_SamuraiId",
                table: "Horses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Horses",
                table: "Horses");

            migrationBuilder.DropColumn(
                name: "DateJoined",
                table: "BattleSamurai");

            migrationBuilder.RenameTable(
                name: "Horses",
                newName: "Horse");

            migrationBuilder.RenameColumn(
                name: "SamuraiId",
                table: "BattleSamurai",
                newName: "SamuraisId");

            migrationBuilder.RenameColumn(
                name: "BattleId",
                table: "BattleSamurai",
                newName: "BattlesBattleId");

            migrationBuilder.RenameIndex(
                name: "IX_BattleSamurai_SamuraiId",
                table: "BattleSamurai",
                newName: "IX_BattleSamurai_SamuraisId");

            migrationBuilder.RenameIndex(
                name: "IX_Horses_SamuraiId",
                table: "Horse",
                newName: "IX_Horse_SamuraiId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Horse",
                table: "Horse",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BattleSamurai_Battles_BattlesBattleId",
                table: "BattleSamurai",
                column: "BattlesBattleId",
                principalTable: "Battles",
                principalColumn: "BattleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BattleSamurai_Samurais_SamuraisId",
                table: "BattleSamurai",
                column: "SamuraisId",
                principalTable: "Samurais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Horse_Samurais_SamuraiId",
                table: "Horse",
                column: "SamuraiId",
                principalTable: "Samurais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
