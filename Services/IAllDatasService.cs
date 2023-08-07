using Book_Store.Dtos;

namespace Book_Store.Services;

public interface IAllDatasService
{
  Task<ResponceAllData> CreateAuthor(CreateAllData model);
  Task<ResponceAllData> CreateGenre(CreateAllData model);
  Task<ResponceAllData> CreatePublisher(CreateAllData model);
  Task<ResponceAllData> CreateDiscounts(Discount model);
  Task<ResponceAllData> CreateBook(CreateBook model);
  Task<List<Book>> GetAllBooks();
  Task<List<Author>> GetAllAuthors();
  Task<List<Publisher>> GetAllPublishers();
  Task<List<Genres>> GetAllGenres();
  Task<List<Book>> GetAllDefaultBooks();
}