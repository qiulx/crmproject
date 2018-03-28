using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student5.Data;
using Student5.Models;

namespace Student5.Controllers.api
{
    [Produces("application/json")]
    [Route("api/Inquiries")]
    public class InquiriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InquiriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Inquiries
        [HttpGet]
        public IEnumerable<Inquery1> GetInquery1()
        {
            return _context.Inquery1;
        }

        // GET: api/Inquiries/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInquery1([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var inquery1 = await _context.Inquery1.SingleOrDefaultAsync(m => m.InquiryId == id);

            if (inquery1 == null)
            {
                return NotFound();
            }

            return Ok(inquery1);
        }

        // PUT: api/Inquiries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInquery1([FromRoute] Guid id, [FromBody] Inquery1 inquery1)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != inquery1.InquiryId)
            {
                return BadRequest();
            }

            _context.Entry(inquery1).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Inquery1Exists(id))
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

        // POST: api/Inquiries
        [HttpPost]
        public async Task<IActionResult> PostInquery1([FromBody] Inquery1 inquery1)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Inquery1.Add(inquery1);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInquery1", new { id = inquery1.InquiryId }, inquery1);
        }

        // DELETE: api/Inquiries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInquery1([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var inquery1 = await _context.Inquery1.SingleOrDefaultAsync(m => m.InquiryId == id);
            if (inquery1 == null)
            {
                return NotFound();
            }

            _context.Inquery1.Remove(inquery1);
            await _context.SaveChangesAsync();

            return Ok(inquery1);
        }

        private bool Inquery1Exists(Guid id)
        {
            return _context.Inquery1.Any(e => e.InquiryId == id);
        }
    }
}