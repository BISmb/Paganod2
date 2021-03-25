using Microsoft.EntityFrameworkCore.Migrations;
using Paganod.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Data.Migrations.Extensions
{
    public static class MigrationBuilderExtensions
    {
        public static void AddTrigger(this MigrationBuilder migrationBuilder, string strTableName, string strSchemaName = "")
        {
            try
            {
                string strDatabase = "paganod2";
                string strTriggerName = $"trigger_{strDatabase}_{strTableName}_create";

                string strTableFullName = $"paganod_{strTableName}";
                string strTriggerFullName = $"{strDatabase}.{strTriggerName}";

                string strCreateTrigger = $@"
                            DROP TRIGGER IF EXISTS {strTriggerFullName};

                            CREATE TRIGGER {strTriggerFullName}
        	                    AFTER INSERT
        	                    ON {strTableFullName} FOR EACH ROW
        			                    UPDATE NEW SET {nameof(_DataEntity.CreatedOn)} = UTC_DATE(),
                                                            {nameof(_DataEntity.ModifiedOn)} = UTC_DATE();";

                string strUpdateTrigger = $@"
                            DROP TRIGGER IF EXISTS {strTriggerFullName};

                            CREATE TRIGGER {strTriggerFullName}
        	                    AFTER INSERT
        	                    ON {strTableFullName} FOR EACH ROW
        			                    UPDATE NEW SET {nameof(_DataEntity.ModifiedOn)} = UTC_DATE();";

                migrationBuilder.Sql(strCreateTrigger);
                migrationBuilder.Sql(strUpdateTrigger);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
