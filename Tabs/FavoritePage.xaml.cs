using static Supabase.Postgrest.Constants;

namespace PetAdoptApp.Tabs;

public partial class FavoritePage : ContentPage
{
    private readonly FavoriteService _favoriteService;
    public ObservableCollection<Pet> Pets { get; } = [];
    public FavoritePage(FavoriteService favoriteService)
	{
		InitializeComponent();
        BindingContext = this;
        _favoriteService = favoriteService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        Pets.Clear(); 
        await LoadPetsAsync(); 
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
            Pets.Clear();

            var favoriteResult = await SupabaseService.SB.From<Favorite>()
                .Select("pet_id")
                .Filter("profile_id", Operator.Equals, userId)
                .Get(token);

            if (favoriteResult.Models == null || !favoriteResult.Models.Any())
                return;

            var favoritePetIds = favoriteResult.Models.Select(fav => fav.PetId).ToList();
            foreach (var petId in favoritePetIds)
            {
                var petResult = await SupabaseService.SB.From<Pet>()
                    .Filter("id", Operator.Equals, petId)
                    .Get(token);

                if (petResult.Models?.FirstOrDefault() is Pet pet)
                {
                    pet.IsFavorite = true;
                    _favoriteService.AddToFavorites(pet);
                    Pets.Add(pet);
                }
            }
        }
        catch (Exception ex)
        {
            await AppShell.Current.DisplayAlert("Ошибка", ex.Message, "OK");
        }
    }

    private async void RemoveFromFavorite_Clicked(object sender, EventArgs e)
    {
        if (sender is not ImageButton button || button.BindingContext is not Pet pet) return;

        var userId = AuthenticationService.Profile?.UserId;
        await SupabaseService.SB.From<Favorite>()
            .Filter("pet_id", Operator.Equals, pet.Id)
            .Filter("profile_id", Operator.Equals, userId)
            .Delete();

        Pets.Remove(pet);
        _favoriteService.RemoveFromFavorites(pet);
        _favoriteService.UpdateFavorites([.. _favoriteService.Favorites], userId!);
    }

    private async void OnPetSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 0) return;
        if (e.CurrentSelection[0] is not Pet selectedPet) return;

        await Shell.Current.GoToAsync("petdetails", new Dictionary<string, object>
    {
        { "Pet", selectedPet },
        { "ReturnRoute", "favorite" }
    });
        ((CollectionView)sender).SelectedItem = null;
    }
}