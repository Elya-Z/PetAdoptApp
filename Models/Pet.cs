namespace PetAdoptApp.Models;

[Table("Pets")]
public class Pet : BaseModel
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Breed { get; set; }
    public float? Age { get; set; }
    public decimal? Weight { get; set; }
    public string? Address { get; set; }
    public string? About { get; set; }
    public string? Sex { get; set; }
    [Column("username")]
    public string? Username { get; set; }
    public string? UserImage { get; set; }
    public string? UserLink { get; set; }
    public string? CategoryId { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsFavorite { get; set; }

    public Pet() {}

    public Pet(string name, string breed, string imageUrl, float age)
    {
        Name = name;
        Breed = breed;
        ImageUrl = imageUrl;
        Age = age;
    }
}