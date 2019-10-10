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
    [RoutePrefix("db/SiteManagers")]
    public class SiteManagersController : ApiController
    {
        [Route("getAllSiteManagers")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    var siteManagers = dbContext.siteManagers.SqlQuery(@"select * from siteManagers").ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, data = siteManagers });
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = e.Message });
                }
            }
        }

        [Route("addSiteManagers")]
        [HttpPost]
        public HttpResponseMessage PostSiteManager(siteManager siteManager)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    var siteManagers = dbContext.siteManagers.Add(siteManager);
                    dbContext.SaveChanges();

                    HttpResponseMessage response = new HttpResponseMessage();
                    response = Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, message = "Site Managers Added Succesfully" });
                    return response;
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = e.Message });
                }

            }
        }

        [Route("deleteSManagers/{id}")]
        [HttpDelete]
        public HttpResponseMessage DeleteSManagers([FromUri]string id)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    siteManager sManagers = dbContext.siteManagers.Find(id);
                    if (sManagers == null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = "Site Managers cannot be found" });
                    }
                    else
                    {
                        dbContext.siteManagers.Remove(sManagers);
                        dbContext.SaveChanges();
                        response = Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, message = "Site Manager deleted successfully" });
                    }

                    return response;
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = e.Message }); ;
                }

            }
        }

        [Route("getSManagerById/{id}")]
        [HttpGet]
        public HttpResponseMessage GetSManagerById([FromUri]string id)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    siteManager sManager = dbContext.siteManagers.Find(id);
                    if (sManager == null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = "Site Manager cannot be found" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, data = sManager });
                    }

                    return response;
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = e.Message }); ;
                }

            }
        }

        [Route("editSManagerById/{id}")]
        [HttpPut]
        public HttpResponseMessage EditSupplierById([FromUri]string id, [FromBody]siteManager siteManager)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    siteManager sManager = dbContext.siteManagers.Find(id);
                    if (sManager == null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = "Site Manager cannot be found" });
                    }
                    else
                    {
                        sManager.smanagerNo = siteManager.smanagerNo;
                        sManager.sname = siteManager.sname;
                        sManager.snic = siteManager.snic;
                        sManager.scontactNo = siteManager.scontactNo;
                        sManager.site = siteManager.site;
                        sManager.approvedValue = siteManager.approvedValue;

                        dbContext.Entry(sManager).State = System.Data.Entity.EntityState.Modified;
                        dbContext.SaveChanges();
                        response = Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, message = "Site Manager edited successfully" });
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
