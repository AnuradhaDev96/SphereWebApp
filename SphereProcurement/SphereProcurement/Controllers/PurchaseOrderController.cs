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

    [RoutePrefix("db/order")]
    public class PurchaseOrderController : ApiController
    {
        private static PurchaseOrderController instance = null;
        private PurchaseOrderController() { }
        public PurchaseOrderController getLoginController()
        {

            if (instance == null)
            {
                return new PurchaseOrderController();
            }
            else
            {
                return instance;
            }
        }

        [Route("addOrder")]
        [HttpPost]
        public HttpResponseMessage PostOrder(order order)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    var purchaseOrders = dbContext.orders.Add(order);
                    dbContext.SaveChanges();
                    purchaseRequest reqObj = dbContext.purchaseRequests.Find(order.reqId);
                    reqObj.requestStatus = "CLOSE";
                    dbContext.SaveChanges();
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, message = "Order Added Succesfully" });
                    return response;
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = e.Message }); ;
                }

            }
        }

        [Route("getAllOrders")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    var orders = dbContext.orders.SqlQuery(@"select * from orders").ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, data = orders });
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = e.Message });
                }
            }
        }

        [Route("getOrderById/{id}")]
        [HttpGet]
        public HttpResponseMessage GetOrderById([FromUri]string id)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    order orderObj = dbContext.orders.Find(id);
                    if (orderObj == null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = "Order cannot be found" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, data = orderObj });
                    }

                    return response;
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = e.Message }); ;
                }

            }
        }

        [Route("editOrderById/{id}")]
        [HttpPut]
        public HttpResponseMessage EditPurchaseById([FromUri]string id, [FromBody]order order)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    order orderObj = dbContext.orders.Find(id);
                    if (orderObj == null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = "Order cannot be found" });
                    }
                    else
                    {
                        orderObj.id = order.id;
                        orderObj.reqId = order.reqId;
                        orderObj.createBy = order.createBy;
                        orderObj.date = order.date;
                        orderObj.status = order.status;
                        dbContext.Entry(orderObj).State = System.Data.Entity.EntityState.Modified;
                        dbContext.SaveChanges();
                        response = Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, message = "Order edited successfully" });
                    }

                    return response;
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = e.Message }); ;
                }

            }
        }


        [Route("deleteOrders/{id}")]
        [HttpDelete]
        public HttpResponseMessage DeleteOrder([FromUri]string id)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    order orderObj = dbContext.orders.Find(id);
                    if (orderObj == null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = "Order cannot be found" });
                    }
                    else
                    {
                        dbContext.orders.Remove(orderObj);
                        dbContext.SaveChanges();
                        response = Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, message = "Order deleted successfully" });
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

