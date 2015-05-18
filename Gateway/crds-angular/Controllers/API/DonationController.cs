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
using MPInterfaces = MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class DonationController : MPAuth
    {
        private MPInterfaces.IDonorService mpDonorService;
        private IPaymentService stripeService;
        private MPInterfaces.IAuthenticationService authenticationService;
        private IDonorService gatewayDonorService;

        public DonationController(MPInterfaces.IDonorService mpDonorService, IPaymentService stripeService,
            MPInterfaces.IAuthenticationService authenticationService, IDonorService gatewayDonorService)
        {
            this.mpDonorService = mpDonorService;
            this.stripeService = stripeService;
            this.authenticationService = authenticationService;
            this.gatewayDonorService = gatewayDonorService;

        }

        [ResponseType(typeof(DonationDTO))]
        [Route("api/donation")]
        public IHttpActionResult Post([FromBody] CreateDonationDTO dto)
        {
            return (Authorized(token => CreateDonationAndDistributionAuthenticated(token, dto), () => CreateDonationAndDistributionUnauthenticated(dto)));
        }

        private IHttpActionResult CreateDonationAndDistributionAuthenticated(String token, CreateDonationDTO dto)
        {
            try{
                var contactId = authenticationService.GetContactId(token);
                var donor = mpDonorService.GetContactDonor(contactId);
                var charge_id = stripeService.chargeCustomer(donor.ProcessorId, dto.amount,donor.DonorId);
                var donationId = mpDonorService.CreateDonationAndDistributionRecord(dto.amount, donor.DonorId, dto.program_id, charge_id, DateTime.Now, true);
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
        }

        private IHttpActionResult CreateDonationAndDistributionUnauthenticated(CreateDonationDTO dto)
        {
            try
            {
                var donor = gatewayDonorService.GetContactDonorForEmail(dto.email_address);
                var charge_id = stripeService.chargeCustomer(donor.ProcessorId, dto.amount, donor.DonorId);
                var donationId = mpDonorService.CreateDonationAndDistributionRecord(dto.amount, donor.DonorId, dto.program_id, charge_id, DateTime.Now, false);

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
        }
        
    }
    
}
