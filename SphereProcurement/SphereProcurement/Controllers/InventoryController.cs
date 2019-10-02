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
    [RoutePrefix("db/Inventory")]
    public class InventoryController : ApiController
    {
        [Route("addNewItem")]
        [HttpPost]
        public HttpResponseMessage PostSupplier([FromBody]inventory item)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    if (item.currentStock > item.maxStock)
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = "Stock Exceeds and Item cannot be Saved" });
                    }
                    else {
                        var result = dbContext.inventories.Add(item);
                        dbContext.SaveChanges();
                        response = Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, message = "Item Added into Inventory Succesfully" });
                    }
                    
                    return response;
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = e.Message });
                }

            }
        }

        [Route("getAllInventoryItems")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {
                try
                {
                    var inventItems = dbContext.inventories.SqlQuery(@"select * from inventory").ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, data = inventItems });
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = e.Message });
                }
            }
        }

    }
}
