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
        public Object Get()
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {
                var suppliers = dbContext.suppliers.SqlQuery(@"select * from suppliers").ToList<supplier>();
                var jsonResult = JsonConvert.SerializeObject(suppliers);
                return jsonResult;
            }
        }
    }
}
