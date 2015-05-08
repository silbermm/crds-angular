using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Security;
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
                        (donor.DonorId).ToString());
              

                var response = new DonationDTO()
                {
                    program_id = dto.program_id,
                    amount = dto.amount,
                    charge_id = charge_id

                };

                return Ok(response);

                }
                catch (Exception e)
                {
                    throw e;
                }

                
            });


        }
    }

    public class CreateDonationDTO  
    {
        public string program_id { get; set; }
        public int amount { get; set; }
        public string donor_id { get; set; }
    }

    public class DonationDTO
    {
        public string program_id { get; set; }
        public int amount { get; set; }
        public string charge_id { get; set; }
    }
}
