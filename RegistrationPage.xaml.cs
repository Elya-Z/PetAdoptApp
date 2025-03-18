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
        await AppShell.Current.GoToAsync("..", true);
    }

    private async void GoToMenu(object sender, EventArgs e)
    {
        var Familiya = famx.Text;
        var Name = namex.Text;
        var Patronymic = patrx.Text;
        var Login_ = loginx.Text;
        var Password = PasswordEntryOne.Text;
        var RemPassword = PasswordEntryTwo.Text;

        bool Entry = string.IsNullOrWhiteSpace(Familiya) || string.IsNullOrWhiteSpace(Name) || 
            string.IsNullOrWhiteSpace(Patronymic) || string.IsNullOrWhiteSpace(Login_) || 
            string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(RemPassword);
        if (Entry)
        {
            await AppShell.Current.DisplayAlert("������", "��� ���� ������ ���� ���������!", "��");
            return;
        }

        if (Password != RemPassword)
        {
            await DisplayAlert("������", "������ �� ���������!", "��");
            return;
        }

        using (AppDbContext dbContext = new AppDbContext())
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Login == Login_);
            if (user != null)
            {
                await DisplayAlert("������", "������������ � ����� ������� ��� ����������!", "��");
                return;
            }
            var newUser = new UserEntity
            {
                Surname = Familiya,
                Name = Name,
                Patronymic = Patronymic,
                Login = Login_,
                Password = Password
            };
            dbContext.Users.Add(newUser);

            dbContext.SaveChanges();

        }
        await AppShell.Current.GoToAsync(nameof(MainPage), true);

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