using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using crds_angular.Controllers.API;
using crds_angular.Models.Crossroads.Stewardship;

namespace crds_angular.Messaging.Contracts
{
    [ServiceContract]
    interface IStripeWebhookService
    {
        [OperationContract(IsOneWay = true)]
        StripeEventResponseDTO OnWebhookReceived(StripeEvent stripeEvent);
    }
}
