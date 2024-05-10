namespace APBDTest.Repositories;

public interface IGenresRepository
{
    public Task<bool> DeleteGenre(int id);
    public Task<bool> DoesGenreExist(int id);
}