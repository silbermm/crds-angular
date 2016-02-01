using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Stewardship;
using crds_angular.Models.Json;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models;
using MPInterfaces = MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class DonorController : MPAuth
    {
        private readonly IDonorService _donorService;
        private readonly IPaymentService _stripePaymentService;
        private readonly IDonationService _donationService;
        private readonly MPInterfaces.IDonorService _mpDonorService;
        private readonly MPInterfaces.IAuthenticationService _authenticationService;
        private readonly IUserImpersonationService _impersonationService;

        public DonorController(IDonorService donorService, 
                                IPaymentService stripePaymentService, 
                                IDonationService donationService, 
                                MPInterfaces.IDonorService mpDonorService, 
                                MPInterfaces.IAuthenticationService authenticationService,
                                IUserImpersonationService impersonationService)
        {
            _donorService = donorService;
            _stripePaymentService = stripePaymentService;
            _donationService = donationService;
            _authenticationService = authenticationService;
            _mpDonorService = mpDonorService;
            _impersonationService = impersonationService;
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
                donor = _donorService.GetContactDonorForEmail(dto.email_address);
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
                donor = _donorService.CreateOrUpdateContactDonor(donor, String.Empty, dto.email_address, dto.stripe_token_id, DateTime.Now);
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
                var donor = _donorService.GetContactDonorForAuthenticatedUser(authToken);
                donor = _donorService.CreateOrUpdateContactDonor(donor, string.Empty, string.Empty, dto.stripe_token_id, DateTime.Now);

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
                var donor = _donorService.GetContactDonorForAuthenticatedUser(token);

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
                var donor = _donorService.GetContactDonorForEmail(email);
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
                    _donorService.GetContactDonorForEmail(dto.EmailAddress) 
                    : 
                    _donorService.GetContactDonorForAuthenticatedUser(token);
              
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

        /// <summary>
        /// Create a recurring gift for the authenticated user.
        /// </summary>
        /// <param name="recurringGiftDto">The data required to setup the recurring gift in MinistryPlatform and Stripe.</param>
        /// <param name="impersonateDonorId">An optional donorId of a donor to impersonate</param>
        /// <returns>The input RecurringGiftDto, with donor email address and recurring gift ID from MinistryPlatform populated</returns>
        [RequiresAuthorization]
        [ResponseType(typeof(RecurringGiftDto))]
        [HttpPost]
        [Route("api/donor/recurrence")]
        public IHttpActionResult CreateRecurringGift([FromBody] RecurringGiftDto recurringGiftDto, [FromUri(Name = "impersonateDonorId")] int? impersonateDonorId = null)
        {
            return (Authorized(token =>
            {
                var impersonateUserId = impersonateDonorId == null ? string.Empty : _mpDonorService.GetEmailViaDonorId(impersonateDonorId.Value).Email;

                try
                {
                    var contactDonor = (impersonateDonorId != null)
                        ? _impersonationService.WithImpersonation(token,
                                                                  impersonateUserId,
                                                                  () =>
                                                                      _donorService.GetContactDonorForAuthenticatedUser(token))
                        : _donorService.GetContactDonorForAuthenticatedUser(token);
                    var donor = _donorService.CreateOrUpdateContactDonor(contactDonor, string.Empty, string.Empty);
                    var recurringGift = !string.IsNullOrWhiteSpace(impersonateUserId)
                        ? _impersonationService.WithImpersonation(token,
                                                                  impersonateUserId,
                                                                  () =>
                                                                      _donorService.CreateRecurringGift(token, recurringGiftDto, donor))
                        : _donorService.CreateRecurringGift(token, recurringGiftDto, donor);

                    recurringGiftDto.EmailAddress = donor.Email;
                    recurringGiftDto.RecurringGiftId = recurringGift;
                    return Ok(recurringGiftDto);
                }
                catch (PaymentProcessorException stripeException)
                {
                    return (stripeException.GetStripeResult());
                }
                catch (ApplicationException applicationException)
                {
                    var apiError = new ApiErrorDto("Error calling Ministry Platform " + applicationException.Message, applicationException);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
                
            }));
        }

        /// <summary>
        /// Edit a recurring gift for the authenticated user.
        /// </summary>
        /// <param name="recurringGiftId">The ID of the recurring gift to edit</param>
        /// <param name="editGift">The data required to edit the recurring gift in MinistryPlatform and/or Stripe.</param>
        /// <param name="impersonateDonorId">An optional donorId of a donor to impersonate</param>
        /// <returns>The input RecurringGiftDto, with donor email address and recurring gift ID from MinistryPlatform populated</returns>
        [RequiresAuthorization]
        [ResponseType(typeof(RecurringGiftDto))]
        [HttpPut]
        [Route("api/donor/recurrence/{recurringGiftId:int}")]
        public IHttpActionResult EditRecurringGift([FromUri]int recurringGiftId, [FromBody] RecurringGiftDto editGift, [FromUri(Name = "impersonateDonorId")] int? impersonateDonorId = null)
        {
            editGift.RecurringGiftId = recurringGiftId;

            return (Authorized(token =>
            {
                var impersonateUserId = impersonateDonorId == null ? string.Empty : _mpDonorService.GetEmailViaDonorId(impersonateDonorId.Value).Email;

                try
                {
                    var donor = (impersonateDonorId != null)
                        ? _impersonationService.WithImpersonation(token,
                                                                  impersonateUserId,
                                                                  () =>
                                                                      _donorService.GetContactDonorForAuthenticatedUser(token))
                        : _donorService.GetContactDonorForAuthenticatedUser(token);

                    var recurringGift = !string.IsNullOrWhiteSpace(impersonateUserId)
                        ? _impersonationService.WithImpersonation(token,
                                                                  impersonateUserId,
                                                                  () =>
                                                                      _donorService.EditRecurringGift(token, editGift, donor))
                        : _donorService.EditRecurringGift(token, editGift, donor);

                    return Ok(recurringGift);
                }
                catch (PaymentProcessorException stripeException)
                {
                    return (stripeException.GetStripeResult());
                }
                catch (ApplicationException applicationException)
                {
                    var apiError = new ApiErrorDto("Error calling Ministry Platform " + applicationException.Message, applicationException);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }

            }));
        }

        /// <summary>
        /// Cancel a recurring gift for the authenticated user.  This simply end-dates the gift, and cancels the subscription at Stripe.
        /// </summary>
        /// <param name="recurringGiftId">The recurring gift ID to delete in MinistryPlatform and Stripe.</param>
        /// <param name="impersonateDonorId">An optional donorId of a donor to impersonate</param>
        /// <returns>The RecurringGiftDto representing the gift that was deleted</returns>
        [RequiresAuthorization]
        [HttpDelete]
        [Route("api/donor/recurrence/{recurringGiftId:int}")]
        public IHttpActionResult CancelRecurringGift([FromUri]int recurringGiftId, [FromUri(Name = "impersonateDonorId")] int? impersonateDonorId = null)
        {
            return(Authorized(token =>
            {

                try
                {
                    var impersonateUserId = impersonateDonorId == null ? string.Empty : _mpDonorService.GetEmailViaDonorId(impersonateDonorId.Value).Email;

                    var result = (impersonateDonorId != null)
                        ? _impersonationService.WithImpersonation(token,
                                                                  impersonateUserId,
                                                                  () =>
                                                                      _donorService.CancelRecurringGift(token, recurringGiftId))
                        : _donorService.CancelRecurringGift(token, recurringGiftId);

                    return (Ok());
                }
                catch (PaymentProcessorException stripeException)
                {
                    return (stripeException.GetStripeResult());
                }
                catch (ApplicationException applicationException)
                {
                    var apiError = new ApiErrorDto("Error calling Ministry Platform " + applicationException.Message, applicationException);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            }));
        }


        /// <summary>
        /// Retrieve list of recurring gifts for the logged-in donor.
        /// </summary>
        /// <param name="impersonateDonorId">An optional donorId of a donor to impersonate</param>
        /// <returns>A list of RecurringGiftDto</returns>
       // [RequiresAuthorization]
        [Route("api/donor/recurrence")]
        [ResponseType(typeof(List<RecurringGiftDto>))]
        [HttpGet]
        public IHttpActionResult GetRecurringGifts([FromUri(Name = "impersonateDonorId")] int? impersonateDonorId = null)
        {
            return (Authorized(token =>
            {
                var impersonateUserId = impersonateDonorId == null ? string.Empty : _mpDonorService.GetEmailViaDonorId(impersonateDonorId.Value).Email;
                
                try
                {
                    var recurringGifts = (impersonateDonorId != null)
                        ? _impersonationService.WithImpersonation(token,
                                                                  impersonateUserId,
                                                                  () =>
                                                                      _donorService.GetRecurringGiftsForAuthenticatedUser(token)) 
                        : _donorService.GetRecurringGiftsForAuthenticatedUser(token);

                    if (recurringGifts == null || !recurringGifts.Any())
                    {
                        return (RestHttpActionResult<ApiErrorDto>.WithStatus(HttpStatusCode.NotFound, new ApiErrorDto("No matching donations found")));
                    }

                    return (Ok(recurringGifts));
                }
                catch (UserImpersonationException e)
                {
                    return (e.GetRestHttpActionResult());
                }
            }));
        }

        /// <summary>
        /// Retrieve list of capital campaign pledges for the logged-in donor.
        /// </summary>
        /// <returns>A list of PledgeDto</returns>
        [Route("api/donor/pledge")]
        [ResponseType(typeof(List<PledgeDto>))]
        [HttpGet]
        public IHttpActionResult GetPledges()
        {
            return (Authorized(token =>
            {
                try
                {
                    var pledges = _donorService.GetCapitalCampaignPledgesForAuthenticatedUser(token);

                    if (pledges == null || !pledges.Any())
                    {
                        return (RestHttpActionResult<ApiErrorDto>.WithStatus(HttpStatusCode.NotFound, new ApiErrorDto("No matching commitments found")));
                    }

                    return (Ok(pledges));
                }
                catch (UserImpersonationException e)
                {
                    return (e.GetRestHttpActionResult());
                }
            }));
        }
    }
}

