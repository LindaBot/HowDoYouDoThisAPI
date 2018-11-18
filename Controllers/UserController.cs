using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HowDoYouDoThis.Models;

namespace HowDoYouDoThis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly HowDoYouDoThisContext _context;

        public UserController(HowDoYouDoThisContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public IEnumerable<UserItem> GetUserItem()
        {
            return _context.UserItem;
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userItem = await _context.UserItem.FindAsync(id);

            if (userItem == null)
            {
                return NotFound();
            }

            return Ok(userItem);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserItem([FromRoute] int id, [FromBody] UserItem userItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userItem.ID)
            {
                return BadRequest();
            }

            _context.Entry(userItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/User
        [HttpPost]
        public async Task<IActionResult> PostUserItem([FromBody] UserItem userItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.UserItem.Add(userItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserItem", new { id = userItem.ID }, userItem);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userItem = await _context.UserItem.FindAsync(id);
            if (userItem == null)
            {
                return NotFound();
            }

            _context.UserItem.Remove(userItem);
            await _context.SaveChangesAsync();

            return Ok(userItem);
        }

        private bool UserItemExists(int id)
        {
            return _context.UserItem.Any(e => e.ID == id);
        }
    }
}