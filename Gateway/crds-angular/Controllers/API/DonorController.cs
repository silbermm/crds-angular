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
        private IDonorService gatewayDonorService;
        private IPaymentService stripeService;
        private MPInterfaces.IAuthenticationService authenticationService;

        public DonorController(MPInterfaces.IDonorService mpDonorService, IPaymentService stripeService,
            MPInterfaces.IAuthenticationService authenticationService, IDonorService gatewayDonorService)
        {
            this.mpDonorService = mpDonorService;
            this.stripeService = stripeService;
            this.authenticationService = authenticationService;
            this.gatewayDonorService = gatewayDonorService;
        }

        [ResponseType(typeof(DonorDTO))]
        [Route("api/donor")]
        public IHttpActionResult Post([FromBody] CreateDonorDTO dto)
        {
            return (Authorized(token => createDonorForAuthenticatedUser(token, dto), () => createDonorForUnauthenticatedUser(dto)));
        }

        private IHttpActionResult createDonorForUnauthenticatedUser(CreateDonorDTO dto)
        {
            var donor = gatewayDonorService.getDonorForEmail(dto.email_address);

            donor = gatewayDonorService.createDonor(donor, dto.email_address, dto.stripe_token_id, DateTime.Now);

            var response = new DonorDTO
            {
                id = donor.DonorId.ToString(),
                stripe_customer_id = donor.StripeCustomerId,
            };

            return Ok(response);
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

