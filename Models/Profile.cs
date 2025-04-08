using Supabase.Postgrest.Attributes;

namespace PetAdoptApp.Models;

public class Profile: BaseModel
{
    [Column("user_id")]
    public string UserId { get; set; } = "";
    [Column("surname")]
    public string Surname { get; set; } = "";
    [Column("firstname")]
    public string Firstname { get; set; } = "";

    public Profile()
    {
    }

    public Profile(string userId, string surname, string firstname)
    {
        UserId = userId;
        Surname = surname;
        Firstname = firstname;
    }
}