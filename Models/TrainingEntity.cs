namespace PetAdoptApp.Models;

public class TrainingEntity
{
    public int Id_Training { get; set; }
    public int Id_User { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DataCreate { get; set; }
    public ICollection<ExerciseEntity> Exercises { get; set; } = new List<ExerciseEntity>();
    public UserEntity User { get; set; }
    public TrainingEntity() { }
    public TrainingEntity(string title, string description, DateTime dataCreate, UserEntity user)
    {
        Title = title;
        Description = description;
        DataCreate = dataCreate;
        Id_User = CurrentUser.Id_Cur_User;
    }
}