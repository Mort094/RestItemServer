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
    [EnableCors("AllowAnyOrigin")]
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
        [EnableCors("AllowAnyOrigin")]
        public IEnumerable<Item> Get()
        {
            return items;
        }
        [HttpGet]
        [EnableCors("AllowAnyOrigin")]
        [Route("Name/{substring}")]
        public IEnumerable<Item> GetFromSubstring(String substring)
        {
            return items.FindAll(i => i.Name.Contains(substring));
        }

        [HttpGet]
        [EnableCors("AllowAnyOrigin")]
        [Route("Quality/{substring}")]
        public IEnumerable<Item> GetQualityString(String substring)
        {
            return items.FindAll(i => i.Quality.Contains(substring));
        }

        [HttpGet]
        [EnableCors("AllowAnyOrigin")]
        [Route("Search")]
        public IEnumerable<Item> GetWithFilter([FromQuery] FilterItem filter)
        {
            return items.FindAll(i => i.Quantity > filter.LowQuantity && i.Quantity < filter.HighQuantity);
        }

        [HttpGet]
        [EnableCors("AllowAnyOrigin")]
        [Route("{id}")]
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
        [HttpGet]
        [EnableCors("AllowAnyOrigin")]
        [Route("{id}")]
        public Item Get(int id)
        {
            return items.Find(i => i.Id == id);
        }

        // POST api/<ItemsController>
        [HttpPost]
        [EnableCors("AllowSpeceficOrigin")]
        public void Post([FromBody] Item value)
        {
            items.Add(value);
        }

        // PUT api/<ItemsController>/5
        [HttpPut]
        [EnableCors("AllowSpeceficOrigin")]
        [Route("{id}")]
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
        [EnableCors("AllowSpeceficOrigin")]
        [Route("{id}")]
        public void Delete(int id)
        {
            Item item = Get(id);
            items.Remove(item);
        }
    }
}
