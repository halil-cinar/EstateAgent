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

namespace EstateAgent.Business
{
    public class MessageManager :  IMessageService
    {
        private IUserRoleService _userRoleService;
        private ISystemSettingService _settingService;

        public MessageManager(IUserRoleService userRoleService, ISystemSettingService settingService)
        {
            _userRoleService = userRoleService;
            _settingService = settingService;
        }

        public async Task<BussinessLayerResult<bool>> Send(MessageDto message)
        {
            var response = new BussinessLayerResult<bool>();

            try
            {

                var userResult = (message.SendUserId == null)
                    ? await _userRoleService.GetAll(new UserRoleFilter
                    {
                        Role = Entities.Enums.RoleTypes.Agent
                    })
                    : await _userRoleService.GetAll(new UserRoleFilter
                    {
                        UserId = message.SendUserId,
                    })
                    ;



                if (userResult.ResultStatus == Dto.Enums.ResultStatus.Error)
                {
                    response.ErrorMessages.AddRange(userResult.ErrorMessages);
                    return response;
                }


                var userList = userResult.Result;
                #region Send Emails
                var smtpResult = await _settingService.GetSmtpValues();
                if (smtpResult.ResultStatus == Dto.Enums.ResultStatus.Error)
                {
                    response.ErrorMessages.AddRange(smtpResult.ErrorMessages);
                    return response;
                }
                foreach (var user in userList)
                {

                    var emailResult = new MailSender(smtpResult.Result).SendEmail(
                        
                        $"{message.FullName} Adlı kullanıcıdan mesajınız var",
                        "<!DOCTYPE html> <html lang=\"tr\"> <head> <meta charset=\"UTF-8\"> <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"> <title>Mesaj Alındı</title> <style> body { font-family: Arial, sans-serif; line-height: 1.6; margin: 0; padding: 0; background-color: #f4f4f4; } .container { max-width: 600px; margin: 20px auto; background-color: #fff; padding: 20px; border-radius: 5px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); } h2 { color: #333; } p { color: #666; } .message-content { margin-top: 20px; border-top: 1px solid #ccc; padding-top: 20px; } .cta-button { display: inline-block; padding: 10px 20px; text-decoration: none; background-color: #3498db; color: #fff; border-radius: 3px; } </style> </head> <body> <div class=\"container\"> <h2>Mesaj Alındı</h2> <p>Merhaba "+user.UserName+" "+user.UserSurname+",</p> <p>Müşterilerimizden biri aşağıdaki mesajı gönderdi:</p> <div class=\"message-content\"> <p><strong>Mesaj:</strong></p> <p>"+message.Message+ "</p> <p><strong>Müşteri Bilgileri:</strong></p> <p><strong>İsim:</strong> "+message.FullName+"</p> <p><strong>E-posta:</strong> "+message.Email+"</p> <p><strong>Telefon:</strong> "+message.PhoneNumber+"</p> </div>  <a href=\"mailto:"+message.Email+"\" class=\"cta-button\">Yanıtla</a> </div> </body> </html>",
                        isHtml:true,
                        user.UserEmail
                        );
                }
                #endregion

                response.Result = true;

            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.MessageMessageSendExceptionError, ex.Message);
            }

            return response;
        }




    }
}
