using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SphereProcurement.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("db/SupItems")]
    public class SupplierItemController : ApiController
    {
        [Route("addItems")]
        [HttpPost]
        public HttpResponseMessage PostSupplier([FromBody]supplier_items items)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    var result = dbContext.supplier_items.Add(items);
                    dbContext.SaveChanges();
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, message = "Items Added Succesfully" });
                    return response;
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = e.Message }); ;
                }

            }
        }

        [Route("getSupplierBySupId/{id}")]
        [HttpGet]
        public HttpResponseMessage Get([FromUri] string id)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    var suppliers_items = dbContext.Database.SqlQuery<supplier_items>("select * from supplier_items where supplierId = @id", new SqlParameter("id", id)).ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, data = suppliers_items });
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = e.Message });
                }
            }
        }

        [Route("deleteItem/{supItemId}/{supplierId}")]
        [HttpDelete]
        public HttpResponseMessage DeleteSupplier([FromUri]string supItemId, [FromUri]string supplierId)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    supplier_items itemObj = dbContext.supplier_items.FirstOrDefault(i => i.supItemId == supItemId && i.supplierId == supplierId);
                    if (itemObj == null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = "Item cannot be found" });
                    }
                    else
                    {
                        dbContext.supplier_items.Remove(itemObj);
                        dbContext.SaveChanges();
                        response = Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, message = "Item deleted successfully" });
                    }

                    return response;
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = e.Message }); ;
                }

            }
        }

        [Route("getByKeys/{supItemId}/{supplierId}")]
        [HttpGet]
        public HttpResponseMessage GetSupplierItemByKeys([FromUri]string supItemId, [FromUri]string supplierId)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    supplier_items itemObj = dbContext.supplier_items.FirstOrDefault(i => i.supItemId == supItemId && i.supplierId == supplierId);
                    //var itemObj = dbContext.supplier_items.SqlQuery("select * from supplier_items").ToList();
                    if (itemObj == null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = "Item cannot be found" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, data = itemObj });
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
