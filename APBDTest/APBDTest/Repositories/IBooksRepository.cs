using APBDTest.Models.DTOs;

namespace APBDTest.Repositories;

public interface IBooksRepository
{
    Task<BookDTO> GetBook(int id);
    Task<bool> DoesBookExist(int id);
}