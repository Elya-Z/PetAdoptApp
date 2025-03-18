using PetAdoptApp.Contexts;
using PetAdoptApp.Models;

namespace PetAdoptApp
{
    public partial class AutorizationPage : ContentPage
    {
        public AutorizationPage()
        {
            InitializeComponent();
        }

        private void GoToRegistration(object sender, EventArgs e)
        {
            AppShell.Current.GoToAsync(nameof(RegistrationPage), true);
        }

        private void GoToMenu(object sender, EventArgs e)
        {
            string email = EmailEntry.Text;
            string password = PasswordEntryOne.Text;

            bool isLoginAndPasswordEmpty = string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password);
            if (isLoginAndPasswordEmpty)
            {
                AppShell.Current.DisplayAlert("Error", "Field cannot be empty!", "OK");
                return;
            }

            using (AppDbContext dbContext = new AppDbContext())
            {
                var user = dbContext.Users.FirstOrDefault(u => u.Login == email);
                if (user == null)
                {
                    DisplayAlert("Error", "User with this login not found!", "OK");
                    return;
                }

                if (user.Password != password)
                {
                    DisplayAlert("Error", "Incorrect password!", "OK");
                    return;
                }

                CurrentUser.Id_Cur_User = user.Id_User;
                CurrentUser.Login = user.Login;
                CurrentUser.Name = user.Name;
                CurrentUser.Surname = user.Surname;
                AppShell.Current.GoToAsync(nameof(MenuPage), true);
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