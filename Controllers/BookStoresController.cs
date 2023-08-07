using Book_Store.Dtos;
using Book_Store.Services;
using Microsoft.AspNetCore.Mvc;

namespace Book_Store.Controllers;
[ApiController]
public class BookStoresController : ControllerBase
{
  private readonly IAllDatasService service;

  public BookStoresController(IAllDatasService service)
  {
    this.service = service;
  }

  [HttpPost("api/authors")]
  public async Task<IActionResult> CreateAuthor([FromBody] CreateAllData model)
  {
    if (!ModelState.IsValid)
      return BadRequest();

    return Ok(await service.CreateAuthor(model));
  }

  [HttpGet("api/authors")]
  public async Task<IActionResult> GetAuthors()
    => Ok(await service.GetAllAuthors());
  
  [HttpPost("api/genres")]
  public async Task<IActionResult> CreateGenre([FromBody] CreateAllData model)
  {
    if (!ModelState.IsValid)
      return BadRequest();

    return Ok(await service.CreateGenre(model));
  }
  
  [HttpGet("api/genres")]
  public async Task<IActionResult> GetGenres()
    => Ok(await service.GetAllGenres());
  
  [HttpPost("api/publishers")]
  public async Task<IActionResult> CreatePublisher([FromBody] CreateAllData model)
  {
    if (!ModelState.IsValid)
      return BadRequest();

    return Ok(await service.CreatePublisher(model));
  }
  
  [HttpGet("api/publishers")]
  public async Task<IActionResult> GetPublishers()
    => Ok(await service.GetAllPublishers());

  [HttpPost("api/discounts")]
  public async Task<IActionResult> CreateDiscount([FromForm] Discount model)
  {
    if (!ModelState.IsValid)
      return BadRequest();

    return Ok(await service.CreateDiscounts(model));
  }

  [HttpPost("api/books")]
  public async Task<IActionResult> CreateBoos([FromBody] CreateBook model)
  {
    if (!ModelState.IsValid)
      return BadRequest();

    return Ok(await service.CreateBook(model));
  }

  [HttpGet("api/books")]
  public async Task<IActionResult> GetBooks()
    => Ok(await service.GetAllBooks());
  
  [HttpGet("api/books/default")]
  public async Task<IActionResult> GetBooksDefault()
    => Ok(await service.GetAllDefaultBooks());
}
