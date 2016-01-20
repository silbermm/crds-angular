using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Events
{
    public class EventRoomDto
    {
        public EventRoomDto()
        {
            this.Equipment = new List<EventRoomEquipmentDto>();
        }

        [JsonProperty(PropertyName = "cancelled")]
        public bool Cancelled { get; set; }

        [JsonProperty(PropertyName = "equipment")]
        public List<EventRoomEquipmentDto> Equipment { get; set; }

        [JsonProperty(PropertyName = "hidden")]
        public bool Hidden { get; set; }

        [JsonProperty(PropertyName = "layoutId")]
        public int LayoutId { get; set; }

        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }

        [JsonProperty(PropertyName = "roomId")]
        public int RoomId { get; set; }

        [JsonProperty(PropertyName = "roomReservationId")]
        public int RoomReservationId { get; set; }
    }
}