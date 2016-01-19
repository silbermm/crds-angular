using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Events
{
    public class EventToolDto
    {
        public EventToolDto()
        {
            this.Rooms = new List<EventRoomDto>();
        }

        [JsonProperty(PropertyName = "congregationId")]
        [Required]
        public int CongregationId { get; set; }

        [JsonProperty(PropertyName = "contactId")]
        [Required]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "donationBatchTool")]
        [Required]
        public bool DonationBatchTool { get; set; }

        [JsonProperty(PropertyName = "endDateTime")]
        [Required]
        public DateTime EndDateTime { get; set; }

        [JsonProperty(PropertyName = "eventTypeId")]
        [Required]
        public int EventTypeId { get; set; }

        [JsonProperty(PropertyName = "meetingInstructions")]
        public string MeetingInstructions { get; set; }

        [JsonProperty(PropertyName = "minutesSetup")]
        [Required]
        public int MinutesSetup { get; set; }

        [JsonProperty(PropertyName = "minutesTeardown")]
        [Required]
        public int MinutesTeardown { get; set; }

        [JsonProperty(PropertyName = "programId")]
        [Required]
        public int ProgramId { get; set; }

        [JsonProperty(PropertyName = "reminderDaysId")]
        public int? ReminderDaysId { get; set; }

        [JsonProperty(PropertyName = "rooms")]
        public List<EventRoomDto> Rooms { get; set; }

        [JsonProperty(PropertyName = "sendReminder")]
        [Required]
        public bool SendReminder { get; set; }

        [JsonProperty(PropertyName = "startDateTime")]
        [Required]
        public DateTime StartDateTime { get; set; }

        [JsonProperty(PropertyName = "title")]
        [Required]
        public string Title { get; set; }
    }

    public class EventRoomDto
    {
        public EventRoomDto()
        {
            this.Equipment = new List<EventRoomEquipmentDto>();
        }

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
    }

    public class EventRoomEquipmentDto
    {
        [JsonProperty(PropertyName = "equipmentId")]
        public int EquipmentId { get; set; }

        [JsonProperty(PropertyName = "quantityRequested")]
        public int QuantityRequested { get; set; }
    }
}