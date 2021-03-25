using Microsoft.EntityFrameworkCore;
using Paganod.Shared;
using Paganod.Sql.Types;
using Paganod.Sql.Utility;
//using Paganod.Types.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Sql.Methods
{
    public static class _SQL
    {
        private static IList<TableMap> _Mappings { get; set; } = new List<TableMap>();

        public static TableMap GetTableMap(string TableName = "", string RecordType = "")
        {
            TableMap tableMap = null;

            if (!TableName.IsNullOrEmpty())
            {
                if (_Mappings.Any(m => m.TableName == TableName))
                    tableMap = _Mappings.First(m => m.TableName == TableName);

                if (tableMap == null)
                    throw new Exception($"Table Map for {TableName} not found.");
            }
            else if (!RecordType.IsNullOrEmpty())
            {
                if (_Mappings.Any(m => m.RecordType == RecordType))
                    tableMap = _Mappings.First(m => m.RecordType == RecordType);

                if (tableMap == null)
                    throw new Exception($"Table Map for {RecordType} not found.");
            }

            return tableMap;
        }

        public static void UpdateTableMapFromSchemaModel(SimpleSchemaModel sm)
        {
            if (_Mappings.Where(x => x.TableName == sm.TableName).Any())
            {
                // Existing Table Mapping
                var mapping = _Mappings.First(x => x.TableName == sm.TableName);
                mapping.RecordType = sm.RecordType;
                
                // Add Columns
                foreach(var col in sm.Columns)
                    if (!mapping.Columns.Select(x => x.ColumnName).Contains(col.Name))
                        mapping.Columns.Add(new ColumnMap(col.Name));

                // Remove Columns
                for(int i = 0; i < mapping.Columns.Count; i++)
                {
                    var colMap = mapping.Columns[i];

                    if(!sm.Columns.Select(x => x.Name).Contains(colMap.ColumnName) && !colMap.IsPrimaryKey)
                        mapping.Columns.Remove(colMap);
                }
            }
            else
            {
                // New Table Mapping
                IList<ColumnMap> columnsMaps = new List<ColumnMap>();

                // Add Primary Key Column
                columnsMaps.Add(new ColumnMap(sm.PrimaryKeyColumnName, true, true, true));

                // Add Columns
                foreach (var col in sm.Columns)
                    columnsMaps.Add(new ColumnMap(col.Name));

                var mapping = new TableMap(sm.TableName, columnsMaps.ToArray(), sm.RecordType);

                _Mappings.Add(mapping);
            }

            foreach(var relation in sm.Relations)
            {
                switch (relation.RelationshipType)
                {
                    case Enums.Data.RelationshipType.OneToMany:
                        // A Column was Added to Related Entity, need to update Table Mapping
                        var relatedTblMap = GetTableMap(TableName: relation.RelatedSchemaName);

                        if (!relatedTblMap.Columns.Any(x => x.ColumnName == relation.PrincipalSchemaPrimaryKeyColumnName))
                            relatedTblMap.Columns.Add(new ColumnMap(relation.PrincipalSchemaPrimaryKeyColumnName));

                        break;
                        //case RelationshipType.Many

                }
            }
        }

        public static void RemoveTableMap(string tblName)
        {
            var predicate = new Func<TableMap, bool>(x => x.TableName == tblName);

            if (_Mappings.Any(predicate))
                _Mappings.RemoveAll(predicate);
        }
    }

    public static class EnumExt
    {
        /// <summary>
        /// Begins tracking the entities (determined by the predicate) in the Microsoft.EntityFrameworkCore.EntityState.Deleted
        ///  state such that it will be removed from the database when Microsoft.EntityFrameworkCore.DbContext.SaveChanges
        ///  is called.
        /// </summary>
        /// <typeparam name="T">The Type of DbSet</typeparam>
        /// <param name="dbSet">The DbSet</param>
        /// <param name="predicate">Predicate expression to determine which entities to make for removal</param>
        public static void RemoveAll<T>(this DbSet<T> dbSet, Func<T, bool> predicate)
            where T: class
        {
            var items = dbSet.Where(predicate).ToList();

            foreach (var item in items)
                dbSet.Remove(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="predicate"></param>
        public static void RemoveAll<T>(this IEnumerable<T> enumerable, Func<T,bool> predicate)
        {
            var items = enumerable.Where(predicate).ToList();

            foreach (var item in items)
                ((ICollection<T>)enumerable).Remove(item);
        }
    }
}
