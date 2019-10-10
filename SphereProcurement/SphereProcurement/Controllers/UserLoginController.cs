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
    [RoutePrefix("db/User")]
    public class UserLoginController : ApiController
    {
        private static UserLoginController instance = null;
        private UserLoginController() { }
        public UserLoginController getLoginController()
        {

            if (instance == null)
            {
                return new UserLoginController();
            }
            else
            {
                return instance;
            }
        }
        [Route("login/{username}/{password}")]
        [HttpGet]
        public HttpResponseMessage UserLogin([FromUri]string username, [FromUri]string password)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    user user = dbContext.users.FirstOrDefault(i => i.user_name == username && i.pass_word == password);
                    if (user == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = "Invalid Username or Password" });
                    }
                    else {
                        return Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, message = "User Logged in Successfully",  data = user });
                    }                    
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { statusCode = HttpStatusCode.NotFound, message = e.Message });
                }
            }
        }

        [Route("register")]
        [HttpPost]
        public HttpResponseMessage UserRegister([FromBody]user user)
        {
            using (ProcurementDBEntities1 dbContext = new ProcurementDBEntities1())
            {

                try
                {
                    var result = dbContext.users.Add(user);
                    dbContext.SaveChanges();
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = Request.CreateResponse(HttpStatusCode.OK, new { statusCode = HttpStatusCode.OK, message = "User Registered Succesfully" });
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
