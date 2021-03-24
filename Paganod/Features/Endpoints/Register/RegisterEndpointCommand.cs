using MediatR;
using Paganod.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Application.Features.Endpoints.Register
{
    public class RegisterEndpointCommand : IRequest<Result<int>>
    {
        public string EndpointName { get; set; }
        public string EndpointMethod { get; set; }
        public string EndpointPath { get; set; } // /transactions/create
    }
}
