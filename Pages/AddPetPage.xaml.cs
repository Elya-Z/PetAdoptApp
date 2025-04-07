using SkiaSharp;
using System.Collections.ObjectModel;

namespace PetAdoptApp.Pages;

public partial class AddPetPage : ContentPage
{
    private FileResult selectedImageFile;
    private string imageUrl;
    private ObservableCollection<Category> _categories = new ObservableCollection<Category>();
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

            Device.BeginInvokeOnMainThread(() =>
            {
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
            });
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
            if (string.IsNullOrWhiteSpace(PetNameEntry.Text) ||
            string.IsNullOrWhiteSpace(BreedEntry.Text) ||
            string.IsNullOrWhiteSpace(AgeEntry.Text) ||
            string.IsNullOrWhiteSpace(WeightEntry.Text) ||
            string.IsNullOrWhiteSpace(AddressEntry.Text) ||
            string.IsNullOrWhiteSpace(AboutEditor.Text) || CategoryPicker.SelectedItem == null)
            {
                await DisplayAlert("Ошибка", "Пожалуйста, заполните все обязательные поля", "OK");
                return;
            }

            if (!MaleCheckBox.IsChecked && !FemaleCheckBox.IsChecked)
            {
                await DisplayAlert("Error", "Please select pet gender", "OK");
                return;
            }

            string gender = MaleCheckBox.IsChecked ? "Male" : "Female";


            if (selectedImageFile == null)
            {
                await DisplayAlert("Ошибка", "Пожалуйста, выберите фото питомца", "OK");
                return;
            }

            // Проверка авторизации пользователя
            if (CurrentUser.Id_Cur_User == 0)
            {
                await DisplayAlert("Ошибка", "Пользователь не авторизован", "OK");
                return;
            }
            var selectedCategory = (Category)CategoryPicker.SelectedItem;

            // Показываем индикатор загрузки
            IsEnabled = false;
            SubmitButton.Text = "Загрузка...";

            // 1. Загружаем изображение питомца в Supabase Storage
            var petFileName = $"{Guid.NewGuid()}{Path.GetExtension(selectedImageFile.FileName)}";
            var fileBytes = await File.ReadAllBytesAsync(selectedImageFile.FullPath);

            var storage = SupabaseService.SB.Storage;
            var bucket = storage.From("pets");
            var uploadedPath = await bucket.Upload(fileBytes, petFileName);
            var petImageUrl = bucket.GetPublicUrl(petFileName);

            // 2. Формируем username из данных CurrentUser
            var username = $"{CurrentUser.Surname} {CurrentUser.Name}";
            if (!string.IsNullOrWhiteSpace(CurrentUser.Patronymic))
            {
                username += $" {CurrentUser.Patronymic}";
            }

            // 3. Создаем запись о питомце
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

            // 4. Сохраняем в Supabase
            var response = await SupabaseService.SB
                .From<Pet>()
                .Insert(pet);

            await DisplayAlert("Успех", "Питомец успешно добавлен", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", $"Произошла ошибка: {ex.Message}", "OK");
        }

        finally
        {
            IsEnabled = true;
            SubmitButton.Text = "Добавить";
        }
}


    private async Task<PermissionStatus> CheckAndRequestPermission()
    {
        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            // Для Android 13+ (API level 33)
            if (DeviceInfo.Version.Major >= 13)
            {
                return await Permissions.CheckStatusAsync<Permissions.Photos>() == PermissionStatus.Granted
                    ? PermissionStatus.Granted
                    : await Permissions.RequestAsync<Permissions.Photos>();
            }
            else
            {
                // Для версий ниже Android 13
                return await Permissions.CheckStatusAsync<Permissions.StorageRead>() == PermissionStatus.Granted
                    ? PermissionStatus.Granted
                    : await Permissions.RequestAsync<Permissions.StorageRead>();
            }
        }
        return PermissionStatus.Granted;
    }

    private async Task ShowPermissionAlert()
    {
        bool openSettings = await DisplayAlert(
            "Доступ запрещен",
            "Для выбора фото необходимо предоставить доступ к хранилищу. Открыть настройки?",
            "Открыть", "Отмена");

        if (openSettings)
        {
            AppInfo.ShowSettingsUI();
        }
    }

    


private async Task<byte[]> OptimizeImage(FileResult imageFile)
{
    try
    {
            // Максимальные размеры для изображения
            const int maxDimension = 1200;
            const int quality = 80;

            using var stream = await imageFile.OpenReadAsync();
            using var original = SKBitmap.Decode(stream);

            // Загружаем изображение
            if (original == null) return null;

            float ratio = Math.Min(
                (float)maxDimension / original.Width,
                (float)maxDimension / original.Height
            );
            ratio = Math.Min(ratio, 1.0f);

            int newWidth = (int)(original.Width * ratio);
            int newHeight = (int)(original.Height * ratio);

            using var resized = original.Resize(
                new SKImageInfo(newWidth, newHeight),
                SKFilterQuality.Medium
            );
            if (resized == null) return null;

            using var image = SKImage.FromBitmap(resized);
            using var data = image.Encode(SKEncodedImageFormat.Jpeg, quality);

            using var memoryStream = new MemoryStream();
            data.SaveTo(memoryStream);
            return memoryStream.ToArray();
        }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка оптимизации изображения: {ex}");
        return null;
    }
}
}