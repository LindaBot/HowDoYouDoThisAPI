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
        public async Task<IActionResult> PostUserItem([FromForm]NewUser newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var usernamesQuery = (from m in _context.UserItem
                                 select m.username).Distinct();

                var usernames = await usernamesQuery.ToListAsync();

                for (int i = 0; i < usernames.Count; i++) {
                    if (usernames[i] == newUser.username) {
                        return BadRequest($"Same username exists, please use a different username");
                    }
                }


                UserItem userItem = new UserItem();
                userItem.firstName = newUser.firstName;
                userItem.lastName = newUser.lastName;
                userItem.username = newUser.username;
                userItem.password = newUser.password;
                userItem.dateCreated = newUser.dateCreated;
                userItem.admin = newUser.admin;
                userItem.TagID = newUser.TagID;

                _context.UserItem.Add(userItem);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetUserItem", new { id = userItem.ID }, userItem);
            }
            catch (Exception ex) {
                return BadRequest($"An error has occured. Details: {ex.Message}");
            }
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