namespace PetAdoptApp.Pages;

[QueryProperty(nameof(Pet), "Pet")]
public partial class PetDetailsPage : ContentPage
{
    private Pet? _pet;
    public Pet? Pet
    {
        get => _pet;
        set
        {
            _pet = value;
            OnPropertyChanged(nameof(Pet)); 
            BindingContext = _pet; 
        }
    }
    public PetDetailsPage()
	{ 
        InitializeComponent();
        BindingContext = this;
    }

    private async void GoToBack(object sender, EventArgs e)
    {
        await AppShell.Current.GoToAsync("..");
    }

    private async void OnTelegramImageButtonClicked(object sender, EventArgs e)
    {
        string telegramUsername = Pet.UserLink;
        string[] telegramUris = {
        $"tg://resolve?domain={telegramUsername}",
        $"tg://t.me/{telegramUsername}",
        $"https://t.me/{telegramUsername}"
    };

        foreach (var uri in telegramUris)
        {
            bool canOpen = await Launcher.CanOpenAsync(uri);

            if (canOpen)
            {
                await Launcher.OpenAsync(uri);
                return; 
            }
        }
        await DisplayAlert("Error", "Telegram was not found. Install it.", "OK");
    }
}


