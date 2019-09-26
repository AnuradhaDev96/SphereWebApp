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
        public HttpResponseMessage PostSupplier([FromBody]supplier supplier)
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

        [Route("deleteSups/{id}")]
        [HttpDelete]
        public HttpResponseMessage DeleteSupplier([FromUri]string id)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    supplier supplierObj = dbContext.suppliers.Find(id);
                    if(supplierObj == null)
                    {                        
                        response = Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = "Supplier cannot be found" });
                    }
                    else
                    {
                        dbContext.suppliers.Remove(supplierObj);
                        dbContext.SaveChanges();
                        response = Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, message = "Supplier deleted successfully" });
                    }
                    
                    return response;
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = e.Message }); ;
                }

            }
        }

        [Route("getSupplierById/{id}")]
        [HttpGet]
        public HttpResponseMessage GetSupplierById([FromUri]string id)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    supplier supplierObj = dbContext.suppliers.Find(id);
                    if (supplierObj == null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = "Supplier cannot be found" });
                    }
                    else
                    {                        
                        response = Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, data = supplierObj });
                    }

                    return response;
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = e.Message }); ;
                }

            }
        }

        [Route("editSupplierById/{id}")]
        [HttpPut]
        public HttpResponseMessage EditSupplierById([FromUri]string id, [FromBody]supplier supplier)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    supplier supplierObj = dbContext.suppliers.Find(id);
                    if (supplierObj == null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = "Supplier cannot be found" });
                    }
                    else
                    {
                        supplierObj.supplierId = supplier.supplierId;
                        supplierObj.name = supplier.name;
                        supplierObj.address = supplier.address;
                        supplierObj.contactNo = supplier.contactNo;
                        supplierObj.email = supplier.email;
                        dbContext.Entry(supplierObj).State = System.Data.Entity.EntityState.Modified;
                        dbContext.SaveChanges();
                        response = Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, message = "Supplier edited successfully" });
                    }

                    return response;
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = e.Message }); ;
                }

            }
        }
    }
}
