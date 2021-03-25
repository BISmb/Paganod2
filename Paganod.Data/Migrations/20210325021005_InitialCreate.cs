using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Paganod.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "paganod");

            migrationBuilder.CreateTable(
                name: "ApiConnectors",
                schema: "paganod",
                columns: table => new
                {
                    ApiConnectorId = table.Column<string>(type: "varchar(36)", nullable: false, defaultValueSql: "(UUID())"),
                    ConnectorId = table.Column<Guid>(type: "char(36)", nullable: false),
                    BaseUrl = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Version = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    OpenAPISpec = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiConnectors", x => x.ApiConnectorId);
                });

            migrationBuilder.CreateTable(
                name: "Connectors",
                schema: "paganod",
                columns: table => new
                {
                    ConnectorId = table.Column<string>(type: "varchar(36)", nullable: false, defaultValueSql: "(UUID())"),
                    Type = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connectors", x => x.ConnectorId);
                });

            migrationBuilder.CreateTable(
                name: "FileLinks",
                schema: "paganod",
                columns: table => new
                {
                    FileLinkId = table.Column<string>(type: "varchar(36)", nullable: false, defaultValueSql: "(UUID())"),
                    FileName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Extension = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Link = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    RecordType = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    RecordId = table.Column<Guid>(type: "char(36)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileLinks", x => x.FileLinkId);
                });

            migrationBuilder.CreateTable(
                name: "Plugins",
                schema: "paganod",
                columns: table => new
                {
                    PluginId = table.Column<string>(type: "varchar(36)", nullable: false, defaultValueSql: "(UUID())"),
                    PluginName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    PluginReference = table.Column<byte[]>(type: "longblob", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plugins", x => x.PluginId);
                });

            migrationBuilder.CreateTable(
                name: "RecordForms",
                schema: "paganod",
                columns: table => new
                {
                    RecordFormId = table.Column<string>(type: "varchar(36)", nullable: false, defaultValueSql: "(UUID())"),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    FormJson = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordForms", x => x.RecordFormId);
                });

            migrationBuilder.CreateTable(
                name: "RecordViewColumns",
                schema: "paganod",
                columns: table => new
                {
                    RecordViewColumnId = table.Column<string>(type: "varchar(36)", nullable: false, defaultValueSql: "(UUID())"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordViewColumns", x => x.RecordViewColumnId);
                });

            migrationBuilder.CreateTable(
                name: "RecordViews",
                schema: "paganod",
                columns: table => new
                {
                    RecordViewId = table.Column<string>(type: "varchar(36)", nullable: false, defaultValueSql: "(UUID())"),
                    SchemaModelId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    QueryAsString = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordViews", x => x.RecordViewId);
                });

            migrationBuilder.CreateTable(
                name: "SchemaModels",
                schema: "paganod",
                columns: table => new
                {
                    SchemaModelId = table.Column<string>(type: "varchar(36)", nullable: false, defaultValueSql: "(UUID())"),
                    TableName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    TableDisplayName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    RecordName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    RecordDisplayName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    PrimaryKeyColumnName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchemaModels", x => x.SchemaModelId);
                });

            migrationBuilder.CreateTable(
                name: "Scripts",
                schema: "paganod",
                columns: table => new
                {
                    ScriptId = table.Column<string>(type: "varchar(36)", nullable: false, defaultValueSql: "(UUID())"),
                    LocationType = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Language = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Value = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scripts", x => x.ScriptId);
                });

            migrationBuilder.CreateTable(
                name: "SolutionItems",
                schema: "paganod",
                columns: table => new
                {
                    SolutionItemId = table.Column<string>(type: "varchar(36)", nullable: false, defaultValueSql: "(UUID())"),
                    SolutionId = table.Column<int>(type: "int", nullable: false),
                    ComponentType = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ComponentId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolutionItems", x => x.SolutionItemId);
                });

            migrationBuilder.CreateTable(
                name: "Solutions",
                schema: "paganod",
                columns: table => new
                {
                    SolutionId = table.Column<string>(type: "varchar(36)", nullable: false, defaultValueSql: "(UUID())"),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    IsConfigurable = table.Column<string>(type: "varchar(5) CHARACTER SET utf8mb4", nullable: false),
                    Version = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    InstalledBy = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    InstalledOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solutions", x => x.SolutionId);
                });

            migrationBuilder.CreateTable(
                name: "Triggers",
                schema: "paganod",
                columns: table => new
                {
                    TriggerId = table.Column<string>(type: "varchar(36)", nullable: false, defaultValueSql: "(UUID())"),
                    Type = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    SubType = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Triggers", x => x.TriggerId);
                });

            migrationBuilder.CreateTable(
                name: "WeatherForecasts",
                schema: "paganod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TemperatureC = table.Column<int>(type: "int", nullable: false),
                    Summary = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    WeatherForecastId = table.Column<string>(type: "varchar(36)", nullable: false, defaultValueSql: "(UUID())"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherForecasts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WebFlowParameters",
                schema: "paganod",
                columns: table => new
                {
                    WebFlowParametersId = table.Column<string>(type: "varchar(36)", nullable: false, defaultValueSql: "(UUID())"),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Value = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebFlowParameters", x => x.WebFlowParametersId);
                });

            migrationBuilder.CreateTable(
                name: "WebFlows",
                schema: "paganod",
                columns: table => new
                {
                    WebFlowId = table.Column<string>(type: "varchar(36)", nullable: false, defaultValueSql: "(UUID())"),
                    ScriptId = table.Column<Guid>(type: "char(36)", nullable: false),
                    FinishedProcessId = table.Column<Guid>(type: "char(36)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebFlows", x => x.WebFlowId);
                });

            migrationBuilder.CreateTable(
                name: "WebResources",
                schema: "paganod",
                columns: table => new
                {
                    WebResourceId = table.Column<string>(type: "varchar(36)", nullable: false, defaultValueSql: "(UUID())"),
                    ShortName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    PreviewImage = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Content = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebResources", x => x.WebResourceId);
                });

            migrationBuilder.CreateTable(
                name: "Workflows",
                schema: "paganod",
                columns: table => new
                {
                    WorkflowId = table.Column<string>(type: "varchar(36)", nullable: false, defaultValueSql: "(UUID())"),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workflows", x => x.WorkflowId);
                });

            migrationBuilder.CreateTable(
                name: "SchemaColumn",
                schema: "paganod",
                columns: table => new
                {
                    SchemaColumnId = table.Column<Guid>(type: "char(36)", nullable: false),
                    SchemaModelId = table.Column<string>(type: "varchar(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    DisplayName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsRequired = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    OptionsJson = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchemaColumn", x => x.SchemaColumnId);
                    table.ForeignKey(
                        name: "FK_SchemaColumn_SchemaModels_SchemaModelId",
                        column: x => x.SchemaModelId,
                        principalSchema: "paganod",
                        principalTable: "SchemaModels",
                        principalColumn: "SchemaModelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Processes",
                schema: "paganod",
                columns: table => new
                {
                    ProcessId = table.Column<string>(type: "varchar(36)", nullable: false, defaultValueSql: "(UUID())"),
                    WorkflowId = table.Column<string>(type: "varchar(36)", nullable: false),
                    ConnectorId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Type = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    Action = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    OpType = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Arguments_Json = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ExpectedOutput_Json = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processes", x => x.ProcessId);
                    table.ForeignKey(
                        name: "FK_Processes_Workflows_WorkflowId",
                        column: x => x.WorkflowId,
                        principalSchema: "paganod",
                        principalTable: "Workflows",
                        principalColumn: "WorkflowId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Processes_WorkflowId",
                schema: "paganod",
                table: "Processes",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_SchemaColumn_SchemaModelId",
                schema: "paganod",
                table: "SchemaColumn",
                column: "SchemaModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiConnectors",
                schema: "paganod");

            migrationBuilder.DropTable(
                name: "Connectors",
                schema: "paganod");

            migrationBuilder.DropTable(
                name: "FileLinks",
                schema: "paganod");

            migrationBuilder.DropTable(
                name: "Plugins",
                schema: "paganod");

            migrationBuilder.DropTable(
                name: "Processes",
                schema: "paganod");

            migrationBuilder.DropTable(
                name: "RecordForms",
                schema: "paganod");

            migrationBuilder.DropTable(
                name: "RecordViewColumns",
                schema: "paganod");

            migrationBuilder.DropTable(
                name: "RecordViews",
                schema: "paganod");

            migrationBuilder.DropTable(
                name: "SchemaColumn",
                schema: "paganod");

            migrationBuilder.DropTable(
                name: "Scripts",
                schema: "paganod");

            migrationBuilder.DropTable(
                name: "SolutionItems",
                schema: "paganod");

            migrationBuilder.DropTable(
                name: "Solutions",
                schema: "paganod");

            migrationBuilder.DropTable(
                name: "Triggers",
                schema: "paganod");

            migrationBuilder.DropTable(
                name: "WeatherForecasts",
                schema: "paganod");

            migrationBuilder.DropTable(
                name: "WebFlowParameters",
                schema: "paganod");

            migrationBuilder.DropTable(
                name: "WebFlows",
                schema: "paganod");

            migrationBuilder.DropTable(
                name: "WebResources",
                schema: "paganod");

            migrationBuilder.DropTable(
                name: "Workflows",
                schema: "paganod");

            migrationBuilder.DropTable(
                name: "SchemaModels",
                schema: "paganod");
        }
    }
}
