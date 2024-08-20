using GiftMatchServer.BL;

using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GiftMatchServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterestsController : ControllerBase
    {
        // GET: api/<InterestsController>
        [HttpGet]
        public IActionResult GetIntersts()
        {
            DBservices dbs = new DBservices();
            List<Interests> res = dbs.getInterests();
            if (res.Count > 0)
                return Ok(res);
            return NotFound();
        }
       
    }
}
