using ChamsICSResidencyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ChamsICSResidencyAPI.Controllers
{
    public class MemberController : ApiController
    {
        // GET: Member
        public HttpResponseMessage Index()
        {
            return Request.CreateResponse<string>(HttpStatusCode.OK, "welcome");
        }

        [HttpGet]
        public HttpResponseMessage FindByID(int id)
        {
            Member member = new Member { Address = "10, Adetoun Close, Off Ahmadu Bello", FirstName = "Timilehin", MembershipNo = "08/13320", LastName = "Ogunseye", MiddleName = "Mayowa", Sex = "Male" };
            if (member != null)
            {
                return Request.CreateResponse<Member>(HttpStatusCode.Created, member);
            }
            else
            {
                return Request.CreateResponse<Member>(HttpStatusCode.NotFound, null);
            }

        }

        [HttpGet]
        public HttpResponseMessage FindByOthers(string search)
        {
            Member member = new Member { Address = "10, Adetoun Close, Off Ahmadu Bello", FirstName = "Timilehin", MembershipNo = "08/13320", LastName = "Ogunseye", MiddleName = "Mayowa", Sex = "Male" };
            if (member != null)
            {
                return Request.CreateResponse<Member>(HttpStatusCode.Created, member);
            }
            else
            {
                return Request.CreateResponse<Member>(HttpStatusCode.NotFound, null);
            }

        }
    }
}