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
    public class SolutionController : ControllerBase
    {
        private readonly HowDoYouDoThisContext _context;

        public SolutionController(HowDoYouDoThisContext context)
        {
            _context = context;
        }

        // GET: api/Solution
        [HttpGet]
        public IEnumerable<SolutionItem> GetSolutionItem()
        {
            return _context.SolutionItem;
        }

        // GET: api/Solution/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSolutionItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var solutionItem = await _context.SolutionItem.FindAsync(id);

            if (solutionItem == null)
            {
                return NotFound();
            }

            return Ok(solutionItem);
        }

        // PUT: api/Solution/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSolutionItem([FromRoute] int id, [FromBody] SolutionItem solutionItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != solutionItem.ID)
            {
                return BadRequest();
            }

            _context.Entry(solutionItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SolutionItemExists(id))
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

        // POST: api/Solution
        [HttpPost]
        public async Task<IActionResult> PostSolutionItem([FromBody] SolutionItem solutionItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.SolutionItem.Add(solutionItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSolutionItem", new { id = solutionItem.ID }, solutionItem);
        }

        // DELETE: api/Solution/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSolutionItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var solutionItem = await _context.SolutionItem.FindAsync(id);
            if (solutionItem == null)
            {
                return NotFound();
            }

            _context.SolutionItem.Remove(solutionItem);
            await _context.SaveChangesAsync();

            return Ok(solutionItem);
        }

        private bool SolutionItemExists(int id)
        {
            return _context.SolutionItem.Any(e => e.ID == id);
        }
    }
}