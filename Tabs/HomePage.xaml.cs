using PetAdoptApp.Contexts;
using PetAdoptApp.Models;
using PetAdoptApp.Pages;
using System.Collections.ObjectModel;

namespace PetAdoptApp.Tabs;

public partial class HomePage : ContentPage
{
    public ObservableCollection<Pet> Pets { get; } = new ObservableCollection<Pet>();

    // Бэк-филд для FilteredPets
    private ObservableCollection<Pet> _filteredPets = new ObservableCollection<Pet>();
    public ObservableCollection<Pet> FilteredPets
    {
        get => _filteredPets;
        set
        {
            if (_filteredPets != value)
            {
                _filteredPets = value;
                OnPropertyChanged(nameof(FilteredPets)); // Уведомляем UI об изменении
            }
        }
    }

    private readonly string[] _imageUrls = new string[] {
        "https://avatars.mds.yandex.net/i?id=1bf0f10e5bceac695d5239040c732aaea0cf5b06-5209794-images-thumbs&n=13",
        "https://creativo.one/adds/adds27062/393baf233768477fd688216d19e39a62ce070a3d.jpg"
    };

    public ObservableCollection<string> ImageUrls { get; set; }
    public string WelcomeMessage { get; set; } = "";

    // Добавляем свойство для выбранной категории
    private string _selectedCategory;
    public string SelectedCategory
    {
        get => _selectedCategory;
        set
        {
            if (_selectedCategory != value)
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
                FilterPetsByCategory();
            }
        }
    }

    public HomePage()
    {
        ImageUrls = new ObservableCollection<string>(_imageUrls);
        InitializeComponent();
        LoadData();
        BindingContext = this;

        // Устанавливаем категорию по умолчанию
        SelectedCategory = "2";
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

            // Инициализация отфильтрованного списка
            FilteredPets = new ObservableCollection<Pet>(Pets);

            // Фильтруем по категории "dogs"
            FilterPetsByCategory();
        }
        catch (Exception ex)
        {
            await AppShell.Current.DisplayAlert("Ошибка", ex.Message, "OK");
        }
    }

    private void FilterPetsByCategory()
    {
        string category = string.IsNullOrEmpty(SelectedCategory) ? "2" : SelectedCategory;

        if (string.IsNullOrEmpty(category))
        {
            // Если категория не выбрана, показываем всех питомцев
            FilteredPets = new ObservableCollection<Pet>(Pets);
        }
        else
        {
            // Фильтруем питомцев по выбранной категории
            FilteredPets = new ObservableCollection<Pet>(
                Pets.Where(p => p.CategoryId == category));
        }
    }

    private async void LoadData()
    {
        var currentUser = CurrentUser.Email;

        using (AppDbContext dbContext = new AppDbContext())
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Email == currentUser);
            if (user != null)
            {
                WelcomeMessage = $"{user.Surname} {user.Name}";
            }
        }

        await LoadPetsAsync();
    }

    private async void GoToAddPet(object sender, EventArgs e)
    {
        await AppShell.Current.GoToAsync(nameof(AddPetPage), true);
    }

    // Команда для выбора категории
    public Command<string> SelectCategoryCommand => new Command<string>(category => SelectedCategory = category);
}