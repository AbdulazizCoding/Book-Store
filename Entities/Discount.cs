namespace Book_Store.Entities;

public class Discount
{
  public int id { get; set; }
  public int bookId { get; set; }
  public int publisherId { get; set; }
  public int authorId { get; set; }
  public int percentage { get; set; }
  public DateTime startDate { get; set; }
  public DateTime endDate { get; set; }
  public bool isActve { get; set; } 
}