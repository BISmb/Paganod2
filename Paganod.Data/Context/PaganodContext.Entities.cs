using Microsoft.EntityFrameworkCore;
using Paganod.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Data.Context
{
    public partial class PaganodContext
    {
        public DbSet<WeatherForecast> Forecasts { get; set; }

        // Bundle
        public DbSet<Solution> Solutions { get; set; }
        public DbSet<SolutionItem> SolutionItems { get; set; }

        // Misc
        public DbSet<FileLink> FileLinks { get; set; }
        public DbSet<WebResource> WebResources { get; set; }

        // Record
        public DbSet<RecordForm> RecordForms { get; set; }
        public DbSet<RecordView> RecordViews { get; set; }
        public DbSet<RecordViewColumn> RecordViewColumns { get; set; }

        // Schema
        public DbSet<SchemaModel> SchemaModels { get; set; }
        public DbSet<SchemaModel> SchemaColumns { get; set; }
        public DbSet<SchemaModel> SchemaRelations { get; set; }

        // Workflow
        public DbSet<Script> Scripts { get; set; }
        public DbSet<WebFlow> WebFlows { get; set; }
        public DbSet<WebFlowParameters> WebFlowParameters { get; set; }
        public DbSet<ApiConnector> ApiConnectors { get; set; }
        public DbSet<Connector> Connectors { get; set; }
        public DbSet<Plugin> Plugins { get; set; }
        public DbSet<Process> Processes { get; set; }
        public DbSet<Trigger> Triggers { get; set; }
        public DbSet<Workflow> Workflows { get; set; }
    }
}
