using EstateAgent.Dto.Abstract;
using EstateAgent.Entities.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Dto.Dtos
{
    public class PropertyDto:DtoBase
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Address { get; set; }
        public long AgentId { get; set; }

        public int BedRoomCount { get; set; }

        public int LivingRoomCount { get; set; }

        public int ParkingCount { get; set; }

        public int KitchenCount { get; set; }

        public string? Details { get; set; }

        public double? LocationLatitude { get; set; }
        
        public double? LocationLongitude { get; set; }

        public PropertySaleStatus PropertySaleStatus { get; set; }

        public List<IFormFile>? Files { get; set; }


    }
}
