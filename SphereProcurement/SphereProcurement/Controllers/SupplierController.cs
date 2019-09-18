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
using Newtonsoft.Json;

namespace SphereProcurement.Controllers
{
    
    [RoutePrefix("api/Supplier")]
    public class SupplierController : ApiController
    {
        public SupplierController()
        {

        }

        [Route("getAllSups")]
        [HttpGet]
        public IEnumerable<supplier> Get()
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {
                return dbContext.suppliers.ToList();
            }
        }

        //method to get all suppliers
        [Route("getAll")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetSuppliers()
        {


            var firebaseClient = new FirebaseClient("https://sphereprocurement.firebaseio.com");

            //Retrieve data from Firebase
            var suppliers = await firebaseClient
              .Child("suppliers")
              .OnceAsync<SupplierModel>();

            //List<SupplierModel> supplierList = new List<SupplierModel>();

            //foreach (var sup in suppliers)
            //{
            //    supplierList.Add(sup.Object);
            //}
            HttpResponseMessage response;
            response = Request.CreateResponse(HttpStatusCode.OK, suppliers);
            return response;
        }

        //POST new supplier
        [Route("addSuppllier")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddSupplier([FromBody] SupplierModel supplier) {

            var firebaseClient = new FirebaseClient("https://sphereprocurement.firebaseio.com");
            var addsupplier = await firebaseClient
                .Child("suppliers")
                .PostAsync(supplier, false);
            HttpResponseMessage response;
            response = Request.CreateResponse(HttpStatusCode.OK, addsupplier);
            return response;
        }

        //GET get suppliers filtering by id
        [Route("getSupplierById/{supplierId}")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetSupplierById([FromUri] string supplierId)
        {

            var firebaseClient = new FirebaseClient("https://sphereprocurement.firebaseio.com");

            //Retrieve data from Firebase
            SupplierModel suppliers = await firebaseClient
              .Child("suppliers/" + supplierId)
              .OnceSingleAsync<SupplierModel>();

            //List<SupplierModel> supplierList = new List<SupplierModel>();
            //supplierList.Add(suppliers);
            //var res = JsonConvert.SerializeObject(suppliers);

            //foreach (var sup in suppliers)
            //{
            //    supplierList.Add(sup.Object);
            //}
            HttpResponseMessage response;
            response = Request.CreateResponse(HttpStatusCode.OK, suppliers);
            return response;
        }

        //PUT - update suppliers
        [Route("updateSuppllier/{supplierId}")]
        [HttpPut]
        public async Task<HttpResponseMessage> UpdateSupplier([FromUri] string supplierId, [FromBody] SupplierModel supplier)
        {

            var firebaseClient = new FirebaseClient("https://sphereprocurement.firebaseio.com");
            var addsupplier = firebaseClient
                .Child("suppliers")
                .Child(supplierId)                
                .PutAsync<SupplierModel>(supplier);
            HttpResponseMessage response;
            response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        [Route("deleteSuppllier/{supplierId}")]
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteSupplier([FromUri] string supplierId)
        {

            var firebaseClient = new FirebaseClient("https://sphereprocurement.firebaseio.com");
            var addsupplier = firebaseClient
                .Child("suppliers")
                .Child(supplierId)
                .DeleteAsync();
            HttpResponseMessage response;
            response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }


    }
}
