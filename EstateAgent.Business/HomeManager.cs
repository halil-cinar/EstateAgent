using AutoMapper;
using EstateAgent.Business.Abstract;
using EstateAgent.DataAccess;
using EstateAgent.Dto.ListDtos;
using EstateAgent.Dto.Result;
using EstateAgent.Entities;
using EstateAgent.Entities.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Business
{
    public class HomeManager : ManagerBase<HomeListEntity, HomeListEntity>, IHomeService
    {
        public HomeManager(BaseEntityValidator<HomeListEntity> validator, IMapper mapper, IEntityRepository<HomeListEntity> repository, IEntityRepository<HomeListEntity> listEntityRepository) : base(validator, mapper, repository, listEntityRepository)
        {
        }

        public async Task<BussinessLayerResult<HomeListDto>> Get()
        {
            var response = new BussinessLayerResult<HomeListDto>();
            try
            {
                var entity = ListEntityRepository.GetById(1);
                response.Result=Mapper.Map<HomeListDto>(entity);
            }catch(Exception ex)
            {
                response.AddError(Dto.Enums.ErrorMessageCode.HomeHomeGetExceptionError, ex.Message);
            }
            return response;
        }
    }
}
