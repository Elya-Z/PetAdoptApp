namespace PetAdoptApp.Models;

public class Category: BaseModel
{
    [PrimaryKey("id")]
    public string? Id { get; set; }
    [Column("name")]
    public string? Name { get; set; }
    public override string? ToString() => Name;
}
