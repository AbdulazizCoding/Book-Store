namespace Book_Store.Entities;

public class Publisher
{
  public int id { get; set; }
  public string name { get; set; }
  public List<Book> books { get; set; }
}