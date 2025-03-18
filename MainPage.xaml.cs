using PetAdoptApp.Contexts;
using PetAdoptApp.Models;

namespace PetAdoptApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();  
        }

        private void GoToRegistration(object sender, EventArgs e)
        {
                
            AppShell.Current.GoToAsync(nameof(RegistrationPage), true);
        }

        private void GoToMenu(object sender, EventArgs e)
        {
            string login = loginx.Text;
            string password = PasswordEntryOne.Text;

            bool isLoginAndPasswordEmpty = string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password);
            if (isLoginAndPasswordEmpty)
            {

                AppShell.Current.DisplayAlert("Ошибка", "Поле не может быть пустым!", "ОК");
                return;
            }

            using (AppDbContext dbContext = new AppDbContext())
            {
                var user = dbContext.Users.FirstOrDefault(u => u.Login == login);
                if (user == null)
                {
                    DisplayAlert("Ошибка", "Пользователь с таким логином не найден!", "ОК");
                    return;
                }

                if (user.Password != password)
                {
                    DisplayAlert("Ошибка", "Неверный пароль!", "ОК");
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
