using PetAdoptApp.Tabs;
using Supabase.Gotrue.Exceptions;

namespace PetAdoptApp;

public partial class RegistrationPage : ContentPage
{
    private readonly AuthenticationService _authService;
    public RegistrationPage()
    {
        InitializeComponent();
        _authService = new AuthenticationService();
    }

    private async void GoToMain(object sender, EventArgs e)
    {
        await AppShell.Current.GoToAsync(nameof(AutorizationPage), true);
    }

    private async void GoToMenu(object sender, EventArgs e)
    {
        try
        {
            var lastName = famx.Text;
            var firstName = namex.Text;
            var email = emailx.Text;
            var password = PasswordEntryOne.Text;
            var confirm = PasswordEntryTwo.Text;

            if (password != confirm)
            {
                await DisplayAlert("Error", "Passwords must match", "OK");
            }
            await _authService.RegisterAsync(email, password, firstName, lastName);
            await Shell.Current.GoToAsync(nameof(HomePage), true);
        }
        catch (ArgumentException)
        {
            await DisplayAlert("Error", "Email shouldn't be empty", "OK");
        }
        catch (GotrueException ex)
        {
            string msg = ex.Reason switch
            {
                FailureHint.Reason.UserBadEmailAddress => "Incorrect email",
                FailureHint.Reason.UserBadPassword => "Password must contain 6 characters",
                _ => "Incorrect data"
            };
            await DisplayAlert("Error", msg, "OK");
        }
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