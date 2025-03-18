namespace PetAdoptApp.Models
{
    public class ExerciseEntity
    {
        public int Id_Exercise { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ExerciseEntity() { }
        public List<TrainingEntity> Trainings { get; set; } = [];
        public ExerciseEntity(string title, string description)
        {
            Title = title; Description = description;
        }
    }
}
