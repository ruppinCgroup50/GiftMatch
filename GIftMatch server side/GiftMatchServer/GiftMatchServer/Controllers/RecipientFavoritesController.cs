using GiftMatchServer.BL;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GiftMatchServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipientFavoritesController : ControllerBase
    {
        // GET: api/<RecipientFavoritesController>
        [HttpGet("GetFavorites/{phone}")]
        public IActionResult Get(string phone)
        {
            DBservices dbs = new DBservices();
            List<RecipientFavorites> res = dbs.GetFavoritesGiftIdea(phone);
            List<GiftIdea> allGiftIdeas = dbs.GetUnifiedList();

            if (res.Count == 0)
            {
                return NotFound();
            }

        // Filter favorite ideas that exist in the allGiftIdeas list
        var matchingIdeas = allGiftIdeas.Where(fav => res.Any(gift => gift.GiftName == fav.GiftName)).ToList();

            if (matchingIdeas.Count > 0)
            {
                return Ok(matchingIdeas);
            }
            return NotFound();
        }

        // POST api/<RecipientFavoritesController>
        [HttpPost("SaveIdea")]
        public IActionResult Post([FromBody] RecipientFavorites gift)
        {
            try
            {
                DBservices dbs = new DBservices();
                int res = dbs.InsertFavoritesGiftIdea(gift);
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

        // DELETE api/<RecipientFavoritesController>/5
        [HttpDelete("RemoveIdea")]
        public IActionResult Delete([FromBody] RecipientFavorites gift)
        {
            try
            {
                DBservices dbs = new DBservices();
                int res = dbs.RemoveFavoritesGiftIdea(gift);
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
