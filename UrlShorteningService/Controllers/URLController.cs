using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShorteningService.Data;
using UrlShorteningService.Models;
using NanoidDotNet;
using UrlShorteningService.Dto;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UrlShorteningService.Controllers
{
    [Route("/shorten")]
    [ApiController]
    public class URLController : ControllerBase
    {
        private readonly URLContext _context;

        public URLController(URLContext context)
        {
            _context = context;
        }

        // GET: /shorten
        [HttpGet]
        public async Task<ActionResult<IEnumerable<URL>>> GetURL()
        {
            return await _context.URL.ToListAsync();
        }

        // GET: /shorten/abc123
        [HttpGet("{shortCode}")]
        public async Task<ActionResult<URLStandar>> GetURL(string shortCode)
        {
            var data = await _context.URL.FirstOrDefaultAsync(u => u.shortCode == shortCode);
            if (data == null)
            {
                return NotFound();
            }

            URLStandar uRL = new URLStandar
            {
                id = data.id,
                url = data.url,
                shortCode = data.shortCode,
                createdAt = data.createdAt,
                updatedAt = data.updatedAt,
            };

            data.accessCount = data.accessCount + 1;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!URLExists(data.id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return uRL;
        }

        // GET: /shorten/abc123/stats
        [HttpGet("{shortCode}/stats")]
        public async Task<ActionResult<URL>> GetURLStats(string shortCode)
        {
            var uRL = await _context.URL.FirstOrDefaultAsync(u => u.shortCode == shortCode);

            if (uRL == null)
            {
                return NotFound();
            }

            return uRL;
        }

        // PUT: /shorten/abc123
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{shortCode}")]
        public async Task<ActionResult<URL>> PutURL(string shortCode, URLDto data)
        {

            var existingURL = await _context.URL.FirstOrDefaultAsync(u => u.shortCode == shortCode);
            if (existingURL == null)
            {
                return NotFound();
            }

            existingURL.url = data.url;
            existingURL.updatedAt = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!URLExists(existingURL.id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            URLStandar uRL = new URLStandar
            {
                id = existingURL.id,
                url = existingURL.url,
                shortCode = existingURL.shortCode,
                createdAt = existingURL.createdAt,
                updatedAt = existingURL.updatedAt,
            };

            return Ok(uRL);
        }

        // POST: /shorten
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<URL>> PostURL(URLDto data)
        {
            URL uRL = new URL
            {
                url = data.url,
                shortCode = Nanoid.Generate(size: 8),
                createdAt = DateTime.Now,
                updatedAt = DateTime.Now,
                accessCount = 0,
            };

            _context.URL.Add(uRL);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetURL", new { id = uRL.id }, uRL);
        }

        // DELETE: /shorten/abc123
        [HttpDelete("{shortCode}")]
        public async Task<IActionResult> DeleteURL(string shortCode)
        {
            var uRL = await _context.URL.FirstOrDefaultAsync(u => u.shortCode == shortCode);
            if (uRL == null)
            {
                return NotFound();
            }

            _context.URL.Remove(uRL);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool URLExists(int id)
        {
            return _context.URL.Any(e => e.id == id);
        }
    }
}
