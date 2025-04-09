namespace PetAdoptApp.Tabs;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        WelcomeMessage.Text = $"{AuthenticationService.Profile!.Surname} {AuthenticationService.Profile.Firstname}";
        SubWelcomeMessage.Text = SupabaseService.Session!.User!.Email!;
    }

    private async void GoToAddPetPage(object sender, TappedEventArgs e)
    {
        await AppShell.Current.GoToAsync(nameof(AddPetPage), true);
    }

    private async void GoToFavoritesPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//favorite");
    }

    private async void GoToInboxPage(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//inbox");
    }

    private async void GoToMainPage(object sender, TappedEventArgs e)
    {
        await AppShell.Current.GoToAsync(nameof(MainPage), true);
    }
}