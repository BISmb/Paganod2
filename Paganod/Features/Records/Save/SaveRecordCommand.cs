using MediatR;
using Paganod.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Paganod.Application.Features.Schema.Modify
{
    public partial class SaveRecordCommand : IRequest<Result<int>>
    {
        public Guid RecordId { get; set; }
        public string RecordType { get; set; }
        public Dictionary<string, object> Data { get; set; } = new();
    }
}
