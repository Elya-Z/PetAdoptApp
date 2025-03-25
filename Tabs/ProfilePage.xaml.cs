using PetAdoptApp.Contexts;
using PetAdoptApp.Models;
using PetAdoptApp.Pages;

namespace PetAdoptApp.Tabs;

public partial class ProfilePage : ContentPage
{
    public string WelcomeMessage { get; set; } = "";
    public string WelcomeMessageTwo { get; set; } = "";

    public ProfilePage()
	{
		InitializeComponent();
        LoadData();
        LoadDataTwo();
        BindingContext = this;
    }

    private void LoadData()
    {
        var currentUser = CurrentUser.Email;

        using (AppDbContext dbContext = new AppDbContext())
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Email == currentUser);
            WelcomeMessage = $"{user.Surname} {user.Name} {user.Patronymic}";
        }
    }

    private void LoadDataTwo()
    {
        var currentUser = CurrentUser.Email;

        using (AppDbContext dbContext = new AppDbContext())
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Email == currentUser);
           
            WelcomeMessageTwo = $"{user.Email}";
        }
    }

    private void OnButtonTapped(object sender, EventArgs e)
    {
        // Handle button tap
        DisplayAlert("Button Tapped", "You tapped the button!", "OK");
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