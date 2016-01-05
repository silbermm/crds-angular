using System;
using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Trip;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Extensions;
using Crossroads.Utilities.Interfaces;
using Crossroads.Utilities.Services;
using log4net;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using Event = MinistryPlatform.Models.Event;
using IDonationService = MinistryPlatform.Translation.Services.Interfaces.IDonationService;
using IDonorService = MinistryPlatform.Translation.Services.Interfaces.IDonorService;
using IGroupService = MinistryPlatform.Translation.Services.Interfaces.IGroupService;
using PledgeCampaign = crds_angular.Models.Crossroads.Stewardship.PledgeCampaign;

namespace crds_angular.Services
{
    public class TripService : MinistryPlatformBaseService, ITripService
    {
        private readonly IEventParticipantService _eventParticipantService;
        private readonly IDonationService _donationService;
        private readonly IGroupService _groupService;
        private readonly IFormSubmissionService _formSubmissionService;
        private readonly MinistryPlatform.Translation.Services.Interfaces.IEventService _mpEventService;
        private readonly IDonorService _mpDonorService;
        private readonly IPledgeService _mpPledgeService;
        private readonly ICampaignService _campaignService;
        private readonly IPrivateInviteService _privateInviteService;
        private readonly ICommunicationService _communicationService;
        private readonly IContactService _contactService;
        private readonly IContactRelationshipService _contactRelationshipService;
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly IPersonService _personService;
        private readonly IServeService _serveService;
        private readonly IDestinationService _destinationService;
        private readonly ILog _logger = LogManager.GetLogger(typeof (TripService));

        public TripService(IEventParticipantService eventParticipant,
                           IDonationService donationService,
                           IGroupService groupService,
                           IFormSubmissionService formSubmissionService,
                           MinistryPlatform.Translation.Services.Interfaces.IEventService eventService,
                           IDonorService donorService,
                           IPledgeService pledgeService,
                           ICampaignService campaignService,
                           IPrivateInviteService privateInviteService,
                           ICommunicationService communicationService,
                           IContactService contactService,
                           IContactRelationshipService contactRelationshipService,
                           IConfigurationWrapper configurationWrapper,
                           IPersonService personService,
                           IServeService serveService,
                           IDestinationService destinationService)
        {
            _eventParticipantService = eventParticipant;
            _donationService = donationService;
            _groupService = groupService;
            _formSubmissionService = formSubmissionService;
            _mpEventService = eventService;
            _mpDonorService = donorService;
            _mpPledgeService = pledgeService;
            _campaignService = campaignService;
            _privateInviteService = privateInviteService;
            _communicationService = communicationService;
            _contactService = contactService;
            _contactRelationshipService = contactRelationshipService;
            _configurationWrapper = configurationWrapper;
            _personService = personService;
            _serveService = serveService;
            _destinationService = destinationService;
        }

        public List<TripGroupDto> GetGroupsByEventId(int eventId)
        {
            var mpGroups = _groupService.GetGroupsForEvent(eventId);

            return mpGroups.Select(record => new TripGroupDto
            {
                GroupId = record.GroupId,
                GroupName = record.Name
            }).ToList();
        }

        public TripFormResponseDto GetFormResponses(int selectionId, int selectionCount, int formResponseId)
        {
            var tripApplicantResponses = GetTripAplicants(selectionId, selectionCount, formResponseId);

            var dto = new TripFormResponseDto();
            if (tripApplicantResponses.Errors != null)
            {
                if (tripApplicantResponses.Errors.Count != 0)
                {
                    dto.Errors = tripApplicantResponses.Errors;
                    return dto;
                }
            }

            //get groups for event, type = go trip
            var eventId = tripApplicantResponses.TripInfo.EventId;
            var eventGroups = _mpEventService.GetGroupsForEvent(eventId); //need to add check for group type?  TM 8/17

            dto.Applicants = tripApplicantResponses.Applicants;
            dto.Groups = eventGroups.Select(s => new TripGroupDto {GroupId = s.GroupId, GroupName = s.Name}).ToList();
            dto.Campaign = new PledgeCampaign
            {
                FundraisingGoal = tripApplicantResponses.TripInfo.FundraisingGoal,
                PledgeCampaignId = tripApplicantResponses.TripInfo.PledgeCampaignId,
                DestinationId = tripApplicantResponses.TripInfo.DestinationId
            };

            return dto;
        }

        public TripCampaignDto GetTripCampaign(int pledgeCampaignId)
        {
            var campaign = _campaignService.GetPledgeCampaign(pledgeCampaignId);
            if (campaign == null)
            {
                return null;
            }
            return new TripCampaignDto()
            {
                Id = campaign.Id,
                Name = campaign.Name,
                FormId = campaign.FormId,
                Nickname = campaign.Nickname,
                YoungestAgeAllowed = campaign.YoungestAgeAllowed,
                RegistrationEnd = campaign.RegistrationEnd,
                RegistrationStart = campaign.RegistrationStart,
                AgeExceptions = campaign.AgeExceptions
            };
        }

        private TripApplicantResponse GetTripAplicants(int selectionId, int selectionCount, int formResponseId)
        {
            var responses = formResponseId == -1
                ? _formSubmissionService.GetTripFormResponsesBySelectionId(selectionId)
                : _formSubmissionService.GetTripFormResponsesByRecordId(formResponseId);

            var messages = ValidateResponse(selectionCount, formResponseId, responses);
            return messages.Count > 0
                ? new TripApplicantResponse {Errors = messages.Select(m => new TripToolError {Message = m}).ToList()}
                : FormatTripResponse(responses);
        }

        private static TripApplicantResponse FormatTripResponse(List<TripFormResponse> responses)
        {
            var tripInfo = responses
                .Select(r =>
                            r.EventId != null
                                ? (r.PledgeCampaignId != null
                                    ? new TripInfo
                                    {
                                        EventId = (int) r.EventId,
                                        FundraisingGoal = r.FundraisingGoal,
                                        PledgeCampaignId = (int) r.PledgeCampaignId,
                                        DestinationId = r.DestinationId
                                    }
                                    : null)
                                : null);

            var applicants = responses.Select(record => new TripApplicant
            {
                ContactId = record.ContactId,
                DonorId = record.DonorId,
                ParticipantId = record.ParticipantId
            }).ToList();

            var resp = new TripApplicantResponse
            {
                Applicants = applicants,
                TripInfo = tripInfo.First()
            };
            return resp;
        }

        public List<FamilyMemberTripDto> GetFamilyMembers(int pledgeId, string token)
        {
            var family = _serveService.GetImmediateFamilyParticipants(token);
            var fam = new List<FamilyMemberTripDto>();
            foreach (var f in family)
            {
                // get status of family member on trip
                var signedUpDate = _formSubmissionService.GetTripFormResponseByContactId(f.ContactId, pledgeId);
                var fm = new FamilyMemberTripDto()
                {
                    Age = f.Age,
                    ContactId = f.ContactId,
                    Email = f.Email,
                    LastName = f.LastName,
                    LoggedInUser = f.LoggedInUser,
                    ParticipantId = f.ParticipantId,
                    PreferredName = f.PreferredName,
                    RelationshipId = f.RelationshipId,
                    SignedUpDate = signedUpDate,
                    SignedUp = (signedUpDate != null)
                };
                fam.Add(fm);
            }
            return fam;
        }

        private static List<string> ValidateResponse(int selectionCount, int formResponseId, List<TripFormResponse> responses)
        {
            var messages = new List<string>();
            if (responses.Count == 0)
            {
                messages.Add("Could not retrieve records from Ministry Platform");
            }

            if (formResponseId == -1 && responses.Count != selectionCount)
            {
                messages.Add("Error Retrieving Selection");
                messages.Add(string.Format("You selected {0} records in Ministry Platform, but only {1} were retrieved.", selectionCount, responses.Count));
                messages.Add("Please verify records you selected.");
            }

            var campaignCount = responses.GroupBy(x => x.PledgeCampaignId)
                .Select(x => new {Date = x.Key, Values = x.Distinct().Count()}).Count();
            if (campaignCount > 1)
            {
                messages.Add("Invalid Trip Selection - Multiple Campaigns Selected");
            }
            return messages;
        }

        public List<TripParticipantDto> Search(string search)
        {
            var results = _eventParticipantService.TripParticipants(search);

            var participants = results.GroupBy(r =>
                                                   new
                                                   {
                                                       r.ParticipantId,
                                                       r.ContactId,
                                                       r.EmailAddress,
                                                       r.Lastname,
                                                       r.Nickname
                                                   }).Select(x => new TripParticipantDto()
                                                   {
                                                       ParticipantId = x.Key.ParticipantId,
                                                       ContactId = x.Key.ContactId,
                                                       Email = x.Key.EmailAddress,
                                                       Lastname = x.Key.Lastname,
                                                       Nickname = x.Key.Nickname,
                                                       ShowGiveButton = true,
                                                       ShowShareButtons = false
                                                   }).ToDictionary(y => y.ParticipantId);

            foreach (var result in results)
            {
                // check status of pledge for campaign
                var pledge = _mpPledgeService.GetPledgeByCampaignAndDonor(result.CampaignId, result.DonorId);
                if (pledge == null || pledge.PledgeStatusId == 3)
                {
                    continue;
                }
                var tp = new TripDto
                {
                    EventParticipantId = result.EventParticipantId,
                    EventEnd = result.EventEndDate.ToString("MMM dd, yyyy"),
                    EventId = result.EventId,
                    EventStartDate = result.EventStartDate.ToUnixTime(),
                    EventStart = result.EventStartDate.ToString("MMM dd, yyyy"),
                    EventTitle = result.EventTitle,
                    EventType = result.EventType,
                    ProgramId = result.ProgramId,
                    ProgramName = result.ProgramName,
                    CampaignId = result.CampaignId,
                    CampaignName = result.CampaignName,
                    PledgeDonorId = result.DonorId
                };
                var participant = participants[result.ParticipantId];
                participant.Trips.Add(tp);
            }
            return participants.Values.Where(x => x.Trips.Count > 0).OrderBy(o => o.Lastname).ThenBy(o => o.Nickname).ToList();
        }

        public MyTripsDto GetMyTrips(string token)
        {
            var family = _serveService.GetImmediateFamilyParticipants(token);
            var familyTrips = new MyTripsDto();

            foreach (var member in family)
            {
                var trips = TripForContact(member.ContactId);
                familyTrips.MyTrips.AddRange(trips.MyTrips);
            }

            return familyTrips;
        }

        private MyTripsDto TripForContact(int contactId)
        {
            var searchString = ",,,,,,,,,,,,," + contactId;
            var trips = _eventParticipantService.TripParticipants(searchString);

            var distributions = _donationService.GetMyTripDistributions(contactId).OrderBy(t => t.EventStartDate);
            var myTrips = new MyTripsDto();

            var events = new List<Trip>();
            var eventIds = new List<int>();
            foreach (var trip in trips.Where(trip => !eventIds.Contains(trip.EventId)))
            {
                var tripParticipant = _eventParticipantService.TripParticipants("," + trip.EventId + ",,,,,,,,,,,," + contactId).FirstOrDefault();
                if (tripParticipant == null)
                {
                    continue;
                }
                eventIds.Add(trip.EventId);

                var campaign = _mpPledgeService.GetPledgeByCampaignAndDonor(trip.CampaignId, trip.DonorId);
                if (campaign.PledgeStatusId != AppSetting("PledgeStatusDiscontinued"))
                {
                    var t = new Trip();
                    t.EventId = trip.EventId;
                    t.EventEndDate = trip.EventEndDate.ToString("MMM dd, yyyy");
                    t.EventStartDate = trip.EventStartDate.ToString("MMM dd, yyyy");
                    t.EventTitle = trip.EventTitle;
                    t.EventType = trip.EventType;
                    t.FundraisingDaysLeft = Math.Max(0, (campaign.CampaignEndDate - DateTime.Today).Days);
                    t.FundraisingGoal = Convert.ToInt32(campaign.PledgeTotal);
                    t.EventParticipantId = tripParticipant.EventParticipantId;
                    t.EventParticipantFirstName = tripParticipant.Nickname;
                    t.EventParticipantLastName = tripParticipant.Lastname;

                    events.Add(t);
                }
               
            }

            foreach (var e in events)
            {
                var donations = distributions.Where(d => d.EventId == e.EventId).OrderByDescending(d => d.DonationDate).ToList();
                foreach (var donation in donations)
                {
                    var gift = new TripGift();
                    if (donation.AnonymousGift)
                    {
                        gift.DonorNickname = "Anonymous";
                        gift.DonorLastName = "";
                    }
                    else
                    {
                        gift.DonorNickname = donation.DonorNickname ?? donation.DonorFirstName;
                        gift.DonorLastName = donation.DonorLastName;
                    }
                    gift.DonationDistributionId = donation.DonationDistributionId;
                    gift.DonorId = donation.DonorId;
                    gift.DonorEmail = donation.DonorEmail;
                    gift.DonationDate = donation.DonationDate.ToShortDateString();
                    gift.DonationAmount = donation.DonationAmount;
                    gift.PaymentTypeId = donation.PaymentTypeId;
                    gift.RegisteredDonor = donation.RegisteredDonor;
                    gift.MessageSent = donation.MessageSent;
                    gift.Anonymous = donation.AnonymousGift;
                    e.TripGifts.Add(gift);
                    e.TotalRaised += donation.DonationAmount;
                }
                myTrips.MyTrips.Add(e);
            }
            return myTrips;
        }

        public List<int> SaveParticipants(SaveTripParticipantsDto dto)
        {
            var groupParticipants = new List<int>();
            var groupStartDate = DateTime.Now;
            const int groupRoleId = 16; // wondering if eventually this will become user input?
            var events = _groupService.getAllEventsForGroup(dto.GroupId);

            foreach (var applicant in dto.Applicants)
            {
                if (_groupService.ParticipantGroupMember(dto.GroupId, applicant.ParticipantId))
                {
                    continue;
                }
                var groupParticipantId = AddGroupParticipant(dto.GroupId, groupRoleId, groupStartDate, events, applicant);
                if (groupParticipantId != 0)
                {
                    groupParticipants.Add(groupParticipantId);
                }
                CreatePledge(dto, applicant);
                EventRegistration(events, applicant, dto.Campaign.DestinationId);
                SendTripParticipantSuccess(applicant.ContactId, events);
            }

            return groupParticipants;
        }

        private int AddGroupParticipant(int groupId, int groupRoleId, DateTime groupStartDate, IList<Event> events, TripApplicant applicant)
        {
            if (_groupService.ParticipantGroupMember(groupId, applicant.ParticipantId))
            {
                return 0;
            }
            var groupParticipantId = _groupService.addParticipantToGroup(applicant.ParticipantId, groupId, groupRoleId, false, groupStartDate);
            return groupParticipantId;
        }

        private void SendTripParticipantSuccess(int contactId, IList<Event> events)
        {
            var htmlTable = FormatParticipantEventTable(events).Build();

            var mergeData = new Dictionary<string, object> {{"Html_Table", htmlTable}};
            SendMessage("TripParticipantSuccessTemplate", contactId, mergeData);
        }

        private HtmlElement FormatParticipantEventTable(IEnumerable<Event> events)
        {
            var tableAttrs = new Dictionary<string, string>()
            {
                {"width", "100%"},
                {"border", "1"},
                {"cellspacing", "0"},
                {"cellpadding", "5"}
            };

            var cellAttrs = new Dictionary<string, string>()
            {
                {"align", "center"}
            };

            var rows = events.Select(e => new HtmlElement("tr")
                                         .Append(new HtmlElement("td", cellAttrs, e.EventTitle))
                                         .Append(new HtmlElement("td", cellAttrs, e.EventLocation))
                                         .Append(new HtmlElement("td", cellAttrs, e.EventStartDate.ToString("g")))).ToList();

            var headerLabels = new List<string> {"Event", "Location", "Date"};
            var headers = new HtmlElement("tr", headerLabels.Select(el => new HtmlElement("th", el)).ToList());

            return new HtmlElement("table", tableAttrs)
                .Append(headers)
                .Append(rows);
        }

        public int GeneratePrivateInvite(PrivateInviteDto dto, string token)
        {
            var invite = _privateInviteService.Create(dto.PledgeCampaignId, dto.EmailAddress, dto.RecipientName, token);
            var communication = PrivateInviteCommunication(invite);
            _communicationService.SendMessage(communication);

            return invite.PrivateInvitationId;
        }

        private Communication PrivateInviteCommunication(PrivateInvite invite)
        {
            var templateId = _configurationWrapper.GetConfigIntValue("PrivateInviteTemplate");
            var template = _communicationService.GetTemplate(templateId);
            var fromContact = _contactService.GetContactById(_configurationWrapper.GetConfigIntValue("DefaultContactEmailId"));
            var mergeData = SetMergeData(invite.PledgeCampaignIdText, invite.PledgeCampaignId, invite.InvitationGuid, invite.RecipientName);

            return new Communication
            {
                AuthorUserId = 5,
                DomainId = 1,
                EmailBody = template.Body,
                EmailSubject = template.Subject,
                FromContact = new Contact {ContactId = fromContact.Contact_ID, EmailAddress = fromContact.Email_Address},
                ReplyToContact = new Contact { ContactId = fromContact.Contact_ID, EmailAddress = fromContact.Email_Address },
                ToContacts = new List<Contact> {new Contact{ContactId = fromContact.Contact_ID, EmailAddress = invite.EmailAddress}},
                MergeData = mergeData
            };
        }

        private Dictionary<string, object> SetMergeData(string tripTitle, int pledgeCampaignId, string inviteGuid, string participantName)
        {
            var mergeData = new Dictionary<string, object>
            {
                {"TripTitle", tripTitle},
                {"PledgeCampaignID", pledgeCampaignId},
                {"InviteGUID", inviteGuid},
                {"ParticipantName", participantName},
                {"BaseUrl", _configurationWrapper.GetConfigValue("BaseUrl")}
            };
            return mergeData;
        }

        private void CreatePledge(SaveTripParticipantsDto dto, TripApplicant applicant)
        {
            int donorId;
            var addPledge = true;

            if (applicant.DonorId != null)
            {
                donorId = (int) applicant.DonorId;
                addPledge = !_mpPledgeService.DonorHasPledge(dto.Campaign.PledgeCampaignId, donorId);
            }
            else
            {
                donorId = _mpDonorService.CreateDonorRecord(applicant.ContactId, null, DateTime.Now);
            }

            if (addPledge)
            {
                _mpPledgeService.CreatePledge(donorId, dto.Campaign.PledgeCampaignId, dto.Campaign.FundraisingGoal);
            }
        }

        private void EventRegistration(IEnumerable<Event> events, TripApplicant applicant, int destinationId)
        {
            var destinationDocuments = _destinationService.DocumentsForDestination(destinationId);
            foreach (var e in events)
            {
                var eventParticipantId=_mpEventService.SafeRegisterParticipant(e.EventId, applicant.ParticipantId);
                _eventParticipantService.AddDocumentsToTripParticipant(destinationDocuments, eventParticipantId);
            }
        }

        public bool ValidatePrivateInvite(int pledgeCampaignId, string guid, string token)
        {
            var person = _personService.GetLoggedInUserProfile(token);
            return _privateInviteService.PrivateInviteValid(pledgeCampaignId, guid, person.EmailAddress);
        }

        public int SaveApplication(TripApplicationDto dto)
        {
            try
            {
                UpdateChildSponsorship(dto);
                var formResponse = new FormResponse();
                formResponse.ContactId = dto.ContactId; //contact id of the person the application is for
                formResponse.FormId = _configurationWrapper.GetConfigIntValue("TripApplicationFormId");
                formResponse.PledgeCampaignId = dto.PledgeCampaignId;

                formResponse.FormAnswers = new List<FormAnswer>(FormatFormAnswers(dto));

                var formResponseId = _formSubmissionService.SubmitFormResponse(formResponse);

                if (dto.InviteGUID != null)
                {
                    _privateInviteService.MarkAsUsed(dto.PledgeCampaignId, dto.InviteGUID);
                }

                SendTripApplicantSuccessMessage(dto.ContactId);

                return formResponseId;
            }
            catch (Exception ex)
            {
                // send applicant message
                SendApplicantErrorMessage(dto.ContactId);

                // send trip admin message
                SendTripAdminErrorMessage(dto.PledgeCampaignId);

                //then re-throw or eat it?
                return 0;
            }
        }

        private void SendMessage(string templateKey, int toContactId, Dictionary<string, object> mergeData = null)
        {
            var templateId = _configurationWrapper.GetConfigIntValue(templateKey);
            var fromContactId = _configurationWrapper.GetConfigIntValue("DefaultContactEmailId");
            var fromContact = _contactService.GetContactById(fromContactId);
            var toContact = _contactService.GetContactById(toContactId);
            var template = _communicationService.GetTemplateAsCommunication(templateId,
                                                                            fromContact.Contact_ID,
                                                                            fromContact.Email_Address,
                                                                            fromContact.Contact_ID,
                                                                            fromContact.Email_Address,
                                                                            toContact.Contact_ID,
                                                                            toContact.Email_Address,
                                                                            mergeData);
            _communicationService.SendMessage(template);
        }

        private void SendTripApplicantSuccessMessage(int contactId)
        {
            SendMessage("TripApplicantSuccessTemplate", contactId);
        }

        private void SendTripAdminErrorMessage(int campaignId)
        {
            var campaign = _campaignService.GetPledgeCampaign(campaignId);
            var tripEvent = _mpEventService.GetEvent(campaign.EventId);
            SendMessage("TripAdminErrorTemplate", tripEvent.PrimaryContact.ContactId);
        }

        private void SendApplicantErrorMessage(int contactId)
        {
            SendMessage("TripApplicantErrorTemplate", contactId);
        }

        private void UpdateChildSponsorship(TripApplicationDto dto)
        {
            if (!RequireSponsoredChild(dto.PageFive))
            {
                return;
            }
            var childId = GetChildId(dto,
                                     (child) =>
                                     {
                                         try
                                         {
                                             return _contactService.CreateContactForSponsoredChild(child.FirstName, child.LastName, child.Town, child.IdNumber);
                                         }
                                         catch (ApplicationException e)
                                         {
                                             _logger.Error("Unable to create the sponsored child: " + e.Message);
                                             return -1;
                                         }
                                     });
            if (childId == -1)
            {
                return;
            }

            // Check if relationship exists...
            var myRelationships = _contactRelationshipService.GetMyCurrentRelationships(dto.ContactId);
            var rel = myRelationships.Where(r => r.RelationshipID == _configurationWrapper.GetConfigIntValue("SponsoredChild") && r.RelatedContactID == childId);
            if (rel.Any())
            {
                return;
            }
            // Update the relationship
            var relationship = new Relationship
            {
                RelationshipID = _configurationWrapper.GetConfigIntValue("SponsoredChild"),
                RelatedContactID = childId,
                StartDate = DateTime.Today
            };
            _contactRelationshipService.AddRelationship(relationship, dto.ContactId);
        }

        private static bool RequireSponsoredChild(TripApplicationDto.ApplicationPageFive page5)
        {
            return page5.SponsorChildFirstName != null || page5.SponsorChildLastName != null || page5.SponsorChildNumber != null;
        }

        private int GetChildId(TripApplicationDto dto, Func<SponsoredChild, int> createChild)
        {
            var child = _contactService.GetContactByIdCard(dto.PageFive.SponsorChildNumber);
            if (child != null)
            {
                return child.Contact_ID;
            }
            var sponsoredChild = new SponsoredChild
            {
                FirstName = dto.PageFive.SponsorChildFirstName,
                LastName = dto.PageFive.SponsorChildLastName,
                IdNumber = dto.PageFive.SponsorChildNumber,
                Town = dto.PageFive.SponsorChildTown
            };
            return createChild(sponsoredChild);
        }

        private IEnumerable<FormAnswer> FormatFormAnswers(TripApplicationDto applicationData)
        {
            var answers = new List<FormAnswer>();

            var page2 = applicationData.PageTwo;

            answers.Add(new FormAnswer {Response = page2.Allergies, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.Allergies")});
            answers.Add(new FormAnswer {Response = page2.Conditions, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.Conditions")});
            answers.Add(new FormAnswer {Response = page2.GuardianFirstName, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.GuardianFirstName")});
            answers.Add(new FormAnswer {Response = page2.GuardianLastName, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.GuardianLastName")});
            answers.Add(new FormAnswer {Response = page2.Referral, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.Referral")});
            answers.Add(new FormAnswer {Response = page2.Why, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.Why")});

            var page3 = applicationData.PageThree;
            answers.Add(new FormAnswer {Response = page3.EmergencyContactEmail, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.EmergencyContactEmail")});
            answers.Add(new FormAnswer {Response = page3.EmergencyContactFirstName, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.EmergencyContactFirstName")});
            answers.Add(new FormAnswer {Response = page3.EmergencyContactLastName, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.EmergencyContactLastName")});
            answers.Add(new FormAnswer {Response = page3.EmergencyContactPrimaryPhone, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.EmergencyContactPrimaryPhone")});
            answers.Add(new FormAnswer
            {
                Response = page3.EmergencyContactSecondaryPhone,
                FieldId = _configurationWrapper.GetConfigIntValue("TripForm.EmergencyContactSecondaryPhone")
            });

            var page4 = applicationData.PageFour;
            answers.Add(new FormAnswer {Response = page4.GroupCommonName, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.GroupCommonName")});
            answers.Add(new FormAnswer {Response = page4.InterestedInGroupLeader, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.InterestedInGroupLeader")});
            answers.Add(new FormAnswer {Response = page4.Lottery, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.Lottery")});
            answers.Add(new FormAnswer {Response = page4.RoommateFirstChoice, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.RoommateFirstChoice")});
            answers.Add(new FormAnswer {Response = page4.RoommateSecondChoice, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.RoommateSecondChoice")});
            answers.Add(new FormAnswer {Response = page4.SupportPersonEmail, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.SupportPersonEmail")});
            answers.Add(new FormAnswer {Response = page4.WhyGroupLeader, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.WhyGroupLeader")});

            var page5 = applicationData.PageFive;
            answers.Add(new FormAnswer {Response = page5.SponsorChildFirstName, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.SponsorChildFirstName")});
            answers.Add(new FormAnswer {Response = page5.SponsorChildLastName, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.SponsorChildLastName")});
            answers.Add(new FormAnswer {Response = page5.SponsorChildNumber, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.SponsorChildNumber")});
            answers.Add(new FormAnswer {Response = page5.NolaFirstChoiceWorkTeam, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.FirstChoiceWorkTeam")});
            answers.Add(new FormAnswer {Response = page5.NolaFirstChoiceExperience, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.FirstChoiceWorkTeamExperience")});
            answers.Add(new FormAnswer {Response = page5.NolaSecondChoiceWorkTeam, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.SecondChoiceWorkTeam")});

            var page6 = applicationData.PageSix;
            answers.Add(new FormAnswer {Response = page6.DescribeExperienceAbroad, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.DescribeExperienceAbroad")});
            answers.Add(new FormAnswer {Response = page6.ExperienceAbroad, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.ExperienceAbroad")});
            answers.Add(new FormAnswer
            {
                Response = page6.InternationalTravelExpericence,
                FieldId = _configurationWrapper.GetConfigIntValue("TripForm.InternationalTravelExpericence")
            });
            answers.Add(new FormAnswer {Response = page6.PassportNumber, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.PassportNumber")});
            answers.Add(new FormAnswer {Response = page6.PassportCountry, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.PassportCountry")});
            answers.Add(new FormAnswer {Response = page6.PassportExpirationDate, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.PassportExpirationDate")});
            answers.Add(new FormAnswer {Response = page6.PassportFirstName, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.PassportFirstName")});
            answers.Add(new FormAnswer {Response = page6.PassportLastName, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.PassportLastName")});
            answers.Add(new FormAnswer {Response = page6.PassportMiddleName, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.PassportMiddleName")});
            answers.Add(new FormAnswer {Response = page6.PastAbuseHistory, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.PastAbuseHistory")});
            answers.Add(new FormAnswer {Response = page6.ValidPassport, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.ValidPassport")});

            return answers;
        }
    }
}