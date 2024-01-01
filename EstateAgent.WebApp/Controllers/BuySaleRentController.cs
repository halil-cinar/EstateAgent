using EnumsNET;
using EstateAgent.Business.Abstract;
using EstateAgent.Dto.Result;
using EstateAgent.Entities.Enums;
using EstateAgent.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace EstateAgent.WebApp.Controllers
{
    public class BuySaleRentController : Controller
    {
        private IPropertyService _propertyService;

        public BuySaleRentController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        [HttpPost("/buysalerent")]
        public IActionResult Index(PropertySearchModel propertySearch,
            [FromQuery] int? page,
            [FromQuery] PropertySaleStatus? status,
            [FromQuery] int? minPrice,
            [FromQuery] int? maxPrice,
            [FromQuery] string? search
            )
        {
            var url = "/buysalerent?";
            if (propertySearch != null)
            {
                url += (page != null) ? "page="+page : "";
                url += (propertySearch.Status != null) ? "status=" + propertySearch.Status+"&&" : "";
                url += (propertySearch.Search != null) ? "search=" + propertySearch.Search+"&&" : "";
                url += (propertySearch.PriceSelect != null)
                    ? (propertySearch.PriceSelect == 1) ? "minPrice=150000&&maxPrice=200000&&"
                        : (propertySearch.PriceSelect == 2) ? "minPrice=200000&&maxPrice=250000&&"
                        : (propertySearch.PriceSelect == 3) ? "minPrice=250000&&maxPrice=300000&&"
                        : (propertySearch.PriceSelect == 4) ? "minPrice=300000&&"
                        : ""
                    : (minPrice != null && maxPrice != null) ? $"minPrice={minPrice} &&maxPrice={maxPrice}&&"
                        : (minPrice != null) ? "minPrice=" + minPrice + "&&"
                        : (maxPrice != null) ? "maxPrice=" + maxPrice + "&&"
                        : "";

                url+=(propertySearch.PriceSelect!=null)?"price="+propertySearch.PriceSelect
                    :"";
                ViewBag.Price = propertySearch.PriceSelect;
            }
            return Redirect(url);
        }

        public async Task<IActionResult> Index(
            [FromQuery]int? page,
            [FromQuery]PropertySaleStatus? status,
            [FromQuery] int? minPrice,
            [FromQuery] int? maxPrice,
            [FromQuery] string? search,
            [FromQuery] int? price

            )
        {
            if (page == null)
            {
                page = 0;
            }
            ViewBag.Page = page;
            ViewBag.Status = status;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.Search = search;
            ViewBag.Price = price;
            var result = await _propertyService.LoadMoreFilter(new Dto.Filter.LoadMoreFilter<Dto.Dtos.PropertyFilter>
            {
                ContentCount = 12,
                PageCount = (int)page,
                Filter=new Dto.Dtos.PropertyFilter
                {
                    PropertySaleStatus= status,
                    MinPrice= minPrice,
                    MaxPrice= maxPrice,
                    Search=search
                }
            });
            var statuses = Enum.GetValues(typeof(PropertySaleStatus));
            var statusList = new List<OptionDto<int>>();
            foreach (var role in statuses)
            {
                statusList.Add(new OptionDto<int>
                {
                    Value = (int)role,
                    Text = ((PropertySaleStatus)role).AsString(EnumFormat.Description)
                });
            }
            ViewBag.StatusOptionList = statusList;
            if (result.ResultStatus==Dto.Enums.ResultStatus.Success)
            {
                return View(result.Result);
            }


            return View();
        }
    }
}
