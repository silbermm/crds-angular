using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly int GroupsParticipantsPageId = Convert.ToInt32(AppSettings("GroupsParticipants"));
        private readonly int GroupsParticipantsSubPageId = Convert.ToInt32(AppSettings("GroupsParticipantsSubPage"));
        private readonly int GroupsPageId = Convert.ToInt32(AppSettings("Groups"));
        private readonly int GroupsEventsPageId = Convert.ToInt32(AppSettings("GroupsEvents"));
        private readonly int EventsGroupsPageId = Convert.ToInt32(AppSettings("EventsGroups"));
        private readonly int GroupsSubgroupsPageId = Convert.ToInt32(AppSettings("GroupsSubgroups"));
        private readonly int GroupSignupRelationsPageId = Convert.ToInt32((AppSettings("GroupSignUpRelations")));
        private readonly int GetServingTeamsPageId = Convert.ToInt32(AppSettings("MyServingTeams"));
        private readonly int CommunityGroupConfirmationTemplateId = Convert.ToInt32(AppSettings(""));

        private readonly int GroupParticipantQualifiedServerPageView =
            Convert.ToInt32(AppSettings("GroupsParticipantsQualifiedServerPageView"));

        private IMinistryPlatformService ministryPlatformService;

        public GroupService(IMinistryPlatformService ministryPlatformService, IConfigurationWrapper configurationWrapper, IAuthenticationService authenticationService, ICommunicationService communicationService)
            : base(authenticationService, configurationWrapper)
        {
            this.ministryPlatformService = ministryPlatformService;
            this._configurationWrapper = configurationWrapper;
            this._communicationService = communicationService;
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

            int groupParticipantId =
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
                Group g = new Group();

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
            var events = new List<Event>();
            var apiToken = ApiLogin();
            var groupEvents = ministryPlatformService.GetSubpageViewRecords("GroupEventsSubPageView", groupId, apiToken);
            if (groupEvents == null || groupEvents.Count == 0)
            {
                return null;
            }
            foreach (var tmpEvent in groupEvents)
            {
                var newEvent = new Event();
                newEvent.EventId = tmpEvent.ToInt("Event_ID");
                newEvent.EventLocation = tmpEvent.ToString("Location_Name");
                newEvent.EventStartDate = tmpEvent.ToDate("Event_Start_Date");
                newEvent.EventTitle = tmpEvent.ToString("Event_Title");

                events.Add(newEvent);
            }
            return events;
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

        public void SendCommunityGroupConfirmationEmail(int p)
        {
            
            var emailTemplate = _communicationService.GetTemplate(CommunityGroupConfirmationTemplateId);
            var fromAddress = _communicationService.GetEmailFromContactId(7);
            var confirmation = new Communication 
            { 
                EmailBody = emailTemplate.Body, 
                EmailSubject = emailTemplate.Subject,
                AuthorUserId = 7,
                DomainId = 1,
                FromContactId = 7,
                FromEmailAddress = fromAddress,
                MergeData = new Dictionary<string, object>(),
                ReplyContactId = 7,
                ReplyToEmailAddress = fromAddress,
                TemplateId = CommunityGroupConfirmationTemplateId,
                ToContactId = 0,
                ToEmailAddress = ""
            };
            _communicationService.SendMessage(confirmation);
        }
    }
}