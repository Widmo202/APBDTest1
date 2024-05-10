using APBDTest.Models.DTOs;
using Microsoft.Data.SqlClient;

namespace APBDTest.Repositories;

public class BooksRepository : IBooksRepository
{
    
    private readonly IConfiguration _configuration;
    public BooksRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<BookDTO> GetBook(int id)
    {
        var query = @"SELECT 
							books.PK AS ID,
							books.title AS Title,
							authors.first_name AS AuthorName,
							authors.last_name as AuthorLastName,
                            genres.name AS Genre
						FROM books
						JOIN books_authors ON books.PK = books_authors.FK_book
						JOIN authors ON books_authors.FK_author = authors.PK
						JOIN books_genres ON books_genres.FK_book = books.PK
						JOIN genres ON books_genres.FK_genre = genres.PK
						WHERE books.PK = @ID";
        
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", id);
        
        await connection.OpenAsync();
        var reader = await command.ExecuteReaderAsync();
        var bookIdOrdinal = reader.GetOrdinal("ID");
        var bookTitleOrdinal = reader.GetOrdinal("Title");
        var authorNameOrdinal = reader.GetOrdinal("AuthorName");
        var authorLastNameOrdinal = reader.GetOrdinal("AuthorLastName");
        var genreOrdinal = reader.GetOrdinal("Genre");
        
        BookDTO bookDto = null;
        while (await reader.ReadAsync())
        {
            
            if (bookDto is not null)
            {
                var author = new AuthorDTO()
                {
                    FirstName = reader.GetString(authorNameOrdinal),
                    LastName = reader.GetString(authorLastNameOrdinal)
                };
                bool contains = false;
                foreach (var auth in bookDto.Authors)
                {
                    if (auth.FirstName.Equals(author.FirstName) && auth.LastName.Equals(author.LastName))
                    {
                        contains = true;
                    }
                }

                if (!contains)
                {
                    bookDto.Authors.Add(author);
                }

                var genre = reader.GetString(genreOrdinal);
                if (!bookDto.Genres.Contains(genre))
                {
                    bookDto.Genres.Add(genre);
                }
            }
            else
            {
                bookDto = new BookDTO()
                {
                    Id = reader.GetInt32(bookIdOrdinal),
                    Title = reader.GetString(bookTitleOrdinal),
                    Authors = new List<AuthorDTO>()
                    {
                        new AuthorDTO()
                        {
                            FirstName = reader.GetString(authorNameOrdinal),
                            LastName = reader.GetString(authorLastNameOrdinal)
                        }
                        
                    },
                    Genres = new List<String>()
                    {
                       reader.GetString(genreOrdinal)
                    }
                };
            }
            
        }
        if (bookDto is null) throw new Exception();
        
        return bookDto;
    }

    public async Task<bool> DoesBookExist(int id)
    {
        var query = "SELECT 1 FROM books WHERE books.PK = @ID";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", id);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();

        return res is not null;
    }

}