namespace MinistryPlatform.Translation.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public string RoomNumber { get; set; }
        public int BuildingId { get; set; }
        public int LocationId { get; set; }
        public int BanquetCapacity { get; set; }
        public string Description { get; set; }
        public int TheaterCapacity { get; set; }
    }

    public class RoomLayout
    {
        public int LayoutId { get; set; }
        public string LayoutName { get; set; }
    }

    public class Equipment
    {
        public int EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public int QuantityOnHand { get; set; }
    }
}