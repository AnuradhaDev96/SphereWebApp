using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SphereProcurement.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("db/Supplier")]
    public class SupplierNewController : ApiController
    {
        
        [Route("getAllSups")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {
                
                try
                {
                    var suppliers = dbContext.suppliers.SqlQuery(@"select * from suppliers").ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, new {statusCode= HttpStatusCode.OK, data = suppliers}); 
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new {statusCode= HttpStatusCode.NotFound, message = e.Message });
                }                
            }
        }

        [Route("addSups")]
        [HttpPost]
        public HttpResponseMessage PostSupplier(supplier supplier)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    var suppliers = dbContext.suppliers.Add(supplier);
                    dbContext.SaveChanges();
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, message = "Supplier Added Succesfully"});
                    return response;
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound,  message = e.Message }); ;
                }

            }
        }


    }
}
