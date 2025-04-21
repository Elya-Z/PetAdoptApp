namespace PetAdoptApp.Models;

[Table("Pets")]
public class Pet : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }
    [Column("name")]
    public string? Name { get; set; }
    [Column("breed")]
    public string? Breed { get; set; }
    [Column("age")]
    public float? Age { get; set; }
    [Column("weight")]
    public decimal? Weight { get; set; }
    [Column("address")]
    public string? Address { get; set; }
    [Column("about")]
    public string? About { get; set; }
    [Column("sex")]
    public string? Sex { get; set; }
    [Column("username")]
    public string? Username { get; set; }
    [Column("userImage")]
    public string? UserImage { get; set; }
    [Column("userLink")]
    public string? UserLink { get; set; }
    [Column("categoryId")]
    public int? CategoryId { get; set; }
    [Column("imageUrl")]
    public string? ImageUrl { get; set; }
    [Column ("isFavorite")]
    public bool IsFavorite { get; set; }

    public ImageSource? ImageSource { get; set; }
    public Pet() {}

    public Pet(string name, string breed, string imageUrl, float age)
    {
        Name = name;
        Breed = breed;
        ImageUrl = imageUrl;
        Age = age;
    }
}