using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AUT03_01_API_Discos.Data;
using AUT03_01_API_Discos.Models;
using Microsoft.AspNetCore.Authorization;

namespace AUT03_01_API_Discos.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AlbumsController : ControllerBase
    {
        private readonly ChinookContext _context;

        public AlbumsController(ChinookContext context)
        {
            _context = context;
        }

        // GET: api/Albums
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Album>>> GetAlbums()
        {
            var albums = await _context.Albums.Include(a => a.Artist).Include(a => a.Tracks).Take(10).OrderByDescending(a => a.Title).ToListAsync();
            return Ok();
        }

        // GET: api/Albums/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Album>> GetAlbum(int id)
        {
            var album = await _context.Albums.Include(a => a.Artist).Include(a => a.Tracks).FirstOrDefaultAsync(a => a.AlbumId == id);
            
            if (album == null)
            {
                return NotFound("Error: No se ha encontrado el Album especificado.");
            }

            return Ok(album);
        }

        // PUT: api/Albums/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutAlbum(int id, Album album)
        {
            if (!AlbumExists(id))
            {
                return NotFound("Error: El Album no se encuentra en la base de datos");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("Error: Los campos de album no cumplen las restricciones.");
            }
            else
            {
                var artists = await _context.Artists.ToListAsync();
                var artist = artists.Find(a => a.Name == album.Artist.Name);
                if (artist == null)
                {
                    return BadRequest("Error: El artista no se encuentra en la base de datos.");
                }

                try
                {
                    album.AlbumId = id;
                    album.Artist = artist;
                    album.ArtistId = artist.ArtistId;
                    _context.Update(album);
                    await _context.SaveChangesAsync();
                    return NoContent();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumExists(id))
                    {
                        return NotFound("Error: No se ha encontrado el Album especificado.");
                    }
                    else
                    {
                        throw;
                    }
                }
            }

        }

        // POST: api/Albums
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAlbum(Album album)
        {
            var artists = await _context.Artists.ToListAsync();
            var artist = artists.Find(a => a.Name == album.Artist.Name);
            if (!ModelState.IsValid)
            {
                return BadRequest("Error: Unos de los campos no cumple las retricciones.");
            }
            if(artist == null)
            {
                return BadRequest("Error: El artista no se encuentra en la base de datos.");
            }
            return CreatedAtAction("GetAlbum", new { id = album.AlbumId }, album);
        }

        // DELETE: api/Albums/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAlbum(int id)
        {
            var album = await _context.Albums.FindAsync(id);
            if (album == null)
            {
                return NotFound("Error: No se ha encontrado el Album especificado.");
            }

            _context.Albums.Remove(album);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool AlbumExists(int id)
        {
            return _context.Albums.Any(e => e.AlbumId == id);
        }
    }
}
