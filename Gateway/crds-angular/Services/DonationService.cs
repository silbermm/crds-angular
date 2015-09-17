using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using crds_angular.Models.Crossroads.Stewardship;
using MPServices=MinistryPlatform.Translation.Services.Interfaces;
using crds_angular.Services.Interfaces;
using crds_angular.Util;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using Newtonsoft.Json;
using DonationStatus = crds_angular.Models.Crossroads.Stewardship.DonationStatus;

namespace crds_angular.Services
{
    public class DonationService: IDonationService
    {
        private readonly MPServices.IDonationService _mpDonationService;
        private readonly MPServices.IDonorService _mpDonorService;
        private readonly MPServices.IAuthenticationService _mpAuthenticationService;
        private readonly IPaymentService _paymentService;
        private readonly MPServices.IContactService _contactService;
        private readonly int _statementTypeFamily;

        public DonationService(MPServices.IDonationService mpDonationService, MPServices.IDonorService mpDonorService, MPServices.IAuthenticationService mpAuthenticationService, IPaymentService paymentService, MPServices.IContactService contactService, IConfigurationWrapper config)
        {
            _mpDonationService = mpDonationService;
            _mpDonorService = mpDonorService;
            _mpAuthenticationService = mpAuthenticationService;
            _paymentService = paymentService;
            _contactService = contactService;
            _statementTypeFamily = config.GetConfigIntValue("DonorStatementTypeFamily");
        }

        public DonationDTO GetDonationByProcessorPaymentId(string processorPaymentId)
        {
            var d = _mpDonationService.GetDonationByProcessorPaymentId(processorPaymentId);
            if (d == null)
            {
                return (null);
            }

            var donation = new DonationDTO
            {
                Amount = d.donationAmt,
                Id = d.donationId + "",
                BatchId = d.batchId
            };
            return (donation);
        }

        public int UpdateDonationStatus(int donationId, int statusId, DateTime? statusDate, string statusNote = null)
        {
            return(_mpDonationService.UpdateDonationStatus(donationId, statusId, statusDate ?? DateTime.Now, statusNote));
        }

        public int UpdateDonationStatus(string processorPaymentId, int statusId, DateTime? statusDate, string statusNote = null)
        {
            return(_mpDonationService.UpdateDonationStatus(processorPaymentId, statusId, statusDate ?? DateTime.Now, statusNote));
        }

        public DonationBatchDTO CreateDonationBatch(DonationBatchDTO batch)
        {
            var batchId = _mpDonationService.CreateDonationBatch(batch.BatchName, batch.SetupDateTime, batch.BatchTotalAmount,batch.ItemCount, batch.BatchEntryType, batch.DepositId, batch.FinalizedDateTime, batch.ProcessorTransferId);

            batch.Id = batchId;

            foreach (var donation in batch.Donations)
            {
                _mpDonationService.AddDonationToBatch(batchId, int.Parse(donation.Id));
            }

            return (batch);
        }

        public DonationBatchDTO GetDonationBatchByProcessorTransferId(string processorTransferId)
        {
            return (Mapper.Map<DonationBatch, DonationBatchDTO>(_mpDonationService.GetDonationBatchByProcessorTransferId(processorTransferId)));
        }

        public DonationBatchDTO GetDonationBatch(int batchId)
        {
            return (Mapper.Map<DonationBatch, DonationBatchDTO>(_mpDonationService.GetDonationBatch(batchId)));
        }

        public DonationsDTO GetDonationsForAuthenticatedUser(string userToken, string donationYear = null, bool softCredit = false)
        {
            var donor = GetDonorForAuthenticatedUser(userToken);
            return (donor == null ? null : GetDonationsForDonor(donor, donationYear, softCredit));
        }

        public DonationYearsDTO GetDonationYearsForAuthenticatedUser(string userToken)
        {
            var donor = GetDonorForAuthenticatedUser(userToken);
            return (donor == null ? null : GetDonationYearsForDonor(donor));
        }

        public DonationsDTO GetDonationsForDonor(int donorId, string donationYear = null, bool softCredit = false)
        {
            var donor = _mpDonorService.GetEmailViaDonorId(donorId);
            return (GetDonationsForDonor(donor, donationYear, softCredit));
        }

        public DonationYearsDTO GetDonationYearsForDonor(int donorId)
        {
            var donor = _mpDonorService.GetEmailViaDonorId(donorId);
            return (GetDonationYearsForDonor(donor));
        }

        private ContactDonor GetDonorForAuthenticatedUser(string userToken)
        {
            var donor = _mpDonorService.GetContactDonor(_mpAuthenticationService.GetContactId(userToken));
            return (donor);
        }

        private DonationsDTO GetDonationsForDonor(ContactDonor donor, string donationYear = null, bool softCredit = false)
        {
            var donorIds = GetDonorIdsForDonor(donor);

            var donations = softCredit ? _mpDonorService.GetSoftCreditDonations(donorIds, donationYear) : _mpDonorService.GetDonations(donorIds, donationYear);
            if (donations == null || donations.Count == 0)
            {
                return (null);
            }

            var response = donations.Select(Mapper.Map<DonationDTO>).ToList();

            foreach (var donation in response)
            {
                StripeCharge charge = null;
                if (!string.IsNullOrWhiteSpace(donation.Source.PaymentProcessorId))
                {
                    // If it is a positive amount, it means it's a Charge, otherwise it's a Refund
                    if (donation.Amount >= 0)
                    {
                        charge = _paymentService.GetCharge(donation.Source.PaymentProcessorId);
                    }
                    else
                    {
                        donation.Status = DonationStatus.Refunded;

                        var refund = _paymentService.GetRefund(donation.Source.PaymentProcessorId);
                        if (refund != null && refund.Charge != null)
                        {
                            charge = refund.Charge;
                        }
                    }
                    
                }

                if (donation.Source.SourceType == PaymentType.Cash)
                {
                    donation.Source.Name = "cash";
                }
                else if (charge != null && charge.Source != null)
                {
                    donation.Source.AccountNumberLast4 = charge.Source.AccountNumberLast4;

                    if (donation.Source.SourceType == PaymentType.CreditCard && charge.Source.Brand != null)
                    {
                        switch (charge.Source.Brand)
                        {
                            case CardBrand.AmericanExpress:
                                donation.Source.CardType = CreditCardType.AmericanExpress;
                                break;
                            case CardBrand.Discover:
                                donation.Source.CardType = CreditCardType.Discover;
                                break;
                            case CardBrand.MasterCard:
                                donation.Source.CardType = CreditCardType.MasterCard;
                                break;
                            case CardBrand.Visa:
                                donation.Source.CardType = CreditCardType.Visa;
                                break;
                            default:
                                donation.Source.CardType = null;
                                break;
                        }
                    }
                }

                // Refund amount should already be negative (when the original donation was reversed), but negative-ify it just in case
                if (donation.Status == DonationStatus.Refunded && donation.Amount > 0)
                {
                    donation.Amount *= -1;
                    donation.Distributions.All(dist =>
                    {
                        dist.Amount *= -1;
                        return (true);
                    });
                }
            }

            var donationsResponse = new DonationsDTO();
            donationsResponse.Donations.AddRange(response.OrderBy(donation => donation.DonationDate).ToList());
            donationsResponse.BeginningDonationDate = donationsResponse.Donations.Last().DonationDate;
            donationsResponse.EndingDonationDate = donationsResponse.Donations.First().DonationDate;

            return (donationsResponse);
        }

        private DonationYearsDTO GetDonationYearsForDonor(ContactDonor donor)
        {
            var donorIds = GetDonorIdsForDonor(donor);
            var donations = _mpDonorService.GetDonations(donorIds, null);
            var softCreditDonations = _mpDonorService.GetSoftCreditDonations(donorIds);

            var years = new HashSet<string>();
            if (softCreditDonations != null && softCreditDonations.Any())
            {
                years.UnionWith(softCreditDonations.Select(d => d.donationDate.Year.ToString()));
            }
            if (donations != null && donations.Any())
            {
                years.UnionWith(donations.Select(d => d.donationDate.Year.ToString()));
            }

            var donationYears = new DonationYearsDTO();
            donationYears.AvailableDonationYears.AddRange(years.ToList());

            return (donationYears);
        }

        private IEnumerable<int> GetDonorIdsForDonor(ContactDonor donor)
        {
            var donorIds = new HashSet<int>();
            if (donor.ExistingDonor)
            {
                donorIds.Add(donor.DonorId);
            }

            if (donor.StatementTypeId != _statementTypeFamily || !donor.HasDetails)
            {
                return (donorIds);
            }

            var household = _contactService.GetHouseholdById(donor.Details.HouseholdId);
            if (household == null || household.HouseholdMembers == null || !household.HouseholdMembers.Any())
            {
                return (donorIds);
            }

            foreach (var member in household.HouseholdMembers)
            {
                if(member.StatementTypeId.HasValue && member.StatementTypeId == _statementTypeFamily && member.DonorId.HasValue)
                {
                    donorIds.Add(member.DonorId.Value);
                }
            }

            return (donorIds);
        }

        public DonationBatchDTO GetDonationBatchByDepositId(int depositId)
        {
            return (Mapper.Map<DonationBatch, DonationBatchDTO>(_mpDonationService.GetDonationBatchByDepositId(depositId)));
        }

        public List<DepositDTO> GetSelectedDonationBatches(int selectionId, string token)
        {
            var selectedDeposits = _mpDonationService.GetSelectedDonationBatches(selectionId, token);
            var deposits = new List<DepositDTO>();

            foreach (var deposit in selectedDeposits)
            {
                deposits.Add(Mapper.Map<Deposit, DepositDTO>(deposit));
            }

            return deposits;
        }

        public void ProcessDeclineEmail(string processorPaymentId)
        {
            _mpDonationService.ProcessDeclineEmail(processorPaymentId);
        }

        public DepositDTO CreateDeposit(DepositDTO deposit)
        {
            deposit.Id = _mpDonationService.CreateDeposit(deposit.DepositName, deposit.DepositTotalAmount, deposit.DepositAmount, deposit.ProcessorFeeTotal, deposit.DepositDateTime,
                deposit.AccountNumber, deposit.BatchCount, deposit.Exported, deposit.Notes, deposit.ProcessorTransferId);
            
            return (deposit);

        }

        public void CreatePaymentProcessorEventError(StripeEvent stripeEvent, StripeEventResponseDTO stripeEventResponse)
        {
            _mpDonationService.CreatePaymentProcessorEventError(stripeEvent.Created, stripeEvent.Id, stripeEvent.Type, JsonConvert.SerializeObject(stripeEvent, Formatting.Indented), JsonConvert.SerializeObject(stripeEventResponse, Formatting.Indented));
        }


        public List<GPExportDatumDTO> GetGPExport(int depositId, string token)
        {
            var gpExportData = _mpDonationService.GetGPExport(depositId, token);
            var gpExport = new List<GPExportDatumDTO>();

            foreach (var gpExportDatum in gpExportData)
            {
                gpExport.Add(Mapper.Map<GPExportDatum, GPExportDatumDTO>(gpExportDatum));
            }

            return gpExport;
        }

        public MemoryStream CreateGPExport(int selectionId, int depositId, string token)
        {
            var gpExport = GetGPExport(depositId, token);
            var stream = new MemoryStream();
            CSV.Create(gpExport, GPExportDatumDTO.Headers, stream, "\t");
            UpdateDepositToExported(selectionId, depositId, token);

            return stream;
        }

        private void UpdateDepositToExported(int selectionId, int depositId, string token)
        {
            _mpDonationService.UpdateDepositToExported(selectionId, depositId, token);
        }

        public List<DepositDTO> GenerateGPExportFileNames(int selectionId, string token)
        {
            var deposits = GetSelectedDonationBatches(selectionId, token);

            foreach (var deposit in deposits)
            {
                deposit.ExportFileName = GPExportFileName(deposit.Id);
            }

            return deposits;
        }

        public string GPExportFileName(int depositId)
        {
            var batch = GetDonationBatchByDepositId(depositId);

            var date = DateTime.Today.ToString("yyMMdd");
            var batchName = batch.BatchName.Replace(" ", "_");
            return string.Format("XRDReceivables-{0}_{1}.txt", batchName, date);
        }
    }
}
