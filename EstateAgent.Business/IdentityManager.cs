using AutoMapper;
using EstateAgent.Business.Abstract;
using EstateAgent.Core.ExtensionsMethods;
using EstateAgent.DataAccess;
using EstateAgent.Dto.Dtos;
using EstateAgent.Dto.Filter;
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
    public class IdentityManager : ManagerBase<IdentityEntity, IdentityEntity>, IIdentityService
    {

        public IdentityManager(BaseEntityValidator<IdentityEntity> validator, IMapper mapper, IEntityRepository<IdentityEntity> repository, IEntityRepository<IdentityEntity> listEntityRepository) : base(validator, mapper, repository, listEntityRepository)
        {
        }

        public async Task<BussinessLayerResult<long?>> Add(IdentityDto identity)
        {
            var response = new BussinessLayerResult<long?>();

            try
            {
                var passwordSalt = ExtensionsMethods.GenerateRandomString(64);

                var usernameControl = Repository.GetAll(x => x.UserName == identity.UserName && x.IsValid == true && (x.ExpiryDate == null || x.ExpiryDate > DateTime.Now)).Count==0;

                if (usernameControl == false)
                {
                    response.AddError(Dto.Enums.ErrorMessageCode.IdentityIdentityAddUsernameError, "Kullanıcı Adı zaten var");
                    return response;
                }

                var entity = new IdentityEntity
                {
                    ExpiryDate = identity.ExpiryDate,
                    IsValid = identity.IsValid,
                    UserName = identity.UserName,
                    UserId = identity.UserId,

                    PasswordSalt = passwordSalt,
                    PasswordHash = ExtensionsMethods.CalculateMD5Hash(passwordSalt + identity.Password + passwordSalt),


                    IsDeletable = true,
                    IsDeleted = false
                };
                
                var validationResult=Validator.Validate(entity);
                if(validationResult.IsValid)
                {
                    Repository.Add(entity);
                    response.Result = entity.Id;

                }
                else
                {
                    response.Result = null;
                    foreach (var err in validationResult.Errors)
                    {
                        response.AddError(Dto.Enums.ErrorMessageCode.IdentityIdentityAddValidationError, err.ErrorMessage);
                    }

                   
                }
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.AddError(Dto.Enums.ErrorMessageCode.IdentityIdentityAddExceptionError,ex.Message);
            }
            return response;


        }

        public async Task<BussinessLayerResult<long?>> ChangePassword(IdentityDto identity)
        {
            var response=new BussinessLayerResult<long?>();
            try
            {
                var oldIdentities = Repository.GetAll(x => x.UserId == identity.UserId);
                foreach (var item in oldIdentities)
                {
                    item.IsDeleted = true;
                    item.IsValid = false;
                    Repository.Update(item);
                }
               var addResult= await Add(identity);
                response = addResult;
            }
            catch (Exception ex)
            {
                response.AddError(Dto.Enums.ErrorMessageCode.IdentityChangePasswordExceptionError,ex.Message);
               
            }
            return response;
        }

        public async Task<BussinessLayerResult<IdentityDto>> ResetPassword(long userId)
        {
            var response = new BussinessLayerResult<IdentityDto>();
            try
            {
                var oldIdentities = Repository.GetAll(x => x.UserId == userId);
                foreach (var item in oldIdentities)
                {
                    item.IsDeleted = true;
                    item.IsValid = false;
                    Repository.Update(item);
                }
                var newIdentity= new IdentityDto
                {
                    UserId = userId,
                    IsValid= true,
                    UserName=oldIdentities.Last().UserName,
                    Password=ExtensionsMethods.GenerateRandomPassword(8)
                };
                var addResult = await Add(newIdentity);
                if (addResult.ResultStatus == Dto.Enums.ResultStatus.Error)
                {
                    response.ErrorMessages.AddRange(addResult.ErrorMessages);
                    return response;
                }
                response.Result = newIdentity;
            }
            catch (Exception ex)
            {
                response.AddError(Dto.Enums.ErrorMessageCode.IdentityChangePasswordExceptionError, ex.Message);

            }
            return response;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <returns>UserId</returns>
        public async Task<BussinessLayerResult<IdentityListDto>> CheckPassword(IdentityDto identity)
        {
            var response = new BussinessLayerResult<IdentityListDto>();
            try
            {

                var entity = Repository.Get(x => x.UserName == identity.UserName&&x.IsDeleted==false&&x.IsValid==true);
                if(entity == null)
                {
                    response.AddError(Dto.Enums.ErrorMessageCode.IdentityCheckPasswordWrongIdentityError, "incorrect user name or password ");
                    response.Result = null;
                    return response;
                }
                var passwordhash = ExtensionsMethods.CalculateMD5Hash(entity.PasswordSalt + identity.Password + entity.PasswordSalt);
                if(passwordhash != entity.PasswordHash)
                {
                    response.AddError(Dto.Enums.ErrorMessageCode.IdentityCheckPasswordWrongIdentityError, "incorrect user name or password ");
                    response.Result = null;
                    return response;
                }

                var dto= Mapper.Map<IdentityListDto>(entity);
                dto.PasswordHash = "";
                dto.PasswordSalt = "";

                response.Result= dto;
               
            }
            catch (Exception ex)
            {
                response.AddError(Dto.Enums.ErrorMessageCode.IdentityChangePasswordExceptionError, ex.Message);

            }
            return response;
        }

        public async Task<BussinessLayerResult<IdentityListDto>> Get(long id)
        {
            var response = new BussinessLayerResult<IdentityListDto>();
            try
            {
                var result = ListEntityRepository.GetById(id);
                if(result!=null)
                {
                    response.Result=Mapper.Map<IdentityListDto>(result);
                }
                else
                {
                    response.AddError(Dto.Enums.ErrorMessageCode.IdentityIdentityGetItemNotFoundError, "item not find");
                    
                }
            }
            catch (Exception ex)
            {
                response.AddError(Dto.Enums.ErrorMessageCode.IdentityIdentityGetExceptionError,ex.Message);
                
            }
            return response;
        }

        public async Task<BussinessLayerResult<bool>> Remove(long id)
        {
            var response = new BussinessLayerResult<bool>();
            try
            {
                var result = Repository.GetById(id);
                if (result != null)
                {
                    result.IsValid= false;
                    result.IsDeleted = true;
                    Repository.Update(result);
                    response.Result = true;
                }
                else
                {
                    response.Result=false;
                    response.AddError(Dto.Enums.ErrorMessageCode.IdentityIdentityGetItemNotFoundError, "item not find");

                }
            }
            catch (Exception ex)
            {
                response.Result=false;
                response.AddError(Dto.Enums.ErrorMessageCode.IdentityIdentityGetExceptionError, ex.Message);

            }
            return response;
        }
    }
}
