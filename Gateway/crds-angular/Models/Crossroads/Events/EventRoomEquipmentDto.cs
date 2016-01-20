using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Events
{
    public class EventRoomEquipmentDto
    {

        [JsonProperty(PropertyName = "cancelled")]
        public bool Cancelled { get; set; }

        [JsonProperty(PropertyName = "equipmentId")]
        public int EquipmentId { get; set; }

        [JsonProperty(PropertyName = "quantityRequested")]
        public int QuantityRequested { get; set; }

        [JsonProperty(PropertyName = "equipmentReservationId")]
        public int EquipmentReservationId { get; set; }
    }
}