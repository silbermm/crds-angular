﻿using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using crds_angular.test.controllers;
using log4net;
using MinistryPlatform.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace crds_angular.Controllers.API
{
    public class DonorController : MPAuth
    {
        private IDonorService gatewayDonorService;

        private static readonly ILog logger = LogManager.GetLogger(typeof(DonorController));

        public DonorController(IDonorService gatewayDonorService)
        {
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
                stripe_customer_id = donor.StripeCustomerId,
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
                var donor = gatewayDonorService.GetDonorForAuthenticatedUser(authToken);
                donor = gatewayDonorService.CreateDonor(donor, string.Empty, dto.stripe_token_id, DateTime.Now);

                var response = new DonorDTO
                {
                    id = donor.DonorId,
                    stripe_customer_id = donor.StripeCustomerId
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
                var donor = gatewayDonorService.GetDonorForAuthenticatedUser(token);

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
                var donor = gatewayDonorService.GetDonorForEmail(email);
                if (donor == null)
                {
                    return NotFound();
                }
                else
                {
                    var response = new DonorDTO
                    {
                        id = donor.DonorId,
                        stripe_customer_id = donor.StripeCustomerId
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

