using Supabase.Postgrest.Attributes;
namespace PetAdoptApp.Models;

[Table("Pets")]
public class Pet : BaseModel
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Breed { get; set; }
    public float Age { get; set; }

    public string CategoryId { get; set; }
    public string ImageUrl { get; set; }

    public Pet()
    {
    }

    public Pet(string name, string breed, string imageUrl, float age)
    {
        Name = name;
        Breed = breed;
        ImageUrl = imageUrl;
        Age = age;
    }

    public override string ToString()
    {
        return $"Pet: {Name}, Breed: {Breed}, ImageUrl: {ImageUrl}, Age: {Age}";
    }
}