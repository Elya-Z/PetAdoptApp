using System.ComponentModel;
using static Supabase.Postgrest.Constants;

namespace PetAdoptApp.Tabs;

public partial class HomePage : ContentPage, INotifyPropertyChanged
{
    private readonly FavoriteService _favoriteService;
    public ObservableCollection<Pet> Pets { get; } = [];

    private ObservableCollection<Pet> _filteredPets = [];
    public ObservableCollection<Pet> FilteredPets
    {
        get => _filteredPets;
        set
        {
            if (_filteredPets != value)
            {
                _filteredPets = value;
                OnPropertyChanged(nameof(FilteredPets));
            }
        }
    }

    private readonly string[] _imageUrls = [
        "https://avatars.mds.yandex.net/i?id=1bf0f10e5bceac695d5239040c732aaea0cf5b06-5209794-images-thumbs&n=13",
        "https://creativo.one/adds/adds27062/393baf233768477fd688216d19e39a62ce070a3d.jpg",
        "https://img.freepik.com/free-psd/adopt-pet-banner-template-concept_23-2148536759.jpg?semt=ais_hybrid",
        "https://avatars.dzeninfra.ru/get-zen_doc/271828/pub_66b79111019e655302817194_66b791dc3d6e070beb5d94fa/scale_1200",
        "https://img.freepik.com/free-psd/banner-template-with-adopt-pet-concept_23-2148537575.jpg?semt=ais_hybrid"
    ];

    public ObservableCollection<string> ImageUrls { get; set; }
    public string WelcomeMessage { get; set; } = "";

    private string? _selectedCategory;
    public string? SelectedCategory
    {
        get => _selectedCategory;
        set
        {
            if (_selectedCategory != value)
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(_selectedCategory));
                FilterPetsByCategory();
            }
        }
    }

    public HomePage(FavoriteService favoriteService)
    {
        ImageUrls = new ObservableCollection<string>(_imageUrls);
        InitializeComponent();
        _favoriteService = favoriteService;
        LoadData();
        BindingContext = this;
        SelectedCategory = "2";
    }

    private async Task LoadPetsAsync()
    {
        try
        {
            var cancel = new CancellationTokenSource();
            cancel.CancelAfter(10000);
            var token = cancel.Token;

            var userId = AuthenticationService.Profile?.UserId;
            if (string.IsNullOrEmpty(userId))
            {
                await AppShell.Current.DisplayAlert("Ошибка", "Не удалось получить ID пользователя.", "OK");
                return;
            }

            var result = await SupabaseService.SB.From<Pet>()
                .Get(token);

            if (result.Models == null)
                return;

            Pets.Clear();

            foreach (var pet in result.Models)
            {
                var favoriteResult = await SupabaseService.SB.From<Favorite>()
                    .Filter("pet_id", Operator.Equals, pet.Id)
                    .Filter("profile_id", Operator.Equals, userId)
                    .Get(token);

                pet.IsFavorite = favoriteResult.Models?.Any() == true;
                Pets.Add(pet);
            }

            FilteredPets = new ObservableCollection<Pet>(Pets);
            FilterPetsByCategory();
        }
        catch (Exception ex)
        {
            await AppShell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void FilterPetsByCategory()
    {
        string category = string.IsNullOrEmpty(SelectedCategory) ? "2" : SelectedCategory;
        if (string.IsNullOrEmpty(category))
        {
            FilteredPets = new ObservableCollection<Pet>(Pets);
        }
        else
        {
            FilteredPets = new ObservableCollection<Pet>(
                Pets.Where(p => p.CategoryId == category));
        }
    }

    private async void LoadData()
    {
        IsBusy = true;
        try
        {
            if (AuthenticationService.Profile is not null)
            {
                WelcomeMessage = $"{AuthenticationService.Profile.Surname} {AuthenticationService.Profile.Firstname}";
            }
            await LoadPetsAsync();
            FilterPetsByCategory();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"{ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async void GoToAddPet(object sender, EventArgs e)
    {
        await AppShell.Current.GoToAsync(nameof(AddPetPage), true);
    }

    public Command<string> SelectCategoryCommand => new(category => SelectedCategory = category);

    private async void OnPetSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 0) return;
        if (e.CurrentSelection[0] is not Pet selectedPet) return;

        await Shell.Current.GoToAsync(
            nameof(PetDetailsPage),
            animate: true,
            new Dictionary<string, object>
            {
                { "Pet", selectedPet }
            });
        ((CollectionView)sender).SelectedItem = null;
    }

    private async void AddToFavorited_Clicked(object sender, EventArgs e)
    {
        if (sender is not ImageButton button || button.BindingContext is not Pet pet) return;

        var userId = AuthenticationService.Profile?.UserId;
        if (pet.IsFavorite)
        {
            await SupabaseService.SB.From<Favorite>()
            .Filter("pet_id", Operator.Equals, pet.Id)
            .Filter("profile_id", Operator.Equals, userId)
            .Delete();

            pet.IsFavorite = false;
            button.Source = "heart_empt.svg"; 
        }
        else
        {
            var favorite = new Favorite
            {
                PetId = pet.Id,
                ProfileId = userId
            };
            await SupabaseService.SB.From<Favorite>().Insert(favorite);

            pet.IsFavorite = true;
            button.Source = "heart.svg"; 
        }
    }
}
