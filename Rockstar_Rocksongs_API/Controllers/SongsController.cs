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
    public class SongsController : ControllerBase
    {
        private readonly RockLibraryContext _context;

        public SongsController(RockLibraryContext context)
        {
            _context = context;
        }

        // GET: api/Songs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Song>>> GetSongs()
        {
            return await _context.Songs.ToListAsync();
        }

        // GET: api/Songs/GetById/5
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Song>> GetSong(int id)
        {
            var song = await _context.Songs.FirstOrDefaultAsync(s => s.Id == id);

            if (song == null)
            {
                return NotFound();
            }

            return song;
        }

        // GET: api/Songs/GetByGenre/Metal
        [HttpGet("GetByGenre/{genre}")]
        public async Task<ActionResult<Song>> GetSongByGenre(string genre)
        {
            var song = await _context.Songs.FirstOrDefaultAsync(s => s.Genre == genre);

            if (song == null)
            {
                return NotFound();
            }

            return song;
        }

        // PUT: api/Songs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSong(int id, Song song)
        {
            if (id != song.Id)
            {
                return BadRequest();
            }

            _context.Entry(song).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SongExists(song))
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

        // POST: api/Songs
        [HttpPost]
        public async Task<ActionResult<Song>> PostSong([FromBody] List<Song> songs)
        {
            foreach (var song in songs)
            {
                if ((song.Genre.Contains("Metal") || song.Year < 2016) && !SongExists(song))
                {
                    _context.Songs.Add(song);
                }
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSongs", new { id = _context.Songs.OrderBy(a => a.Id).LastOrDefault().Id }, _context.Songs);
        }

        // DELETE: api/Songs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSong(int id)
        {
            var song = await _context.Songs.FirstOrDefaultAsync(s => s.Id == id);
            if (song == null)
            {
                return NotFound();
            }

            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SongExists(Song song)
        {
            return _context.Songs.Any(e => e.Id == song.Id && e.Name == song.Name);
        }
    }
}
