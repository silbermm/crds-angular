using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Stewardship;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models;

namespace crds_angular.Controllers.API
{
    public class DonorController : MPAuth
    {
        private readonly IDonorService _gatewayDonorService;
        private readonly IPaymentService _stripePaymentService;

        public DonorController(IDonorService gatewayDonorService, IPaymentService stripePaymentService)
        {
            _gatewayDonorService = gatewayDonorService;
            _stripePaymentService = stripePaymentService;
        }

        [ResponseType(typeof(DonorDTO))]
        [Route("api/donor/{email?}")]
        public IHttpActionResult Get(string email="")
        {
            return (Authorized(GetDonorForAuthenticatedUser, () => GetDonorForUnauthenticatedUser(email)));
        }

        [ResponseType(typeof (DonorDTO))]
        [Route("api/donor")]
        public IHttpActionResult Post([FromBody] CreateDonorDTO dto)
        {
            return (Authorized(token => CreateDonorForAuthenticatedUser(token, dto), () => CreateDonorForUnauthenticatedUser(dto)));
        }

        private IHttpActionResult CreateDonorForUnauthenticatedUser(CreateDonorDTO dto)
        {
            ContactDonor donor;
            try
            {
                donor = _gatewayDonorService.GetContactDonorForEmail(dto.email_address);
            }
            catch (Exception e)
            {
                var msg = "Error getting donor for email " + dto.email_address;
                logger.Error(msg, e);
                var apiError = new ApiErrorDto(msg, e);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
            int existingDonorId = 
                (donor == null) ? 
                    0 : 
                    donor.DonorId;

            try
            {
                donor = _gatewayDonorService.CreateOrUpdateContactDonor(donor, String.Empty, dto.email_address, dto.stripe_token_id, DateTime.Now);
            }
            catch (PaymentProcessorException e)
            {
                return (e.GetStripeResult());
            }
            catch (Exception e)
            {
                var msg = "Error creating donor for email " + dto.email_address;
                logger.Error(msg, e);
                var apiError = new ApiErrorDto(msg, e);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }

            var responseBody = new DonorDTO
            {
                Id = donor.DonorId,
                ProcessorId = donor.ProcessorId,
                RegisteredUser = false,
                Email = donor.Email
            };

            // HTTP StatusCode should be 201 (Created) if we created a donor, or 200 (Ok) if returning an existing donor
            var statusCode =
                (existingDonorId == donor.DonorId) ?
                    HttpStatusCode.OK :
                    HttpStatusCode.Created;
            return (ResponseMessage(Request.CreateResponse(statusCode, responseBody)));
        }

        private IHttpActionResult CreateDonorForAuthenticatedUser(string authToken, CreateDonorDTO dto)
        {
            try
            {
                var donor = _gatewayDonorService.GetContactDonorForAuthenticatedUser(authToken);
                donor = _gatewayDonorService.CreateOrUpdateContactDonor(donor, string.Empty, string.Empty, dto.stripe_token_id, DateTime.Now);

                var response = new DonorDTO
                {
                    Id = donor.DonorId,
                    ProcessorId = donor.ProcessorId,
                    RegisteredUser = true,
                    Email = donor.Email
                };

                return Ok(response);
            }
            catch (PaymentProcessorException e)
            {
                return (e.GetStripeResult());
            }
            catch (Exception exception)
            {
                var apiError = new ApiErrorDto("Donor Post Failed", exception);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }

        private IHttpActionResult GetDonorForAuthenticatedUser(string token)
        {
            try
            {
                var donor = _gatewayDonorService.GetContactDonorForAuthenticatedUser(token);

                if (donor == null || !donor.HasPaymentProcessorRecord)
                {
                    return (NotFound());
                }
                else
                {
                    var defaultSource = _stripePaymentService.GetDefaultSource(donor.ProcessorId);
                    
                    var response = new DonorDTO()   
                    {
                        Id = donor.DonorId,
                        ProcessorId = donor.ProcessorId,
                        DefaultSource = new DefaultSourceDTO
                        {
                            credit_card = new CreditCardDTO
                            {
                              last4 = defaultSource.last4,
                              brand = defaultSource.brand,
                              address_zip = defaultSource.address_zip,
                              exp_date = defaultSource.exp_month + defaultSource.exp_year
                            },
                            bank_account = new BankAccountDTO
                            {
                              last4 = defaultSource.bank_last4,
                              routing = defaultSource.routing_number
                            }
                         },
                         RegisteredUser = donor.RegisteredUser,
                         Email = donor.Email
                    };

                    return Ok(response);
                }
            }
            catch (PaymentProcessorException stripeException)
            {
                return (stripeException.GetStripeResult());
            }
            catch (Exception exception)
            {
                var apiError = new ApiErrorDto("Donor Get Failed", exception);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }

        private IHttpActionResult GetDonorForUnauthenticatedUser(string email)
        {
            try
            {
                var donor = _gatewayDonorService.GetContactDonorForEmail(email);
                if (donor == null || !donor.HasPaymentProcessorRecord)
                {
                    return NotFound();
                }
                else
                {
                    var response = new DonorDTO
                    {
                        Id = donor.DonorId,
                        ProcessorId = donor.ProcessorId,
                        RegisteredUser = donor.RegisteredUser,
                        Email = donor.Email
                    };

                    return Ok(response); 
                }
            }
            catch (PaymentProcessorException stripeException)
            {
                return (stripeException.GetStripeResult());
            }
            catch (Exception exception)
            {
                var apiError = new ApiErrorDto("Donor Get Failed", exception);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }

        [ResponseType(typeof (DonorDTO))]
        [Route("api/donor")]
        [HttpPut]
        public IHttpActionResult UpdateDonor([FromBody] UpdateDonorDTO dto)
        {
            return (Authorized(token => UpdateDonor(token, dto), () => UpdateDonor(null, dto)));
        }

        private IHttpActionResult UpdateDonor(string token, UpdateDonorDTO dto)
        {
            ContactDonor contactDonor;
            SourceData sourceData;

            try
            {
                contactDonor = 
                    token == null ? 
                    _gatewayDonorService.GetContactDonorForEmail(dto.EmailAddress) 
                    : 
                    _gatewayDonorService.GetContactDonorForAuthenticatedUser(token);

                //Post apistripe/customer/{custID}/sources pass in the dto.stripe_token_id
                sourceData = _stripePaymentService.UpdateCustomerSource(contactDonor.ProcessorId, dto.StripeTokenId);
            }
            catch (PaymentProcessorException stripeException)
            {
                return (stripeException.GetStripeResult());
            }
            catch (ApplicationException applicationException)
            {
                var apiError = new ApiErrorDto("Error calling Ministry Platform" + applicationException.Message, applicationException);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }

            //return donor
            var donor = new DonorDTO
            {
                Id = contactDonor.DonorId,
                ProcessorId = contactDonor.ProcessorId,
                DefaultSource = new DefaultSourceDTO
                {
                    credit_card = new CreditCardDTO
                    {
                        brand = sourceData.brand,
                        last4 = sourceData.last4,
                        address_zip = sourceData.address_zip,
                        exp_date = sourceData.exp_month + sourceData.exp_year
                    },
                    bank_account = new BankAccountDTO
                    {
                        last4 = sourceData.bank_last4,
                        routing = sourceData.routing_number
                    }
                },
                RegisteredUser = contactDonor.RegisteredUser,
                Email = contactDonor.Email
            };

            return Ok(donor);
        }
    }
}

