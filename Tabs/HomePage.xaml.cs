using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using PetAdoptApp.Contexts;
using PetAdoptApp.Models;
using System.Collections.ObjectModel;


namespace PetAdoptApp.Tabs;

public partial class HomePage : ContentPage
{
    private readonly string[] _imageUrls = new string[]
        {
            "https://avatars.mds.yandex.net/i?id=1bf0f10e5bceac695d5239040c732aaea0cf5b06-5209794-images-thumbs&n=13",
            "https://creativo.one/adds/adds27062/393baf233768477fd688216d19e39a62ce070a3d.jpg"

        };
    public ObservableCollection<string> ImageUrls { get; set; }

   
    public string WelcomeMessage { get; set; } = "";

    public HomePage()
	{
        ImageUrls = new ObservableCollection<string>(_imageUrls);
        InitializeComponent();
        LoadData();
        BindingContext = this;

      
    }

    private void LoadData()
    {
        var currentUser = CurrentUser.Email;

        using (AppDbContext dbContext = new AppDbContext())
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Email == currentUser);
            WelcomeMessage = $"{user.Surname} {user.Name}";
            
        }
    }


}
