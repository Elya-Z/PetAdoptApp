namespace PetAdoptApp.Models;

public class Favorite: BaseModel
{
    [Column("pet_id")]
    public string PetId { get; set; }
    [Column("profile_id")]
    public string ProfileId { get; set; }
}