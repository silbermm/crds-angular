using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Events
{
    public class EventToolDto
    {
        [JsonProperty(PropertyName = "congregationId")]
        public int CongregationId { get; set; }

        [JsonProperty(PropertyName = "contactId")]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "donationBatchTool")]
        public bool DonationBatchTool { get; set; }

        [JsonProperty(PropertyName = "endDateTime")]
        public DateTime EndDateTime { get; set; }

        [JsonProperty(PropertyName = "eventTypeId")]
        public int EventTypeId { get; set; }

        [JsonProperty(PropertyName = "meetingInstructions")]
        public string MeetingInstructions { get; set; }

        [JsonProperty(PropertyName = "minutesSetup")]
        public int MinutesSetup { get; set; }

        [JsonProperty(PropertyName = "minutesTeardown")]
        public int MinutesTeardown { get; set; }

        [JsonProperty(PropertyName = "programId")]
        public int ProgramId { get; set; }

        [JsonProperty(PropertyName = "reminderDaysId")]
        public int ReminderDaysId { get; set; }

        [JsonProperty(PropertyName = "rooms")]
        public List<EventRoomDto> Rooms { get; set; }

        [JsonProperty(PropertyName = "statDateTime")]
        public DateTime StatDateTime { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
    }

    public class EventRoomDto
    {
        [JsonProperty(PropertyName = "equipments")]
        public List<EventRoomEquipmentDto> Equipments { get; set; }
        [JsonProperty(PropertyName = "hidden")]
        public bool Hidden { get; set; }
        [JsonProperty(PropertyName = "layoutId")]
        public int LayoutId { get; set; }
        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }
        [JsonProperty(PropertyName = "roomId")]
        public int RoomId { get; set; }
    }

    public class EventRoomEquipmentDto
    {
        [JsonProperty(PropertyName = "equipmentId")]
        public int EquipmentId { get; set; }
        [JsonProperty(PropertyName = "quantityRequested")]
        public int QuantityRequested { get; set; }
    }
}