using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancePlus.Migrations
{
    /// <inheritdoc />
    public partial class AddVarlikToplamiAndUserMeSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Users_UserId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Categories_CategoryId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Users_UserId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Users_ReceiverId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Users_SenderId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_SenderId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_CategoryId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Categories_UserId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ReceiverIBAN",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "Verified",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "IsRecurring",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Transfers",
                newName: "TransferType");

            migrationBuilder.RenameColumn(
                name: "ReferenceCode",
                table: "Transfers",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "Transfers",
                newName: "UserId1");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_ReceiverId",
                table: "Transfers",
                newName: "IX_Transfers_UserId1");

            migrationBuilder.AddColumn<decimal>(
                name: "VarlikToplami",
                table: "Users",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "ReceiverUuid",
                table: "Transfers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Transfers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "senderUuid",
                table: "Transfers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Transactions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Transactions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryUuid",
                table: "Transactions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserUuid",
                table: "Transactions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_UserId",
                table: "Transfers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Users_UserId",
                table: "Transactions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Users_UserId",
                table: "Transfers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Users_UserId1",
                table: "Transfers",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Users_UserId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Users_UserId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Users_UserId1",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_UserId",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "VarlikToplami",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ReceiverUuid",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "senderUuid",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "CategoryUuid",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "UserUuid",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "Transfers",
                newName: "ReceiverId");

            migrationBuilder.RenameColumn(
                name: "TransferType",
                table: "Transfers",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Transfers",
                newName: "ReferenceCode");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_UserId1",
                table: "Transfers",
                newName: "IX_Transfers_ReceiverId");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverIBAN",
                table: "Transfers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SenderId",
                table: "Transfers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Verified",
                table: "Transfers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Transactions",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Transactions",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Transactions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Transactions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRecurring",
                table: "Transactions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Categories",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_SenderId",
                table: "Transfers",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CategoryId",
                table: "Transactions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_UserId",
                table: "Categories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Users_UserId",
                table: "Categories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Categories_CategoryId",
                table: "Transactions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Users_UserId",
                table: "Transactions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Users_ReceiverId",
                table: "Transfers",
                column: "ReceiverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Users_SenderId",
                table: "Transfers",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
