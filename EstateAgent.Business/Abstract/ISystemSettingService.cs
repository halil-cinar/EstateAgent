using EstateAgent.Core.ExtensionsMethods;
using EstateAgent.Dto.Dtos;
using EstateAgent.Dto.Filter;
using EstateAgent.Dto.ListDtos;
using EstateAgent.Dto.LoadMoreDtos;
using EstateAgent.Dto.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Business.Abstract
{
    public interface ISystemSettingService
    {

        public Task<BussinessLayerResult<bool>> Add(SystemSettingsDto systemSettings);
        public Task<BussinessLayerResult<bool>> Update(SystemSettingsDto systemSettings);
        public Task<BussinessLayerResult<bool>> Remove(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<SystemSettingsListDto>>> LoadMoreFilter(LoadMoreFilter<SystemSettingsFilter> filter);
        public Task<BussinessLayerResult<SystemSettingsListDto>> Get(long id);
        public Task<BussinessLayerResult<SystemSettingsListDto>> Get(string key);
        public Task<BussinessLayerResult<bool>> ChangeLogo(LogoDto logo);
        public Task<BussinessLayerResult<SmtpValues>> GetSmtpValues();
    }
}
