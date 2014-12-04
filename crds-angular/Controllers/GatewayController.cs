using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Security;
using System.Web.SessionState;

namespace crds_angular.Controllers
{
    public class GatewayController : ApiController
    {
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        this.db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //[ResponseType(typeof(Profile))]
        //public  IHttpActionResult Get(int id)
        //{
        //    if (id == 1)
        //    {
        //        return this.NotFound();}

        //    var p =  new Profile()
        //    {
        //        Email = "tony.maddox@ingagepartners.com"
        //    };
        //    return this.Ok(p);
        //}

        [ResponseType(typeof(Dictionary<string, object>))]
        public IHttpActionResult Get(int id)
        {
            if (id == 1)
            {
                return this.NotFound();
            }

            var d = new Dictionary<string, object>();
            d.Add("cat", 2);
            d.Add("Dog", "fido");
            d.Add("llama", 0);
            d.Add("iguana", -1);

            var p = new Profile()
            {
                Email = "tony.maddox@ingagepartners.com",
                FirstName = "Tony"

            };
            return this.Ok(p);
        }


        //private async Task<Profile> GetProfile()
        //{
        //    var p = new Profile()
        //    {
        //        Email = "tony.maddox@ingagepartners.com"
        //    };
        //    return this.NotFound();

        //}
    }

    public class Profile
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string MaidenName { get; set; }
        public string NickName { get; set; }
        public string MobilePhone { get; set; }
        public string ServiceProvider { get; set; }
        public string BirthDate { get; set; }
        public string MaritalStatus { get; set; }
        public string Gender { get; set; }
        public string Employer { get; set; }
        public string CrossroadsStartDate { get; set; }
    }
}