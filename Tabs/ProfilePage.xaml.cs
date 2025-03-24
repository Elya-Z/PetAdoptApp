using PetAdoptApp.Contexts;
using PetAdoptApp.Models;

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
}