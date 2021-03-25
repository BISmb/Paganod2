using MediatR;
using Paganod.Application.Features.Schema.Modify;
using Paganod.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Paganod.Application.Services
{
    public partial class ConfigService
    {
        private readonly IMediator _mediator;

        public ConfigService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<Result<int>> ModifySchemaAsync(CancellationToken oCencelToken)
        {
            ModifySchemaCommand oModifySchemaCommand = new ModifySchemaCommand();
            return _mediator.Send(oModifySchemaCommand);
        }

        //public bool ReadSchema()
        //{

        //}

        //public bool ReadSchemaDetailed()
        //{

        //}

        //public bool DeleteSchema()
        //{

        //}
    }
}
