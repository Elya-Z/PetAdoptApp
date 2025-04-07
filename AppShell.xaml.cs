using PetAdoptApp.Pages;
using PetAdoptApp.Tabs;

namespace PetAdoptApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        Routing.RegisterRoute(nameof(AutorizationPage), typeof(AutorizationPage));
        Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        Routing.RegisterRoute(nameof(FavoritePage), typeof(FavoritePage));
        Routing.RegisterRoute(nameof(InboxPage), typeof(InboxPage));
        Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
        Routing.RegisterRoute(nameof(AddPetPage), typeof(AddPetPage));
        Routing.RegisterRoute(nameof(PetDetailsPage), typeof(PetDetailsPage));




    }

}
