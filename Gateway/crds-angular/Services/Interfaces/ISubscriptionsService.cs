using System;
using System.Collections.Generic;
using crds_angular.Models.Crossroads;

namespace crds_angular.Services.Interfaces
{
    public interface ISubscriptionsService
    {
        List<Dictionary<string, object>> GetSubscriptions(int contactId, string token);

        int SetSubscriptions(Dictionary<string, object> subscription, int contactId, string token);
    }
}