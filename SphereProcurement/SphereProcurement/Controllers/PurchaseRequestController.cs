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
    }
}
