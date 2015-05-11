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
using crds_angular.Security;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using crds_angular.test.controllers;
using MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class DonationController : MPAuth
    {
        private IDonorService donorService;
        private IPaymentService stripeService;
        private IAuthenticationService authenticationService;

        public DonationController(IDonorService donorService, IPaymentService stripeService,
            IAuthenticationService authenticationService)
        {
            this.donorService = donorService;
            this.stripeService = stripeService;
            this.authenticationService = authenticationService;

        }

        [ResponseType(typeof(DonationDTO))]
        [Route("api/donation")]
        public IHttpActionResult Post([FromBody] CreateDonationDTO dto)
        {
            return Authorized(token =>
            {
                try
                {
                    var contactId = authenticationService.GetContactId(token);
                    var donor = donorService.GetDonorRecord(contactId);
                    var charge_id = stripeService.chargeCustomer(donor.StripeCustomerId, dto.amount,
                        donor.DonorId);
                    var donationId = donorService.CreateDonationAndDistributionRecord(dto.amount, donor.DonorId,
                        dto.program_id, charge_id, DateTime.Now);

                    var response = new DonationDTO()
                    {
                        program_id = dto.program_id,
                        amount = dto.amount,
                        donation_id = donationId.ToString()
                    };

                    return Ok(response);
                }
                catch (StripeException stripeException)
                {
                    var apiError = new ApiErrorDto(stripeException.Message, stripeException);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
                catch (Exception exception)
                {
                    var apiError = new ApiErrorDto("Donation Post Failed", exception);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
                
             });
        }
    }
    
}
