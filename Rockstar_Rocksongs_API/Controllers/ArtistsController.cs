using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rockstar_RockSongs_API.Attributes;
using Rockstar_RockSongs_API.Models;

namespace Rockstar_RockSongs_API.Controllers
{
    [ApiKey]
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly RockLibraryContext _context;

        public ArtistsController(RockLibraryContext context)
        {
            _context = context;
        }

        // GET: api/Artists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Artist>>> GetArtists()
        {
            return await _context.Artists.ToListAsync();
        }

        // GET: api/Artists/5
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Artist>> GetArtist(int id)
        {
            var artist = await _context.Artists.FirstOrDefaultAsync(a => a.Id == id);

            if (artist == null)
            {
                return NotFound();
            }

            return artist;
        }

        // GET: api/Artists/Vapors
        [HttpGet("GetByName/{name}")]
        public async Task<ActionResult<Artist>> GetArtistByName(string name)
        {
            var artist = await _context.Artists.FirstOrDefaultAsync(a => a.Name == name);

            if (artist == null)
            {
                return NotFound();
            }

            return artist;
        }

        // PUT: api/Artists/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArtist(int id, Artist artist)
        {
            if (id != artist.Id)
            {
                return BadRequest();
            }

            _context.Entry(artist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtistExists(artist))
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

        // POST: api/Artists
        [HttpPost]
        public async Task<ActionResult<Artist>> PostArtist([FromBody] List<Artist> artists)
        {
            foreach (var artist in artists)
            {
                if (!ArtistExists(artist))
                {
                    _context.Artists.Add(artist);
                }
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArtists", _context.Artists);
        }

        // DELETE: api/Artists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtist(int id)
        {
            var artist = await _context.Artists.FirstOrDefaultAsync(a => a.Id == id);
            if (artist == null)
            {
                return NotFound();
            }

            _context.Artists.Remove(artist);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArtistExists(Artist artist)
        {
            return _context.Artists.Any(e => e.Id == artist.Id && e.Name == artist.Name);
        }
    }
}
