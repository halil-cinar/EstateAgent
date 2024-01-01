using EstateAgent.Business.Abstract;
using EstateAgent.Dto.Dtos;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Diagnostics;

namespace EstateAgent.WebApp.Controllers
{
    [Route("/Subscribe")]
    public class SubscribeController : Controller
    {
        private ISubscribeService _subscribeService;
        private readonly IToastNotification _toastNotification;

        public SubscribeController(ISubscribeService subscribeService, IToastNotification toastNotification)
        {
            _subscribeService = subscribeService;
            _toastNotification = toastNotification;
        }

        [HttpPost("Add")]
        public async Task<bool> Add([FromBody] SubscribeDto subscribe)
        {
            var result = await _subscribeService.Add(subscribe);

            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                _toastNotification.AddSuccessToastMessage("Eklendi");
                return true;
            }

            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return false;
        }
        [HttpDelete("Delete/{id:long}")]
        public async Task<bool> Delete([FromBody] long id)
        {
            var result =  await _subscribeService.Remove(id);

            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                _toastNotification.AddSuccessToastMessage("Silindi");
                return true;
            }

            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return false;
        }


    }
}
