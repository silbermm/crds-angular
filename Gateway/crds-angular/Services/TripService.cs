using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Trip;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Extensions;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using IDonationService = MinistryPlatform.Translation.Services.Interfaces.IDonationService;
using IGroupService = MinistryPlatform.Translation.Services.Interfaces.IGroupService;

namespace crds_angular.Services
{
    public class TripService : MinistryPlatformBaseService, ITripService
    {
        private readonly IEventParticipantService _eventParticipantService;
        private readonly IDonationService _donationService;
        private readonly IGroupService _groupService;
        private readonly IDbConnection _dbConnection;

        public TripService(IEventParticipantService eventParticipant, IDonationService donationService, IGroupService groupService, IDbConnection dbConnection)
        {
            _eventParticipantService = eventParticipant;
            _donationService = donationService;
            _groupService = groupService;
            _dbConnection = dbConnection;
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

        public List<TripFormResponsesDto> GetFormResponses(int selectionId, int selectionCount)
        {
            
        }

        private List<TripApplicant> GetTripAplicants(int selectionId, int selectionCount)
        {
            // 1. get selection records
            // 2. look each one up to return a list of responses
            var connection = _dbConnection;
            try
            {
                connection.Open();

                var command = CreateSqlCommand(selectionId);
                command.Connection = connection;
                var reader = command.ExecuteReader();
                var tripApplicants = new List<TripApplicant>();
                //var rowNumber = 0;
                while (reader.Read())
                {
                    //var rowContactId = reader.GetInt32(reader.GetOrdinal("Contact_ID"));
                    //var loggedInUser = (loggedInContactId == rowContactId);
                    //rowNumber = rowNumber + 1;
                    var tripApplicant = new TripApplicant();
                    tripApplicant.ContactId = reader.GetInt32(reader.GetOrdinal("Contact_ID"));
                    tripApplicant.PledgeCampaignId = reader.GetInt32(reader.GetOrdinal("Pledge_Campaign_ID"));


                    tripApplicants.Add(tripApplicant);
                }
                return tripApplicants;
            }
            finally
            {
                connection.Close();
            }
        }

        private static IDbCommand CreateSqlCommand(int selectionId)
        {
            const string query = @"SELECT fr.Contact_ID, fr.Pledge_Campaign_ID
                                  FROM [MinistryPlatform].[dbo].[dp_Selected_Records] sr
                                  INNER JOIN [MinistryPlatform].[dbo].[Form_Responses] fr on sr.Record_ID = fr.Form_Response_ID
                                  WHERE sr.Selection_ID = @selectionId";

            using (IDbCommand command = new SqlCommand(string.Format(query)))
            {
                command.Parameters.Add(new SqlParameter("@selectionId", selectionId) { DbType = DbType.Int32 });
                command.CommandType = CommandType.Text;
                return command;
            }
        }

        public MyTripsDTO GetMyTrips(int contactId, string token)
        {
            var trips = _donationService.GetMyTripDistributions(contactId, token).OrderBy(t => t.EventStartDate);

            var myTrips = new MyTripsDTO();

            var events = new List<Trip>();
            var eventIds = new List<int>();
            foreach (var trip in trips.Where(trip => !eventIds.Contains(trip.EventId)))
            {
                eventIds.Add(trip.EventId);
                events.Add(new Trip
                {
                    EventId = trip.EventId,
                    EventType = trip.EventTypeId.ToString(),
                    EventTitle = trip.EventTitle,
                    EventStartDate = trip.EventStartDate.ToString("MMM dd, yyyy"),
                    EventEndDate = trip.EventEndDate.ToString("MMM dd, yyyy"),
                    FundraisingDaysLeft = Math.Max(0, (trip.CampaignEndDate - DateTime.Today).Days),
                    FundraisingGoal = trip.TotalPledge
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
                    gift.DonorEmail = donation.DonorEmail;
                    gift.DonationDate = donation.DonationDate.ToShortDateString();
                    gift.DonationAmount = donation.DonationAmount;
                    gift.RegisteredDonor = donation.RegisteredDonor;
                    e.TripGifts.Add(gift);
                    e.TotalRaised += donation.DonationAmount;
                }
                myTrips.MyTrips.Add(e);
            }
            return myTrips;
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
                var tp = new TripDto();
                tp.EventParticipantId = result.EventParticipantId;
                tp.EventEnd = result.EventEndDate.ToString("MMM dd, yyyy");
                tp.EventId = result.EventId;
                tp.EventStartDate = result.EventStartDate.ToUnixTime();
                tp.EventStart = result.EventStartDate.ToString("MMM dd, yyyy");
                tp.EventTitle = result.EventTitle;
                tp.EventType = result.EventType;
                var participant = participants[result.ParticipantId];
                participant.Trips.Add(tp);
            }

            return participants.Values.OrderBy(o => o.Lastname).ThenBy(o => o.Nickname).ToList();
        }

        
    }
}