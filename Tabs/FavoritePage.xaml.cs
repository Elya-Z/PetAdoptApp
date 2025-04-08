using System.Collections.ObjectModel;

namespace PetAdoptApp.Tabs;

public partial class FavoritePage : ContentPage
{
    public ObservableCollection<Pet> Pets { get; } = new ObservableCollection<Pet>();
    public FavoritePage()
	{
		InitializeComponent();
        LoadPetsAsync();
        BindingContext = this;
    }

    private async Task LoadPetsAsync()
    {
        try
        {
            var cancel = new CancellationTokenSource();
            cancel.CancelAfter(10000);
            var token = cancel.Token;
            var result = await SB.From<Pet>().Get(token);

            if (result.Models == null)
                return;

            foreach (var model in result.Models)
                Pets.Add(model);
        }
        catch (Exception ex)
        {
            await AppShell.Current.DisplayAlert("Ошибка", ex.Message, "OK");
        }
    }
}