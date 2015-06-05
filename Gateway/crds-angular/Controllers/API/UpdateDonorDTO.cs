using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace crds_angular.Controllers.API
{
    public class UpdateDonorDTO
    {
        public string stripe_token_id { get; set; }
        public string donor_id { get; set; }
    }
}
