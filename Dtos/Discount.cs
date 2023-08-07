using System.ComponentModel.DataAnnotations;

namespace Book_Store.Dtos;

public class Discount
{
  public int? bookId { get; set; }
  public int? publisherId { get; set; }
  public int? authorId { get; set; }
  [Required]
  public int percentage { get; set; }

  [Required] public DateTime? startDate { get; set; } = DateTime.Parse("2023-08-08T12:57:22Z");
  [Required]
  public DateTime? endDate { get; set; } = DateTime.Parse("2023-08-08T12:57:22Z");
}