using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using crds_angular.Exceptions.Models;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using crds_angular.test.controllers;
using MPInterfaces = MinistryPlatform.Translation.Services.Interfaces;
using crds_angular.Models.Crossroads;

namespace crds_angular.Controllers.API
{
    public class DonorController : MPAuth
    {
        private MPInterfaces.IDonorService mpDonorService;
        private IPaymentService stripeService;
        private MPInterfaces.IAuthenticationService authenticationService;

        public DonorController(MPInterfaces.IDonorService mpDonorService, IPaymentService stripeService,
            MPInterfaces.IAuthenticationService authenticationService)
        {
            this.mpDonorService = mpDonorService;
            this.stripeService = stripeService;
            this.authenticationService = authenticationService;

        }

        [ResponseType(typeof(DonorDTO))]
        [Route("api/donor")]
        public IHttpActionResult Post([FromBody] CreateDonorDTO dto)
        {
            return (Authorized(token => createDonorForAuthenticatedUser(token, dto), () => createDonorForUnauthenticatedUser(dto)));
        }

        private IHttpActionResult createDonorForUnauthenticatedUser(CreateDonorDTO dto)
        {
            // 1) Get Donor from MP
            //       contact_id, donor_id, email_address, stripe_cust_id
            // 2) If we get donor with stripe_id, return

            return (null);
        }

        private IHttpActionResult createDonorForAuthenticatedUser(String authToken, CreateDonorDTO dto)
        {
            try
            {
                var contactId = authenticationService.GetContactId(authToken);

                var customerId = stripeService.createCustomer(dto.stripe_token_id);

                var donorId = mpDonorService.CreateDonorRecord(contactId, customerId, DateTime.Now);

                var response = new DonorDTO
                {
                    id = donorId.ToString(),
                    stripe_customer_id = customerId
                };

                return Ok(response);
            }
            catch (Exception exception)
            {
                var apiError = new ApiErrorDto("Donor Post Failed", exception);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }
    }
}

