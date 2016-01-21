using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads.Profile;
using crds_angular.Models.Json;
using crds_angular.Security;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models.DTO;

namespace crds_angular.Controllers.API
{
    public class LoginController : MPAuth
    {

        private readonly IPersonService _personService;
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService, IPersonService personService)
        {
            _loginService = loginService;
            _personService = personService;
        }

        [HttpPost]
        [Route("api/requestpasswordreset/")]
        public IHttpActionResult RequestPasswordReset(PasswordResetRequest request)
        {
            try
            {
                _loginService.PasswordResetRequest(request.Email);
                return this.Ok();
            }
            catch (Exception ex)
            {
                return this.InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/verifyresettoken/{token}")]
        public IHttpActionResult VerifyResetTokenRequest(string token)
        {
            try
            {
                ResetTokenStatus status = new ResetTokenStatus();
                status.TokenValid = _loginService.VerifyResetToken(token);
                return Ok(status);
            }
            catch (Exception ex)
            {
                return this.InternalServerError();
            }
        }

        [HttpPost]
        [Route("api/resetpassword/")]
        public IHttpActionResult ResetPassword(PasswordReset request)
        {
            try
            {
                var userEmail = _loginService.ResetPassword(request.Password, request.Token);
                return Ok();
            }
            catch (Exception ex)
            {
                return this.InternalServerError();
            }
        }

        [ResponseType(typeof (LoginReturn))]
        [HttpGet]
        [Route("api/authenticated")]
        public IHttpActionResult isAuthenticated()
        {
            return Authorized(token =>
            {
                try
                {
                    //var personService = new PersonService();
                    var person = _personService.GetLoggedInUserProfile(token);

                    if (person == null)
                    {
                        return this.Unauthorized();
                    }
                    else
                    {
                        var roles = _personService.GetLoggedInUserRoles(token);
                        var l = new LoginReturn(token, person.ContactId, person.FirstName, person.EmailAddress, roles);
                        return this.Ok(l);
                    }
                }
                catch (Exception)
                {
                    return this.Unauthorized();
                }
            });
        }


        [ResponseType(typeof (LoginReturn))]
        public IHttpActionResult Post([FromBody] Credentials cred)
        {
            try
            {
                // try to login 
                var authData = TranslationService.Login(cred.username, cred.password);
                var token = authData["token"].ToString();
                var exp = authData["exp"].ToString();
                var refreshToken = authData["refreshToken"].ToString();

                if (token == "")
                {
                    return this.Unauthorized();
                }

                var userRoles = _personService.GetLoggedInUserRoles(token);
                var p = _personService.GetLoggedInUserProfile(token);
                var r = new LoginReturn
                {
                    userToken = token,
                    userTokenExp = exp,
                    refreshToken = refreshToken,
                    userId = p.ContactId,
                    username = p.FirstName,
                    userEmail = p.EmailAddress,
                    roles = userRoles,
                    age = p.Age
                };

                _loginService.ClearResetToken(cred.username);

                //ttpResponseHeadersExtensions.AddCookies();

                return this.Ok(r);
            }
            catch (Exception e)
            {
                var apiError = new ApiErrorDto("Login Failed", e);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }

        [HttpPost]
        [Route("api/verifycredentials")]
        public IHttpActionResult VerifyCredentials([FromBody] Credentials cred)
        {
            return Authorized(token =>
            {
                try
                {
                    var authData = TranslationService.Login(cred.username, cred.password);

                    // if the username or password is wrong, auth data will be null
                    if (authData == null)
                    {
                        return this.Unauthorized();
                    }
                    else
                    {
                        return this.Ok();
                    } 
                }
                catch (Exception e)
                {
                    var apiError = new ApiErrorDto("Verify Credentials Failed", e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

    }

    public class LoginReturn
    {
        public LoginReturn(){}
        public LoginReturn(string userToken, int userId, string username, string userEmail, List<RoleDto> roles){
            this.userId = userId;
            this.userToken = userToken;
            this.username = username;
            this.userEmail = userEmail;
            this.roles = roles;
        }
        public string userToken { get; set; }
        public string userTokenExp { get; set; }
        public string refreshToken { get; set; }
        public int userId { get; set; }
        public string username { get; set; }
        public string userEmail { get; set;  }
        public List<RoleDto> roles { get; set; }
        public int age { get; set; }
    }

    public class Credentials
    {
        public string username { get; set; }
        public string password { get; set; }
    }

}
