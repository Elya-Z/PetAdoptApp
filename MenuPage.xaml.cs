using PetAdoptApp.Contexts;
using PetAdoptApp.Models;

namespace PetAdoptApp;

public partial class MenuPage : ContentPage
{
    public string WelcomeMessage { get; set; }
    public string Statistics { get; set; }
    public List<TrainingEntity> RecentTrainings { get; set; }

    public MenuPage()
    {
        InitializeComponent();
        LoadData();
        BindingContext = this;
    }

    private void LoadData()
    {
        var currentUser = CurrentUser.Login;

        using (AppDbContext dbContext = new AppDbContext())
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Login == currentUser);
            WelcomeMessage = $"Добро пожаловать, {user.Surname} {user.Name} {user.Patronymic}!";
            RecentTrainings = dbContext.TrainingEntities
                .Where(t => t.Id_User == user.Id_User)
                .OrderByDescending(t => t.DataCreate)
                .Take(5)
                .ToList();
        }
    }

    private async void GoToCreateTraining(object sender, EventArgs e)
    {
        await AppShell.Current.GoToAsync(nameof(CreateTrainingPage), true);
    }

    private async void GoToAllTrainings(object sender, EventArgs e)
    {
        await AppShell.Current.GoToAsync(nameof(AllTrainingsPage), true);
    }

    private async void GoToAddExercise(object sender, EventArgs e)
    {
        await AppShell.Current.GoToAsync(nameof(AddExercisePage), true);
    }

    private async void GoToAllExercises(object sender, EventArgs e) =>
        await AppShell.Current.GoToAsync(nameof(AllExercisesPage), true);

    private async void GoToBack(object sender, EventArgs e) =>
        await AppShell.Current.GoToAsync("..", true);

}