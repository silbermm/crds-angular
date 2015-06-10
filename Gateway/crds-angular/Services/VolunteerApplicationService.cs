using System;
using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.VolunteerApplication;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using Newtonsoft.Json;
using Attribute = System.Attribute;

//using Attribute = System.Attribute;

namespace crds_angular.Services
{
    public class VolunteerApplicationService : IVolunteerApplicationService
    {
        private readonly IFormSubmissionService _formSubmissionService;
        private List<FormField> _formFields;

        public VolunteerApplicationService(IFormSubmissionService formSubmissionService)
        {
            _formSubmissionService = formSubmissionService;
        }
        public bool SaveStudent(StudentApplicationDto application)
        {
            //throw new NotImplementedException();

            var opportunityResponseId = application.ResponseOpportunityId;
            _formFields = _formSubmissionService.GetFieldsForForm(application.FormId);

            var formResponse = new FormResponse();
            formResponse.ContactId = application.ContactId;  //contact id of the person the application is for
            formResponse.FormId = application.FormId; // form id is different for student and volunteer; this should probably be a field in CMS
            formResponse.OpportunityId = application.OpportunityId; // we know this from CMS
            formResponse.OpportunityResponseId = opportunityResponseId; // not sure we know this yet; we're only checking for it's existance
            //call svc to create
            //var id = 0; //tmp

            //formResponse.FormAnswers= new List<FormAnswer>();
            //var formAnswer = new FormAnswer();
            //formAnswer.FieldId = 0; // field id from MP; how to tie this back to html form?
            ////formAnswer.FormResponseId = 0; // we don't have to populate this; code handles it.
            //formAnswer.OpportunityResponseId = 0; // same as OpportunityResponseId from formResponse?  rename OpportunityResponseId?
            //formAnswer.Response = ""; // string value entered; how to handle non-string data types?

            

            formResponse.FormAnswers.Add(SetCustomField(application.FirstName, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.LastName, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.MiddleInitial, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.Email, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.BirthDate, opportunityResponseId));
            
            //what to do with this gem and others that will follow?
            switch (application.Gender.Value)
            {
                case "1":
                    application.Gender.Value = Gender.Male.ToString();
                    break;
                case "2":
                    application.Gender.Value = Gender.Female.ToString();
                    break;
            }
            formResponse.FormAnswers.Add(SetCustomField(application.Gender, opportunityResponseId));

            formResponse.FormAnswers.Add(SetCustomField(application.SiteAttend, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.NameForNameTag, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.School,opportunityResponseId));


            formResponse.FormAnswers.Add(SetCustomField(application.HowLongAttending, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.ServiceAttend, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.StreetAddress, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.City, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.State, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.Zip, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.MobilePhone, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.Grade, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.WhereYouAre, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.ExplainFaith, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.WhyServe, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.SpecialTalents, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.AvailabilityDuringWeek, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.AvailabilityDuringWeekend, opportunityResponseId));
            //formResponse.FormAnswers.Add(SetCustomField(application.ServeSite, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.AvailabilityOakley, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.AvailabilityFlorence, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.AvailabilityWestSide, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.AvailabilityMason, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.AvailabilityClifton, opportunityResponseId));

            formResponse.FormAnswers.Add(SetCustomField(application.ServeServiceTimes, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.ServeAgeKids1To2, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.ServeAgeKids3ToPreK, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.ServeAgeKidsKto5Grade, opportunityResponseId));
            
            formResponse.FormAnswers.Add(SetCustomField(application.Reference1Name, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.Reference1TimeKnown, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.Reference1HomePhone, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.Reference1MobilePhone, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.Reference1WorkPhone, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.Reference1Email, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.Reference1Association, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.Reference1Occupation, opportunityResponseId));


            formResponse.FormAnswers.Add(SetCustomField(application.Reference2Name, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.Reference2TimeKnown, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.Reference2HomePhone, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.Reference2MobilePhone, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.Reference2WorkPhone, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.Reference2Email, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.Reference2Association, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.Reference2Occupation, opportunityResponseId));
            
            
            
            
            formResponse.FormAnswers.Add(SetCustomField(application.ParentLastName, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.ParentFirstName, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.ParentHomePhone, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.ParentMobilePhone, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.ParentEmail, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.ParentSignature, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.ParentSignatureDate, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.StudentSignature, opportunityResponseId));
            formResponse.FormAnswers.Add(SetCustomField(application.StudentSignatureDate, opportunityResponseId));



            var response = _formSubmissionService.SubmitFormResponse(formResponse);
            return response != 0;
        }



        private FormAnswer SetCustomField(CustomField customField, int opportunityResponseId)
        {
            int fieldId;
            try
            {
                fieldId = _formFields.Single(f => f.CrossroadsId == customField.CrossroadsId).FormFieldId;
            }
            catch (Exception exception)
            {
                throw new ApplicationException(string.Format("Failed to locate id for crossroads field {0}", customField.CrossroadsId));
            }
            var answer = new FormAnswer();
            answer.FieldId = fieldId;
            answer.OpportunityResponseId = opportunityResponseId;
            answer.Response = customField.Value;
            return answer;
        }
    }
}