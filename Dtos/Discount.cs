using System.ComponentModel.DataAnnotations;

namespace Book_Store.Dtos;

public class Discount
{
  public int? bookId { get; set; }
  public int? publisherId { get; set; }
  public int? authorId { get; set; }
  [Required]
  public int percentage { get; set; }
  [Required]
  public string? startDate { get; set; }
  [Required]
  public string? endDate { get; set; }
}