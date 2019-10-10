using Newtonsoft.Json;
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
    [RoutePrefix("db/Payment")]

    public class PaymentController : ApiController
    {
        private static PaymentController instance = null;
        private PaymentController() { }
        public PaymentController getLoginController()
        {

            if (instance == null)
            {
                return new PaymentController();
            }
            else
            {
                return instance;
            }
        }

        [Route("getAllPays")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    var payments = dbContext.payments.SqlQuery(@"select * from payments where status = 'OPEN'").ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, data = payments });
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = e.Message });
                }
            }
        }

        [Route("addPays")]
        [HttpPost]
        public HttpResponseMessage PostPayment([FromBody]payment payment)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    var payments = dbContext.payments.Add(payment);
                    dbContext.SaveChanges();
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, message = "Payments Added Succesfully" });
                    return response;
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = e.Message }); ;
                }

            }
        }

        [Route("getPaymentById/{id}")]
        [HttpGet]
        public HttpResponseMessage GetPaymentById([FromUri]string id)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    payment paymentObj = dbContext.payments.Find(id);
                    if (paymentObj == null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = "Payment cannot be found" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, data = paymentObj });
                    }

                    return response;
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = e.Message }); ;
                }

            }
        }

        [Route("editPaymentById/{id}")]
        [HttpPut]
        public HttpResponseMessage EditSupplierById([FromUri]string id, [FromBody]payment payment)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    payment paymentObj = dbContext.payments.Find(id);
                    if (paymentObj == null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = "Payment cannot be found" });
                    }
                    else
                    {
                        paymentObj.paymentId = payment.paymentId;
                        paymentObj.purchaseOrderId = payment.purchaseOrderId;
                        paymentObj.amount = payment.amount;
                        paymentObj.paidOn = payment.paidOn;
                        paymentObj.paidBy = payment.paidBy;
                        paymentObj.status = payment.status;
                        dbContext.Entry(paymentObj).State = System.Data.Entity.EntityState.Modified;
                        dbContext.SaveChanges();
                        response = Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, message = "Payments edited successfully" });
                    }

                    return response;
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = e.Message }); ;
                }

            }
        }

        //........................................................................................................


        [Route("searchPayById/{paymentId}")]
        [HttpGet]
        public HttpResponseMessage searchPayById([FromUri] string paymentId)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    //var payments = dbContext.Database.SqlQuery<payment>(@"select * from payments where paymentId=@id", new SqlParameter("id", paymentId)).ToList();
                    var payments = (from p in dbContext.payments
                                    where p.paymentId == paymentId
                                    select p).ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, data = payments });
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = e.Message });
                }
            }
        }



        //...............................................................................................................

        [Route("Delete/{id}")]
        [HttpPut]
        public HttpResponseMessage DeletePaymentById([FromUri]string id, [FromBody]payment payment)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    payment paymentObj = dbContext.payments.Find(id);
                    if (paymentObj == null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = "Payment cannot be found" });
                    }
                    else
                    {
                        paymentObj.status = "BLOCK";
                        dbContext.Entry(paymentObj).State = System.Data.Entity.EntityState.Modified;
                        dbContext.SaveChanges();
                        response = Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, message = "Payments edited successfully" });
                    }

                    return response;
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = e.Message }); ;
                }

            }
        }

        //................................................................................................


        [Route("viewDeletedPayments")]
        [HttpGet]
        public HttpResponseMessage DeletePaymentById()
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    var payments = dbContext.payments.SqlQuery(@"select * from payments where status = 'BLOCK'").ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, data = payments });
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = e.Message });
                }
            }
        }

    }
}
