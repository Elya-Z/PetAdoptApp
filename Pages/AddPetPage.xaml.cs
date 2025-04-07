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

                // ��������� ������ ��������� �� ���������
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
            // �������� � ������ ���������� ��� Android
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                // ��� Android 13+ (API level 33)
                if (DeviceInfo.Version.Major >= 13)
                {
                    var status = await Permissions.RequestAsync<Permissions.Photos>();
                    if (status != PermissionStatus.Granted)
                    {
                        await DisplayAlert("��������� ����������", "��� ������ ���� ���������� ������������ ������ � �������", "OK");
                        return;
                    }
                }
                else
                {
                    // ��� ������ ���� Android 13
                    var status = await Permissions.RequestAsync<Permissions.StorageRead>();
                    if (status != PermissionStatus.Granted)
                    {
                        await DisplayAlert("��������� ����������", "��� ������ ���� ���������� ������������ ������ � ���������", "OK");
                        return;
                    }
                }
            }

            // ����� ����
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
            await DisplayAlert("������", ex.Message, "OK");
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
                await DisplayAlert("������", "����������, ��������� ��� ������������ ����", "OK");
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
                await DisplayAlert("������", "����������, �������� ���� �������", "OK");
                return;
            }

            // �������� ����������� ������������
            if (CurrentUser.Id_Cur_User == 0)
            {
                await DisplayAlert("������", "������������ �� �����������", "OK");
                return;
            }
            var selectedCategory = (Category)CategoryPicker.SelectedItem;

            // ���������� ��������� ��������
            IsEnabled = false;
            SubmitButton.Text = "��������...";

            // 1. ��������� ����������� ������� � Supabase Storage
            var petFileName = $"{Guid.NewGuid()}{Path.GetExtension(selectedImageFile.FileName)}";
            var fileBytes = await File.ReadAllBytesAsync(selectedImageFile.FullPath);

            var storage = SupabaseService.SB.Storage;
            var bucket = storage.From("pets");
            var uploadedPath = await bucket.Upload(fileBytes, petFileName);
            var petImageUrl = bucket.GetPublicUrl(petFileName);

            // 2. ��������� username �� ������ CurrentUser
            var username = $"{CurrentUser.Surname} {CurrentUser.Name}";
            if (!string.IsNullOrWhiteSpace(CurrentUser.Patronymic))
            {
                username += $" {CurrentUser.Patronymic}";
            }

            // 3. ������� ������ � �������
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

            // 4. ��������� � Supabase
            var response = await SupabaseService.SB
                .From<Pet>()
                .Insert(pet);

            await DisplayAlert("�����", "������� ������� ��������", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("������", $"��������� ������: {ex.Message}", "OK");
        }

        finally
        {
            IsEnabled = true;
            SubmitButton.Text = "��������";
        }
}


    private async Task<PermissionStatus> CheckAndRequestPermission()
    {
        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            // ��� Android 13+ (API level 33)
            if (DeviceInfo.Version.Major >= 13)
            {
                return await Permissions.CheckStatusAsync<Permissions.Photos>() == PermissionStatus.Granted
                    ? PermissionStatus.Granted
                    : await Permissions.RequestAsync<Permissions.Photos>();
            }
            else
            {
                // ��� ������ ���� Android 13
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
            "������ ��������",
            "��� ������ ���� ���������� ������������ ������ � ���������. ������� ���������?",
            "�������", "������");

        if (openSettings)
        {
            AppInfo.ShowSettingsUI();
        }
    }

    


private async Task<byte[]> OptimizeImage(FileResult imageFile)
{
    try
    {
            // ������������ ������� ��� �����������
            const int maxDimension = 1200;
            const int quality = 80;

            using var stream = await imageFile.OpenReadAsync();
            using var original = SKBitmap.Decode(stream);

            // ��������� �����������
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
        Console.WriteLine($"������ ����������� �����������: {ex}");
        return null;
    }
}
}