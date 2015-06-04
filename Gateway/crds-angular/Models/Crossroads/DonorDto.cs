using crds_angular.Services.Interfaces;

namespace crds_angular.test.controllers
{
    public class DonorDTO 
    {
        public int id { get; set; }
        public string Processor_ID { get; set; }
        public DefaultSourceDTO default_source { get; set; }
    }
}