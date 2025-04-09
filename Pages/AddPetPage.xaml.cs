namespace PetAdoptApp.Pages;

public partial class AddPetPage : ContentPage
{
    private FileResult selectedImageFile;
    private string imageUrl;
    private ObservableCollection<Category> _categories = [];
    private string _selectedCategoryId;

    public AddPetPage()
    {
        InitializeComponent();
        MaleCheckBox.CheckedChanged += OnGenderCheckedChanged;
        FemaleCheckBox.CheckedChanged += OnGenderCheckedChanged;
        LoadCategories();
    }

    private async void LoadCategories()
    {
        try
        {
            var result = await SupabaseService.SB
                .From<Category>()
                .Select("*")
                .Get();

            _categories.Clear();
            foreach (var category in result.Models)
            {
                _categories.Add(category);
            }
            CategoryPicker.ItemsSource = _categories;

            // Установим первую категорию по умолчанию
            if (_categories.Any())
            {
                CategoryPicker.SelectedIndex = 0;
                _selectedCategoryId = _categories[0].Id;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load categories: {ex.Message}", "OK");
        }
    }

    private void CategoryPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        if (picker.SelectedIndex != -1)
        {
            var selectedCategory = (Category)picker.SelectedItem;
            _selectedCategoryId = selectedCategory.Id;
        }
    }

    private void OnGenderCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender == MaleCheckBox && e.Value)
        {
            FemaleCheckBox.IsChecked = false;
        }
        else if (sender == FemaleCheckBox && e.Value)
        {
            MaleCheckBox.IsChecked = false;
        }
    }

    private async void OnSelectImageTapped(object sender, EventArgs e)
    {
        try
        {
            // Проверка и запрос разрешений для Android
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                // Для Android 13+ (API level 33)
                if (DeviceInfo.Version.Major >= 13)
                {
                    var status = await Permissions.RequestAsync<Permissions.Photos>();
                    if (status != PermissionStatus.Granted)
                    {
                        await DisplayAlert("Требуется разрешение", "Для выбора фото необходимо предоставить доступ к галерее", "OK");
                        return;
                    }
                }
                else
                {
                    // Для версий ниже Android 13
                    var status = await Permissions.RequestAsync<Permissions.StorageRead>();
                    if (status != PermissionStatus.Granted)
                    {
                        await DisplayAlert("Требуется разрешение", "Для выбора фото необходимо предоставить доступ к хранилищу", "OK");
                        return;
                    }
                }
            }

            // Выбор фото
            var result = await MediaPicker.PickPhotoAsync();

            if (result != null)
            {
                selectedImageFile = result;
                var stream = await result.OpenReadAsync();
                PetImage.Source = ImageSource.FromStream(() => stream);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", ex.Message, "OK");
        }
    }

    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        try
        {
            ValidateInputs(out string gender);
            var selectedCategory = (Category)CategoryPicker.SelectedItem;

            // 1. Загружаем изображение питомца в Supabase Storage
            var petFileName = $"{Guid.NewGuid()}{Path.GetExtension(selectedImageFile.FileName)}";
            var fileBytes = await File.ReadAllBytesAsync(selectedImageFile.FullPath);

            var storage = SupabaseService.SB.Storage;
            var bucket = storage.From("pets");
            var uploadedPath = await bucket.Upload(fileBytes, petFileName);
            var petImageUrl = bucket.GetPublicUrl(petFileName);

            // TODO: Get data from Profile
            var username = $"{CurrentUser.Surname} {CurrentUser.Name}";

            var pet = new Pet
            {
                Name = PetNameEntry.Text,
                Breed = BreedEntry.Text,
                Age = int.Parse(AgeEntry.Text),
                Weight = decimal.Parse(WeightEntry.Text),
                Address = AddressEntry.Text,
                About = AboutEditor.Text,
                ImageUrl = petImageUrl,
                Username = username,
                Sex = gender,
                CategoryId = _selectedCategoryId
            };

            var response = await SupabaseService.SB
                .From<Pet>()
                .Insert(pet);

            await DisplayAlert("Successed", "Pet successed add", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"{ex.Message}", "OK");
        }
        finally
        {
            IsEnabled = true;
            SubmitButton.Text = "Add";
        }
    }

    private void ValidateInputs(out string gender)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(PetNameEntry.Text, nameof(PetNameEntry));
        ArgumentException.ThrowIfNullOrWhiteSpace(BreedEntry.Text, nameof(BreedEntry));
        ArgumentException.ThrowIfNullOrWhiteSpace(AgeEntry.Text, nameof(AgeEntry));
        ArgumentException.ThrowIfNullOrWhiteSpace(WeightEntry.Text, nameof(WeightEntry));
        ArgumentException.ThrowIfNullOrWhiteSpace(AddressEntry.Text, nameof(AddressEntry));
        ArgumentException.ThrowIfNullOrWhiteSpace(AboutEditor.Text, nameof(AboutEditor));

        if (CategoryPicker.SelectedItem == null)
        {
            throw new ArgumentException("Please enter all lines", nameof(CategoryPicker));
        }

        if (!MaleCheckBox.IsChecked && !FemaleCheckBox.IsChecked)
        {
            throw new ArgumentException("Please select pet`s gender");
        }

        gender = MaleCheckBox.IsChecked ? "Male" : "Female";
        if (selectedImageFile == null)
        {
            throw new ArgumentException("Please select a photo pet");
        }
    }
}