using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Book_Store.Entities;

public class Author
{
  [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int id { get; set; }
  public string name { get; set; }
  public List<Book> books { get; set; }
}