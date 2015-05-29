using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace crds_angular.Controllers.API
{
    class UpdateDonorDTO
    {
        string stripe_token_id { get; set; }
        string donor_id { get; set; }
    }
}
