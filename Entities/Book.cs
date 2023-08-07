using System.ComponentModel.DataAnnotations.Schema;

namespace Book_Store.Entities;

public class Book
{
  public int Id { get; set; }
  public string name { get; set; }
  public int authorId { get; set; }
  [ForeignKey(nameof(authorId))]
  public Author author { get; set; }
  public int publisherId { get; set; }
  [ForeignKey(nameof(publisherId))]
  public Publisher publisher { get; set; }
  public int year { get; set; }
  public int genreId { get; set; }
  [ForeignKey(nameof(genreId))]
  public Genres genre { get; set; }
  public decimal price { get; set; }
  public int percentage { get; set; }
  public decimal percentagePrice { get; set; }
}