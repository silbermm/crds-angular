using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Crossroads.Utilities.Extensions;
using log4net;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    class SendRsvpConfirmationService
    {
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
 
        private ICommunicationService _communicationService;
        private IContactService _contactService;
        private IOpportunityService _opportunityService;

        public SendRsvpConfirmationService(ICommunicationService communicationService, IContactService contactService, IOpportunityService opportunityService)
        {
            _communicationService = communicationService;
            _contactService = contactService;
            _opportunityService = opportunityService;
        }


        public void run(Dictionary<string,object> info)
        {
            var thread = new Thread(() => sendRsvp(info) );            
            thread.Start();
        }

        private void sendRsvp(Dictionary<string,object> info)
        {
            var opp = new Opportunity();
            var prevOpp = new Opportunity();
            var template = _communicationService.GetTemplate(Convert.ToInt32(info["templateId"]));

            //Go get from/to contact info
            var fromEmail = _contactService.GetContactById(Convert.ToInt32(info["groupContactId"]));
            var toEmail = _contactService.GetContactEmail(Convert.ToInt32(info["contactId"]));

            var opportunityId = Convert.ToInt32(info["opportunityId"]);
            var token = Convert.ToString(info["token"]);
            var prevOppId = Convert.ToInt32(info["prevOppId"]);
            var groupContactId = Convert.ToInt32(info["groupContactId"]);
            var contactId = Convert.ToInt32(info["contactId"]);
            var startDate = Convert.ToDateTime(info["startDate"]);
            var endDate = Convert.ToDateTime(info["endDate"]);
            var groupName = Convert.ToString(info["groupName"]);

            if (opportunityId != 0)
            {
                opp = _opportunityService.GetOpportunityById(opportunityId, token);
            }

            if (prevOppId > 0)
            {
                prevOpp = _opportunityService.GetOpportunityById(prevOppId, token);
            }

            var comm = new Communication
            {
                AuthorUserId = 5,
                DomainId = 1,
                EmailBody = template.Body,
                EmailSubject = template.Subject,
                FromContactId = groupContactId,
                FromEmailAddress = fromEmail.Email_Address,
                ReplyContactId = groupContactId,
                ReplyToEmailAddress = fromEmail.Email_Address,
                ToContactId = contactId,
                ToEmailAddress = toEmail
            };

            var mergeData = new Dictionary<string, object>
            {
                {"Opportunity_Name", opportunityId == 0 ? "Not Available" : opp.OpportunityName},
                {"Start_Date", startDate.ToShortDateString()},
                {"End_Date", endDate.ToShortDateString()},
                {"Shift_Start", opp.ShiftStart.FormatAsString() ?? string.Empty},
                {"Shift_End", opp.ShiftEnd.FormatAsString() ?? string.Empty},
                {"Room", opp.Room ?? string.Empty},
                {"Group_Contact", fromEmail.Nickname + " " + fromEmail.Last_Name},
                {"Group_Name", groupName},
                {"Previous_Opportunity_Name", prevOppId > 0 ? prevOpp.OpportunityName : @"Not Available"}
            };

            try
            {
                _communicationService.SendMessage(comm, mergeData);
            }
            catch (MinistryPlatform.Translation.Exceptions.TemplateParseException ex)
            {
                logger.Debug(string.Format("Sending email to {0} failed due to a template parsing error: {1}", contactId, ex.Message));
            }
        }

    }
}
