using AutoMapper;
using EstateAgent.Business.Abstract;
using EstateAgent.Core.ExtensionsMethods;
using EstateAgent.DataAccess;
using EstateAgent.Dto.Dtos;
using EstateAgent.Dto.Filter;
using EstateAgent.Dto.ListDtos;
using EstateAgent.Dto.LoadMoreDtos;
using EstateAgent.Dto.Result;
using EstateAgent.Entities;
using EstateAgent.Entities.Validators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;
using static System.Formats.Asn1.AsnWriter;

namespace EstateAgent.Business
{
    public class UserManager : ManagerBase<UserEntity, UserEntity>, IUserService
    {
        private IUserRoleService _userRoleService;
        private IIdentityService _identityService;
        private IMediaService _mediaService;
        private ISystemSettingService _settingService;
        public UserManager(BaseEntityValidator<UserEntity> validator, IMapper mapper, IEntityRepository<UserEntity> repository, IEntityRepository<UserEntity> listEntityRepository, IUserRoleService userRoleService, IIdentityService identityService, IMediaService mediaService, ISystemSettingService settingService) : base(validator, mapper, repository, listEntityRepository)
        {
            _userRoleService = userRoleService;
            _identityService = identityService;
            _mediaService = mediaService;
            _settingService = settingService;
        }

        public async Task<BussinessLayerResult<long?>> Add(UserDto user)
        {
            var response = new BussinessLayerResult<long?>();
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    long? photoId = null;
                    if (user.ProfilePhoto != null)
                    {
                        var mediaResult = await _mediaService.Add(new MediaDto
                        {
                            File = user.ProfilePhoto,
                        });
                        if (mediaResult.ResultStatus == Dto.Enums.ResultStatus.Error)
                        {
                            scope.Dispose();
                            response.ErrorMessages.AddRange(mediaResult.ErrorMessages.ToList());
                            return response;
                        }
                        photoId = mediaResult.Result;
                    }


                    var entity = new UserEntity
                    {
                        BirthDate = user.BirthDate,
                        Email = user.Email,
                        Name = user.Name,
                        Surname = user.Surname,
                        PhoneNumber = user.PhoneNumber,
                        ProfilePhotoId = photoId,

                        IsDeletable = true,
                        IsDeleted = false
                    };
                    var validationResult = Validator.Validate(entity);
                    if (validationResult.IsValid)
                    {
                        Repository.Add(entity);

                        //Add Role
                        var roleResult = await _userRoleService.Add(new UserRoleDto
                        {
                            UserId = entity.Id,
                            Role = user.Role
                        });

                        if (roleResult.ResultStatus == Dto.Enums.ResultStatus.Error)
                        {
                            scope.Dispose();
                            response.ErrorMessages.AddRange(roleResult.ErrorMessages.ToList());
                            return response;
                        }

                        //Add Identity

                        var identityResult = await _identityService.Add(new IdentityDto
                        {
                            UserId = entity.Id,
                            IsValid = true,
                            ExpiryDate = null,
                            Password = user.Password,
                            UserName = user.UserName,

                        });
                        if (identityResult.ResultStatus == Dto.Enums.ResultStatus.Error)
                        {
                            scope.Dispose();
                            response.ErrorMessages.AddRange(identityResult.ErrorMessages.ToList());
                            return response;
                        }

                        scope.Complete();


                    }
                    else
                    {
                        scope.Dispose();
                        response.Result = null;
                        foreach (var err in validationResult.Errors)
                        {
                            response.AddError(Dto.Enums.ErrorMessageCode.UserUserAddValidationError, err.ErrorMessage);
                        }


                    }

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    response.Result = null;
                    response.AddError(Dto.Enums.ErrorMessageCode.UserUserAddExceptionError, ex.Message);
                }
            }
            return response;
        }

        public async Task<BussinessLayerResult<bool>> Remove(long id)
        {
            var response = new BussinessLayerResult<bool>();
            try
            {
                var entity = Repository.GetById(id);
                if (entity != null)
                {
                    entity.IsDeleted = true;
                    Repository.Update(entity);
                    response.Result = true;

                }
                else
                {
                    response.Result = false;
                    response.AddError(Dto.Enums.ErrorMessageCode.UserUserDeleteItemNotFoundError, "item is not found");
                }

            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.UserUserDeleteExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<long?>> Update(UserDto user)
        {
            var response = new BussinessLayerResult<long?>();

            try
            {
                var entity = Repository.GetById(user.Id);
                if (entity == null)
                {
                    response.Result = null;
                    response.AddError(Dto.Enums.ErrorMessageCode.UserUserUpdateItemNotFoundError, "");
                    return response;
                }

                entity.BirthDate = user.BirthDate;
                entity.Email = user.Email;
                entity.Name = user.Name;
                entity.Surname = user.Surname;
                entity.PhoneNumber = user.PhoneNumber;




                var validationResult = Validator.Validate(entity);
                if (validationResult.IsValid)
                {
                    Repository.Update(entity);

                    response.Result = entity.Id;

                }
                else
                {
                    response.Result = null;
                    foreach (var err in validationResult.Errors)
                    {
                        response.AddError(Dto.Enums.ErrorMessageCode.UserUserAddValidationError, err.ErrorMessage);
                    }


                }

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.AddError(Dto.Enums.ErrorMessageCode.UserUserAddExceptionError, ex.Message);
            }

            return response;
        }

        public async Task<BussinessLayerResult<long?>> ChangePhoto(UserDto user)
        {
            var response = new BussinessLayerResult<long?>();
            try
            {
                var entity = Repository.GetById(user.Id);
                if (entity == null)
                {
                    response.Result = null;
                    response.AddError(Dto.Enums.ErrorMessageCode.UserUserUpdateItemNotFoundError, "");
                    return response;
                }
                if (user.ProfilePhoto != null)
                {
                    var mediaResult = (entity.ProfilePhotoId != null)
                        ? await _mediaService.Update(new MediaDto
                        {
                            File = user.ProfilePhoto,
                            Id = (long)entity.ProfilePhotoId
                        })
                        : await _mediaService.Add(new MediaDto
                        {
                            File = user.ProfilePhoto,
                        });
                    if (mediaResult.ResultStatus == Dto.Enums.ResultStatus.Error)
                    {
                        response.ErrorMessages.AddRange(mediaResult.ErrorMessages.ToList());
                        return response;
                    }
                    entity.ProfilePhotoId = mediaResult.Result;
                    Repository.Update(entity);
                }
            }
            catch (Exception ex)
            {
                response.AddError(Dto.Enums.ErrorMessageCode.UserUserChangePhotoExceptionError, ex.Message);

            }
            return response;
        }

        public async Task<BussinessLayerResult<long?>> ChangeIdentity(UserDto user)
        {
            var response = new BussinessLayerResult<long?>();
            try
            {
                var entity = Repository.GetById(user.Id);
                if (entity == null)
                {
                    response.Result = null;
                    response.AddError(Dto.Enums.ErrorMessageCode.UserUserUpdateItemNotFoundError, "");
                    return response;
                }
                //Update Identity

                var identityResult = await _identityService.ChangePassword(new IdentityDto
                {
                    UserId = entity.Id,
                    IsValid = true,
                    ExpiryDate = null,
                    Password = user.Password,
                    UserName = user.UserName,

                });
                if (identityResult.ResultStatus == Dto.Enums.ResultStatus.Error)
                {
                    response.ErrorMessages.AddRange(identityResult.ErrorMessages.ToList());
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.AddError(Dto.Enums.ErrorMessageCode.UserUserChangePhotoExceptionError, ex.Message);

            }
            return response;
        }

        public async Task<BussinessLayerResult<long?>> ResetPassword(long id)
        {
            var response = new BussinessLayerResult<long?>();
            try
            {
                var entity = Repository.GetById(id);
                if (entity == null)
                {
                    response.Result = null;
                    response.AddError(Dto.Enums.ErrorMessageCode.UserUserUpdateItemNotFoundError, "");
                    return response;
                }
                
                //Update Identity
                var newPassword = ExtensionsMethods.GenerateRandomPassword(8);

                var identityResult = await _identityService.ResetPassword(entity.Id);
                if (identityResult.ResultStatus == Dto.Enums.ResultStatus.Error)
                {
                    response.ErrorMessages.AddRange(identityResult.ErrorMessages.ToList());
                    return response;
                }

                var smtpResult = await _settingService.GetSmtpValues();
                if (smtpResult.ResultStatus == Dto.Enums.ResultStatus.Error)
                {
                    response.ErrorMessages.AddRange(smtpResult.ErrorMessages.ToList()); 
                    return response;
                }

                var mailSender=new MailSender(smtpResult.Result);
                mailSender.SendEmail(
                    "Şifre Sıfırlama Bilgileri",
                    "<!DOCTYPE html> <html lang=\"tr\"> <head> <meta charset=\"UTF-8\"> <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"> <title>Şifre Sıfırlama Bilgileri</title> <style> body { font-family: Arial, sans-serif; line-height: 1.6; margin: 0; padding: 0; background-color: #f4f4f4; } .container { max-width: 600px; margin: 20px auto; background-color: #fff; padding: 20px; border-radius: 5px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); } h2 { color: #333; } p { color: #666; } .user-info { margin-top: 20px; border-top: 1px solid #ccc; padding-top: 20px; } .cta-button { display: inline-block; padding: 10px 20px; text-decoration: none; background-color: #3498db; color: #fff; border-radius: 3px; } </style> </head> <body> <div class=\"container\"> <h2>Şifre Sıfırlama Bilgileri</h2> <p>Merhaba "+entity.Name+" "+entity.Surname+",</p> <p>Yönetici tarafından şifreniz sıfırlandı. Yeni bilgileriniz aşağıdaki gibidir:</p> <div class=\"user-info\"> <p><strong>Kullanıcı Adı:</strong> "+identityResult.Result.UserName+"</p> <p><strong>Yeni Parola:</strong> "+identityResult.Result.Password+"</p> </div> <p>Yeni bilgilerinizle giriş yapabilirsiniz. Lütfen parolanızı güvenli tutun.</p>  </div> </body> </html>",
                    isHtml: true,
                    entity.Email
                    );

            }
            catch (Exception ex)
            {
                response.AddError(Dto.Enums.ErrorMessageCode.UserUserChangePhotoExceptionError, ex.Message);

            }
            return response;
        }

        public async Task<BussinessLayerResult<UserListDto>> Get(long id)
        {
            var response = new BussinessLayerResult<UserListDto>();
            try
            {
                var entity = ListEntityRepository.GetById(id);
                if (entity == null)
                {
                    response.AddError(Dto.Enums.ErrorMessageCode.UserUserGetItemNotFoundError, "item is not found");

                }
                else
                {
                    response.Result = Mapper.Map<UserListDto>(entity);
                }

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.AddError(Dto.Enums.ErrorMessageCode.UserUserGetExceptionError, ex.Message);
            }
            return response;

        }

        public async Task<BussinessLayerResult<GenericLoadMoreDto<UserListDto>>> LoadMoreFilter(LoadMoreFilter<UserFilter> filter)
        {
            var response = new BussinessLayerResult<GenericLoadMoreDto<UserListDto>>();
            try
            {
                List<UserEntity> dataList;
                List<UserListDto> result = new List<UserListDto>();
                if (filter.Filter != null)
                {
                    dataList = ListEntityRepository.GetAll(x =>
                        (
                        (filter.Filter.Name == null || filter.Filter.Name == "" || x.Name.Contains(filter.Filter.Name))
                        && (filter.Filter.PhoneNumber == null || filter.Filter.PhoneNumber == "" || x.PhoneNumber.Contains(filter.Filter.PhoneNumber))
                        && (filter.Filter.Email == null || filter.Filter.Email == "" || x.Email.Contains(filter.Filter.Email))
                        && (filter.Filter.Surname == null || filter.Filter.Surname == "" || x.Surname.Contains(filter.Filter.Surname))
                        && (filter.Filter.UserId == null || filter.Filter.UserId == x.Id)
                        && (x.IsDeleted == false)
                        ));
                }
                else
                {
                    dataList = ListEntityRepository.GetAll(x => x.IsDeleted == false);
                }

                var firstIndex = filter.PageCount * filter.ContentCount;
                var endIndex = firstIndex + filter.ContentCount;

                for (int i = firstIndex; i < endIndex; i++)
                {
                    if (i >= dataList.Count)
                    {
                        break;
                    }
                    result.Add(Mapper.Map<UserListDto>(dataList[i]));
                }

                response.Result = new GenericLoadMoreDto<UserListDto>
                {
                    Values = result,
                    ContentCount = filter.ContentCount,
                    PageCount = filter.PageCount,
                    NextPage = endIndex < dataList.Count,
                    PrevPage = firstIndex > 0,
                    TotalContentCount = dataList.Count,
                    TotalPageCount = dataList.Count / filter.ContentCount
                };

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.AddError(Dto.Enums.ErrorMessageCode.UserUserGetAllExceptionError, ex.Message);
            }
            return response;
        }
        public async Task<BussinessLayerResult<List<UserListDto>>> GetAll(UserFilter filter)
        {
            var response = new BussinessLayerResult<List<UserListDto>>();
            try
            {
                List<UserEntity> dataList;
                List<UserListDto> result = new List<UserListDto>();
                if (filter != null)
                {
                    dataList = ListEntityRepository.GetAll(x =>
                        (
                        (filter.Name == null || filter.Name == "" || x.Name.Contains(filter.Name))
                        && (filter.PhoneNumber == null || filter.PhoneNumber == "" || x.PhoneNumber.Contains(filter.PhoneNumber))
                        && (filter.Email == null || filter.Email == "" || x.Email.Contains(filter.Email))
                        && (filter.Surname == null || filter.Surname == "" || x.Surname.Contains(filter.Surname))
                        && (filter.UserId == null || filter.UserId == x.Id)
                        && (x.IsDeleted == false)
                        ));
                }
                else
                {
                    dataList = ListEntityRepository.GetAll(x => x.IsDeleted == false);
                }

                var firstIndex = 0;
                var endIndex = dataList.Count;

                for (int i = firstIndex; i < endIndex; i++)
                {
                    if (i >= dataList.Count)
                    {
                        break;
                    }
                    result.Add(Mapper.Map<UserListDto>(dataList[i]));
                }

                response.Result = result;

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.AddError(Dto.Enums.ErrorMessageCode.UserUserGetAllExceptionError, ex.Message);
            }
            return response;
        }

    }
}
