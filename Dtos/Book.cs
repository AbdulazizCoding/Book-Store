namespace Book_Store.Dtos;

public class Book
{
  public string? name { get; set; }
  public string? author { get; set; }
  public string? publisher { get; set; }
  public int? year { get; set; }
  public string? genre { get; set; }
  public decimal? price { get; set; }
}