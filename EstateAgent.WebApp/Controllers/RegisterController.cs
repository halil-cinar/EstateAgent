using EstateAgent.Business.Abstract;
using EstateAgent.Dto.Dtos;
using EstateAgent.Dto.Enums;
using Microsoft.AspNetCore.Mvc;

namespace EstateAgent.WebApp.Controllers
{
    public class RegisterController : Controller
    {
        private IUserService _userService;

        public RegisterController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost()]
        public async Task<IActionResult> Index([FromForm]UserDto userDto)
        {
            userDto.Role = Entities.Enums.RoleTypes.User;
            var result= await _userService.Add(userDto);
            if (result.ResultStatus==ResultStatus.Success)
            {
                return RedirectToAction("Index","Home");
            }
            return View();
        }


    }
}
