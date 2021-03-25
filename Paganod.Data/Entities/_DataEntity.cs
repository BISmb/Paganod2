using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using Paganod.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Data.Entities
{
    // Generate Migrations: dotnet ef migrations add InitialCreate --project ..\Paganod.Data\
    // Apply Migrations: dotnet ef database update --project ..\Paganod.Data\

    public interface ISaveTransformation
    {
        public void ApplyTransformations();
    }

    public abstract class _DataEntity : INotifyPropertyChanged
    {
        public Guid Id => (Guid)this.GetType().GetProperty($"{this.GetType().Name}Id").GetValue(this);
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void AddEfConfig(Type typeEntity, ref EntityTypeBuilder oEntityTypeBuilder, out string strTableName)
        {
            try
            {
                PropertyChanged += (property, value) =>
                {
                    if (CreatedOn == null)
                        CreatedOn = DateTime.UtcNow;

                    ModifiedOn = DateTime.UtcNow;
                };


                IEnumerable<PropertyInfo> lstProperties = typeEntity.GetProperties();

                string RecordName = typeEntity.Name.Dehumanize();
                string PrimaryKeyColumnName = $"{RecordName}Id";
                oEntityTypeBuilder.Property<Guid>($"{PrimaryKeyColumnName}")
                                    .HasColumnType("varchar(36)")
                                    .HasDefaultValueSql("(UUID())")
                                    .IsRequired();

                // Calculate Database Defaults for Columns
                //oEntityTypeBuilder.Property(nameof(ModifiedOn)).HasComputedColumnSql();
                //oEntityTypeBuilder.Property(nameof(ModifiedOn)).HasComputedColumnSql();
                // Add Triggers to update

                // Declare Entity Key
                if (lstProperties.Any(x => x.Name == PrimaryKeyColumnName))
                    oEntityTypeBuilder.HasKey(PrimaryKeyColumnName);

                strTableName = RecordName.Pluralize(inputIsKnownToBeSingular: false);
                oEntityTypeBuilder.ToTable(strTableName, "paganod");

                // Bools stored as strings
                foreach (var property in lstProperties.Where(prop => prop.PropertyType == typeof(bool)))
                    oEntityTypeBuilder.Property(property.Name).HasConversion(new BoolToStringConverter("FALSE", "TRUE"));

                // Enums stored as strings
                foreach (var property in lstProperties.Where(prop => prop.PropertyType.BaseType == typeof(Enum)))
                    oEntityTypeBuilder.Property(property.Name).HasConversion<string>();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Entity base configuration", ex);
            }
        }
    }
}
