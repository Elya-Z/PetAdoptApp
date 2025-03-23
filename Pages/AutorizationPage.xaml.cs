using PetAdoptApp.Contexts;
using PetAdoptApp.Models;
using PetAdoptApp.Tabs;

namespace PetAdoptApp
{
    public partial class AutorizationPage : ContentPage
    {
        public AutorizationPage()
        {
            InitializeComponent();
        }

        private async void GoToRegistration(object sender, EventArgs e)
        {
            await AppShell.Current.GoToAsync(nameof(RegistrationPage), true);
        }

        private async void GoToMenu(object sender, EventArgs e)
        {
            string email = EmailEntry.Text;
            string password = PasswordEntryOne.Text;

            bool isLoginAndPasswordEmpty = string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password);
            if (isLoginAndPasswordEmpty)
            {
                await AppShell.Current.DisplayAlert("Error", "Field cannot be empty!", "OK");
                return;
            }

            using (AppDbContext dbContext = new AppDbContext())
            {
                var user = dbContext.Users.FirstOrDefault(u => u.Email == email);
                if (user == null)
                {
                    await DisplayAlert("Error", "User with this email not found!", "OK");
                    return;
                }

                if (user.Password != password)
                {
                    await DisplayAlert("Error", "Incorrect password!", "OK");
                    return;
                }

                CurrentUser.Id_Cur_User = user.Id_User;
                CurrentUser.Email = user.Email;
                CurrentUser.Name = user.Name;
                CurrentUser.Surname = user.Surname;
                await AppShell.Current.GoToAsync($"//{nameof(HomePage)}");
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
}