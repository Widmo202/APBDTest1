using System.ComponentModel.DataAnnotations;

namespace APBDTest.Models.DTOs;

public class BookDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public List<AuthorDTO> Authors { get; set; } = null!;
    public List<String> Genres { get; set; } = null!;
}
public class AuthorDTO
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
