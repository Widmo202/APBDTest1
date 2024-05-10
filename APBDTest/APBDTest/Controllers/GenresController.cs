using APBDTest.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APBDTest.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenresController : ControllerBase
{
    private readonly IGenresRepository _genresRepository;
    [HttpDelete("{id}")]

    public async Task<IActionResult> DeleteGenre(int id)
    {
        if (!await _genresRepository.DoesGenreExist(id))
            return NotFound($"Genre with given ID - {id} doesn't exist");

        await _genresRepository.DeleteGenre(id);

        
        
        return NoContent();
    }
}