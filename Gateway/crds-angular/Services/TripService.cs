using System;
using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.Trip;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Extensions;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;
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
        private readonly IEventService _mpEventService;
        private readonly IDonorService _mpDonorService;
        private readonly IPledgeService _mpPledgeService;
        private readonly ICampaignService _campaignService;
        private readonly IPrivateInviteService _privateInviteService;
        private readonly ICommunicationService _communicationService;
        private readonly IContactService _contactService;
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly IPersonService _personService;
        private readonly IServeService _serveService;
        private readonly IDestinationService _destinationService;

        public TripService(IEventParticipantService eventParticipant,
                           IDonationService donationService,
                           IGroupService groupService,
                           IFormSubmissionService formSubmissionService,
                           IEventService eventService,
                           IDonorService donorService,
                           IPledgeService pledgeService,
                           ICampaignService campaignService,
                           IPrivateInviteService privateInviteService,
                           ICommunicationService communicationService,
                           IContactService contactService,
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

        public List<FamilyMemberTripDto> GetFamilyMembers(int contactId, int pledgeId, string token)
        {
            var family = _serveService.GetImmediateFamilyParticipants(contactId, token);
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
                                                       r.EmailAddress,
                                                       r.Lastname,
                                                       r.Nickname
                                                   }).Select(x => new TripParticipantDto()
                                                   {
                                                       ParticipantId = x.Key.ParticipantId,
                                                       Email = x.Key.EmailAddress,
                                                       Lastname = x.Key.Lastname,
                                                       Nickname = x.Key.Nickname,
                                                       ShowGiveButton = true,
                                                       ShowShareButtons = false
                                                   }).ToDictionary(y => y.ParticipantId);

            foreach (var result in results)
            {
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

            return participants.Values.OrderBy(o => o.Lastname).ThenBy(o => o.Nickname).ToList();
        }

        public MyTripsDto GetMyTrips(int contactId)
        {
            var trips = _donationService.GetMyTripDistributions(contactId).OrderBy(t => t.EventStartDate);
            var myTrips = new MyTripsDto();

            var events = new List<Trip>();
            var eventIds = new List<int>();
            foreach (var trip in trips.Where(trip => !eventIds.Contains(trip.EventId)))
            {
                var eventParticipantId = 0;
                var eventParticipantIds = _eventParticipantService.TripParticipants("," + trip.EventId + ",,,,,,,,,,,," + contactId).FirstOrDefault();
                if (eventParticipantIds != null)
                {
                    eventParticipantId = eventParticipantIds.EventParticipantId;
                }
                eventIds.Add(trip.EventId);
                events.Add(new Trip
                {
                    EventId = trip.EventId,
                    EventType = trip.EventTypeId.ToString(),
                    EventTitle = trip.EventTitle,
                    EventStartDate = trip.EventStartDate.ToString("MMM dd, yyyy"),
                    EventEndDate = trip.EventEndDate.ToString("MMM dd, yyyy"),
                    FundraisingDaysLeft = Math.Max(0, (trip.CampaignEndDate - DateTime.Today).Days),
                    FundraisingGoal = trip.TotalPledge,
                    EventParticipantId = eventParticipantId
                });
            }

            foreach (var e in events)
            {
                var donations = trips.Where(d => d.EventId == e.EventId).OrderByDescending(d => d.DonationDate).ToList();
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
                if (!_groupService.ParticipantGroupMember(dto.GroupId, applicant.ParticipantId))
                {
                    var groupParticipantId = _groupService.addParticipantToGroup(applicant.ParticipantId, dto.GroupId, groupRoleId, groupStartDate);
                    groupParticipants.Add(groupParticipantId);
                }

                CreatePledge(dto, applicant);

                EventRegistration(events, applicant, dto.Campaign.DestinationId);
            }

            return groupParticipants;
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
            var fromContact = _contactService.GetContactById(_configurationWrapper.GetConfigIntValue("UnassignedContact"));
            var mergeData = SetMergeData(invite.PledgeCampaignIdText, invite.PledgeCampaignId, invite.InvitationGuid, invite.RecipientName);

            return new Communication
            {
                AuthorUserId = 5,
                DomainId = 1,
                EmailBody = template.Body,
                EmailSubject = template.Subject,
                FromContactId = fromContact.Contact_ID,
                FromEmailAddress = fromContact.Email_Address,
                ReplyContactId = fromContact.Contact_ID,
                ReplyToEmailAddress = fromContact.Email_Address,
                ToContactId = fromContact.Contact_ID,
                ToEmailAddress = invite.EmailAddress,
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
                if (_mpEventService.EventHasParticipant(e.EventId, applicant.ParticipantId))
                {
                    continue;
                }
                var eventParticipantId = _mpEventService.registerParticipantForEvent(applicant.ParticipantId, e.EventId);
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
            var formResponse = new FormResponse();
            formResponse.ContactId = dto.ContactId; //contact id of the person the application is for
            formResponse.FormId = _configurationWrapper.GetConfigIntValue("TripApplicationFormId");
            formResponse.PledgeCampaignId = dto.PledgeCampaignId;

            formResponse.FormAnswers = new List<FormAnswer>(FormatFormAnswers(dto));

            var formResponseId = _formSubmissionService.SubmitFormResponse(formResponse);
            return formResponseId;
        }

        private IEnumerable<FormAnswer> FormatFormAnswers(TripApplicationDto applicationData)
        {
            var answers = new List<FormAnswer>();

            var page2 = applicationData.PageTwo;
            FormatAnswer(page2.Allergies, answers);
            FormatAnswer(page2.Conditions, answers);
            FormatAnswer(page2.GuardianFirstName, answers);
            FormatAnswer(page2.GuardianLastName, answers);
            FormatAnswer(page2.Referral, answers);
            FormatAnswer(page2.ScrubSize, answers);
            FormatAnswer(page2.SpiritualLifeObedience, answers);
            FormatAnswer(page2.SpiritualLifeReceived, answers);
            FormatAnswer(page2.SpiritualLifeReplicating, answers);
            FormatAnswer(page2.SpiritualLifeSearching, answers);
            FormatAnswer(page2.TshirtSize, answers);
            FormatAnswer(page2.Vegetarian, answers);
            FormatAnswer(page2.Why, answers);

            var page3 = applicationData.PageThree;
            FormatAnswer(page3.EmergencyContactEmail, answers);
            FormatAnswer(page3.EmergencyContactFirstName, answers);
            FormatAnswer(page3.EmergencyContactLastName, answers);
            FormatAnswer(page3.EmergencyContactPrimaryPhone, answers);
            FormatAnswer(page3.EmergencyContactSecondaryPhone, answers);

            var page4 = applicationData.PageFour;
            FormatAnswer(page4.GroupCommonName, answers);
            FormatAnswer(page4.InterestedInGroupLeader, answers);
            FormatAnswer(page4.Lottery, answers);
            FormatAnswer(page4.RoommateFirstChoice, answers);
            FormatAnswer(page4.RoommateSecondChoice, answers);
            FormatAnswer(page4.SupportPersonEmail, answers);
            FormatAnswer(page4.WhyGroupLeader, answers);

            var page5 = applicationData.PageFive;
            FormatAnswer(page5.PreviousTripExperience, answers);
            FormatAnswer(page5.ProfessionalSkillBusiness, answers);
            FormatAnswer(page5.ProfessionalSkillConstruction, answers);
            FormatAnswer(page5.ProfessionalSkillDental, answers);
            FormatAnswer(page5.ProfessionalSkillEducation, answers);
            FormatAnswer(page5.ProfessionalSkillInformationTech, answers);
            FormatAnswer(page5.ProfessionalSkillMedia, answers);
            FormatAnswer(page5.ProfessionalSkillMedical, answers);
            FormatAnswer(page5.ProfessionalSkillMusic, answers);
            FormatAnswer(page5.ProfessionalSkillOther, answers);
            FormatAnswer(page5.ProfessionalSkillPhotography, answers);
            FormatAnswer(page5.ProfessionalSkillSocialWorker, answers);
            FormatAnswer(page5.ProfessionalSkillStudent, answers);
            FormatAnswer(page5.SponsorChildFirstName, answers);
            FormatAnswer(page5.SponsorChildInNicaragua, answers);
            FormatAnswer(page5.SponsorChildLastName, answers);
            FormatAnswer(page5.SponsorChildNumber, answers);

            var page6 = applicationData.PageSix;
            FormatAnswer(page6.DeltaFrequentFlyer, answers);
            FormatAnswer(page6.DescribeExperienceAbroad, answers);
            FormatAnswer(page6.ExperienceAbroad, answers);
            FormatAnswer(page6.InternationalTravelExpericence, answers);
            FormatAnswer(page6.PassportBirthday, answers);
            FormatAnswer(page6.PassportCountry, answers);
            FormatAnswer(page6.PassportExpirationDate, answers);
            FormatAnswer(page6.PassportFirstName, answers);
            FormatAnswer(page6.PassportLastName, answers);
            FormatAnswer(page6.PassportMiddleName, answers);
            FormatAnswer(page6.PastAbuseHistory, answers);
            FormatAnswer(page6.SouthAfricanFrequentFlyer, answers);
            FormatAnswer(page6.UnitedFrequentFlyer, answers);
            FormatAnswer(page6.UsAirwaysFrequentFlyer, answers);
            FormatAnswer(page6.ValidPassport, answers);

            return answers;
        }

        private void FormatAnswer(TripApplicationDto.TripApplicationField field, List<FormAnswer> answers)
        {
            if (field == null)
            {
                return;
            }
            if (field.Value == null)
            {
                return;
            }
            answers.Add(FormatTripFormAnswer(field));
        }

        private FormAnswer FormatTripFormAnswer(TripApplicationDto.TripApplicationField field)
        {
            try
            {
                var answer = new FormAnswer();
                answer.FieldId = field.FormFieldId;
                answer.Response = field.Value;
                return answer;
            }
            catch (Exception exception)
            {
                throw new ApplicationException(string.Format("Form Field Id: {0}", field.FormFieldId), exception);
            }
        }
    }
}