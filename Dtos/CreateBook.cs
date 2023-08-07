using System.ComponentModel.DataAnnotations;

namespace Book_Store.Dtos;

public class CreateBook
{
  [Required]
  public string? name { get; set; }
  [Required]
  public int? authorId { get; set; }
  [Required]
  public int? publisherId { get; set; }
  [Required]
  public int? year { get; set; }
  [Required]
  public int? genreId { get; set; }
  [Required]
  public decimal? price { get; set; }
}