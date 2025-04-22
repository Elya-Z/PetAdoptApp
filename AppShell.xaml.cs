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
        Routing.RegisterRoute("home", typeof(HomePage));
        Routing.RegisterRoute("favorite", typeof(FavoritePage));
        Routing.RegisterRoute("profile", typeof(ProfilePage));
        Routing.RegisterRoute(nameof(AddPetPage), typeof(AddPetPage));
        Routing.RegisterRoute("profile/myPosts", typeof(MyPostPage));
        Routing.RegisterRoute("petdetails", typeof(PetDetailsPage));
    }
}
