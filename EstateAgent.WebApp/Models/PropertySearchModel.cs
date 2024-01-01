using EstateAgent.Entities.Enums;

namespace EstateAgent.WebApp.Models
{
    public class PropertySearchModel
    {
        public string? Search { get; set; }
        public PropertySaleStatus? Status { get; set; }

        public int? PriceSelect { get; set; }

    }
}
