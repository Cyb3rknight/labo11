using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using semaine11_serveur.Data;
using semaine11_serveur.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace semaine11_serveur.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        private readonly semaine11_serveurContext _context;

        public PicturesController(semaine11_serveurContext context)
        {
            _context = context;
        }

        [HttpGet("GetPictureIds")]
        public async Task<ActionResult<List<int>>> GetPictureIds()
        {
            var pictureIds = await _context.Picture.Select(p => p.Id).ToListAsync();
            return Ok(pictureIds);
        }

        [HttpGet("{size}/{id}")]
        public async Task<IActionResult> GetPicture(string size, int id)
        {
            var picture = await _context.Picture.FindAsync(id);
            if (picture == null)
            {
                return NotFound();
            }

            string filePath;
            if (size.ToLower() == "small")
            {
                filePath = Path.Combine("images", "small", Path.GetFileName(picture.FilePath));
            }
            else
            {
                filePath = Path.Combine("images", "large", Path.GetFileName(picture.FilePath));
            }

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(fileBytes, "image/png"); // Assurez-vous que le type MIME est correct
        }


        [HttpPost("PostPicture")]
        public async Task<ActionResult> PostPicture([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Aucun fichier fourni.");
            }

            var picture = new Picture
            {
                CreatedAt = DateTime.UtcNow
            };

            var largePath = Path.Combine("images", "large", file.FileName);
            var smallPath = Path.Combine("images", "small", file.FileName);

            using (var stream = new FileStream(largePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            using (var image = Image.Load(file.OpenReadStream()))
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(0, 200),
                    Mode = ResizeMode.Max
                }));

                await image.SaveAsync(smallPath);
            }

            picture.FilePath = largePath;
            _context.Picture.Add(picture);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPictureIds), new { id = picture.Id }, picture);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePicture(int id)
        {
            var picture = await _context.Picture.FindAsync(id);
            if (picture == null)
            {
                return NotFound();
            }

            var largePath = Path.Combine("images", "large", Path.GetFileName(picture.FilePath));
            var smallPath = Path.Combine("images", "small", Path.GetFileName(picture.FilePath));

            if (System.IO.File.Exists(largePath))
            {
                System.IO.File.Delete(largePath);
            }

            if (System.IO.File.Exists(smallPath))
            {
                System.IO.File.Delete(smallPath);
            }

            _context.Picture.Remove(picture);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}