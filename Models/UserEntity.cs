namespace PetAdoptApp.Models
{
    public class UserEntity
    {
        public int Id_User { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string? Patronymic { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public UserEntity() { }

        public UserEntity(string surname, string name, string patronymic, string email, string password)
        {
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
            this.Email = email;
            Password = password;
        }

    }
    public static class CurrentUser
    {
        public static int Id_Cur_User { get; set; }
        public static string Email { get; set; }
        public static string Name { get; set; }
        public static string Surname { get; set; }
        public static string? Patronymic { get; set; }

    }
}
