using AutoMapper;
using Paganod.Application.Features.Schema.Modify;
using Paganod.Shared.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Application.Mapping
{
    public class RecordProfile : Profile
    {
        public RecordProfile()
        {
            CreateMap<SaveRecordCommand, Record>().ReverseMap();
        }
    }
}
