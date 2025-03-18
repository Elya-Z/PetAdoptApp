namespace PetAdoptApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        Routing.RegisterRoute(nameof(MenuPage), typeof(MenuPage));
        Routing.RegisterRoute(nameof(CreateTrainingPage), typeof(CreateTrainingPage));
        Routing.RegisterRoute(nameof(AddExercisePage), typeof(AddExercisePage));
        Routing.RegisterRoute(nameof(AllExercisesPage), typeof(AllExercisesPage));
        Routing.RegisterRoute(nameof(AllTrainingsPage), typeof(AllTrainingsPage));
        Routing.RegisterRoute(nameof(AutorizationPage), typeof(AutorizationPage));

    }

}
