using EstateAgent.Dto.Abstract;
using EstateAgent.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Dto.Dtos
{
    public class PropertyFilter
    {
        public string? Title { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public string? Address { get; set; }
        public long? AgentId { get; set; }

        public int? MinBedRoomCount { get; set; }

        public int? MinLivingRoomCount { get; set; }

        public int? MinParkingCount { get; set; }

        public int? MinKitchenCount { get; set; }

        public int? MaxBedRoomCount { get; set; }

        public int? MaxLivingRoomCount { get; set; }

        public int? MaxParkingCount { get; set; }

        public int? MaxKitchenCount { get; set; }

        public PropertySaleStatus? PropertySaleStatus { get; set; }

        public string?  Search { get; set; }

    }
}
