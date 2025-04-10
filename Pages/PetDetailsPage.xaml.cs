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
}


