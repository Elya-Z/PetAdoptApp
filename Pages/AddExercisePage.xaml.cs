using PetAdoptApp.Contexts;
using PetAdoptApp.Models;

namespace PetAdoptApp;

public partial class AddExercisePage : ContentPage
{
    public AddExercisePage()
    {
        InitializeComponent();
    }
    private void AddExercise(object sender, EventArgs e)
    {
        var title = TitleEntry.Text;
        var description = DescriptionEntry.Text;

        if (string.IsNullOrWhiteSpace(title))
        {
            DisplayAlert("Ошибка", "Заголовок упражнения не может быть пустым!", "ОК");
            return;
        }

        using (AppDbContext dbContext = new AppDbContext())
        {
            var newExercise = new ExerciseEntity
            {
                Title = title,
                Description = description,
                
            };
            dbContext.ExerciseEntities.Add(newExercise);
            dbContext.SaveChanges();
        }

        DisplayAlert("Успех", "Упражнение добавлено!", "ОК");
        AppShell.Current.GoToAsync(nameof(MenuPage), true);
    }

    private void GoBack(object sender, EventArgs e)
    {
        AppShell.Current.GoToAsync(nameof(MenuPage), true);
    }
}