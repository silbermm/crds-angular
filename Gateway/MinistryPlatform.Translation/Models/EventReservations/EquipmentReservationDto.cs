namespace MinistryPlatform.Translation.Models.EventReservations
{
    public class EquipmentReservationDto
    {
        public int EventEquipmentId { get; set; }
        public int EventId { get; set; }
        public int EventRoomId { get; set; }
        public int EquipmentId { get; set; }
        public int RoomId { get; set; }
        public string Notes { get; set; }
        public bool Cancelled { get; set; }
        public bool Approved { get; set; }
        public int QuantityRequested { get; set; }
    }
}