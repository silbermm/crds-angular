using System;
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

namespace crds_angular.Controllers.API
{
    public class DonationController : MPAuth
    {
        private readonly MPInterfaces.IDonorService _mpDonorService;
        private readonly IPaymentService _stripeService;
        private readonly MPInterfaces.IAuthenticationService _authenticationService;
        private readonly IDonorService _gatewayDonorService;
        private readonly IDonationService _gatewayDonationService;

        public DonationController(MPInterfaces.IDonorService mpDonorService, IPaymentService stripeService,
            MPInterfaces.IAuthenticationService authenticationService, IDonorService gatewayDonorService, IDonationService gatewayDonationService)
        {
            _mpDonorService = mpDonorService;
            _stripeService = stripeService;
            _authenticationService = authenticationService;
            _gatewayDonorService = gatewayDonorService;
            _gatewayDonationService = gatewayDonationService;
        }

        [ResponseType(typeof(DonationDTO))]
        [Route("api/donation")]
        public IHttpActionResult Post([FromBody] CreateDonationDTO dto)
        {
            return (Authorized(token => CreateDonationAndDistributionAuthenticated(token, dto), () => CreateDonationAndDistributionUnauthenticated(dto)));
        }

        [AcceptVerbs("GET")]
        [Route("api/gpexport/{depositId}")]
        public IHttpActionResult GetGPExportFiles(int depositId)
        {
            return Authorized(token =>
            {
                try
                {
                    // get export file and name
                    var stream = _gatewayDonationService.CreateGPExport(depositId, token);
                    var batch = _gatewayDonationService.GetDonationBatchByDepositId(depositId);
                    _gatewayDonationService.GPExportFileName(batch);
                    var contentType = MimeMapping.GetMimeMapping(batch.ExportFileName);

                    // set batch/deposite to exported
                    _gatewayDonationService.UpdateDepositToExported(depositId);

                    return new FileResult(stream, contentType);
                }
                catch (Exception ex)
                {
                    var apiError = new ApiErrorDto("GP Export File Creation Failed", ex);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        [AcceptVerbs("GET")]
        [ResponseType(typeof(DonationBatchDTO))]
        [Route("api/gpexport/filenames/{selectionId}")]
        public IHttpActionResult GetGPExportFileNames(int selectionId)
        {
            return Authorized(token =>
            {
                try
                {
                    _gatewayDonationService.GenerateGPExportFileNames(selectionId, token);
                    return Ok("");
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

                var donationId = _mpDonorService.CreateDonationAndDistributionRecord(dto.Amount, fee, donor.DonorId, dto.ProgramId, charge.Id, dto.PaymentType, donor.ProcessorId, DateTime.Now, true);
                var response = new DonationDTO()
                    {
                        program_id = dto.ProgramId,
                        amount = dto.Amount,
                        donation_id = donationId.ToString(),
                        email = donor.Email
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

                var donationId = _mpDonorService.CreateDonationAndDistributionRecord(dto.Amount, fee, donor.DonorId, dto.ProgramId, charge.Id, dto.PaymentType, donor.ProcessorId, DateTime.Now, false);

                var response = new DonationDTO()
                {
                    program_id = dto.ProgramId,
                    amount = dto.Amount,
                    donation_id = donationId.ToString(),
                    email = donor.Email
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
    }

    class FileResult : IHttpActionResult
    {
        private readonly MemoryStream _stream;
        private readonly string _contentType;

        public FileResult(MemoryStream stream, string contentType = null)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            _stream = stream;
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

                return response;

            }, cancellationToken);
        }
    }
}