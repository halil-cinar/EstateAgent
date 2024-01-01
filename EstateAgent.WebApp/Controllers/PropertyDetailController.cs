using EstateAgent.Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EstateAgent.WebApp.Controllers
{
    [Route("/propertydetail")]
    public class PropertyDetailController : Controller
    {
        private IPropertyService _propertyService;

        public PropertyDetailController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> Index(long id)
        {
            var result=await _propertyService.Get(id);


            if(result.ResultStatus==Dto.Enums.ResultStatus.Success)
            {
                return View(result.Result);
            }

            return NotFound();
        }
    }
}
