using Book_Store.Data;
using Book_Store.Dtos;
using Book_Store.Entities;
using Microsoft.EntityFrameworkCore;
using Author = Book_Store.Dtos.Author;
using Book = Book_Store.Dtos.Book;
using Discount = Book_Store.Dtos.Discount;
using Genres = Book_Store.Dtos.Genres;
using Publisher = Book_Store.Dtos.Publisher;

namespace Book_Store.Services;

public class AllDatasService : IAllDatasService
{
  private readonly AppDbContext context;

  public AllDatasService(AppDbContext context)
  {
    this.context = context;
  }
  
  public async Task<ResponceAllData> CreateAuthor(CreateAllData model)
  {
    ValidateAllDataModel(model);

    var newAutor = new Entities.Author()
    {
      name = model.name
    };

    await context.Authors.AddAsync(newAutor);
    await context.SaveChangesAsync();

    return new ResponceAllData() { id = newAutor.id };
  }

  public async Task<ResponceAllData> CreateGenre(CreateAllData model)
  {
    ValidateAllDataModel(model);

    var newGenre = new Entities.Genres()
    {
      name = model.name
    };

    await context.Genres.AddAsync(newGenre);
    await context.SaveChangesAsync();

    return new ResponceAllData() { id = newGenre.id };
  }

  public async Task<ResponceAllData> CreatePublisher(CreateAllData model)
  {
    ValidateAllDataModel(model);

    var newPublisher = new Entities.Publisher()
    {
      name = model.name
    };

    await context.Publishers.AddAsync(newPublisher);
    await context.SaveChangesAsync();

    return new ResponceAllData() { id = newPublisher.id };
  }

  public async Task<ResponceAllData> CreateDiscounts(Discount model)
  {
    if (model.percentage <= 0 )
      throw new Exception("Foiz 0 dan katta bo'lishi kerak");

    /*var startDate = DateTime.Parse(model.startDate);
    var endDate = DateTime.Parse(model.endDate);*/

    var startDate = DateTime.UtcNow.AddDays(1);
    var endDate = DateTime.UtcNow.AddDays(7);
    
    if (startDate < DateTime.UtcNow)
      throw new Exception("Boshlanish sana bugun yoki kelasi sana bo'lishi kerak!");
    
    if (endDate <= DateTime.UtcNow)
      throw new Exception("Tushgash vaqti bugungi sanadan katta bo'lishi kerak!");

    var newDiscount = new Entities.Discount()
    {
      percentage = model.percentage,
      startDate = startDate,
      endDate = endDate,
      isActve = true
    };

    if (model.bookId is not null)
    {
      newDiscount.bookId = model.bookId ?? 0;
      await UpdateBookById(model.bookId, model.percentage);
    }

    if (model.authorId is not null)
    {
      newDiscount.authorId = model.authorId ?? 0;
      await UpdateBookByAuthorId(model.authorId, model.percentage);
    }

    if (model.publisherId is not null)
    {
      newDiscount.publisherId = model.publisherId ?? 0;
      await UpdateBookByPublisherId(model.publisherId, model.percentage);
    }

    await context.Discounts.AddAsync(newDiscount);
    await context.SaveChangesAsync();

    return new ResponceAllData() { id = newDiscount.id };
  }

  public async Task<ResponceAllData> CreateBook(CreateBook model)
  {
    if (string.IsNullOrEmpty(model.name) || string.IsNullOrWhiteSpace(model.name))
      throw new Exception("Iltimos kitobni to'g'ri nomini kiriting!");

    if (model.authorId is null || model.authorId <= 0)
      throw new Exception("Iltimos Authorni to'g'ri kiriting");
    
    if(model.publisherId is null || model.publisherId <= 0)
      throw new Exception("Iltimos Publisherni to'g'ri kiriting");
    
    if(model.genreId is null || model.genreId <= 0)
      throw new Exception("Iltimos Janrni to'g'ri kiriting");

    if(model.year is null || model.year <= 0)
      throw new Exception("Iltimos Yilni to'g'ri kiriting");
    
    if(model.price is null || model.price <= 0)
      throw new Exception("Iltimos Narxni to'g'ri kiriting");

    var author = await context.Authors.FirstOrDefaultAsync(a => a.id == model.authorId);
    if (author is null)
      throw new Exception("Siz kiritgan ID bo'yicha Author topilmadi!");

    var publisher = await context.Publishers.FirstOrDefaultAsync(p => p.id == model.publisherId);
    if (publisher is null)
      throw new Exception("Siz kiritgan ID bo'yicha Publisher Topilmadi!");

    var genre = await context.Genres.FirstOrDefaultAsync(g => g.id == model.genreId);
    if (genre is null)
      throw new Exception("Siz kiritgan ID bo'yicha Janr topilmadi!");

    var newBook = new Entities.Book()
    {
      name = model.name,
      authorId = author.id,
      author = author,
      publisherId = publisher.id,
      publisher = publisher,
      genreId = genre.id,
      genre = genre,
      year = model.year ?? 0,
      price = model.price ?? 0
    };

    await context.Books.AddAsync(newBook);
    await context.SaveChangesAsync();

    return new ResponceAllData() { id = newBook.Id };
  }

  public async Task<List<Dtos.Book>> GetAllBooks()
  {
    var books = await context.Books.
                            Include(b => b.author).
                            Include(b => b.publisher).
                            Include(b => b.genre).ToListAsync();

    await Calculate();

    var responce =  books.Select(b => ToDto(b)).ToList();

    return responce;
  }

  public async Task<List<Author>> GetAllAuthors()
  {
    var authors = await context.Authors.Include(a => a.books).ToListAsync();

    return authors.Select(a => ToDto(a)).ToList();
  }

  public async Task<List<Publisher>> GetAllPublishers()
  {
    var publishers = await context.Publishers.Include(p => p.books).ToListAsync();

    return publishers.Select(p => ToDto(p)).ToList();
  }

  public async Task<List<Genres>> GetAllGenres()
  {
    var genres = await context.Genres.Include(g => g.books).ToArrayAsync();

    return genres.Select(g => ToDto(g)).ToList();
  }

  public async Task<List<Book>> GetAllDefaultBooks()
  {
    var books = await context.Books.
      Include(b => b.publisher).
      Include(b => b.author).
      Include(b => b.genre).ToListAsync();

    return books.Select(b => ToDtoDefault(b)).ToList();
  }

  // qo'shimcha metodlar!
  private Dtos.Book ToDto(Entities.Book book)
  {
    var responce = new Dtos.Book()
    {
      name = book.name,
      author = book.author.name,
      publisher = book.publisher.name,
      year = book.year,
      genre = book.genre.name,
    };

    if (book.percentagePrice == 0)
    {
      responce.price = book.price;
    }
    else
    {
      responce.price = book.percentagePrice;
    }

    return responce;
  }

  private Author ToDto(Entities.Author model)
  {
    var author = new Author()
    {
      id = model.id,
      name = model.name,
      books = model.books.Select(b => b.name).ToList()
    };

    return author;
  }
  
  private Publisher ToDto(Entities.Publisher model)
  {
    var publisher = new Publisher()
    {
      id = model.id,
      name = model.name,
      books = model.books.Select(b => b.name).ToList()
    };

    return publisher;
  }
  
  private Genres ToDto(Entities.Genres model)
  {
    var genre = new Genres()
    {
      id = model.id,
      name = model.name,
      books = model.books.Select(b => b.name).ToList()
    };

    return genre;
  }
  
  private Book ToDtoDefault(Entities.Book model)
  {
    var book = new Book()
    {
      name = model.name,
      author = model.author.name,
      publisher = model.publisher.name,
      year = model.year,
      genre = model.genre.name,
      price = model.price
    };

    return book;
  }
  
  private void ValidateAllDataModel(CreateAllData model)
  {
    if (string.IsNullOrEmpty(model.name) || string.IsNullOrWhiteSpace(model.name))
      throw new Exception("Iltimos ma'lumot nomini kiriting!");
  }
  
  private async Task UpdateBookByPublisherId(int? modelPublisherId, int percentage)
  {
    var publisher = await context.Publishers.Include(p => p.books).FirstOrDefaultAsync(p => p.id == modelPublisherId);
    if (publisher is null)
      throw new Exception("SIz kiritgan ID bo'yicha PUBLISHER topilmadi!");

    if (publisher.books.Count > 0)
    {
      await UpdateBook(publisher.books, percentage);
    }
  }

  private async Task UpdateBookByAuthorId(int? modelAuthorId, int percentage)
  {
    var author = await context.Authors.Include(a => a.books).FirstOrDefaultAsync(a => a.id == modelAuthorId);
    if (author is null)
      throw new Exception("Siz kiritgan ID bo'yicha Author topilmadi!");

    if (author.books.Count > 0)
    {
      await UpdateBook(author.books, percentage);
    }
  }

  private async Task UpdateBookById(int? modelBookId, int percentage)
  {
    var book = await context.Books.FirstOrDefaultAsync(b => b.Id == modelBookId);
    if (book is null)
      throw new Exception("Siz kiritgan ID bo'yicha Kitob topilmadi!");

    if (percentage >= book.percentage)
    {
      book.percentagePrice = book.price - ((book.price / 100) * percentage);
      book.percentage = percentage;
    }

    context.Books.Update(book);
    await context.SaveChangesAsync();
  }

  private async Task UpdateBook(List<Entities.Book> books,int percentage)
  {
    foreach (var book in books)
    {
      if (percentage > book.percentage)
      {
        book.percentagePrice = book.price - ((book.price / 100) * percentage);
        
        context.Books.UpdateRange(books);
        await context.SaveChangesAsync();
      }
    }
  }

  private async Task Calculate()
  {
    var discounts = await context.Discounts.ToListAsync();

    foreach (var discount in discounts)
    {
      if (discount.endDate > DateTime.UtcNow)
        discount.isActve = false;

      if (discount.isActve = false)
      {
        if (discount.bookId >= 0)
        {
          var book = await context.Books.FirstOrDefaultAsync(b => b.Id == discount.bookId);

          book.percentagePrice = 0;

          context.Books.Update(book);
          await context.SaveChangesAsync();
        }

        if (discount.publisherId >= 0)
        {
          var publisher = await context.Publishers.FirstOrDefaultAsync(p => p.id == discount.publisherId);

          foreach (var book in publisher.books)
          {
            book.percentagePrice = 0;
            
            context.Books.Update(book);
            await context.SaveChangesAsync();
          }
        }

        if (discount.authorId >= 0)
        {
          var author = await context.Publishers.FirstOrDefaultAsync(a => a.id == discount.authorId);
          
          foreach (var book in author.books)
          {
            book.percentagePrice = 0;
            
            context.Books.Update(book);
            await context.SaveChangesAsync();
          }
        }
      }

      context.Discounts.Update(discount);
    }
  }
}