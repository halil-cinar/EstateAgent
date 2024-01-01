using EstateAgent.Business.Abstract;
using EstateAgent.Dto.Dtos;
using EstateAgent.Dto.ListDtos;
using EstateAgent.Dto.Result;
using EstateAgent.Entities.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Business
{
    public class AccountManager:IAccountService
    {
        private IIdentityService _identityService;
        private ISessionService _sessionService;
        private IHttpContextAccessor _contextAccessor;
        private IUserRoleService _userRoleService;
        private IRoleMethodService _roleMethodService;

        public AccountManager(IIdentityService identityService, ISessionService sessionService, IHttpContextAccessor contextAccessor, IUserRoleService userRoleService, IRoleMethodService roleMethodService)
        {
            _identityService = identityService;
            _sessionService = sessionService;
            _contextAccessor = contextAccessor;
            _userRoleService = userRoleService;
            _roleMethodService = roleMethodService;
        }


        public async Task<BussinessLayerResult<SessionListDto>> GetSession()
        {
            var response= new BussinessLayerResult<SessionListDto>();
            try
            {
                var result = await _sessionService.GetAll(new Dto.Filter.SessionFilter
                {
                    IpAddress = CreateIp(),
                    IsActive = true,

                });
                if(result.ResultStatus==Dto.Enums.ResultStatus.Success)
                {
                    if(result.Result!=null&&result.Result.Count>0)
                    {
                        response.Result = result.Result[0];
                        
                    }
                }
                else
                {
                    response.ErrorMessages.AddRange(result.ErrorMessages);
                }
            }catch(Exception ex)
            {

            }
            return response;
        }


        public async Task<BussinessLayerResult<SessionListDto>> Login(IdentityDto identity)
        {
            var response=new BussinessLayerResult<SessionListDto>();
            try
            {
                response.Result = null;
                var identityResult=await _identityService.CheckPassword(identity);
                if(identityResult.ResultStatus==Dto.Enums.ResultStatus.Error)
                {
                    response.ErrorMessages.AddRange(identityResult.ErrorMessages);
                   
                    return response;
                }
                if (identityResult.Result != null)
                {
                    var sessionResult =await _sessionService.Add(new SessionDto
                    {
                        IdentityId = identityResult.Result.Id,
                        UserId = identityResult.Result.UserId,
                        ExpiryDate = DateTime.Now.AddDays(1),
                        IpAddress = CreateIp(),
                        IsActive = true
                    });
                    if (sessionResult.ResultStatus == Dto.Enums.ResultStatus.Error)
                    {
                        response.ErrorMessages.AddRange(sessionResult.ErrorMessages);

                        return response;
                    }
                    var sessionGetResult=await _sessionService.Get((long)sessionResult.Result);
                    if (sessionGetResult.ResultStatus == Dto.Enums.ResultStatus.Error)
                    {
                        response.ErrorMessages.AddRange(sessionGetResult.ErrorMessages);

                        return response;
                    }

                    response.Result=sessionGetResult.Result;
                }
               
            }catch(Exception ex)
            {
                response.AddError(Dto.Enums.ErrorMessageCode.AccountAccountLoginExceptionError,ex.Message); 
                return response;
            }
            return response;
        }

        public async Task<BussinessLayerResult<List<MethodTypes>>> AuthorizationControl(long? sessionId)
        {
            var response=new BussinessLayerResult<List<MethodTypes>>();
           
            try
            {
                List<RoleTypes> RoleIds = new List<RoleTypes>();
                RoleIds.Add(RoleTypes.Guest);
                List<MethodTypes> methods = new List<MethodTypes>();
                if(sessionId != null)
                {
                    var sessionResult = await _sessionService.Get((long)sessionId);
                    if (sessionResult.ResultStatus == Dto.Enums.ResultStatus.Error)
                    {
                        response.ErrorMessages.AddRange(sessionResult.ErrorMessages);
                        return response;
                    }
                    var userRoleResult = await _userRoleService.GetAll(new UserRoleFilter
                    {
                        UserId = sessionResult.Result.UserId
                    });
                    if(userRoleResult.ResultStatus== Dto.Enums.ResultStatus.Error)
                    {
                        response.ErrorMessages.AddRange(userRoleResult.ErrorMessages);
                        return response;
                    }
                    RoleIds.AddRange(userRoleResult.Result.Select(x=>x.Role).ToList());
                }

                foreach (var role in RoleIds)
                {
                    var methodResuult=await _roleMethodService.GetAll(new Dto.Filter.RoleMethodFilter
                    {
                        Role = role
                    });

                    if (methodResuult.ResultStatus == Dto.Enums.ResultStatus.Error)
                    {
                        response.ErrorMessages.AddRange(methodResuult.ErrorMessages);
                        return response;
                    }
                    methods.AddRange(methodResuult.Result.Select(x=>x.Method).ToList());
                }

                response.Result = methods;

            }catch(Exception ex)
            {
                response.AddError(Dto.Enums.ErrorMessageCode.AccountAccountAuthorizationControlExceptionError, ex.Message);
            }
            return response;
            
            
        }

        public async Task<BussinessLayerResult<SessionListDto>> Logout()
        {
            var response = new BussinessLayerResult<SessionListDto>();
            try
            {
                var result = await _sessionService.GetAll(new Dto.Filter.SessionFilter
                {
                    IpAddress = CreateIp(),
                    IsActive = true,

                });
                if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
                {
                    if (result.Result != null && result.Result.Count > 0)
                    {
                        foreach (var item in result.Result)
                        {
                            var delResult= await _sessionService.Remove(item.Id);
                            if(delResult.ResultStatus==Dto.Enums.ResultStatus.Error)
                            {
                                response.ErrorMessages.AddRange(delResult.ErrorMessages);
                            }
                        }

                    }
                }
                else
                {
                    response.ErrorMessages.AddRange(result.ErrorMessages);
                }
            }
            catch (Exception ex)
            {

            }
            return response;
        }


        private string CreateIp()
        {
            var ip = "";
            ip += _contextAccessor.HttpContext.Connection.RemoteIpAddress ;
            return ip;
        }


    }
}
