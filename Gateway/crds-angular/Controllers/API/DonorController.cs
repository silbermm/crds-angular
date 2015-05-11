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
using MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class DonorController : MPAuth
    {
        private IDonorService donorService;
        private IPaymentService stripeService;
        private IAuthenticationService authenticationService;

        public DonorController(IDonorService donorService, IPaymentService stripeService,
            IAuthenticationService authenticationService)
        {
            this.donorService = donorService;
            this.stripeService = stripeService;
            this.authenticationService = authenticationService;

        }

        [ResponseType(typeof (DonorDTO))]
        [Route("api/donor")]
        public IHttpActionResult Get(string email="")
        {
            return (Authorized(token => GetDonorForAuthenticatedUser(token), () => GetDonorForUnauthenticatedUser(email)));
        }

        [ResponseType(typeof (DonorDTO))]
        [Route("api/donor")]
        public IHttpActionResult Post([FromBody] CreateDonorDTO dto)
        {
            return Authorized(token =>
            {
                try
                {
                    var contactId = authenticationService.GetContactId(token);

                    var customerId = stripeService.createCustomer(dto.stripe_token_id);

                    var donorId = donorService.CreateDonorRecord(contactId, customerId, DateTime.Now);

                    var response = new DonorDTO
                    {
                        id = donorId,
                        stripe_customer_id = customerId
                    };

                    return Ok(response);
                }
                catch (Exception exception)
                {
                    var apiError = new ApiErrorDto("Donor Post Failed", exception);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });


        }

        private IHttpActionResult GetDonorForAuthenticatedUser(string token)
        {
            try
            {
                var contactId = authenticationService.GetContactId(token);
                var donor = donorService.GetDonorRecord(contactId);

                var response = new DonorDTO
                {
                    id = donor.DonorId,
                    stripe_customer_id = donor.StripeCustomerId
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
                var donor = donorService.GetPossibleGuestDonorContact(email);

                var response = new DonorDTO
                {
                    id = donor.DonorId,
                    stripe_customer_id = donor.StripeCustomerId
                };

                return Ok(response);
            }
            catch (Exception exception)
            {
                var apiError = new ApiErrorDto("Donor Get Failed", exception);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }
    }
}

