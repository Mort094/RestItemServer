using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLib;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestItemServer.Controllers
{
    [Route("api/Items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private static List<Item> items = new List<Item>()
        {
            new Item(1, "bread", "low", 33),
            new Item(2, "bread", "middel", 21),
            new Item(3, "beer", "low", 70.5),
            new Item(4, "soda", "high", 21.4),
            new Item(5, "milk", "low", 55.8)
        };

        // GET: api/<ItemsController>
        [HttpGet]
        public IEnumerable<Item> Get()
        {
            return items;
        }
        [HttpGet]
        [Route("Name/{substring}")]
        [EnableCors("AllowAnyOrigin")]
        public IEnumerable<Item> GetFromSubstring(String substring)
        {
            return items.FindAll(i => i.Name.Contains(substring));
        }

        [HttpGet]
        [Route("Quality/{substring}")]
        [EnableCors("AllowAnyOrigin")]
        public IEnumerable<Item> GetQualityString(String substring)
        {
            return items.FindAll(i => i.Quality.Contains(substring));
        }

        [HttpGet]
        [Route("Search")]
        [EnableCors("AllowAnyOrigin")]
        public IEnumerable<Item> GetWithFilter([FromQuery] FilterItem filter)
        {
            return items.FindAll(i => i.Quantity > filter.LowQuantity && i.Quantity < filter.HighQuantity);
        }

        [HttpGet]
        [Route("{id}")]
        [EnableCors("AllowAnyOrigin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult myGet(int id)
        {
            if (items.Exists(i => i.Id == id))
            {
                return Ok(items.Find(i => i.Id == id));
            }

            return NotFound($"item id {id} not found");
        }

        // GET api/<ItemsController>/5
        //[HttpGet]
        //[Route("{id}")]
        //[EnableCors("AllowAnyOrigin")]
        private Item Get(int id)
        {
            return items.Find(i => i.Id == id);
        }

        // POST api/<ItemsController>
        [HttpPost]
        [EnableCors("AllowSpecifOrigin")]
        public void Post([FromBody] Item value)
        {
            items.Add(value);
        }

        // PUT api/<ItemsController>/5
        [HttpPut]
        [Route("{id}")]
        [EnableCors("AllowSpecifOrigin")]
        public void Put(int id, [FromBody] Item value)
        {
            Item item = Get(id);
            if (item != null)
            {
                item.Id = value.Id;
                item.Name = value.Name;
                item.Quality = value.Quality;
                item.Quantity = value.Quantity;
            }
        }

        // DELETE api/<ItemsController>/5
        [HttpDelete]
        [Route("{id}")]
        [DisableCors]
        public void Delete(int id)
        {
            Item item = Get(id);
            items.Remove(item);
        }
    }
}
