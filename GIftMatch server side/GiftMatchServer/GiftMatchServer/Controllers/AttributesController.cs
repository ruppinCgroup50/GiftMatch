using GiftMatchServer.BL;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GiftMatchServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttributesController : ControllerBase
    {
        [HttpPost("insertNewsAssociatedAttr")]
        public IActionResult insertNewsAssociatedAttr([FromBody] AssociatedAtrr attr)
        {
            try
            {
                DBservices dbs = new DBservices();
                int res = dbs.insertNewsAssociatedAttr(attr);

                if (res > 0)
                {
                    return Ok(res);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
