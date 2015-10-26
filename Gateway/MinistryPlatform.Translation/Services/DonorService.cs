using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Crossroads.Utilities;
using Crossroads.Utilities.Extensions;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Models;
using MinistryPlatform.Models.DTO;
using MinistryPlatform.Translation.Enum;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.PlatformService;
using MinistryPlatform.Translation.Services.Interfaces;
using Communication = MinistryPlatform.Models.Communication;

namespace MinistryPlatform.Translation.Services
{
    public class DonorService : BaseService, IDonorService
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(DonorService));

        private readonly int _donorPageId;
        private readonly int _donationPageId;
        private readonly int _donationDistributionPageId;
        private readonly int _donorAccountsPageId;
        private readonly int _findDonorByAccountPageViewId;
        private readonly int _donationStatusesPageId;
        private readonly int _donorLookupByEncryptedAccount;
        private readonly int _myHouseholdDonationDistributions;
        private readonly int _recurringGiftBySubscription;
        private readonly int _recurringGiftPageId;
        private readonly int _myHouseholdDonationRecurringGifts;
        private readonly int _myHouseholdRecurringGiftsApiPageView;
        
        public const string DonorRecordId = "Donor_Record";
        public const string DonorProcessorId = "Processor_ID";
        public const string EmailReason = "None";
        public const string DefaultInstitutionName = "Bank";
        public const string DonorRoutingNumberDefault = "0";
        public const string DonorAccountNumberDefault = "0";
        // This is taken from GnosisChecks: 
        // https://github.com/crdschurch/GnosisChecks/blob/24edc373ae62660028c1637396a9b834dfb2fd4d/Modules.vb#L12
        public const string HashKey = "Mcc3#e758ebe8Seb1fdeF628dbK796e5";

        private readonly IMinistryPlatformService _ministryPlatformService;
        private readonly IProgramService _programService;
        private readonly ICommunicationService _communicationService;
        private readonly IContactService _contactService;
        private readonly ICryptoProvider _crypto;
       
        public DonorService(IMinistryPlatformService ministryPlatformService, IProgramService programService, ICommunicationService communicationService, IAuthenticationService authenticationService, IContactService contactService,  IConfigurationWrapper configuration, ICryptoProvider crypto)
            : base(authenticationService, configuration)
        {
            _ministryPlatformService = ministryPlatformService;
            _programService = programService;
            _communicationService = communicationService;
            _contactService = contactService;
            _crypto = crypto;

            _donorPageId = configuration.GetConfigIntValue("Donors");
            _donationPageId = configuration.GetConfigIntValue("Donations");
            _donationDistributionPageId = configuration.GetConfigIntValue("Distributions");
            _donorAccountsPageId = configuration.GetConfigIntValue("DonorAccounts");
            _findDonorByAccountPageViewId = configuration.GetConfigIntValue("FindDonorByAccountPageView");
            _donationStatusesPageId = configuration.GetConfigIntValue("DonationStatus");
            _donorLookupByEncryptedAccount = configuration.GetConfigIntValue("DonorLookupByEncryptedAccount");
            _myHouseholdDonationDistributions = configuration.GetConfigIntValue("MyHouseholdDonationDistributions");
            _recurringGiftBySubscription = configuration.GetConfigIntValue("RecurringGiftBySubscription");
            _recurringGiftPageId = configuration.GetConfigIntValue("RecurringGifts");
            _myHouseholdDonationRecurringGifts = configuration.GetConfigIntValue("MyHouseholdDonationRecurringGifts");
            _myHouseholdRecurringGiftsApiPageView = configuration.GetConfigIntValue("MyHouseholdRecurringGiftsApiPageView");
        }


        public int CreateDonorRecord(int contactId, string processorId, DateTime setupTime, int? statementFrequencyId = 1,
                                     // default to quarterly
                                     int? statementTypeId = 1,
                                     //default to individual
                                     int? statementMethodId = 2,
                                     // default to email/online
                                     DonorAccount donorAccount = null
            )
        {
            //this assumes that you do not already have a donor record - new giver
            var values = new Dictionary<string, object>
            {
                {"Contact_ID", contactId},
                {"Statement_Frequency_ID", statementFrequencyId},
                {"Statement_Type_ID", statementTypeId},
                {"Statement_Method_ID", statementMethodId},
                {"Setup_Date", setupTime}, //default to current date/time
                {"Processor_ID", processorId}
            };

            var apiToken = ApiLogin();

            int donorId;

            try
            {
                donorId = _ministryPlatformService.CreateRecord(_donorPageId, values, apiToken, true);
            }
            catch (Exception e)
            {
                throw new ApplicationException(string.Format("CreateDonorRecord failed.  Contact Id: {0}", contactId), e);
            }
   
            if (donorAccount!= null)
            {
                CreateDonorAccount(DefaultInstitutionName,
                                   DonorAccountNumberDefault,
                                   DonorRoutingNumberDefault,
                                   donorAccount.EncryptedAccount,
                                   donorId,
                                   donorAccount.ProcessorAccountId,
                                   processorId);
            }
            
            return donorId;
        }

        // TODO Need this method to accept an authorized user token in order to facilitate admin setup/edit
        public int CreateDonorAccount(string giftType, string routingNumber, string acctNumber, string encryptedAcct, int donorId, string processorAcctId, string processorId)
        {
            var apiToken = ApiLogin();

            var institutionName = giftType ?? DefaultInstitutionName;
            var accountType = (institutionName == "Bank") ? AccountType.Checking : AccountType.Credit;

            try
            {
              var  values = new Dictionary<string, object>
                {
                    { "Institution_Name", institutionName },
                    { "Account_Number", acctNumber },
                    { "Routing_Number", DonorRoutingNumberDefault },
                    { "Encrypted_Account", encryptedAcct },
                    { "Donor_ID", donorId },
                    { "Non-Assignable", false },
                    { "Account_Type_ID", (int)accountType},
                    { "Closed", false },
                    {"Processor_Account_ID", processorAcctId},
                    {"Processor_ID", processorId}
                };

                 var donorAccountId = _ministryPlatformService.CreateRecord(_donorAccountsPageId, values, apiToken);  
                 return donorAccountId; 
            }
            catch (Exception e)
            {
                throw new ApplicationException(string.Format("CreateDonorAccount failed.  Donor Id: {0}", donorId), e);
            }
           
        }

        public void DeleteDonorAccount(string authorizedUserToken, int donorAccountId)
        {
            try
            {
                _ministryPlatformService.DeleteRecord(_donorAccountsPageId, donorAccountId, new []
                {
                    new DeleteOption
                    {
                        Action = DeleteAction.Delete
                    }
                }, authorizedUserToken);
            }
            catch (Exception e)
            {
                throw new ApplicationException(string.Format("RemoveDonorAccount failed.  Donor Id: {0}", donorAccountId), e);
            }
        }

        public void UpdateRecurringGiftDonorAccount(string authorizedUserToken, int recurringGiftId, int donorAccountId)
        {
            var recurringGiftValues = new Dictionary<string, object>
            {
                {"Donor_Account_ID", donorAccountId}
            };

            UpdateRecurringGift(_myHouseholdDonationRecurringGifts, authorizedUserToken, recurringGiftId, recurringGiftValues);
        }

        public void CancelRecurringGift(string authorizedUserToken, int recurringGiftId)
        {
            var recurringGiftValues = new Dictionary<string, object>
            {
                {"End_Date", DateTime.Now.Date}
            };
           
            UpdateRecurringGift(_myHouseholdDonationRecurringGifts, authorizedUserToken, recurringGiftId, recurringGiftValues);
        }

        public void CancelRecurringGift(int recurringGiftId)
        {
            var recurringGiftValues = new Dictionary<string, object>
            {
                {"End_Date", DateTime.Now.Date}
            };
            var apiToken = ApiLogin();
            UpdateRecurringGift(_recurringGiftPageId, apiToken, recurringGiftId, recurringGiftValues);
        }
        
        public void UpdateRecurringGiftFailureCount(int recurringGiftId, int failCount)
        {
            var recurringGiftValues = new Dictionary<string, object>
            {
                {"Consecutive_Failure_Count", failCount}
            };

            try
            {
                var apiToken = ApiLogin();
                UpdateRecurringGift(_recurringGiftPageId, apiToken, recurringGiftId, recurringGiftValues);
            }
            catch (Exception e)
            {
                throw new ApplicationException(
                       string.Format(
                           "Update Recurring Gift Failure Count failed.  Recurring Gift Id: {0}"),e);
            }
        }


        public void UpdateRecurringGift(int pageView, string token, int recurringGiftId, Dictionary<string, object> recurringGiftValues)
        {
            recurringGiftValues["Recurring_Gift_ID"] = recurringGiftId;

            try
            {
               _ministryPlatformService.UpdateRecord(pageView, recurringGiftValues, token);
            }
            catch (Exception e)
            {
                throw new ApplicationException(
                    string.Format(
                        "Update Recurring Gift Donor Account failed.  Recurring Gift Id: {0}, Updates: {1}"
                        , recurringGiftId
                        , string.Join(";", recurringGiftValues)),
                    e);
            }
            
        }
        
        public int CreateDonationAndDistributionRecord(int donationAmt, int? feeAmt, int donorId, string programId, int? pledgeId, string chargeId, string pymtType, string processorId, DateTime setupTime, bool registeredDonor, bool anonymous, bool recurringGift, int? recurringGiftId, string donorAcctId, string checkScannerBatchName = null, int? donationStatus = null)
        {
            var pymtId = PaymentType.GetPaymentType(pymtType).id;
            var fee = feeAmt.HasValue ? feeAmt / Constants.StripeDecimalConversionValue : null;

            var apiToken = ApiLogin();

            var donationValues = new Dictionary<string, object>
            {
                {"Donor_ID", donorId},
                {"Donation_Amount", donationAmt},
                {"Processor_Fee_Amount", fee},
                {"Payment_Type_ID", pymtId},
                {"Donation_Date", setupTime},
                {"Transaction_code", chargeId},
                {"Registered_Donor", registeredDonor},
                {"Anonymous", anonymous},
                {"Processor_ID", processorId },
                {"Donation_Status_Date", setupTime},
                {"Donation_Status_ID", donationStatus ?? 1}, //hardcoded to pending if no status specified
                {"Recurring_Gift_ID", recurringGiftId},
                {"Is_Recurring_Gift", recurringGift},
                {"Donor_Account_ID", donorAcctId}
            };
            if (!string.IsNullOrWhiteSpace(checkScannerBatchName))
            {
                donationValues["Check_Scanner_Batch"] = checkScannerBatchName;
            }

            int donationId;

            try
            {
                donationId = _ministryPlatformService.CreateRecord(_donationPageId, donationValues, apiToken, true);
            }
            catch (Exception e)
            {
                throw new ApplicationException(string.Format("CreateDonationRecord failed.  Donor Id: {0}", donorId), e);
            }

            if (string.IsNullOrWhiteSpace(programId))
            {
                return (donationId);
            }
            
            var distributionValues = new Dictionary<string, object>
            {
                {"Donation_ID", donationId},
                {"Amount", donationAmt},
                {"Program_ID", programId}                
            };
            if (pledgeId != null)
            {
                distributionValues.Add("Pledge_ID", pledgeId);
            }

            try
            {
                _ministryPlatformService.CreateRecord(_donationDistributionPageId, distributionValues, apiToken, true);
            }
            catch (Exception e)
            {
                throw new ApplicationException(
                    string.Format("CreateDonationDistributionRecord failed.  Donation Id: {0}", donationId), e);
            }

            try
            {
                SetupConfirmationEmail(Convert.ToInt32(programId), donorId, donationAmt, setupTime, pymtType);
            }
            catch (Exception)
            {
                _logger.Error(string.Format("Failed when processing the template for Donation Id: {0}", donationId));
            }

            return donationId;
        }

        public ContactDonor GetContactDonor(int contactId)
        {
            ContactDonor donor;
            try
            {
                var searchStr = string.Format("\"{0}\",", contactId);
                var records =
                    WithApiLogin(
                        apiToken => (_ministryPlatformService.GetPageViewRecords("DonorByContactId", apiToken, searchStr)));
                if (records != null && records.Count > 0)
                {
                    var record = records.First();
                    donor = new ContactDonor()
                    {
                        DonorId = record.ToInt("Donor_ID"),
                        //we only want processorID from the donor if we are not processing a check
                        ProcessorId = record.ToString(DonorProcessorId),
                        ContactId = record.ToInt("Contact_ID"),
                        RegisteredUser = true,
                        Email = record.ToString("Email"),
                        StatementType = record.ToString("Statement_Type"),
                        StatementTypeId = record.ToInt("Statement_Type_ID"),
                        StatementFreq = record.ToString("Statement_Frequency"),
                        StatementMethod = record.ToString("Statement_Method"),
                        Details = new ContactDetails
                        {
                            EmailAddress = record.ToString("Email"),
                            HouseholdId = record.ToInt("Household_ID")
                        }
                    };
                }
                else
                {
                    donor = new ContactDonor
                    {
                        ContactId = contactId,
                        RegisteredUser = true
                    };
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    string.Format("GetDonorRecord failed.  Contact Id: {0}", contactId), ex);
            }

            return donor;

        }
        public ContactDonor GetPossibleGuestContactDonor(string email)
        {
            ContactDonor donor;
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return null;
                }
                var searchStr =  "," + email;
                var records =
                    WithApiLogin(
                        apiToken => (_ministryPlatformService.GetPageViewRecords("PossibleGuestDonorContact", apiToken, searchStr)));
                if (records != null && records.Count > 0)
                {
                    var record = records.First();
                    donor = new ContactDonor()
                    {
                        
                        DonorId = record.ToInt(DonorRecordId),
                        ProcessorId = record.ToString(DonorProcessorId),
                        ContactId = record.ToInt("Contact_ID"),
                        Email = record.ToString("Email_Address"),
                        RegisteredUser = false
                    };
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    string.Format("GetPossibleGuestDonorContact failed. Email: {0}", email), ex);
            }

            return donor;

        }

        public ContactDonor GetContactDonorForDonorAccount(string accountNumber, string routingNumber)
        {
            var search = string.Format(",\"{0}\"", CreateHashedAccountAndRoutingNumber(accountNumber, routingNumber));

            var accounts = WithApiLogin(apiToken => _ministryPlatformService.GetPageViewRecords(_findDonorByAccountPageViewId, apiToken, search));
            if (accounts == null || accounts.Count == 0)
            {
                return (null);
            }

            var contactId = accounts[0]["Contact_ID"] as int? ?? -1;
            return contactId == -1 ? (null) : (GetContactDonor(contactId));
        }

        public ContactDonor GetContactDonorForCheckAccount(string encrptedKey)
        {
            var donorAccount = WithApiLogin(apiToken => _ministryPlatformService.GetPageViewRecords(_donorLookupByEncryptedAccount, apiToken, "," + encrptedKey));
            if (donorAccount == null || donorAccount.Count == 0)
            {
                return null;
            }
            var contactId = Convert.ToInt32(donorAccount[0]["Contact_ID"]);
            var myContact = _contactService.GetContactById(contactId);

            var details = new ContactDonor
            {
               DonorId = (int) donorAccount[0]["Donor_ID"],
               Details = new ContactDetails
               {
                   DisplayName = donorAccount[0]["Display_Name"].ToString(),
                   Address = new PostalAddress
                   {
                       Line1 = myContact.Address_Line_1,
                       Line2 = myContact.Address_Line_2,
                       City = myContact.City,
                       State = myContact.State,
                       PostalCode = myContact.Postal_Code
                   } 
               }
               
            };

            return details;
        }

        /// <summary>
        /// Create a SHA256 of the given account and routing number.  The algorithm for this matches the same from GnosisChecks:
        /// https://github.com/crdschurch/GnosisChecks/blob/24edc373ae62660028c1637396a9b834dfb2fd4d/Modules.vb#L52
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <param name="routingNumber"></param>
        /// <returns></returns>
        public string CreateHashedAccountAndRoutingNumber(string accountNumber, string routingNumber)
        {
            var crypt = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(string.Concat(routingNumber, accountNumber, HashKey));
            byte[] crypto = crypt.ComputeHash(bytes, 0, bytes.Length);
            var hashString = Convert.ToBase64String(crypto).Replace('/', '~');
            return (hashString);
        }

        public string DecryptCheckValue(string value)
        {
            var valueDecrypt = _crypto.DecryptValue(value);
            return valueDecrypt;
        }

        public int UpdatePaymentProcessorCustomerId(int donorId, string paymentProcessorCustomerId)
        {
            var parms = new Dictionary<string, object> {
                { "Donor_ID", donorId },
                { DonorProcessorId, paymentProcessorCustomerId },
            };

            try
            {
                _ministryPlatformService.UpdateRecord(_donorPageId, parms, ApiLogin());
            }
            catch (Exception e)
            {
                throw new ApplicationException(
                    string.Format("UpdatePaymentProcessorCustomerId failed. donorId: {0}, paymentProcessorCustomerId: {1}", donorId, paymentProcessorCustomerId), e);
            }

            return (donorId);
        }

        public string UpdateDonorAccount(string encryptedKey, string sourceId, string customerId)
        {
            try
            {
                var donorAccount = WithApiLogin(apiToken => _ministryPlatformService.GetPageViewRecords(_donorLookupByEncryptedAccount, apiToken, "," + encryptedKey));
                var donorAccountId = donorAccount[0]["dp_RecordID"].ToString();
                var updateParms = new Dictionary<string, object>
                {
                    {"Donor_Account_ID", donorAccountId},
                    {"Processor_Account_ID", sourceId},
                    {"Processor_ID", customerId}
                };
                _ministryPlatformService.UpdateRecord(_donorAccountsPageId, updateParms, ApiLogin());
                 return donorAccountId;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    string.Format("UpdateDonorAccount failed.  Donor Account: {0}", encryptedKey), ex);
            }
        }

        public void SetupConfirmationEmail(int programId, int donorId, int donationAmount, DateTime setupDate, string pymtType)
        {
            var program = _programService.GetProgramById(programId);
            //If the communcations admin does not link a message to the program, the default template will be used.
            var communicationTemplateId = program.CommunicationTemplateId ?? AppSetting("DefaultGiveConfirmationEmailTemplate");

            SendEmail(communicationTemplateId, donorId, donationAmount, pymtType, setupDate, program.Name, EmailReason);
        }

        public ContactDonor GetEmailViaDonorId(int donorId)
        {
            var donor = new ContactDonor();
            try
            {
                var searchStr = string.Format(",\"{0}\"", donorId);
                var records =
                    WithApiLogin(
                        apiToken => (_ministryPlatformService.GetPageViewRecords("DonorByContactId", apiToken, searchStr)));
                if (records != null && records.Count > 0)
                {
                    var record = records.First();

                    donor.DonorId = record.ToInt("Donor_ID");
                    donor.ProcessorId = record.ToString(DonorProcessorId);
                    donor.ContactId = record.ToInt("Contact_ID");
                    donor.RegisteredUser = true;
                    donor.Email = record.ToString("Email");
                    donor.StatementType = record.ToString("Statement_Type");
                    donor.StatementTypeId = record.ToInt("Statement_Type_ID");
                    donor.StatementFreq = record.ToString("Statement_Frequency");
                    donor.StatementMethod = record.ToString("Statement_Method");
                    donor.Details = new ContactDetails
                    {
                        EmailAddress = record.ToString("Email"),
                        HouseholdId = record.ToInt("Household_ID")
                    };
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    string.Format("GetEmailViaDonorId failed.  Donor Id: {0}", donorId), ex);
            }

            return donor;
        }

        public void SendEmail(int communicationTemplateId, int donorId, decimal donationAmount, string paymentType, DateTime setupDate, string program, string emailReason, string frequency = null)
        {
            var template = _communicationService.GetTemplate(communicationTemplateId);

            var contact = GetEmailViaDonorId(donorId);

            var comm = new Communication
            {
                AuthorUserId = 5,
                DomainId = 1,
                EmailBody = template.Body,
                EmailSubject = template.Subject,
                FromContactId = 5,
                FromEmailAddress = "giving@crossroads.net",
                ReplyContactId = 5,
                ReplyToEmailAddress = "giving@crossroads.net",
                ToContactId = contact.ContactId,
                ToEmailAddress = contact.Email,
                MergeData = new Dictionary<string, object>
                {
                    {"Program_Name", program},
                    {"Donation_Amount", donationAmount},
                    {"Donation_Date", setupDate.ToString("MM/dd/yyyy HH:mm tt")},
                    {"Payment_Method", paymentType},
                    {"Decline_Reason", emailReason},
                    {"Frequency", frequency}
                }
            };


            _communicationService.SendMessage(comm);
        }

        public List<Donation> GetDonations(IEnumerable<int> donorIds, string donationYear = null)
        {
            var search = string.Format("{0},,,,,,,,,,{1}", YearSearch(donationYear), DonorIdSearch(donorIds));
            var records = WithApiLogin(token => _ministryPlatformService.GetRecordsDict(_donationDistributionPageId, token, search));

            return MapDonationRecords(records);
        }

        public List<Donation> GetDonations(int donorId, string donationYear = null)
        {
            return (GetDonations(new [] {donorId}, donationYear));
        }

        private List<DonationStatus> GetDonationStatuses()
        {
            var statuses = WithApiLogin(token => _ministryPlatformService.GetRecordsDict(_donationStatusesPageId, token));

            if (statuses == null || statuses.Count == 0)
            {
                return (new List<DonationStatus>());
            }

            var result = statuses.Select(s => new DonationStatus
            {
                DisplayOnGivingHistory = s["Display_On_Giving_History"] as bool? ?? true,
                DisplayOnStatement = s["Display_On_Statements"] as bool? ?? false,
                DisplayOnMyTrips = s["Display_On_MyTrips"] as bool? ?? false,
                Id = s["dp_RecordID"] as int? ?? 0,
                Name = s["Donation_Status"] as string
            }).ToList();

            return (result);
        }

        public List<Donation> GetSoftCreditDonations(IEnumerable<int> donorIds, string donationYear = null)
        {
            var search = string.Format("{0},,,,,,,,,,,,,,,,,{1}", YearSearch(donationYear), DonorIdSearch(donorIds));
            var records = WithApiLogin(token => _ministryPlatformService.GetRecordsDict(_donationDistributionPageId, token, search));

            return MapDonationRecords(records);
        }

        public List<Donation> GetDonationsForAuthenticatedUser(string userToken, bool? softCredit = null, string donationYear = null)
        {
            var search = string.Format("{0},{1}", YearSearch(donationYear), softCredit.HasValue ? softCredit.Value.ToString() : string.Empty);
            var records = _ministryPlatformService.GetRecordsDict(_myHouseholdDonationDistributions, userToken, search);

            return MapDonationRecords(records);
        }

        private List<Donation> MapDonationRecords(List<Dictionary<string, Object>> records)
        {
            if (records == null || records.Count == 0)
            {
                return null;
            }

            var statuses = GetDonationStatuses();

            var donationMap = new Dictionary<int, Donation>();
            foreach (var record in records)
            {
                var donationId = record["Donation_ID"] as int? ?? 0;

                var donation = GetDonationFromMap(donationMap, record, donationId, statuses);
                AddDistributionToDonation(record, donation);
                donationMap[donation.donationId] = donation;
            }

            return donationMap.Values.ToList();
        }

        private static Donation GetDonationFromMap(Dictionary<int, Donation> donationMap,
                                            Dictionary<string, Object> record,
                                            int donationId,
                                            List<DonationStatus> statuses)
        {
            if (donationMap.ContainsKey(donationId))
            {
                return donationMap[donationId];
            }
            
            var donation = new Donation
            {
                donationDate = record["Donation_Date"] as DateTime? ?? DateTime.Now,
                batchId = null,
                donationId = record["Donation_ID"] as int? ?? 0,
                donationNotes = null,
                donationStatus = record["Donation_Status_ID"] as int? ?? 0,
                donationStatusDate = record["Donation_Status_Date"] as DateTime? ?? DateTime.Now,
                donorId = record["Donor_ID"] as int? ?? 0,
                paymentTypeId = record["Payment_Type_ID"] as int? ?? 0,
                transactionCode = record["Transaction_Code"] as string,
                softCreditDonorId = record["Soft_Credit_Donor_ID"] as int? ?? 0,
                donorDisplayName = record["Donor_Display_Name"] as string,
            };

            var status = statuses.Find(x => x.Id == donation.donationStatus) ?? new DonationStatus();
            donation.IncludeOnGivingHistory = status.DisplayOnGivingHistory;
            donation.IncludeOnPrintedStatement = status.DisplayOnStatement;

            return donation;
        }

        private static void AddDistributionToDonation(Dictionary<string, Object> record, Donation donation)
        {

            var amount = Convert.ToInt32((record["Amount"] as decimal? ?? 0) * Constants.StripeDecimalConversionValue);
            donation.donationAmt += amount;

            donation.Distributions.Add(new DonationDistribution
            {
                donationDistributionProgram = record["dp_RecordName"] as string,
                donationDistributionAmt = amount
            });
        }

        private static string YearSearch(string year)
        {
            return string.IsNullOrWhiteSpace(year) ? string.Empty : string.Format("\"*/{0}*\"", year);
        }

        private static string DonorIdSearch(IEnumerable<int> ids)
        {
            return string.Join(" or ", ids.Select(id => string.Format("\"{0}\"", id)));
        }

        public int CreateRecurringGiftRecord(string authorizedUserToken, int donorId, int donorAccountId, string planInterval, decimal planAmount, DateTime startDate, string program, string subscriptionId, int congregationId)
        {
            int? dayOfWeek = null;
            int? dayOfMonth = null;
            int frequencyId;
            if (planInterval == "week")
            {
                dayOfWeek = NumericDayOfWeek.GetDayOfWeekID((startDate.DayOfWeek).ToString());
                frequencyId = 1;
            }
            else
            {
                dayOfMonth = startDate.Day;
                frequencyId = 2;
            }
          
            var values = new Dictionary<string, object>
            {
                {"Donor_ID", donorId},
                {"Donor_Account_ID", donorAccountId},
                {"Frequency_ID", frequencyId},
                {"Day_Of_Month", dayOfMonth},    
                {"Day_Of_Week_ID", dayOfWeek},
                {"Amount", planAmount},
                {"Start_Date", startDate},
                {"Program_ID", program},
                {"Congregation_ID", congregationId},
                {"Subscription_ID", subscriptionId}
            };

            int recurringGiftId;
            try
            {
                recurringGiftId = _ministryPlatformService.CreateRecord(_myHouseholdDonationRecurringGifts, values, authorizedUserToken, true);
            }
            catch (Exception e)
            {
                throw new ApplicationException(string.Format("Create Recurring Gift failed.  Donor Id: {0}", donorId), e);
            }

            return recurringGiftId;
        }

        public CreateDonationDistDto GetRecurringGiftById(string authorizedUserToken, int recurringGiftId)
        {
            var searchStr = string.Format("\"{0}\",", recurringGiftId);
            CreateDonationDistDto createDonation = null;
            try
            {
                var records = _ministryPlatformService.GetPageViewRecords(_myHouseholdRecurringGiftsApiPageView, authorizedUserToken, searchStr);
                if (records != null && records.Any())
                {
                    var record = records.First();
                    createDonation = new CreateDonationDistDto
                    {
                        RecurringGiftId = record.ToNullableInt("Recurring_Gift_ID"),
                        DonorId = record.ToInt("Donor_ID"),
                        Frequency = record.ToInt("Frequency_ID"),
                        DayOfWeek = record.ToInt("Day_Of_Week_ID"),
                        DayOfMonth = record.ToInt("Day_Of_Month"),
                        StartDate = record.ToDate("Start_Date"),
                        Amount = (int)((record["Amount"] as decimal? ?? 0.00M) * Constants.StripeDecimalConversionValue),
                        ProgramId = record.ToString("Program_ID"),
                        CongregationId = record.ToInt("Congregation_ID"),
                        PaymentType = (int)AccountType.Checking == record.ToInt("Account_Type_ID") ? PaymentType.Bank.abbrv : PaymentType.CreditCard.abbrv,
                        DonorAccountId = record.ToNullableInt("Donor_Account_ID"),
                        SubscriptionId = record.ToString("Subscription_ID"),
                        StripeCustomerId = record.ToString("Processor_ID"),
                        StripeAccountId = record.ToString("Processor_Account_ID")
                    };
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    string.Format("GetRecurringGift failed.  Recurring Gift Id: {0}", recurringGiftId), ex);
            }

            return createDonation;
        }

        public CreateDonationDistDto GetRecurringGiftForSubscription(string subscription)
        {
            var searchStr = string.Format(",\"{0}\",", subscription);
            CreateDonationDistDto createDonation = null;
            try
            {
                var records =
                    WithApiLogin(
                        apiToken => (_ministryPlatformService.GetPageViewRecords(_recurringGiftBySubscription, apiToken, searchStr)));
                if (records != null && records.Count > 0)
                {
                    var record = records.First();
                    createDonation = new CreateDonationDistDto
                    {
                        DonorId = record.ToInt("Donor_ID"),
                        Amount = record["Amount"] as decimal? ?? 0,
                        ProgramId = record.ToString("Program_ID"),
                        CongregationId = record.ToInt("Congregation_ID"),
                        PaymentType = (int)AccountType.Checking == record.ToInt("Account_Type_ID") ? PaymentType.Bank.abbrv : PaymentType.CreditCard.abbrv,
                        RecurringGiftId = record.ToNullableInt("Recurring_Gift_ID"),
                        DonorAccountId = record.ToNullableInt("Donor_Account_ID"),
                        SubscriptionId = record.ToString("Subscription_ID"),
                        Frequency = record.ToInt("Frequency_ID"),
                        ConsecutiveFailureCount = record.ToInt("Consecutive_Failure_Count")
                    };
                }
                
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    string.Format("GetRecurringGift failed.  Subscription Id: {0}", subscription), ex);
            }

           return createDonation;
        }

        public List<RecurringGift> GetRecurringGiftsForAuthenticatedUser(string userToken)
        {
            var records = _ministryPlatformService.GetRecordsDict(_myHouseholdDonationRecurringGifts, userToken);
            return records.Select(MapRecordToRecurringGift).ToList();
        }


        public void ProcessRecurringGiftDecline(string subscriptionId)
        {
            var recurringGift = GetRecurringGiftForSubscription(subscriptionId);
            UpdateRecurringGiftFailureCount(recurringGift.RecurringGiftId.Value, recurringGift.ConsecutiveFailureCount + 1);
            
            var acctType = GetDonorAccountPymtType(recurringGift.DonorAccountId.Value);
            var paymentType = PaymentType.GetPaymentType(acctType).name;
            var templateId = recurringGift.ConsecutiveFailureCount >= 2 ? PaymentType.GetPaymentType(acctType).recurringGiftCancelEmailTemplateId : PaymentType.GetPaymentType(acctType).recurringGiftDeclineEmailTemplateId;
            var frequency = recurringGift.Frequency == 1 ? "Weekly" : "Monthly";
            var program = _programService.GetProgramById(Convert.ToInt32(recurringGift.ProgramId));
            var amt = decimal.Round(recurringGift.Amount, 2, MidpointRounding.AwayFromZero);

            SendEmail(templateId, recurringGift.DonorId, amt, paymentType, DateTime.Now, program.Name, "fail", frequency);
        }

        public int GetDonorAccountPymtType(int donorAccountId)
        {
            var donorAccount = _ministryPlatformService.GetRecordDict(_donorAccountsPageId, donorAccountId, ApiLogin());

            if (donorAccount == null)
            {
               throw new ApplicationException(
                   string.Format("Donor Account not found.  Donor Account Id: {0}", donorAccountId));
            }
            
            return donorAccount.ToInt("Account_Type_ID");
        }

        // ReSharper disable once FunctionComplexityOverflow
        private RecurringGift MapRecordToRecurringGift(Dictionary<string, object> record)
        {
            return new RecurringGift
            {
                RecurringGiftId = record["Recurring_Gift_ID"] as int? ?? 0,
                DonorID = record["Donor_ID"] as int? ?? 0,
                EmailAddress = record["User_Email"] as string,
                Frequency = (record["Frequency"] as string ?? string.Empty).Trim(),
                Recurrence = record["Recurrence"] as string,
                StartDate = record["Start_Date"] as DateTime? ?? DateTime.Now,
                EndDate = record["End_Date"] as DateTime?,
                Amount = record["Amount"] as decimal? ?? 0,
                ProgramID = record["Program_ID"] as int? ?? 0,
                ProgramName = record["Program_Name"] as string,
                CongregationName = record["Congregation_Name"] as string,
                AccountTypeID = record["Account_Type_ID"] as int? ?? 0,
                AccountNumberLast4 = record["Account_Number"] as string,
                InstitutionName = record["Institution_Name"] as string,
                SubscriptionID = record["Subscription_ID"] as string,
                ProcessorAccountId = record["Processor_Account_ID"] as string,
                ProcessorId = record["Processor_ID"] as string
            };
        }
    }

}
