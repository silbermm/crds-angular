namespace MinistryPlatform.Translation.Models.EventReservations
{
    public class RoomReservationDto
    {
        public int EventId { get; set; }
        public int EventRoomId { get; set; }
        public int RoomId { get; set; }
        public int RoomLayoutId { get; set; }
        public string Notes { get; set; }
        public bool Hidden { get; set; }
        public bool Cancelled { get; set; }
    }
}