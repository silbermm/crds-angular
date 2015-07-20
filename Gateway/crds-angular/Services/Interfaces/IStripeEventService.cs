using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using crds_angular.Controllers.API;
using crds_angular.Models.Crossroads.Stewardship;

namespace crds_angular.Services.Interfaces
{
    public interface IStripeEventService
    {
        void ChargeSucceeded(DateTime? eventTimestamp, StripeCharge charge);
        void ChargeFailed(DateTime? eventTimestamp, StripeCharge charge);
        TransferPaidResponseDTO TransferPaid(DateTime? eventTimestamp, StripeTransfer transfer);
    }
}
