using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using crds_angular.test.controllers;
using MPInterfaces = MinistryPlatform.Translation.Services.Interfaces;
using crds_angular.Models.Crossroads;
using MinistryPlatform.Models;
using log4net;
using crds_angular.Services;

namespace crds_angular.Controllers.API
{
    public class DonorController : MPAuth
    {
        private MPInterfaces.IDonorService mpDonorService;
        private IDonorService gatewayDonorService;
        private IPaymentService stripeService;
        private MPInterfaces.IAuthenticationService authenticationService;

        private static readonly ILog logger = LogManager.GetLogger(typeof(DonorController));

        public DonorController(MPInterfaces.IDonorService mpDonorService, IPaymentService stripeService,
            MPInterfaces.IAuthenticationService authenticationService, IDonorService gatewayDonorService)
        {
            this.mpDonorService = mpDonorService;
            this.stripeService = stripeService;
            this.authenticationService = authenticationService;
            this.gatewayDonorService = gatewayDonorService;
        }

        [ResponseType(typeof(DonorDTO))]
        [Route("api/donor/{email?}")]
        public IHttpActionResult Get(string email="")
        {
            return (Authorized(token => GetDonorForAuthenticatedUser(token), () => GetDonorForUnauthenticatedUser(email)));
        }

        [ResponseType(typeof (DonorDTO))]
        [Route("api/donor")]
        public IHttpActionResult Post([FromBody] CreateDonorDTO dto)
        {
            return (Authorized(token => CreateDonorForAuthenticatedUser(token, dto), () => CreateDonorForUnauthenticatedUser(dto)));
        }

        private IHttpActionResult CreateDonorForUnauthenticatedUser(CreateDonorDTO dto)
        {
            Donor donor;
            try
            {
                donor = gatewayDonorService.GetDonorForEmail(dto.email_address);
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
                donor = gatewayDonorService.CreateDonor(donor, dto.email_address, dto.stripe_token_id, DateTime.Now);
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
                id = donor.DonorId,
                Processor_ID = donor.ProcessorId,
            };

            // HTTP StatusCode should be 201 (Created) if we created a donor, or 200 (Ok) if returning an existing donor
            var statusCode =
                (existingDonorId == donor.DonorId) ?
                    HttpStatusCode.OK :
                    HttpStatusCode.Created;
            return (ResponseMessage(Request.CreateResponse<DonorDTO>(statusCode, responseBody)));
        }

        private IHttpActionResult CreateDonorForAuthenticatedUser(String authToken, CreateDonorDTO dto)
        {
            try
            {
                var contactId = authenticationService.GetContactId(authToken);

                var customerId = stripeService.createCustomer(dto.stripe_token_id);

                var donorId = mpDonorService.CreateDonorRecord(contactId, customerId, DateTime.Now);

                var response = new DonorDTO
                {
                    id = donorId,
                    Processor_ID = customerId
                };

                return Ok(response);
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
                var contactId = authenticationService.GetContactId(token);
                var donor = mpDonorService.GetDonorRecord(contactId);

                var response = new DonorDTO
                {
                    id = donor.DonorId,
                    Processor_ID = donor.ProcessorId
                };

                return Ok(response);
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
                var donor = mpDonorService.GetPossibleGuestDonorContact(email);
                if (donor == null)
                {
                    return NotFound();
                }
                else
                {
                    var response = new DonorDTO
                    {
                        id = donor.DonorId,
                        Processor_ID = donor.ProcessorId
                    };

                    return Ok(response); 
                }
            }
            catch (Exception exception)
            {
                var apiError = new ApiErrorDto("Donor Get Failed", exception);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }
    }
}

