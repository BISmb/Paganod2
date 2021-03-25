using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Paganod.Shared;
using Paganod.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Paganod.Data.Entities
{
    public class RecordForm : _DataEntity, ISaveTransformation
    {
        public Guid RecordFormId { get; set; }
        public string Name { get; set; }
        public string FormJson { get; set; }
        //public FormJsonModel FormModel { get; set; }

        public RecordForm()
        {
            //FormModel = !FormJson.IsNullOrEmpty()
            //    ? JsonSerializer.Deserialize<FormJsonModel>(FormJson)
            //    : new FormJsonModel();
        }

        public void ApplyTransformations()
        {
            if (FormJson != null)
            {
                //FormJson = JsonSerializer.Serialize<FormJsonModel>(FormModel);
            }
        }
    }
}
