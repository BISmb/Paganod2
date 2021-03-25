using AutoMapper;
using MediatR;
using Paganod.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Paganod.Application.Features.Endpoints.Register
{
    public class RegisterEndpointCommandHandler : IRequestHandler<RegisterEndpointCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        //private readonly IUnitOfWork _unitOfWork;

        public RegisterEndpointCommandHandler(/*IUnitOfWork unitOfWork, IMapper mapper*/)
        {
            //_unitOfWork = unitOfWork;
            //_mapper = mapper;
        }

        public async Task<Result<int>> Handle(RegisterEndpointCommand command, CancellationToken cancellationToken)
        {
            //var brand = _mapper.Map<SchemaModel>(command);
            //if (brand.Id == 0)
            //{
            //    await _unitOfWork.Repository<SchemaModel>().AddAsync(brand);
            //    await _unitOfWork.Commit(cancellationToken);
            //    return Result<int>.Success(brand.Id, "Brand Saved");
            //}
            //else
            //{
            //    await _unitOfWork.Repository<Brand>().UpdateAsync(brand);
            //    await _unitOfWork.Commit(cancellationToken);
            return await Task.FromResult(Result<int>.Success(0, "Brand Updated"));
            //}
        }
    }
}
