using GiftMatchServer.BL;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GiftMatchServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Big5qsController : ControllerBase
    {
        // GET: api/<Big5qsController>
        [HttpGet]
        public IActionResult GetQuestion()
        {
            DBservices dbs = new DBservices();
            List<Big5Q> res = dbs.GetQuestion();
            if (res.Count > 0)
                return Ok(res);
            return NotFound();
        }

       
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

     
        
    }
}
