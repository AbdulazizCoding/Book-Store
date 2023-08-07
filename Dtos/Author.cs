namespace Book_Store.Dtos;

public class Author
{
  public int id { get; set; }
  public string name { get; set; }
  public List<string> books { get; set; }
}