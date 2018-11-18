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
    public class QuestionController : ControllerBase
    {
        private readonly HowDoYouDoThisContext _context;

        public QuestionController(HowDoYouDoThisContext context)
        {
            _context = context;
        }

        // GET: api/Question
        [HttpGet]
        public IEnumerable<QuestionItem> GetQuestionItem()
        {
            return _context.QuestionItem;
        }

        // GET: api/Question/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuestionItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var questionItem = await _context.QuestionItem.FindAsync(id);

            if (questionItem == null)
            {
                return NotFound();
            }

            return Ok(questionItem);
        }

        // PUT: api/Question/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestionItem([FromRoute] int id, [FromBody] QuestionItem questionItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != questionItem.ID)
            {
                return BadRequest();
            }

            _context.Entry(questionItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionItemExists(id))
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

        // POST: api/Question
        [HttpPost]
        public async Task<IActionResult> PostQuestionItem([FromBody] QuestionItem questionItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.QuestionItem.Add(questionItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuestionItem", new { id = questionItem.ID }, questionItem);
        }

        // DELETE: api/Question/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestionItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var questionItem = await _context.QuestionItem.FindAsync(id);
            if (questionItem == null)
            {
                return NotFound();
            }

            _context.QuestionItem.Remove(questionItem);
            await _context.SaveChangesAsync();

            return Ok(questionItem);
        }

        private bool QuestionItemExists(int id)
        {
            return _context.QuestionItem.Any(e => e.ID == id);
        }
    }
}