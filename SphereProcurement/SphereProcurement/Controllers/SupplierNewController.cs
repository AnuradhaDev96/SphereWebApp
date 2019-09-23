using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SphereProcurement.Controllers
{
    [RoutePrefix("db/Supplier")]
    public class SupplierNewController : ApiController
    {
        //ProcurementDBEntities1 dbContext = new ProcurementDBEntities1();

        [Route("getAllSups")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {
                
                try
                {
                    var suppliers = dbContext.suppliers.SqlQuery(@"select * from suppliers").ToList();
                    //var jsonResult = JsonConvert.SerializeObject(suppliers);
                    //return jsonResult;
                    return Ok(suppliers);
                }
                catch (Exception e)
                {
                    //var jsonResult = JsonConvert.SerializeObject(e.Message);
                    //return jsonResult;
                    return NotFound();
                }                
            }
        }

        [Route("addSups")]
        [HttpPost]
        public IHttpActionResult PostSupplier(supplier supplier)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    var suppliers = dbContext.suppliers.Add(supplier);
                    dbContext.SaveChanges();
                    //var jsonResult = JsonConvert.SerializeObject(suppliers);
                    //return jsonResult;
                    return Ok(suppliers);
                }
                catch (Exception e)
                {
                    //var jsonResult = JsonConvert.SerializeObject(e.Message);
                    //return jsonResult;
                    return NotFound();
                }

            }
        }


    }
}
