namespace crds_angular.Models.Crossroads.Opportunity
{
    public class OpportunityCapacityDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int? Min { get; set; }
        public int? Max { get; set; }
    }
}