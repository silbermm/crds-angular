using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads.Stewardship;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using MPInterfaces = MinistryPlatform.Translation.Services.Interfaces;
using System.Threading.Tasks;
using System.Web;
using crds_angular.Models.Json;
using Microsoft.Ajax.Utilities;

namespace crds_angular.Controllers.API
{
    public class DonationController : MPAuth
    {
        private readonly MPInterfaces.IDonorService _mpDonorService;
        private readonly IPaymentService _stripeService;
        private readonly MPInterfaces.IAuthenticationService _authenticationService;
        private readonly IDonorService _gatewayDonorService;
        private readonly IDonationService _gatewayDonationService;
        private readonly IUserImpersonationService _impersonationService;
        private readonly MPInterfaces.IDonationService _mpDonationService;
        private readonly MPInterfaces.IPledgeService _mpPledgeService;

        public DonationController(MPInterfaces.IDonorService mpDonorService, IPaymentService stripeService,
            MPInterfaces.IAuthenticationService authenticationService, IDonorService gatewayDonorService, IDonationService gatewayDonationService, MPInterfaces.IDonationService mpDonationService, MPInterfaces.IPledgeService mpPledgeService, IUserImpersonationService impersonationService)
        {
            _mpDonorService = mpDonorService;
            _stripeService = stripeService;
            _authenticationService = authenticationService;
            _gatewayDonorService = gatewayDonorService;
            _gatewayDonationService = gatewayDonationService;
            _impersonationService = impersonationService;
            _mpDonationService = mpDonationService;
            _mpPledgeService = mpPledgeService;
        }

        /// <summary>
        /// Retrieve list of donations for the logged-in donor, optionally for the specified year, and optionally returns only soft credit donations (by default returns only direct gifts).
        /// </summary>
        /// <param name="softCredit">A bool indicating if the result should contain only soft-credit (true), only direct (false), or all (null) donations.  Defaults to null.</param>
        /// <param name="donationYear">A year filter (YYYY format) for donations returned - defaults to null, meaning return all available donations regardless of year.</param>
        /// <param name="impersonateDonorId">An optional donorId of a donor to impersonate</param>
        /// <param name="limit">A limit of donations to return starting at the most resent - defaults to null, meaning return all available donations with no limit.</param>
        /// <returns>A list of DonationDTOs</returns>
        [Route("api/donations/{donationYear:regex(\\d{4})?}")]
        [HttpGet]
        public IHttpActionResult GetDonations(string donationYear = null, int? limit = null, [FromUri(Name = "softCredit")]bool? softCredit = null, [FromUri(Name = "impersonateDonorId")]int? impersonateDonorId = null)
        {
            return (Authorized(token =>
            {
                var impersonateUserId = impersonateDonorId == null ? string.Empty : _mpDonorService.GetEmailViaDonorId(impersonateDonorId.Value).Email;
                try
                {
                    var donations = !string.IsNullOrWhiteSpace(impersonateUserId)
                        ? _impersonationService.WithImpersonation(token,
                                                                  impersonateUserId,
                                                                  () =>
                                                                      _gatewayDonationService.GetDonationsForAuthenticatedUser(token, donationYear, limit, softCredit))
                        : _gatewayDonationService.GetDonationsForAuthenticatedUser(token, donationYear, limit, softCredit);
                    if (donations == null || !donations.HasDonations)
                    {
                        return (RestHttpActionResult<ApiErrorDto>.WithStatus(HttpStatusCode.NotFound, new ApiErrorDto("No matching donations found")));
                    }

                    return (Ok(donations));
                }
                catch (UserImpersonationException e)
                {
                    return (e.GetRestHttpActionResult());
                }
            }));
        }

        /// <summary>
        /// Retrieve a list of donation years for the logged-in donor.  This includes any year the donor has given either directly, or via soft-credit.
        /// </summary>
        /// <param name="impersonateDonorId">An optional donorId of a donor to impersonate</param>
        /// <returns>A list of years (string)</returns>
        [Route("api/donations/years")]
        [HttpGet]
        public IHttpActionResult GetDonationYears([FromUri(Name = "impersonateDonorId")]int? impersonateDonorId = null)
        {
            return (Authorized(token =>
            {
                var impersonateUserId = impersonateDonorId == null ? string.Empty : _mpDonorService.GetEmailViaDonorId(impersonateDonorId.Value).Email;
                try
                {
                    var donationYears = !string.IsNullOrWhiteSpace(impersonateUserId)
                        ? _impersonationService.WithImpersonation(token, impersonateUserId, () => _gatewayDonationService.GetDonationYearsForAuthenticatedUser(token))
                        : _gatewayDonationService.GetDonationYearsForAuthenticatedUser(token);

                    if (donationYears == null || !donationYears.HasYears)
                    {
                        return (RestHttpActionResult<ApiErrorDto>.WithStatus(HttpStatusCode.NotFound, new ApiErrorDto("No donation years found")));
                    }

                    return (Ok(donationYears));
                }
                catch (UserImpersonationException e)
                {
                    return (e.GetRestHttpActionResult());
                }
            }));
        }

        [ResponseType(typeof(DonationDTO))]
        [Route("api/donation")]
        public IHttpActionResult Post([FromBody] CreateDonationDTO dto)
        {
            return (Authorized(token => 
                CreateDonationAndDistributionAuthenticated(token, dto), 
                () => CreateDonationAndDistributionUnauthenticated(dto)));
        }

        [Route("api/gpexport/file/{selectionId}/{depositId}")]
        [HttpGet]
        public IHttpActionResult GetGPExportFile(int selectionId, int depositId)
        {
            return Authorized(token =>
            {
                try
                {
                    // get export file and name
                    var fileName = _gatewayDonationService.GPExportFileName(depositId);
                    var stream = _gatewayDonationService.CreateGPExport(selectionId, depositId, token);
                    var contentType = MimeMapping.GetMimeMapping(fileName);

                    return new FileResult(stream, fileName, contentType);
                }
                catch (Exception ex)
                {
                    var apiError = new ApiErrorDto("GP Export File Creation Failed", ex);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        [ResponseType(typeof(List<DepositDTO>))]
        [Route("api/gpexport/filenames/{selectionId}")]
        [HttpGet]
        public IHttpActionResult GetGPExportFileNames(int selectionId)
        {
            return Authorized(token =>
            {
                try
                {
                    var batches = _gatewayDonationService.GenerateGPExportFileNames(selectionId, token);
                    return Ok(batches);
                }
                catch (Exception ex)
                {
                    var apiError = new ApiErrorDto("Getting GP Export File Names Failed", ex);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        private IHttpActionResult CreateDonationAndDistributionAuthenticated(String token, CreateDonationDTO dto)
        {
            try{
                var contactId = _authenticationService.GetContactId(token);
                var donor = _mpDonorService.GetContactDonor(contactId);
                var charge = _stripeService.ChargeCustomer(donor.ProcessorId, dto.Amount, donor.DonorId);
                var fee = charge.BalanceTransaction != null ? charge.BalanceTransaction.Fee : null;

                int? pledgeId = null;
                if (dto.PledgeCampaignId != null && dto.PledgeDonorId != null)
                {
                  pledgeId  = _mpPledgeService.GetPledgeByCampaignAndDonor(dto.PledgeCampaignId.Value, dto.PledgeDonorId.Value);
                }

                var donationId = _mpDonorService.CreateDonationAndDistributionRecord(dto.Amount, fee, donor.DonorId, dto.ProgramId, pledgeId, charge.Id, dto.PaymentType, donor.ProcessorId, DateTime.Now, true);
                if (!dto.GiftMessage.IsNullOrWhiteSpace() && pledgeId != null)
                {
                    SendMessageFromDonor(pledgeId.Value, dto.GiftMessage);
                }
                var response = new DonationDTO
                {
                        ProgramId = dto.ProgramId,
                        Amount = dto.Amount,
                        Id = donationId.ToString(),
                        Email = donor.Email
                    };

                    return Ok(response);
                }
                catch (PaymentProcessorException stripeException)
                {
                    return (stripeException.GetStripeResult());
                }
                catch (Exception exception)
                {
                    var apiError = new ApiErrorDto("Donation Post Failed", exception);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }       
        }

        private IHttpActionResult CreateDonationAndDistributionUnauthenticated(CreateDonationDTO dto)
        {
            try
            {
                var donor = _gatewayDonorService.GetContactDonorForEmail(dto.EmailAddress);
                var charge = _stripeService.ChargeCustomer(donor.ProcessorId, dto.Amount, donor.DonorId);
                var fee = charge.BalanceTransaction != null ? charge.BalanceTransaction.Fee : null;
                int? pledgeId = null;
                if (dto.PledgeCampaignId != null && dto.PledgeDonorId != null)
                {
                    pledgeId = _mpPledgeService.GetPledgeByCampaignAndDonor(dto.PledgeCampaignId.Value, dto.PledgeDonorId.Value);
                }

                var donationId = _mpDonorService.CreateDonationAndDistributionRecord(dto.Amount, fee, donor.DonorId, dto.ProgramId, pledgeId, charge.Id, dto.PaymentType, donor.ProcessorId, DateTime.Now, false);
                if (!dto.GiftMessage.IsNullOrWhiteSpace() && pledgeId != null)
                {
                    SendMessageFromDonor(pledgeId.Value, dto.GiftMessage);
                }

                var response = new DonationDTO()
                {
                    ProgramId = dto.ProgramId,
                    Amount = dto.Amount,
                    Id = donationId.ToString(),
                    Email = donor.Email
                };

                return Ok(response);
            }
            catch (PaymentProcessorException stripeException)
            {
                return (stripeException.GetStripeResult());
            }
            catch (Exception exception)
            {
                var apiError = new ApiErrorDto("Donation Post Failed", exception);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }       
        }

        private void SendMessageFromDonor(int pledgeId, string message)
        {
            _mpDonationService.SendMessageFromDonor(pledgeId, message);
        }
    }

    class FileResult : IHttpActionResult
    {
        private readonly MemoryStream _stream;
        private readonly string _fileName;
        private readonly string _contentType;

        public FileResult(MemoryStream stream, string fileName, string contentType = null)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            _stream = stream;
            _fileName = fileName;
            _contentType = contentType;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(_stream)
                };

                var contentType = _contentType;
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = _fileName
                };

                return response;

            }, cancellationToken);
        }
    }
}
