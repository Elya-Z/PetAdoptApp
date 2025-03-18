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

        private void GoToAutorizationPage(object sender, EventArgs e)
        {
            AppShell.Current.GoToAsync(nameof(AutorizationPage), true);
        }

        

    }

}
