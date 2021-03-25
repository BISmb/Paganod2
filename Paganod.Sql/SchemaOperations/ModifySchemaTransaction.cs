using FluentMigrator;
using FluentMigrator.Builders.Create.Table;

using System;
using System.Collections.Generic;
using System.Text;

using Create = FluentMigrator.Builders.Create.Table.ICreateTableWithColumnOrSchemaOrDescriptionSyntax;
using Alter = FluentMigrator.Builders.Alter.Table.IAlterTableAddColumnOrAlterColumnSyntax;
using Paganod.Shared;
using System.Data;
using System.Threading.Tasks;

using Dapper.Contrib.Extensions;
using Paganod.Sql.Utility;
using FluentMigrator.Builders.Alter.Table;
using System.Linq;
using Dapper;
using FluentMigrator.Runner.Generators.SqlServer;
using FluentMigrator.Runner.Generators;
using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Paganod.Sql.Types;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Npgsql;
using MySqlConnector;

namespace Paganod.Sql.SchemaOperations
{
    public class ModifySchemaTransaction : IDisposable
    {
        private IDbConnection _Db { get; set; }
        private SchemaMigration _SchemaMigration { get; set; }

        public ModifySchemaTransaction(IDbConnection dbContext, SimpleSchemaModel desiredModel, SimpleSchemaModel currentModel = null)
        {
            _Db = dbContext;
            _SchemaMigration = new SchemaMigration(dbContext, desiredModel, currentModel);
        }

        public Task RunAsync()
        {
            try
            {
                var migrationRunner = _Db.GetMigrationRunner();
                SchemaMigration.SetTypeMapper(migrationRunner);
                    
                migrationRunner.Up(_SchemaMigration);

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task UndoAsync()
        {
            try
            {
                var migrationRunner = _Db.GetMigrationRunner();
                migrationRunner.Down(_SchemaMigration);
                return Task.CompletedTask;

                //return new ModifySchemaResponse(true, $"{_SchemaModelName} created successfully");
            }
            catch
            {
                throw;

                //return new ModifySchemaResponse(false, $"{_SchemaModelName} was not modified");
            }
        }

        public void Dispose()
        {
            //_Db.Dispose();
            _SchemaMigration = null;
        }

        public class SchemaMigration : Migration
        {
            protected Guid _DeleteSchemaId { get; set; }
            protected SimpleSchemaModel _CurrentSchemaModel { get; set; }
            protected SimpleSchemaModel _DesiredSchemaModel { get; set; }
            protected IDbConnection _Db { get; set; }
            protected IList<string> AppendSql { get; set; } = new List<string>();

            protected static Func<DbType, int?, int?, string> GetDbType { get; set; }

            public SchemaMigration(IDbConnection dbConnection, Guid schemaId)
            {
                _DeleteSchemaId = schemaId;
                _Db = dbConnection;
            }

            public SchemaMigration(IDbConnection dbConnection, SimpleSchemaModel defModel, SimpleSchemaModel currentModel = null)
            {
                _DesiredSchemaModel = defModel;
                _Db = dbConnection;

                if (currentModel != null)
                    _CurrentSchemaModel = currentModel;
            }

            public override void Down()
            {
                RunUndoStatement(); 
            }

            public override void Up()
            {
                if (Schema.Schema("ex").Table(_DesiredSchemaModel.TableName).Exists())
                    RunAlterOperation();
                else
                    RunCreateOperation();
            }

            private void RunUndoStatement()
            {
                /*
                 * If this is a new schema, then delete the table as part of the rollback
                 */

                if (_CurrentSchemaModel == null)
                    this.DropTableIfExists("paganod", _DesiredSchemaModel.TableName);
                else
                {

                }
            }

            private void DropTableIfExists(string schemaName, string tableName)
            {
                string sql = _Db switch
                {
                    MySqlConnection => $"DROP TABLE IF EXISTS `{tableName}`;",
                    SqlConnection => $"DROP TABLE IF EXISTS [{schemaName}].[{tableName}];",
                    SqliteConnection => $"DROP TABLE IF EXISTS [{schemaName}].[{tableName}];",
                    NpgsqlConnection => throw new NotImplementedException(),
                    _ => throw new NotImplementedException(),
                };

                this.Execute.Sql(sql);
            }

            private void RunCreateOperation()
            {
                var createTableStatement = Create.Table(_DesiredSchemaModel.TableName);

                //if (_Db.GetSqlOptions().SupportsSchemaOwner)
                //    createTableStatement.InSchema("ex");

                AddTablePrimaryKeyColumn(ref createTableStatement);

                AddTableColumns(ref createTableStatement);

                AddTableRelations(ref createTableStatement);

                //AddTableConstraints(ref createTableStatement);

                // Some database types need triggers so we can retrieve the last Id (If it is a guid)
                //RegisterTriggers();
            }

            private void RunAlterOperation()
            {
                //SchemaModel sm = null;

                //using(ReadExistingSchemaTransaction readTransaction = new ReadExistingSchemaTransaction(_Db))
                //    sm = readTransaction.GetExistingSchema().Where(x => x.TableName == _DesiredSchemaModel.TableName).First();

                //SchemaModel sm = new ReadExistingSchemaTransaction(_Db).GetExistingSchema();
                //SchemaModel sm = GetSchemaModel();

                IAlterTableAddColumnOrAlterColumnSyntax tableAlterStatement = Alter.Table(_DesiredSchemaModel.TableName).InSchema("dbo");

                HandleColumnChanges(ref tableAlterStatement);

                HandleRelationChanges(ref tableAlterStatement);

                //_Db.SaveChanges();

                bool isNotSQLite = true;
                if (isNotSQLite)
                    foreach (var sql in AppendSql)
                        Execute.Sql(sql);
            }

            #region Shared Operations

            //protected static string CreateDropdownConstraint(string TableName, SchemaColumn colModel)
            //{
            //    string strDropdownBackingType = $"{colModel.Options["DROPDOWN_BACKING"]}";

            //    Enums.FieldType dropdownBackingType = Enum.Parse<Enums.FieldType>(strDropdownBackingType);
            //    string[] dropdownOptionsArray = strDropdownBackingType.Split(',');

            //    string inConstraint;
            //    if (dropdownBackingType == Enums.FieldType.Text)
            //        inConstraint = $"'{string.Join("','", dropdownOptionsArray)}'";
            //    else
            //        inConstraint = $"{string.Join(",", dropdownOptionsArray)}";

            //    return $"ALTER TABLE {TableName} ADD CONSTRAINT {colModel.Name}SelectValues CHECK({colModel.Name} IN ({inConstraint}));";
            //}

            public void AddRelation<T>(ref T statement, SimpleSchemaRelation relation)
            {
                Type TType = typeof(T);

                // One to One: Principal -- "${Referenced}Id"
                // One To Many: Referenced -- "${PrincipalSchemaName}Id"
                // Many To One: Principal -- $"{Referenced}Id"

                // Many To Many: Intersection Table => ...

                string keyType = "MySql" switch
                {
                    "MySql" => GetDbType(DbType.String, 36, null),

                    _ => GetDbType(DbType.Guid, null, null),
                };

                switch (relation.RelationshipType)
                {
                    case Enums.Data.RelationshipType.OneToOne:

                        relation.ReferencedSchemaPrimaryKeyColumnName = $"{relation.RelatedSchemaName}Id";

                        if (TType.IsAssignableTo(typeof(Create)))
                            ((Create)statement).WithColumn(relation.ReferencedSchemaPrimaryKeyColumnName).AsCustom(keyType).Nullable();
                        else if (TType.IsAssignableTo(typeof(Alter)))
                            ((Alter)statement).AddColumn(relation.ReferencedSchemaPrimaryKeyColumnName).AsCustom(keyType).Nullable();

                        break;

                    case Enums.Data.RelationshipType.OneToMany:

                        Alter.Table(relation.RelatedSchemaName)
                            .AddColumn(_DesiredSchemaModel.PrimaryKeyColumnName).AsCustom(keyType).Nullable();

                        break;

                    case Enums.Data.RelationshipType.ManyToOne:

                        //relation.ReferencedSchemaPrimaryKeyColumnName = $"{relation.RelatedSchemaName}Id";

                        if (TType.IsAssignableTo(typeof(Create)))
                            ((Create)statement).WithColumn(relation.ReferencedSchemaPrimaryKeyColumnName).AsCustom(keyType).Nullable();
                        else if (TType.IsAssignableTo(typeof(Alter)))
                            ((Alter)statement).AddColumn(relation.ReferencedSchemaPrimaryKeyColumnName).AsCustom(keyType).Nullable();

                        break;

                    case Enums.Data.RelationshipType.ManyToMany:

                        /* TODO: Need to create an intersection table */

                        throw new NotImplementedException();

                        //createTableStatement.WithColumn(referencedSchema.PrimaryKeyColumnDbName).AsGuid();

                        //Alter.Table(referencedSchema.TableDbName).AddColumn(_DefTable.PrimaryKeyColumnDbName).AsGuid();

                        //break;
                }
            }

            #endregion

            #region Create Operations

            private void RegisterTriggers()
            {
                // TODO: Check for Database Type and alter the syntax as needed

                //string sql =
                //    $@" CREATE TRIGGER ai_{_DesiredSchemaModel.RecordName}
                //    AFTER INSERT ON {_DesiredSchemaModel.TableName}
                //    FOR EACH ROW
                //    SET @last_uuid = NEW.{_DesiredSchemaModel.PrimaryKeyColumnDbName};";

                //Execute.Sql(sql);
            }

            private void AddTableConstraints(ref ICreateTableWithColumnOrSchemaOrDescriptionSyntax createTableStatement)
            {
                throw new NotImplementedException();
            }

            private void AddTableRelations(ref ICreateTableWithColumnOrSchemaOrDescriptionSyntax createTableStatement)
            {
                foreach (var relModel in _DesiredSchemaModel.Relations)
                {
                    CreateForeignKeyPair(ref createTableStatement, relModel);

                    AddRelation(ref createTableStatement, relModel);
                }
            }

            private void AddTableColumns(ref ICreateTableWithColumnOrSchemaOrDescriptionSyntax createTableStatement)
            {
                foreach (var colModel in _DesiredSchemaModel.Columns)
                {
                    AddTableColumns(ref createTableStatement, colModel);
                }
            }

            private void AddTableColumns(ref Create createTableStatement, SimpleSchemaColumn colModel)
            {
                TypeOptions typeOptions = new TypeOptions(colModel.TypeOptions);

                string dbFieldType = FromTypeDefinition(colModel.Type, new TypeOptions(colModel.TypeOptions));

                if (colModel.IsRequired && colModel.TypeOptions.ContainsKey("DEFAULT"))
                    createTableStatement.WithColumn(colModel.Name).AsCustom(dbFieldType).NotNullable().WithDefaultValue(colModel.TypeOptions["DEFAULT"]);
                else
                    createTableStatement.WithColumn(colModel.Name).AsCustom(dbFieldType).Nullable();

                //if (colModel.Type == Enums.FieldType.Dropdown)
                //    AppendSql.Add(CreateDropdownConstraint(_DesiredSchemaModel.TableName, colModel));

                //if (colModel.IsUnique)
                //    colDef.Unique(); // TODO: Name a unique index

                //if (colModel.IsRequired)
                //    colDef.NotNullable(); // TODO: Need to ensure column gets a default value
            }

            private string FromTypeDefinition(DbType dbType, TypeOptions options)
            {
                int? size = null;
                int? precision = null;

                if (options.ContainsKey("SIZE"))
                    if (Int32.TryParse(options["SIZE"], out int tmpSize))
                        size = tmpSize;

                if (options.ContainsKey("PRECISION"))
                    if (Int32.TryParse(options["PRECISION"], out int tmpPrecision))
                        size = tmpPrecision;

                string strDbType = SchemaMigration.GetDbType(dbType, size, precision);

                if (options.ContainsKey("DROPDOWN_VALUES"))
                {
                    throw new NotImplementedException();
                }

                return strDbType;
            }

            public void AddTablePrimaryKeyColumn(ref ICreateTableWithColumnOrSchemaOrDescriptionSyntax createTableStatement)
            {
                if (string.IsNullOrEmpty(_DesiredSchemaModel.PrimaryKeyColumnName))
                    _DesiredSchemaModel.PrimaryKeyColumnName = $"{_DesiredSchemaModel.TableName}Id";

                createTableStatement.WithColumn(_DesiredSchemaModel.PrimaryKeyColumnName).AsCustom("varchar(36) DEFAULT(UUID())").NotNullable().PrimaryKey();
            }

            private void CreateForeignKeyPair(ref ICreateTableWithColumnOrSchemaOrDescriptionSyntax createTableStatement, SimpleSchemaRelation relation)
            {
                //    SchemaModel referencedSchema = _Db.SchemaModels.First(x => x.SchemaModelId == relation.ReferencedSchemaId);

                //    IList<string> tokenizedSql = new List<string>();
                //    IList<string> additionalSql = new List<string>();

                //    char l = _SqlOptions.LeftSeperator;
                //    char r = _SqlOptions.RightSeperator;

                //    // Create Relation Column
                //    switch (relation.Type)
                //    {
                //        case Enums.Data.RelationshipType.OneToOne:

                //            tokenizedSql.Add($"{l}{referencedSchema.PrimaryKeyColumnDbName}{r}");
                //            tokenizedSql.Add("bigint");

                //            // TODO: Add relartion options: CASCADE, REQUIRED, etc.

                //            Create.ForeignKey()
                //                .FromTable(_DefTable.TableDbName)
                //                    .InSchema(_DefTable.OwningSchema)
                //                    .ForeignColumn(referencedSchema.PrimaryKeyColumnDbName)
                //                .ToTable(referencedSchema.TableDbName)
                //                    .InSchema(referencedSchema.OwningSchema)
                //                    .PrimaryColumn(referencedSchema.PrimaryKeyColumnDbName);

                //            break;

                //        case Enums.Data.RelationshipType.OneToMany:

                //            additionalSql.Add("ALTER");
                //            additionalSql.Add("TABLE");

                //            if (_SqlOptions.SupportsSchemaOwner)
                //                additionalSql.Add($"{l}{referencedSchema.OwningSchema}{r}.");

                //            additionalSql.Add($"{l}{referencedSchema.TableDbName}{r}");

                //            additionalSql.Add("ADD");

                //            additionalSql.Add($"{l}{_DefTable.PrimaryKeyColumnDbName}{r}");

                //            additionalSql.Add($"bigint");
                //            additionalSql.Add($";");

                //            //Alter.Table(referencedSchema.TableDbName).InSchema(referencedSchema.OwningSchema)
                //            //    .AddColumn(_DefTable.PrimaryKeyColumnDbName).AsInt64();

                //            //Create.ForeignKey()
                //            //    .FromTable(_DefTable.TableDbName)
                //            //        .InSchema(_DefTable.OwningSchema)
                //            //        .ForeignColumn(referencedSchema.PrimaryKeyColumnDbName)
                //            //    .ToTable(referencedSchema.TableDbName)
                //            //        .InSchema(referencedSchema.OwningSchema)
                //            //        .PrimaryColumn(referencedSchema.PrimaryKeyColumnDbName);

                //            break;

                //        case Enums.Data.RelationshipType.ManyToMany:

                //            // 1. Create An Intersection Table
                //            Create.Table($"{_DefTable.TableDbName}{referencedSchema.TableDbName}").InSchema(_DefTable.OwningSchema)
                //                .WithColumn($"{_DefTable.RecordDbName}{referencedSchema.RecordDbName}Id").AsGuid().PrimaryKey()
                //                .WithColumn(referencedSchema.PrimaryKeyColumnDbName).AsInt64()
                //                .WithColumn(_DefTable.PrimaryKeyColumnDbName).AsInt64();

                //            // 2. Make sure Foreign Key Columns on both Principal and Referenced in relationship

                //            // Create for _Def
                //            tokenizedSql.Add($"{l}{referencedSchema.PrimaryKeyColumnDbName}{r}");
                //            tokenizedSql.Add($"bigint");
                //            //tableCreateStatement.WithColumn(referencedSchema.PrimaryKeyColumnDbName).AsInt64();

                //            // Alter for _Referenced
                //            Alter.Table(referencedSchema.TableDbName).InSchema(referencedSchema.OwningSchema)
                //                .AddColumn(_DefTable.PrimaryKeyColumnDbName).AsInt64();

                //            // HOLD OFF 3. Create Foreign Keys

                //            break;

                //        case Enums.Data.RelationshipType.ManyToOne:

                //            tokenizedSql.Add($"{l}{referencedSchema.PrimaryKeyColumnDbName}{r}");
                //            tokenizedSql.Add("bigint");

                //            //tableCreateStatement.WithColumn(referencedSchema.PrimaryKeyColumnDbName).AsInt64();

                //            break;
                //    }

                //    return (string.Join(" ", tokenizedSql), string.Join(" ", additionalSql));
            }

            #endregion

            #region Alter Operations

            private void HandleRelationChanges(ref IAlterTableAddColumnOrAlterColumnSyntax tableAlterStatement)
            {
                foreach (var relation in _DesiredSchemaModel.Relations)
                {
                    // Addition
                    if (!_CurrentSchemaModel.Relations.Where(x => x.SchemaRelationId == relation.SchemaRelationId).Any())
                        AddRelation(ref tableAlterStatement, relation);
                }

                foreach (var relation in _CurrentSchemaModel.Relations)
                {
                    //// Deletion
                    if (!_DesiredSchemaModel.Relations.Where(x => x.SchemaRelationId == relation.SchemaRelationId).Any())
                        DropRelation(ref tableAlterStatement, relation);

                    //var desiredValueExistingColumn = _Desired.ColumnDefinitions.First(x => x.SchemaModelColumnId == rel.SchemaModelColumnId);

                    //// Rename
                    //if (col.DbName != desiredValueExistingColumn.DbName)
                    //    RenameColumn(ref tableAlterStatement, rel.DbName, desiredValueExistingColumn.DbName);

                    //// Type Change
                    //if (col.Type != desiredValueExistingColumn.Type)
                    //    AlterColumn(ref tableAlterStatement, desiredValueExistingColumn);
                }
            }

            //private void AddRelation(ref IAlterTableAddColumnOrAlterColumnSyntax tableAlterStatement, SchemaRelationship relation)
            //    => base.AddRelation(ref tableAlterStatement, relation);

            private void DropRelation(ref IAlterTableAddColumnOrAlterColumnSyntax tableAlterStatement, SimpleSchemaRelation relation)
            {
                throw new NotImplementedException();

                //if (relation.Type == Enums.Data.RelationshipType.OneToMany || relation.Type == Enums.Data.RelationshipType.OneToOne)
                //    Delete.Column(relation.).FromTable(_DefTable.TableDbName).InSchema(_DefTable.OwningSchema);
            }

            private void HandleColumnChanges(ref IAlterTableAddColumnOrAlterColumnSyntax tableAlterStatement)
            {
                foreach (var col in _DesiredSchemaModel.Columns)
                {
                    // Addition
                    if (!_CurrentSchemaModel.Columns.Where(x => x.TempId == col.TempId).Any())
                        AddColumn(ref tableAlterStatement, col);
                }

                foreach (var col in _CurrentSchemaModel.Columns)
                {
                    // Deletion
                    if (!_DesiredSchemaModel.Columns.Select(x => x.TempId == col.TempId).Any())
                        DropColumn(ref tableAlterStatement, col);

                    var desiredValueExistingColumn = _DesiredSchemaModel.Columns.First(x => x.TempId == col.TempId);

                    // Rename
                    if (col.Name != desiredValueExistingColumn.Name)
                        RenameColumn(ref tableAlterStatement, col.Name, desiredValueExistingColumn);

                    // Type Change
                    if (col.Type != desiredValueExistingColumn.Type)
                        AlterColumn(ref tableAlterStatement, desiredValueExistingColumn);
                }
            }

            private void AlterColumn(ref IAlterTableAddColumnOrAlterColumnSyntax tableAlterStatement, SimpleSchemaColumn desiredColumn)
            {
                var alterColStatement = tableAlterStatement.AlterColumn(desiredColumn.Name);

                string dbFieldType = FromTypeDefinition(desiredColumn.Type, new TypeOptions(desiredColumn.TypeOptions));
                alterColStatement.AsCustom(dbFieldType);

                //var schemaCol = _Db.SchemaColumns.First(x => x.SchemaColumnId == desiredColumn.SchemaColumnId);
                //schemaCol.Type = desiredColumn.Type;
            }

            private void RenameColumn(ref IAlterTableAddColumnOrAlterColumnSyntax tableAlterStatement, string currentDbName, SimpleSchemaColumn colDesired)
            {
                Rename.Column(currentDbName).OnTable(_DesiredSchemaModel.TableName).InSchema("ex").To(colDesired.Name);

                //var schemaCol = _Db.SchemaColumns.First(x => x.SchemaColumnId == colDesired.SchemaColumnId);
                //schemaCol.DisplayName = colDesired.DisplayName;
                //schemaCol.Name = colDesired.Name;
            }

            private void DropColumn(ref IAlterTableAddColumnOrAlterColumnSyntax tableAlterStatement, SimpleSchemaColumn col)
            {
                Delete.Column(col.Name).FromTable(_DesiredSchemaModel.TableName);//.InSchema(_DesiredSchemaModel.OwningSchema);
                //_Db.SchemaModelColumns.Remove(col); // TODO:
            }

            private void AddColumn(ref IAlterTableAddColumnOrAlterColumnSyntax tableAlterStatement, SimpleSchemaColumn desiredValue)
            {
                var createColStatement = tableAlterStatement.AddColumn(desiredValue.Name);

                // TODO: Make this a common method that will add the correct type to a column
                var createColStatementTypeDefined = createColStatement.AsString();

                //if (desiredValue.Type == Enums.FieldType.Dropdown)
                //    AppendSql.Add(CreateDropdownConstraint(_DesiredSchemaModel.TableName, desiredValue));

                //if (desiredValue.IsUnique)
                //    createColStatementTypeDefined.Unique(); // TODO: Name a unique index

                //if (desiredValue.IsRequired)
                //    createColStatementTypeDefined.NotNullable(); // TODO: Need to ensure column gets a default value

                //_Db.SchemaModelColumns.Add(desiredValue); // TODO:
            }

            internal static void SetTypeMapper(IMigrationRunner runner)
            {
                var availableFields = runner.Processor.GetType().GetRuntimeFields();

                foreach (var c in availableFields)
                {
                    var myVal = c.GetValue(runner.Processor);

                    if (c.Name == "Generator")
                    {
                        foreach (var f in myVal.GetType().GetRuntimeProperties())
                        {
                            var myVal2 = f.GetValue(myVal);

                            if (typeof(IColumn).IsAssignableFrom(myVal2.GetType()))
                            {
                                var fields = myVal2.GetType().GetRuntimeFields();
                                var properties = myVal2.GetType().GetProperties();
                                var members = myVal2.GetType().GetMembers();
                                var interfaces = myVal2.GetType().GetCustomAttributes();
                                var k = myVal2.GetType().GetRuntimeMethods();

                                foreach (var method in myVal2.GetType().GetRuntimeMethods())
                                {
                                    if (method.Name == "GetTypeMap")
                                    {
                                        //GetTypeMap = method;

                                        GetDbType = new Func<DbType, int?, int?, string>((type, size, precision) => (string)method.Invoke(myVal2, new object[] { type, size, precision }));

                                        //var typeMap = method.Invoke(myVal2, new object[] { DbType.Decimal, 5, 6 });
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #endregion
        }
    }
}
