using SphereProcurement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Firebase.Database;
using Firebase.Database.Query;

using System.Threading.Tasks;

namespace SphereProcurement.Controllers
{
    
    [RoutePrefix("api/Supplier")]
    public class SupplierController : ApiController
    {
        public SupplierController()
        {

        }

        //method to post new supplier
        [Route("getAll")]
        public async Task<HttpResponseMessage> GetSuppliers(SupplierModel supplier)
        {


            var firebaseClient = new FirebaseClient("https://sphereprocurement.firebaseio.com");

            //Retrieve data from Firebase
            var suppliers = await firebaseClient
              .Child("Team-Awesome")
              .Child("Members")
              .OnceAsync<SupplierModel>();

            List<SupplierModel> supplierList = new List<SupplierModel>();

            foreach (var sup in suppliers)
            {
                supplierList.Add(sup.Object);
            }
            HttpResponseMessage response;
            response = Request.CreateResponse(HttpStatusCode.OK, supplierList);
            return response;
        }

        

        //post
        //valdates
        //if (!ModelState.IsValid)
        //{
        //    return BadRequest("Invalid Data");
        //}
    }
}
