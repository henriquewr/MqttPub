using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MqttPub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppAction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppAction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MqttConnection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BrokerAddress = table.Column<string>(type: "TEXT", nullable: false),
                    Topic = table.Column<string>(type: "TEXT", nullable: false),
                    ClientId = table.Column<string>(type: "TEXT", nullable: false),
                    Port = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MqttConnection", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MqttAction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    MqttConnectionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MqttAction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MqttAction_MqttConnection_MqttConnectionId",
                        column: x => x.MqttConnectionId,
                        principalTable: "MqttConnection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppActionMqttAction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    MqttActionId = table.Column<int>(type: "INTEGER", nullable: false),
                    AppActionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppActionMqttAction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppActionMqttAction_AppAction_AppActionId",
                        column: x => x.AppActionId,
                        principalTable: "AppAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppActionMqttAction_MqttAction_MqttActionId",
                        column: x => x.MqttActionId,
                        principalTable: "MqttAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MqttMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    MqttActionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MqttMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MqttMessage_MqttAction_MqttActionId",
                        column: x => x.MqttActionId,
                        principalTable: "MqttAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppAction_Name",
                table: "AppAction",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppActionMqttAction_AppActionId",
                table: "AppActionMqttAction",
                column: "AppActionId");

            migrationBuilder.CreateIndex(
                name: "IX_AppActionMqttAction_MqttActionId",
                table: "AppActionMqttAction",
                column: "MqttActionId");

            migrationBuilder.CreateIndex(
                name: "IX_MqttAction_MqttConnectionId",
                table: "MqttAction",
                column: "MqttConnectionId");

            migrationBuilder.CreateIndex(
                name: "IX_MqttAction_Name",
                table: "MqttAction",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MqttMessage_MqttActionId",
                table: "MqttMessage",
                column: "MqttActionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppActionMqttAction");

            migrationBuilder.DropTable(
                name: "MqttMessage");

            migrationBuilder.DropTable(
                name: "AppAction");

            migrationBuilder.DropTable(
                name: "MqttAction");

            migrationBuilder.DropTable(
                name: "MqttConnection");
        }
    }
}
