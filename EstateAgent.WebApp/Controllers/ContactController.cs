using EstateAgent.Business.Abstract;
using EstateAgent.Dto.Dtos;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace EstateAgent.WebApp.Controllers
{
    [Route("/Contact/")]
    public class ContactController : Controller
    {
        private IContactInfoService _contactInfoService;
        private IMessageService _messageService;
        private readonly IToastNotification _toastNotification;

        public ContactController(IContactInfoService contactInfoService, IMessageService messageService, IToastNotification toastNotification)
        {
            _contactInfoService = contactInfoService;
            _messageService = messageService;
            _toastNotification = toastNotification;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var result = await _contactInfoService.LoadMoreFilter(new Dto.Filter.LoadMoreFilter<ContactInfoFilter>
            {
                PageCount = 0,
                ContentCount = 1,
            });
            if(result.ResultStatus==Dto.Enums.ResultStatus.Success)
            {
                if(result.Result.Values.Count > 0)
                {
                    return View(result.Result.Values[0]);
                }
                
            }

            return View();
        }

        [HttpGet("/ContactInfo/{key}")]
        public async Task<string> GetContactInfo(string key)
        {
            var result = await _contactInfoService.LoadMoreFilter(new Dto.Filter.LoadMoreFilter<Dto.Dtos.ContactInfoFilter>
            {
                ContentCount = 1,
                PageCount = 0
            });
            if (result.ResultStatus == Dto.Enums.ResultStatus.Error)
            {
                return "";
            }
            if (key == "email")
            {
                return result.Result.Values[0].Email;
            }
            else if (key == "name")
            {
                return result.Result.Values[0].Name;
            }
            else if (key == "address")
            {
                return result.Result.Values[0].Address;
            }
            else if (key == "phone")
            {
                return result.Result.Values[0].Phone;
            }
            else
            {
                return "";
            }


        }

        [HttpPost("")]
        public async Task<IActionResult> Index([FromBody]MessageDto message)
        {
            var result = await _messageService.Send(message);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Error)
            {
                _toastNotification.AddSuccessToastMessage("Gönderildi");
                return NotFound();
            }
            var errormMessage = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(errormMessage);
            return NotFound();
        }

    }
}
