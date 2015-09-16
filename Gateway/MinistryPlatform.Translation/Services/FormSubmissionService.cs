using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class FormSubmissionService : BaseService, IFormSubmissionService
    {
        private readonly int _formResponsePageId = AppSettings("FormResponsePageId");
        private readonly int _formAnswerPageId = AppSettings("FormAnswerPageId");
        private readonly int _formFieldCustomPage = AppSettings("AllFormFieldsView");
        private readonly int _formResponseGoTripView = AppSettings("GoTripFamilySignup");

        private readonly IMinistryPlatformService _ministryPlatformService;
        private IDbConnection _dbConnection;

        public FormSubmissionService(IMinistryPlatformService ministryPlatformService, IDbConnection dbConnection, IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper)
            : base(authenticationService,configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
            _dbConnection = dbConnection;
        }

        public int GetFormFieldId(int crossroadsId)
        {
            var searchString = string.Format(",{0}", crossroadsId);
            var formFields = _ministryPlatformService.GetPageViewRecords(_formFieldCustomPage, ApiLogin(), searchString);

            var field = formFields.Single();
            var formFieldId = field.ToInt("Form_Field_ID");
            return formFieldId;
        }

        public List<FormField> GetFieldsForForm(int formId)
        {
            var searchString = string.Format(",,,,{0}", formId);
            var formFields = _ministryPlatformService.GetPageViewRecords(_formFieldCustomPage, ApiLogin(), searchString);

            return formFields.Select(formField => new FormField
            {
                CrossroadsId = formField.ToInt("CrossroadsId"),
                FieldLabel = formField.ToString("Field_Label"),
                FieldOrder = formField.ToInt("Field_Order"),
                FieldType = formField.ToString("Field_Type"),
                FormFieldId = formField.ToInt("Form_Field_ID"),
                FormId = formField.ToInt("Form_ID"),
                FormTitle = formField.ToString("Form_Title"),
                Required = formField.ToBool("Required")
            }).ToList();
        }

        public List<TripFormResponse> GetTripFormResponsesByRecordId(int recordId)
        {
            var command = CreateTripFormResponsesSqlCommandWithRecordId(recordId);
            return GetTripFormResponses(command);
        }

        public List<TripFormResponse> GetTripFormResponsesBySelectionId(int selectionId)
        {
            var command = CreateTripFormResponsesSqlCommandWithSelectionId(selectionId);
            return GetTripFormResponses(command);
        }
        private List<TripFormResponse> GetTripFormResponses(IDbCommand command)
        {
            var connection = _dbConnection;
            try
            {
                connection.Open();

                command.Connection = connection;
                var reader = command.ExecuteReader();
                var responses = new List<TripFormResponse>();
                while (reader.Read())
                {
                    var response = new TripFormResponse();
                    response.ContactId = reader.GetInt32(reader.GetOrdinal("Contact_ID"));
                    var donorId = SafeInt32(reader, "Donor_ID");
                    response.DonorId = donorId;
                    response.FundraisingGoal = SafeDecimal(reader,"Fundraising_Goal");
                    response.ParticipantId = reader.GetInt32(reader.GetOrdinal("Participant_ID"));
                    response.PledgeCampaignId = SafeInt32(reader, "Pledge_Campaign_ID");
                    response.EventId = SafeInt32(reader, "Event_ID"); 

                    responses.Add(response);
                }
                return responses;
            }
            finally
            {
                connection.Close();
            }
        }

        private static decimal SafeDecimal(IDataRecord record, string fieldName)
        {
            var ordinal = record.GetOrdinal(fieldName);
            return !record.IsDBNull(ordinal) ? record.GetDecimal(ordinal) : 0;
        }

        private static int? SafeInt(IDataRecord record, string fieldName)
        {
            var ordinal = record.GetOrdinal(fieldName);
            return !record.IsDBNull(ordinal) ? record.GetInt16(ordinal) : (int?)null;
        }

        private static int? SafeInt32(IDataRecord record, string fieldName)
        {
            var ordinal = record.GetOrdinal(fieldName);
            return !record.IsDBNull(ordinal) ? record.GetInt32(ordinal) : (int?)null;
        }

        private static IDbCommand CreateTripFormResponsesSqlCommandWithRecordId(int recordId)
        {
            const string query = @"SELECT fr.Contact_ID, fr.Pledge_Campaign_ID, pc.Event_ID, pc.Fundraising_Goal, p.Participant_ID, d.Donor_ID
                                  FROM [MinistryPlatform].[dbo].[Form_Responses] fr
                                  INNER JOIN [MinistryPlatform].[dbo].[Participants] p on fr.Contact_ID = p.Contact_ID
                                  LEFT OUTER JOIN [MinistryPlatform].[dbo].[Pledge_Campaigns] pc on fr.Pledge_Campaign_ID = pc.Pledge_Campaign_ID
                                  LEFT OUTER JOIN [MinistryPlatform].[dbo].[Donors] d on fr.Contact_ID = d.Contact_ID
                                  WHERE fr.Form_Response_ID = @recordId";

            using (IDbCommand command = new SqlCommand(string.Format(query)))
            {
                command.Parameters.Add(new SqlParameter("@recordId", recordId) { DbType = DbType.Int32 });
                command.CommandType = CommandType.Text;
                return command;
            }
        }

        private static IDbCommand CreateTripFormResponsesSqlCommandWithSelectionId(int selectionId)
        {
            const string query = @"SELECT fr.Contact_ID, fr.Pledge_Campaign_ID, pc.Event_ID, pc.Fundraising_Goal, p.Participant_ID, d.Donor_ID
                                  FROM [MinistryPlatform].[dbo].[dp_Selected_Records] sr
                                  INNER JOIN [MinistryPlatform].[dbo].[Form_Responses] fr on sr.Record_ID = fr.Form_Response_ID
                                  INNER JOIN [MinistryPlatform].[dbo].[Participants] p on fr.Contact_ID = p.Contact_ID
                                  LEFT OUTER JOIN [MinistryPlatform].[dbo].[Pledge_Campaigns] pc on fr.Pledge_Campaign_ID = pc.Pledge_Campaign_ID
                                  LEFT OUTER JOIN [MinistryPlatform].[dbo].[Donors] d on fr.Contact_ID = d.Contact_ID
                                  WHERE sr.Selection_ID = @selectionId";

            using (IDbCommand command = new SqlCommand(string.Format(query)))
            {
                command.Parameters.Add(new SqlParameter("@selectionId", selectionId) { DbType = DbType.Int32 });
                command.CommandType = CommandType.Text;
                return command;
            }
        }

        public int SubmitFormResponse(FormResponse form)
        {
            var token = ApiLogin();
            var responseId = CreateFormResponse(form, token);
            foreach (var answer in form.FormAnswers)
            {
                answer.FormResponseId = responseId;
                CreateFormAnswer(answer, token);
            }
            return responseId;
        }

        public DateTime? GetTripFormResponseByContactId(int contactId, int pledgeId)
        {
            var searchString = string.Format(",{0},,{1}", contactId, pledgeId);
            var signedUp = _ministryPlatformService.GetPageViewRecords(_formResponseGoTripView, ApiLogin(), searchString);
            if (signedUp.Count < 1)
            {
                return null;
            }
            return DateTime.Parse(signedUp.First()["Response_Date"].ToString());
        }

        private int CreateFormResponse(FormResponse formResponse, string token)
        {
            var record = new Dictionary<string, object>
            {
                {"Form_ID", formResponse.FormId},
                {"Response_Date", DateTime.Today},
                {"Contact_ID", formResponse.ContactId},
                {"Opportunity_ID",  formResponse.OpportunityId },
                {"Opportunity_Response", formResponse.OpportunityResponseId},
                {"Pledge_Campaign_ID", formResponse.PledgeCampaignId}
            };

            var responseId = _ministryPlatformService.CreateRecord(_formResponsePageId, record, token, true);
            return responseId;
        }

        private void CreateFormAnswer(FormAnswer answer, string token)
        {
            var formAnswer = new Dictionary<string, object>
            {
                {"Form_Response_ID", answer.FormResponseId},
                {"Form_Field_ID", answer.FieldId},
                {"Response", answer.Response},
                {"Opportunity_Response", answer.OpportunityResponseId}
            };

            try
            {
                _ministryPlatformService.CreateRecord(_formAnswerPageId, formAnswer, token, true);
            }
            catch (Exception exception)
            {
                throw new ApplicationException(string.Format("CreateFormAnswer failed.  Field Id: {0}", answer.FieldId), exception);
            }
        }
    }
}