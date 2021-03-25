//using AutoMapper;
//using MediatR;
//using Paganod.Application.Interfaces;
//using Paganod.Shared;
//using Paganod.Shared.Type;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Paganod.Application.Features.Schema.Modify
//{
//    public class SaveRecordCommandHandler : IRequestHandler<SaveRecordCommand, Result<int>>
//    {
//        private readonly IMapper _mapper;
//        private readonly IUnitOfWork _unitOfWork;

//        public SaveRecordCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
//        {
//            _unitOfWork = unitOfWork;
//            _mapper = mapper;
//        }

//        public async Task<Result<int>> Handle(SaveRecordCommand command, CancellationToken cancellationToken)
//        {
//            Record r = _mapper.Map<Record>(command);


//            Guid guidRecordId = command.RecordId;
//            string strRecordType = command.RecordType;
//            Dictionary<string, object> data = command.Data;

//            //if(data)


//            //var brand = _mapper.Map<Brand>(command);
//            //if (brand.Id == 0)
//            //{
//            //    await _unitOfWork.Repository<Brand>().AddAsync(brand);
//            //    await _unitOfWork.Commit(cancellationToken);
//            //    return Result<int>.Success(brand.Id, "Brand Saved");
//            //}
//            //else
//            //{
//            //    await _unitOfWork.Repository<Brand>().UpdateAsync(brand);
//            //    await _unitOfWork.Commit(cancellationToken);
//                return Result<int>.Success(0, "Brand Updated");
//            //}
//        }
//    }
//}
