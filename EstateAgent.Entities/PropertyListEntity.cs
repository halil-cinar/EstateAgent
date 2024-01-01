using EstateAgent.Entities.Abstract;
using EstateAgent.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Entities
{
    [Table("PropertyListView")]
    public class PropertyListEntity:EntityBase
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

        public long AgentUserId { get; set; }
        public string AgentName { get; set; }
        public string AgentDescription { get; set; }

        public string AgentEmail { get; set; }

        public string AgentPhoneNumber { get; set; }

        public long AgentMediaId { get; set; }

        public string AgentUserName { get; set; }
        public string AgentUserSurname { get; set; }
        public DateTime AgentUserBirthDate { get; set; }
        public string AgentUserPhoneNumber { get; set; }
        public string AgentUserEmail { get; set; }
        public long? AgentUserProfilePhotoId { get; set; }
    }
}
