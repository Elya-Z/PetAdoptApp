using PetAdoptApp.Contexts;
using PetAdoptApp.Models;

namespace PetAdoptApp;

public partial class RegistrationPage : ContentPage
{
    public RegistrationPage()
    {
        InitializeComponent();
    }

    private async void GoToMain(object sender, EventArgs e)
    {
        await AppShell.Current.GoToAsync(nameof(AutorizationPage), true);
    }

    private async void GoToMenu(object sender, EventArgs e)
    {
        var LastName = famx.Text;
        var FirstName = namex.Text;
        var MiddleName = patrx.Text;
        var email = emailx.Text;
        var Password = PasswordEntryOne.Text;
        var ConfirmPassword = PasswordEntryTwo.Text;

        bool Entry = string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword);
        if (Entry)
        {
            await AppShell.Current.DisplayAlert("Error", "All fields must be filled!", "OK");
            return;
        }

        if (Password != ConfirmPassword)
        {
            await DisplayAlert("Error", "Passwords do not match!", "OK");
            return;
        }

        using (AppDbContext dbContext = new AppDbContext())
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                await DisplayAlert("Error", "User with this username already exists!", "OK");
                return;
            }
            var newUser = new UserEntity
            {
                Surname = LastName,
                Name = FirstName,
                Patronymic = MiddleName,
                Email = email,
                Password = Password
            };
            dbContext.Users.Add(newUser);
            dbContext.SaveChanges();
        }
        await AppShell.Current.GoToAsync(nameof(AutorizationPage), true);
    }

    private void PasswordOnOne(object sender, EventArgs e)
    {
        PasswordEntryOne.IsPassword = !PasswordEntryOne.IsPassword;
        PasswordEntryTwo.IsPassword = !PasswordEntryTwo.IsPassword;

        if (PasswordEntryOne.IsPassword)
        {
            ((ImageButton)sender).Source = "not_visible.png";
        }
        else
        {
            ((ImageButton)sender).Source = "visible.png";
        }
    }
}