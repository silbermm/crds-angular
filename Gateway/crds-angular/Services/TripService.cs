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
            // US2086 - refactor GetMyTripDistributions to exclude Pledges with status = 'discontinued'
            var trips = _donationService.GetMyTripDistributions(contactId).OrderBy(t => t.EventStartDate);
            var myTrips = new MyTripsDto();

            var events = new List<Trip>();
            var eventIds = new List<int>();
            foreach (var trip in trips.Where(trip => !eventIds.Contains(trip.EventId)))
            {
                var eventParticipantId = 0;
                // US2086 - verify TripParticipants is still valid
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
            
            SaveContact(dto);
            SaveParticipant(dto);

            if (dto.InviteGUID != null)
            {
                _privateInviteService.MarkAsUsed(dto.PledgeCampaignId, dto.InviteGUID);
            }

            return formResponseId;
        }

        private Boolean SaveContact(TripApplicationDto dto)
        {
            return true;
        }

        private Boolean SaveParticipant(TripApplicationDto dto)
        {
            return false;
        }

        private IEnumerable<FormAnswer> FormatFormAnswers(TripApplicationDto applicationData)
        {
            var answers = new List<FormAnswer>();

            var page2 = applicationData.PageTwo;

            answers.Add(new FormAnswer { Response = page2.Allergies, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.Allergies") });
            answers.Add(new FormAnswer { Response = page2.Conditions, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.Conditions") });
            answers.Add(new FormAnswer { Response = page2.GuardianFirstName, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.GuardianFirstName") });
            answers.Add(new FormAnswer { Response = page2.GuardianLastName, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.GuardianLastName") });
            answers.Add(new FormAnswer { Response = page2.Referral, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.Referral") });
            answers.Add(new FormAnswer { Response = page2.ScrubSizeTop, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.ScrubSizeTop") });
            answers.Add(new FormAnswer { Response = page2.ScrubSizeBottom, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.ScrubSizeBottom") });
            answers.Add(new FormAnswer { Response = page2.SpiritualLifeObedience, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.SpiritualLifeObedience") });
            answers.Add(new FormAnswer { Response = page2.SpiritualLifeReceived, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.SpiritualLifeReceived") });
            answers.Add(new FormAnswer { Response = page2.SpiritualLifeReplicating, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.SpiritualLifeReplicating") });
            answers.Add(new FormAnswer { Response = page2.SpiritualLifeSearching, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.SpiritualLifeSearching") });
            answers.Add(new FormAnswer { Response = page2.TshirtSize, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.TshirtSize") });
            answers.Add(new FormAnswer { Response = page2.Vegetarian, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.Vegetarian") });
            answers.Add(new FormAnswer { Response = page2.Why, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.Why") });

            var page3 = applicationData.PageThree;
            answers.Add(new FormAnswer { Response = page3.EmergencyContactEmail, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.EmergencyContactEmail") }); 
            answers.Add(new FormAnswer { Response = page3.EmergencyContactFirstName, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.EmergencyContactFirstName") }); 
            answers.Add(new FormAnswer { Response = page3.EmergencyContactLastName, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.EmergencyContactLastName") });
            answers.Add(new FormAnswer { Response = page3.EmergencyContactPrimaryPhone, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.EmergencyContactPrimaryPhone") });
            answers.Add(new FormAnswer { Response = page3.EmergencyContactSecondaryPhone, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.EmergencyContactSecondaryPhone") }); 

            var page4 = applicationData.PageFour;
            answers.Add(new FormAnswer { Response = page4.GroupCommonName, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.GroupCommonName") });
            answers.Add(new FormAnswer { Response = page4.InterestedInGroupLeader, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.InterestedInGroupLeader") });
            answers.Add(new FormAnswer { Response = page4.Lottery, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.Lottery") });
            answers.Add(new FormAnswer { Response = page4.RoommateFirstChoice, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.RoommateFirstChoice") });
            answers.Add(new FormAnswer { Response = page4.RoommateSecondChoice, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.RoommateSecondChoice") });
            answers.Add(new FormAnswer { Response = page4.SupportPersonEmail, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.SupportPersonEmail") });
            answers.Add(new FormAnswer { Response = page4.WhyGroupLeader, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.WhyGroupLeader") });

            var page5 = applicationData.PageFive;
            answers.Add(new FormAnswer { Response = page5.PreviousTripExperience, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.PreviousTripExperience") });
            answers.Add(new FormAnswer { Response = page5.ProfessionalSkillBusiness, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.ProfessionalSkillBusiness") });
            answers.Add(new FormAnswer { Response = page5.ProfessionalSkillConstruction, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.ProfessionalSkillConstruction") });
            answers.Add(new FormAnswer { Response = page5.ProfessionalSkillDental, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.ProfessionalSkillDental") });
            answers.Add(new FormAnswer { Response = page5.ProfessionalSkillEducation, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.ProfessionalSkillEducation") });
            answers.Add(new FormAnswer { Response = page5.ProfessionalSkillInformationTech, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.ProfessionalSkillInformationTech") });
            answers.Add(new FormAnswer { Response = page5.ProfessionalSkillMedia, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.ProfessionalSkillMedia") });
            answers.Add(new FormAnswer { Response = page5.ProfessionalSkillMedical, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.ProfessionalSkillMedical") });
            answers.Add(new FormAnswer { Response = page5.ProfessionalSkillMusic, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.ProfessionalSkillMusic") });
            answers.Add(new FormAnswer { Response = page5.ProfessionalSkillOther, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.ProfessionalSkillOther") });
            answers.Add(new FormAnswer { Response = page5.ProfessionalSkillPhotography, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.ProfessionalSkillPhotography") });
            answers.Add(new FormAnswer { Response = page5.ProfessionalSkillSocialWorker, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.ProfessionalSkillSocialWorker") });
            answers.Add(new FormAnswer { Response = page5.ProfessionalSkillStudent, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.ProfessionalSkillStudent") });
            answers.Add(new FormAnswer { Response = page5.SponsorChildFirstName, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.SponsorChildFirstName") });
            answers.Add(new FormAnswer { Response = page5.SponsorChildInNicaragua, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.SponsorChildInNicaragua") });
            answers.Add(new FormAnswer { Response = page5.SponsorChildLastName, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.SponsorChildLastName") });
            answers.Add(new FormAnswer { Response = page5.SponsorChildNumber, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.SponsorChildNumber") });

            var page6 = applicationData.PageSix;

            answers.Add(new FormAnswer { Response = page6.DeltaFrequentFlyer, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.DeltaFrequentFlyer") });
            answers.Add(new FormAnswer { Response = page6.DescribeExperienceAbroad, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.DescribeExperienceAbroad") });
            answers.Add(new FormAnswer { Response = page6.ExperienceAbroad, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.ExperienceAbroad") });
            answers.Add(new FormAnswer { Response = page6.InternationalTravelExpericence, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.InternationalTravelExpericence") });
            answers.Add(new FormAnswer { Response = page6.PassportNumber, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.PassportNumber")});
            answers.Add(new FormAnswer { Response = page6.PassportCountry, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.PassportCountry") });
            answers.Add(new FormAnswer { Response = page6.PassportExpirationDate, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.PassportExpirationDate") });
            answers.Add(new FormAnswer { Response = page6.PassportFirstName, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.PassportFirstName") });
            answers.Add(new FormAnswer { Response = page6.PassportLastName, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.PassportLastName") });
            answers.Add(new FormAnswer { Response = page6.PassportMiddleName, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.PassportMiddleName") });
            answers.Add(new FormAnswer { Response = page6.PastAbuseHistory, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.PastAbuseHistory") });
            answers.Add(new FormAnswer { Response = page6.SouthAfricanFrequentFlyer, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.SouthAfricanFrequentFlyer") });
            answers.Add(new FormAnswer { Response = page6.UnitedFrequentFlyer, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.UnitedFrequentFlyer") });
            answers.Add(new FormAnswer { Response = page6.UsAirwaysFrequentFlyer, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.UsAirwaysFrequentFlyer") });
            answers.Add(new FormAnswer { Response = page6.ValidPassport, FieldId = _configurationWrapper.GetConfigIntValue("TripForm.ValidPassport") });

            return answers;
        }

    }
}