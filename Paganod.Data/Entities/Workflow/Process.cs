using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Paganod.Shared;
using Paganod.Data.Entities;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Paganod.Data.Entities
{
    public class Process : _DataEntity
    {
        //public Guid ProcessId { get; set; }
        public Guid WorkflowId { get; set; }
        public Guid ConnectorId { get; set; }
        public Enums.Workflow.FlowType Type { get; set; }
        public string Action { get; set; }
        public string OpType { get; set; }
        public string Arguments_Json { get; set; }
        public string ExpectedOutput_Json { get; set; }
        //public IDictionary<string, object> Arguments { get; set; }
        //public IDictionary<string, object> Outputs { get; set; }

        public Process()
        {
            //Arguments = !Arguments_Json.IsNullOrEmpty() 
            //    ? JsonSerializer.Deserialize<IDictionary<string, object>>(Arguments_Json)
            //    : new Dictionary<string, object>();

            //Outputs = !ExpectedOutput_Json.IsNullOrEmpty()
            //    ? JsonSerializer.Deserialize<IDictionary<string, object>>(ExpectedOutput_Json)
            //    : new Dictionary<string, object>();
        }

        //public override void AddEfConfig(Type typeEntity, ref EntityTypeBuilder oEntityTypeBuilder)
        //{
        //    oEntityTypeBuilder.Ignore(nameof(Arguments));
        //    oEntityTypeBuilder.Ignore(nameof(Outputs));
        //    base.AddEfConfig(typeEntity, ref oEntityTypeBuilder);
        //}

        //public void ApplyTransformations()
        //{
        //    Arguments_Json = Arguments != null
        //        ? JsonSerializer.Serialize(Arguments)
        //        : null;

        //    ExpectedOutput_Json = Outputs != null
        //        ? JsonSerializer.Serialize(Outputs)
        //        : null;
        //}
    }
}
