using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CloudCare.API.Controllers
{
    [Route("api/UserLOL")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public ActionResult Test_get()
        {
            return Ok("HELO");
        }

        [HttpGet("loll")]
        public ActionResult <List<List<string>>> GetIAction_get()
        {
var list = new List<List<string>>();

            list.Add(new List<string> { "YELLO  ", "FELLO" });
            return list;


        }
    }
}
