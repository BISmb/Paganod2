//using AutoMapper;
//using MediatR;
//using Paganod.Application.Interfaces;
//using Paganod.Data.Entities;
//using Paganod.Shared;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Paganod.Application.Features.Schema.Modify
//{
//    public class ModifySchemaCommandHandler : IRequestHandler<ModifySchemaCommand, Result<int>>
//    {
//        private readonly IMapper _mapper;
//        private readonly IUnitOfWork _unitOfWork;

//        public ModifySchemaCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
//        {
//            _unitOfWork = unitOfWork;
//            _mapper = mapper;
//        }

//        public async Task<Result<int>> Handle(ModifySchemaCommand command, CancellationToken cancellationToken)
//        {
//            // Map from Command Object class to Domain (database) model

//            // Execute Logic

//            // Return Results


//            var brand = _mapper.Map<SchemaModel>(command);
//            //if (brand.SchemaModelId == 0)
//            //{
//            //    await _unitOfWork.Repository<SchemaModel>().AddAsync(brand);
//            //    await _unitOfWork.Commit(cancellationToken);
//            //    return Result<int>.Success(brand.Id, "Brand Saved");
//            //}
//            //else
//            //{
//            //    await _unitOfWork.Repository<SchemaModel>().UpdateAsync(brand);
//            //    await _unitOfWork.Commit(cancellationToken);
//                return Result<int>.Success(0, "Brand Updated");
//            //}
//        }
//    }
//}
