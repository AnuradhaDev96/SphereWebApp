using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SphereProcurement.Controllers
{
    [RoutePrefix("db/SiteManagers")]
    public class SiteManagersController : ApiController
    {
        [Route("getAllSiteManagers")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    var siteManagers = dbContext.siteManagers.SqlQuery(@"select * from siteManagers").ToList();
                    //var jsonResult = JsonConvert.SerializeObject(suppliers);
                    //return jsonResult;
                    return Ok(siteManagers);
                }
                catch (Exception e)
                {
                    //var jsonResult = JsonConvert.SerializeObject(e.Message);
                    //return jsonResult;
                    return NotFound();
                    //return Ok(e.Message);
                }
            }
        }

        [Route("addSiteManagers")]
        [HttpPost]
        public IHttpActionResult PostSiteManager(siteManager siteManager)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    var siteManagers = dbContext.siteManagers.Add(siteManager);
                    dbContext.SaveChanges();
                    //var jsonResult = JsonConvert.SerializeObject(suppliers);
                    //return jsonResult;
                    return Ok(siteManagers);
                }
                catch (Exception e)
                {
                    //var jsonResult = JsonConvert.SerializeObject(e.Message);
                    //return jsonResult;
                    //return NotFound();
                    return Ok(e.Message);
                }

            }
        }

    }
}
