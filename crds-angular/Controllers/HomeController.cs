using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using crds_angular.Models;

namespace crds_angular.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //Login();

            //FormsAuthentication.SetAuthCookie(user.Id, createPersistentCookie: true);
            //FormsAuthentication.SetAuthCookie("tmaddox", createPersistentCookie: true);

            //Thread.CurrentPrincipal = principal;
            //if (HttpContext.Current != null)
            //{
            //    HttpContext.Current.User = principal;
            //}

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //private void Whatever()
        //{
        //    ApplicationUser user;
        //    bool isPersistent;

        //    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
        //    var identity = UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
        //    AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        //}

        private void Login()
        {
            var username = "tmaddox";

            //           var a = new FormsAuthenticationTicket()

            //           FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
            //1,
            //user.Email,
            //DateTime.Now,
            //DateTime.Now.AddMinutes(15),
            //false, //pass here true, if you want to implement remember me functionality
            //userData);

            var authTicket = new FormsAuthenticationTicket(
                2,
                username,
                DateTime.Now,
                DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes),
                false,
                "some token that will be used to access the web service and that you have fetched"
                );

            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            Response.Cookies.Add(faCookie);



            //var authCookie = new HttpCookie(
            //    FormsAuthentication.FormsCookieName,
            //    FormsAuthentication.Encrypt(authTicket)
            //    )
            //{
            //    HttpOnly = true
            //};
            //Response.AppendCookie(authCookie);
        }
    }
}