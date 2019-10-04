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
    [RoutePrefix("db/PurchReq")]
    public class PurchaseRequestController : ApiController
    {
        [Route("addReq")]
        [HttpPost]
        public HttpResponseMessage PostPurchaseRequest([FromBody]purchaseRequest req)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    var result = dbContext.purchaseRequests.Add(req);
                    dbContext.SaveChanges();
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, message = "Purchase Request Added Succesfully" });
                    return response;
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = e.Message }); ;
                }

            }
        }

        //post method to add items into purchase request
        [Route("PurchItems/addItem")]
        [HttpPost]
        public HttpResponseMessage PostPurchReqItem([FromBody]purchaseRequestItem item)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    var result = dbContext.purchaseRequestItems.Add(item);
                    dbContext.SaveChanges();

                    purchaseRequest reqObj = dbContext.purchaseRequests.FirstOrDefault(i => i.requestId == item.requestId);
                    double totalAmount = Convert.ToDouble((from p in dbContext.purchaseRequestItems
                                                           where p.requestId == item.requestId
                                                           group p by p.requestId into g
                                                           select g.Sum(c => c.subTotal).Value).FirstOrDefault());
                    reqObj.totalAmount = totalAmount;
                    dbContext.Entry(reqObj).State = System.Data.Entity.EntityState.Modified;
                    dbContext.SaveChanges();

                    HttpResponseMessage response = new HttpResponseMessage();
                    response = Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, message = "Item Added to Cart Succesfully" });
                    return response;
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = e.Message }); ;
                }

            }
        }

        [Route("getPurchReqByReqId/{requestId}")]
        [HttpGet]
        public HttpResponseMessage GetPurchReqByReqId([FromUri]string requestId)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {                  
                    purchaseRequest reqObj = dbContext.purchaseRequests.FirstOrDefault(i => i.requestId == requestId);
                    return Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, data = reqObj });
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = e.Message }); ;
                }

            }
        }

        [Route("getOpenPurch")]
        [HttpGet]
        public HttpResponseMessage GetOpenPurchReq()
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    var reqObj = dbContext.purchaseRequests.SqlQuery("select * from purchaseRequests where requestStatus = 'OPEN'").ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, data = reqObj });
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = e.Message }); ;
                }

            }
        }

        [Route("getNeedApprovalPurch")]
        [HttpGet]
        public HttpResponseMessage GetNeedApprovalPurchReq()
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    var reqObj = dbContext.purchaseRequests.SqlQuery("select * from purchaseRequests where requestStatus = 'NEED APPROVAL'").ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, data = reqObj });
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = e.Message }); ;
                }

            }
        }

        [Route("getCartByPurchReqId/{requestId}/")]
        [HttpGet]
        public HttpResponseMessage GetCartByPurchReqId([FromUri]string requestId)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    HttpResponseMessage response = new HttpResponseMessage();

                    var itemObj = dbContext.Database.SqlQuery<purchaseRequestItem>("select * from purchaseRequestItems where requestId = @id", new SqlParameter("id", requestId)).ToList();
                    if (itemObj == null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = "Items cannot be found" });
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

        [Route("deleteCartItem/{requestItemId}/{requestId}/{supplierId}")]
        [HttpDelete]
        public HttpResponseMessage DeleteCartItem([FromUri]string requestItemId, [FromUri]string requestId, [FromUri]string supplierId)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    purchaseRequestItem itemObj = dbContext.purchaseRequestItems.FirstOrDefault(i => i.requestItemId == requestItemId && i.requestId == requestId && i.supplierId == supplierId);
                    if (itemObj == null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = "Item cannot be found" });
                    }
                    else
                    {
                        dbContext.purchaseRequestItems.Remove(itemObj);
                        dbContext.SaveChanges();

                        purchaseRequest reqObj = dbContext.purchaseRequests.FirstOrDefault(i => i.requestId == requestId);
                        double totalAmount = Convert.ToDouble((from p in dbContext.purchaseRequestItems
                                                               where p.requestId == requestId
                                                               group p by p.requestId into g
                                                               select g.Sum(c => c.subTotal).Value).FirstOrDefault());
                        reqObj.totalAmount = totalAmount;
                        dbContext.Entry(reqObj).State = System.Data.Entity.EntityState.Modified;
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

        [Route("placeOrder/{requestId}")]
        [HttpPut]
        public HttpResponseMessage PlaceOrder([FromUri]string requestId, [FromBody]purchaseRequest reqObj)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    purchaseRequest requestObj = dbContext.purchaseRequests.Find(requestId);        
                    if (requestObj == null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = "Purchase Request cannot be found" });
                    }
                    else
                    {
                        double approvedValue = Convert.ToDouble((from p in dbContext.siteManagers
                                                                 where p.smanagerNo == reqObj.createdBy
                                                                 select (p.approvedValue).Value).FirstOrDefault());
                        if (reqObj.totalAmount > approvedValue) {
                            requestObj.requestStatus = "NEED APPROVAL";
                        }
                        else {
                            requestObj.requestStatus = reqObj.requestStatus;
                        }                        
                        dbContext.Entry(requestObj).State = System.Data.Entity.EntityState.Modified;
                        dbContext.SaveChanges();
                        response = Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, message = "Purchase Request edited successfully" });
                    }

                    return response;
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = e.Message }); ;
                }

            }
        }

        //if purchase request has supplierId navigate to addtocart screen
        //if purchase request does not has supplierId navigate to select supplier screen
        //implement above by validating      
        [Route("getTotalAmount/{requestId}")]
        [HttpGet]
        public HttpResponseMessage GetSumOfSubTotal([FromUri]string requestId)
        {

            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    //var totalAmount = dbContext.purchaseRequestItems.SqlQuery("select sum(p.subTotal) from purchaseRequestItems p where p.requestId = @id", new SqlParameter("id", requestId));
                    double totalAmount = Convert.ToDouble((from p in dbContext.purchaseRequestItems
                                         where p.requestId == requestId
                                         group p by p.requestId into g
                                         select g.Sum(c => c.subTotal).Value).FirstOrDefault());


                    return Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, data = totalAmount });
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = e.Message }); ;
                }

            }
        }

        [Route("getApprovedVal/{siteManagerId}")]
        [HttpGet]
        public HttpResponseMessage GetApprovedValue([FromUri]string siteManagerId)
        {

            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    //var totalAmount = dbContext.purchaseRequestItems.SqlQuery("select sum(p.subTotal) from purchaseRequestItems p where p.requestId = @id", new SqlParameter("id", requestId));
                    double approvedValue = Convert.ToDouble((from p in dbContext.siteManagers
                                                           where p.smanagerNo == siteManagerId
                                                           select (p.approvedValue).Value).FirstOrDefault());


                    return Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, data = approvedValue });
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = e.Message }); ;
                }

            }
        }

        [Route("approvePurchReq/{requestId}")]
        [HttpPut]
        public HttpResponseMessage AprrovePurchReq([FromUri]string requestId, [FromBody]purchaseRequest reqObj)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    purchaseRequest requestObj = dbContext.purchaseRequests.Find(requestId);
                    if (requestObj == null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = "Purchase Request cannot be found" });
                    }
                    else
                    {
                        requestObj.requestStatus = "OPEN";                        
                        dbContext.Entry(requestObj).State = System.Data.Entity.EntityState.Modified;
                        dbContext.SaveChanges();
                        response = Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, message = "Purchase Request Approved successfully" });
                    }

                    return response;
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = e.Message }); ;
                }

            }
        }

        [Route("deletePurchReq/{id}")]
        [HttpDelete]
        public HttpResponseMessage DeletePurchReq([FromUri]string id)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    purchaseRequest requestObj = dbContext.purchaseRequests.Find(id);
                    if (requestObj == null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = "Purchase Request cannot be found" });
                    }
                    else
                    {
                        dbContext.purchaseRequests.Remove(requestObj);
                        dbContext.SaveChanges();
                        response = Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, message = "Purchase Request deleted successfully" });
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
