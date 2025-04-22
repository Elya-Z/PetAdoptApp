using static Supabase.Postgrest.Constants;

namespace PetAdoptApp.Pages;

public partial class MyPostPage : ContentPage
{
	public MyPostPage()
	{
		InitializeComponent();
        BindingContext = this;
    }
    public ObservableCollection<Pet> Pets { get; } = [];

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
            var username = $"{AuthenticationService.Profile?.Surname} {AuthenticationService.Profile?.Firstname}";
            if (string.IsNullOrEmpty(username))
            {
                await DisplayAlert("Error", "User not found.", "OK");
                return;
            }

            var result = await SupabaseService.SB.From<Pet>()
                .Filter("username", Operator.Equals, username)
                .Get();

            Pets.Clear();
            foreach (var pet in result.Models)
            {
                Pets.Add(pet);
            }
        }
        catch (Exception ex)
        {
            await AppShell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnPetSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 0) return;
        if (e.CurrentSelection[0] is not Pet selectedPet) return;

        await Shell.Current.GoToAsync("petdetails", new Dictionary<string, object>
    {
        { "Pet", selectedPet },
        { "ReturnRoute", "profile/myPosts" }
    });
        ((CollectionView)sender).SelectedItem = null;
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        try
        {
            if (sender is not ImageButton button || button.CommandParameter is not int petId)
            {
                await DisplayAlert("Error", "Failed to delete pet.", "OK");
                return;
            }

            var username = $"{AuthenticationService.Profile?.Surname} {AuthenticationService.Profile?.Firstname}";
            if (string.IsNullOrEmpty(username))
            {
                await DisplayAlert("Error", "User not found.", "OK");
                return;
            }

            var pet = await SupabaseService.SB.From<Pet>()
                .Filter("id", Operator.Equals, petId)
                .Filter("username", Operator.Equals, username)
                .Single();

            if (pet == null)
            {
                await DisplayAlert("Error", "You can only delete your own pets.", "OK");
                return;
            }

            await SupabaseService.SB.From<Pet>().Filter("id", Operator.Equals, petId).Delete();

            var petToRemove = Pets.FirstOrDefault(p => p.Id == petId);
            if (petToRemove != null)
            {
                Pets.Remove(petToRemove);
            }

            await DisplayAlert("Success", "Pet deleted successfully.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to delete pet: {ex.Message}", "OK");
        }
    }
}