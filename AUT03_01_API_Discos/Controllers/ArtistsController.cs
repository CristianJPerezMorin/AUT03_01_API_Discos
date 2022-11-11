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
    public class ArtistsController : ControllerBase
    {
        private readonly ChinookContext _context;

        public ArtistsController(ChinookContext context)
        {
            _context = context;
        }

        // GET: api/Artists
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Artist>>> GetArtists()
        {
            return Ok(await _context.Artists.Include(a => a.Albums).OrderByDescending(a => a.Name).Take(10).ToListAsync());
        }

        // GET: api/Artists/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Artist>> GetArtist(int id)
        {
            var artist = await _context.Artists.Include(a => a.Albums).FirstOrDefaultAsync(a => a.ArtistId == id);

            if (artist == null)
            {
                return NotFound("Error: No se ha encontrado el Artista especificado.");
            }

            return Ok(artist);
        }

        // PUT: api/Artists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutArtist(int id, Artist artist)
        {
            if (!ArtistExists(id))
            {
                return NotFound("Error: El Artista no se encuentra en la base de datos");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("Error: Los campos de album no cumplen las restricciones.");
            }
            else
            {

                try
                {
                    artist.ArtistId = id;
                    _context.Update(artist);
                    await _context.SaveChangesAsync();
                    return NoContent();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistExists(id))
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

        // POST: api/Artists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostArtist(Artist artist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Error: Unos de los campos no cumple las retricciones.");
            }
            _context.Artists.Add(artist);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetArtist", new { id = artist.ArtistId }, artist);
        }

        // DELETE: api/Artists/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteArtist(int id)
        {
            var artist = await _context.Artists.FindAsync(id);
            if (artist == null)
            {
                return NotFound("Error: No se ha encontrado el Artista especificado.");
            }

            _context.Artists.Remove(artist);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool ArtistExists(int id)
        {
            return _context.Artists.Any(e => e.ArtistId == id);
        }
    }
}
