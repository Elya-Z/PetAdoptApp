namespace PetAdoptApp.Pages;

public partial class AddPetPage : ContentPage
{
    private FileResult? _fileResult;
    private readonly ObservableCollection<Category> _categories = [];
    private int? _selectedCategoryId;
    public AddPetPage()
    {
        InitializeComponent();
        MaleCheckBox.CheckedChanged += OnGenderCheckedChanged!;
        FemaleCheckBox.CheckedChanged += OnGenderCheckedChanged!;
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

    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        try
        {
            ValidateInputs(out string gender);
            if (_fileResult == null)
            {
                await DisplayAlert("Select File", "No file selected!", "OK");
                return;
            }
            var username = $"{AuthenticationService.Profile!.Surname} {AuthenticationService.Profile.Firstname}";
            var supabasePath = $"PetAdopt/{DateTime.UtcNow.ToShortTimeString()}-{Guid.NewGuid()}-{_fileResult.FileName}";

            await SB.Storage
                .From("pets")
                .Upload(
                    localFilePath: _fileResult.FullPath,
                    supabasePath: supabasePath,
                    options: new Supabase.Storage.FileOptions { ContentType = _fileResult.ContentType });

            var signedUrl = await SB.Storage
             .From("pets")
             .CreateSignedUrl(supabasePath, (int)TimeSpan.FromHours(1).TotalSeconds);

            var result = await SB.From<Pet>().Insert(new Pet
            {
                Name = PetNameEntry.Text,
                Breed = BreedEntry.Text,
                Age = int.Parse(AgeEntry.Text),
                Weight = decimal.Parse(WeightEntry.Text),
                Address = AddressEntry.Text,
                About = AboutEditor.Text,
                ImageUrl = signedUrl,
                Username = username,
                Sex = gender,
                UserLink = userLinkEntry.Text,
                CategoryId = _selectedCategoryId
            });

            await DisplayAlert("Success!", "Pet added successfully", "OK");
            await AppShell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to add pet: {ex.Message}", "OK");
        }
    }

    private void ValidateInputs(out string gender)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(PetNameEntry.Text, nameof(PetNameEntry));
        ArgumentException.ThrowIfNullOrWhiteSpace(BreedEntry.Text, nameof(BreedEntry));
        ArgumentException.ThrowIfNullOrWhiteSpace(AgeEntry.Text, nameof(AgeEntry));
        ArgumentException.ThrowIfNullOrWhiteSpace(WeightEntry.Text, nameof(WeightEntry));
        ArgumentException.ThrowIfNullOrWhiteSpace(AddressEntry.Text, nameof(AddressEntry));
        ArgumentException.ThrowIfNullOrWhiteSpace(userLinkEntry.Text, nameof(userLinkEntry));
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
    }

    private async void OnSelectImage_Click(object sender, EventArgs e)
    {
        try
        {
            var pickOptions = new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Select Image"
            };

            _fileResult = await FilePicker.PickAsync(pickOptions);
            if (_fileResult is null) return;

            using var stream = await _fileResult.OpenReadAsync();
            var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            PetImage.Source = ImageSource.FromStream(() => memoryStream);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}