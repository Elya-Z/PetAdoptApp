using PetAdoptApp.Tabs;
using Supabase.Gotrue.Exceptions;

namespace PetAdoptApp;

public partial class AutorizationPage : ContentPage
{
    private readonly AuthenticationService _authService;
    public AutorizationPage()
    {
        InitializeComponent();
        _authService = new AuthenticationService();
    }

    private async void GoToRegistration(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(RegistrationPage), true);
    }

    private async void GoToMenu(object sender, EventArgs e)
    {
        try
        {
            var email = EmailEntry.Text;
            var password = PasswordEntryOne.Text;

            await _authService.LoginAsync(email, password);
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}", true);
        }
        catch (ArgumentException)
        {
            await DisplayAlert("Error","Email shouldn't be empty", "OK");
        }
        catch (GotrueException ex)
        {
            string msg = ex.Reason switch
            {
                FailureHint.Reason.UserBadEmailAddress => "Incorrect email",
                FailureHint.Reason.UserBadPassword => "Password must contain 6 characters",
                _ => "Incorrect data "
            };
            await DisplayAlert("Error", msg, "OK");
        }
    }

    private void PasswordOnOne(object sender, EventArgs e)
    {
        PasswordEntryOne.IsPassword = !PasswordEntryOne.IsPassword;

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