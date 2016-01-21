using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class GroupService : BaseService, IGroupService
    {
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly ICommunicationService _communicationService;
        private readonly IContactService _contactService;
        private readonly IContentBlockService _contentBlockService;
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly int GroupsParticipantsPageId = Convert.ToInt32(AppSettings("GroupsParticipants"));
        private readonly int GroupsParticipantsSubPageId = Convert.ToInt32(AppSettings("GroupsParticipantsSubPage"));
        private readonly int GroupsPageId = Convert.ToInt32(AppSettings("Groups"));
        private readonly int GroupsSubgroupsPageId = Convert.ToInt32(AppSettings("GroupsSubgroups"));
        private readonly int DefaultEmailContactId = Convert.ToInt32(AppSettings("DefaultContactEmailId"));
        private readonly int GroupSignupRelationsPageId = Convert.ToInt32((AppSettings("GroupSignUpRelations")));
        private readonly int CommunityGroupConfirmationTemplateId = Convert.ToInt32(AppSettings("CommunityGroupConfirmationTemplateId"));
        private readonly int CommunityGroupWaitListConfirmationTemplateId = Convert.ToInt32(AppSettings("CommunityGroupWaitListConfirmationTemplateId"));

        private readonly int GroupParticipantQualifiedServerPageView =
            Convert.ToInt32(AppSettings("GroupsParticipantsQualifiedServerPageView"));

        private IMinistryPlatformService ministryPlatformService;

        public GroupService(IMinistryPlatformService ministryPlatformService, IConfigurationWrapper configurationWrapper, IAuthenticationService authenticationService, ICommunicationService communicationService, IContactService contactService, IContentBlockService contentBlockService)
            : base(authenticationService, configurationWrapper)
        {
            this.ministryPlatformService = ministryPlatformService;
            this._configurationWrapper = configurationWrapper;
            this._communicationService = communicationService;
            this._contactService = contactService;
            this._contentBlockService = contentBlockService;
        }

        public int addParticipantToGroup(int participantId,
                                         int groupId,
                                         int groupRoleId,
                                         Boolean childCareNeeded,
                                         DateTime startDate,
                                         DateTime? endDate = null,
                                         Boolean? employeeRole = false)
        {
            logger.Debug("Adding participant " + participantId + " to group " + groupId);

            var values = new Dictionary<string, object>
            {
                {"Participant_ID", participantId},
                {"Group_Role_ID", groupRoleId},
                {"Start_Date", startDate},
                {"End_Date", endDate},
                {"Employee_Role", employeeRole},
                {"Child_Care_Requested", childCareNeeded}
            };

            var groupParticipantId =
                WithApiLogin<int>(
                    apiToken =>
                    {
                        return
                            (ministryPlatformService.CreateSubRecord(GroupsParticipantsPageId,
                                                                     groupId,
                                                                     values,
                                                                     apiToken,
                                                                     true));
                    });

            logger.Debug("Added participant " + participantId + " to group " + groupId + ": record id: " +
                         groupParticipantId);
            return (groupParticipantId);
        }

        public Group getGroupDetails(int groupId)
        {
            return (WithApiLogin<Group>(apiToken =>
            {
                logger.Debug("Getting group details for group " + groupId);
                var groupDetails = ministryPlatformService.GetRecordDict(GroupsPageId, groupId, apiToken, false);
                if (groupDetails == null)
                {
                    logger.Debug("No group found for group id " + groupId);
                    return (null);
                }
                var g = new Group();

                object gid = null;
                groupDetails.TryGetValue("Group_ID", out gid);
                if (gid != null)
                {
                    g.GroupId = (int) gid;
                }

                object gn = null;
                groupDetails.TryGetValue("Group_Name", out gn);
                if (gn != null)
                {
                    g.Name = (string) gn;
                }

                object gt = null;
                groupDetails.TryGetValue("Group_Type_ID", out gt);
                if (gt != null)
                {
                    g.GroupType = (int) gt;
                }

                object gsz = null;
                groupDetails.TryGetValue("Target_Size", out gsz);
                if (gsz != null)
                {
                    g.TargetSize = (short) gsz;
                }

                object gf = null;
                groupDetails.TryGetValue("Group_Is_Full", out gf);
                if (gf != null)
                {
                    g.Full = (Boolean) gf;
                }

                object gwl = null;
                groupDetails.TryGetValue("Enable_Waiting_List", out gwl);
                if (gwl != null)
                {
                    g.WaitList = (Boolean) gwl;
                }

                object gcc = null;
                groupDetails.TryGetValue("Child_Care_Available", out gcc);
                if (gcc != null)
                {
                    g.ChildCareAvailable = (Boolean) gcc;
                }

                object gc = null;
                groupDetails.TryGetValue("Congregation_ID_Text", out gc);
                if (gc != null)
                {
                    g.Congregation = (string)gc;
                }

                object ma = null;
                groupDetails.TryGetValue("Online_RSVP_Minimum_Age", out ma);
                if (ma != null)
                {
                    g.MinimumAge = (int) ma;
                }

                if (g.WaitList)
                {
                    var subGroups = ministryPlatformService.GetSubPageRecords(GroupsSubgroupsPageId,
                                                                              groupId,
                                                                              apiToken);
                    if (subGroups != null)
                    {
                        foreach (var i in subGroups)
                        {
                            if (i.ContainsValue("Wait List"))
                            {
                                object gd = null;
                                i.TryGetValue("dp_RecordID", out gd);
                                g.WaitListGroupId = (int) gd;
                                break;
                            }
                        }
                    }
                    else
                    {
                        logger.Debug("No wait list found for group id " + groupId);
                    }
                }

                logger.Debug("Getting participants for group " + groupId);
                var participants = ministryPlatformService.GetSubpageViewRecords(GroupsParticipantsSubPageId, groupId, apiToken);
                if (participants != null && participants.Count > 0)
                {
                    foreach (Dictionary<string, object> p in participants)
                    {
                        object pid = null;
                        p.TryGetValue("Participant_ID", out pid);
                        if (pid != null)
                        {
                            g.Participants.Add(new GroupParticipant
                            {
                                ContactId = p.ToInt("Contact_ID"),
                                ParticipantId = p.ToInt("Participant_ID"),
                                GroupRoleId = p.ToInt("Group_Role_ID"),
                                GroupRoleTitle = p.ToString("Role_Title"),
                                LastName = p.ToString("Last_Name"),
                                NickName = p.ToString("Nickname")
                            });
                        }
                    }
                }
                else
                {
                    logger.Debug("No participants found for group id " + groupId);
                }

                logger.Debug("Group details: " + g);
                return (g);
            }));
        }

        public IList<Event> getAllEventsForGroup(int groupId)
        {
            var apiToken = ApiLogin();
            var groupEvents = ministryPlatformService.GetSubpageViewRecords("GroupEventsSubPageView", groupId, apiToken);
            if (groupEvents == null || groupEvents.Count == 0)
            {
                return null;
            }
            return groupEvents.Select(tmpEvent => new Event
            {
                EventId = tmpEvent.ToInt("Event_ID"),
                Congregation = tmpEvent.ToString("Congregation_Name"),
                EventStartDate = tmpEvent.ToDate("Event_Start_Date"),
                EventEndDate = tmpEvent.ToDate("Event_End_Date"),
                EventTitle = tmpEvent.ToString("Event_Title")
            }).ToList();
        }

        public IList<string> GetEventTypesForGroup(int groupId, string token)
        {
            var loginToken = token ?? ApiLogin();
            var records = ministryPlatformService.GetSubpageViewRecords("GroupOpportunitiesEvents", groupId, loginToken);
            return records.Select(e =>
            {
                try
                {
                    return e.ToString("Event Type");
                }
                catch (Exception exception)
                {
                    logger.Debug("tried to parse a Event_Type_ID for a record and failed");
                    return String.Empty;
                }
            }).ToList();
        }

        public bool ParticipantQualifiedServerGroupMember(int groupId, int participantId)
        {
            var searchString = string.Format(",{0},,{1}", groupId, participantId);
            var teams = ministryPlatformService.GetPageViewRecords(GroupParticipantQualifiedServerPageView, ApiLogin(), searchString);
            return teams.Count != 0;
        }

        public bool ParticipantGroupMember(int groupId, int participantId)
        {
            var searchString = string.Format(",{0},{1}", groupId, participantId);
            var teams = ministryPlatformService.GetPageViewRecords("GroupParticipantsById", ApiLogin(), searchString);
            return teams.Count != 0;
        }

        public bool checkIfUserInGroup(int participantId, IList<GroupParticipant> groupParticipants)
        {
            return groupParticipants.Select(p => p.ParticipantId).Contains(participantId);
        }

        public bool checkIfRelationshipInGroup(int relationshipId, IList<int> currRelationshipsList)
        {
            return currRelationshipsList.Contains(relationshipId);
        }

        public List<GroupSignupRelationships> GetGroupSignupRelations(int groupType)
        {
            var response = WithApiLogin<List<GroupSignupRelationships>>(
                apiToken =>
                {
                    var relationRecords = ministryPlatformService.GetSubPageRecords(GroupSignupRelationsPageId,
                                                                                    groupType,
                                                                                    apiToken);

                    return relationRecords.Select(relationRecord => new GroupSignupRelationships
                    {
                        RelationshipId = relationRecord.ToInt("Relationship_ID"),
                        RelationshipMinAge = relationRecord.ToNullableInt("Min_Age"),
                        RelationshipMaxAge = relationRecord.ToNullableInt("Max_Age")
                    }).ToList();
                });
            return response;
        }

        public List<Group> GetGroupsForEvent(int eventId)
        {
            var searchString = eventId + ",";
            var pageViewId = _configurationWrapper.GetConfigIntValue("GroupsByEventId");
            var token = ApiLogin();
            var records = ministryPlatformService.GetPageViewRecords(pageViewId, token, searchString);
            if (records == null)
            {
                return null;
            }
            return records.Select(record => new Group
            {
                GroupId = record.ToInt("Group_ID"),
                Name = record.ToString("Group_Name")
            }).ToList();
        }

        public void SendCommunityGroupConfirmationEmail(int participantId, int groupId, bool waitlist, bool childcareNeeded)
        {
            var emailTemplate = _communicationService.GetTemplate(waitlist ? CommunityGroupWaitListConfirmationTemplateId : CommunityGroupConfirmationTemplateId);
            var toContact = _contactService.GetContactIdByParticipantId(participantId);
            var toContactInfo = _contactService.GetContactById(toContact);
            var groupInfo = getGroupDetails(groupId);

            var mergeData = new Dictionary<string, object>
            {
                {"Nickname", toContactInfo.Nickname},
                {"Group_Name", groupInfo.Name},
                {"Congregation_Name", groupInfo.Congregation},
                {"Childcare_Needed", (childcareNeeded) ? _contentBlockService["communityGroupChildcare"].Content : ""}
            };

            var domainId = Convert.ToInt32(AppSettings("DomainId"));
            var from = new Contact()
            {
                ContactId = DefaultEmailContactId,
                EmailAddress = _communicationService.GetEmailFromContactId(DefaultEmailContactId)
            };

            var to = new List<Contact>
            {
                new Contact {
                    ContactId = toContact,
                    EmailAddress = toContactInfo.Email_Address
                }
            };

            var confirmation = new Communication 
            { 
                EmailBody = emailTemplate.Body, 
                EmailSubject = emailTemplate.Subject,
                AuthorUserId = 5,
                DomainId = domainId,
                FromContact = from,
                MergeData = mergeData,
                ReplyToContact = from,
                TemplateId = CommunityGroupConfirmationTemplateId,
                ToContacts = to
            };
            _communicationService.SendMessage(confirmation);
        }

        public List<GroupParticipant> getEventParticipantsForGroup(int groupId, int eventId)
        {
            var records = ministryPlatformService.GetPageViewRecords("ParticipantsByGroupAndEvent", ApiLogin(), String.Format("{0},{1}", groupId, eventId));
            return records.Select(rec => new GroupParticipant
            {
                ContactId = rec.ToInt("Contact_ID"), NickName = rec.ToString("Nickname"), LastName = rec.ToString("Last_Name")
            }).ToList();
        }
    }
}