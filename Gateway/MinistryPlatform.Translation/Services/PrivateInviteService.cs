using System;
using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class PrivateInviteService : BaseService, IPrivateInviteService
    {
        private readonly IMinistryPlatformService _ministryPlatformService;

        public PrivateInviteService(IMinistryPlatformService ministryPlatformService, IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper)
            : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
        }

        public PrivateInvite Create(int pledgeCampaignId, string emailAddress, string recipientName, string token)
        {
            //var token = ApiLogin();
            var values = new Dictionary<string, object>
            {
                {"Email_Address", emailAddress},
                {"Recipient_Name", recipientName}
            };
            var v = new Dictionary<string, object>
            {
                {"Pledge_Campaign_ID", pledgeCampaignId},
                {"Email_Address", emailAddress},
                {"Recipient_Name", recipientName}
            };

            try
            {
                var privateInviteId = _ministryPlatformService.CreateRecord(515, v, token, true);
                //var privateInviteId = _ministryPlatformService.CreateSubRecord("TripPrivateInvite", pledgeCampaignId, values, token, false);
                //var record = _ministryPlatformService.GetSubPageRecord("TripPrivateInvite", privateInviteId, token);
                var record = _ministryPlatformService.GetRecordDict(515, privateInviteId, token, false);
                var invite = new PrivateInvite();
                invite.EmailAddress = record.ToString("Email_Address");
                invite.InvitationGuid = record.ToString("Invitation_GUID");
                invite.InvitationUsed = record.ToBool("_Invitation_Used");
                invite.PledgeCampaignId = record.ToInt("Pledge_Campaign_ID");
                invite.PledgeCampaignIdText = record.ToString("Pledge_Campaign_ID_Text");
                invite.PrivateInvitationId = record.ToInt("Private_Invitation_ID");
                invite.RecipientName = record.ToString("Recipient_Name");

                return invite;
            }
            catch (Exception e)
            {
                throw new ApplicationException(string.Format("Create Private Invite failed.  Pledge Campaign Id: {0}", pledgeCampaignId), e);
            }
        }

        public void Email()
        {
            throw new NotImplementedException();
        }

        public bool PrivateInviteValid(int pledgeCampaignId, string guid)
        {
            var token = ApiLogin();
            var searchString = string.Format(@",,""{0}""", guid);
            var records = _ministryPlatformService.GetSubpageViewRecords("TripPrivateInviteValid", pledgeCampaignId, token, searchString);
            return records.Count == 1;
        }
    }
}