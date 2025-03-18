using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using PetAdoptApp.Contexts;
using PetAdoptApp.Models;

namespace PetAdoptApp;

public partial class CreateTrainingPage : ContentPage
{
    public ObservableCollection<ExerciseViewModel> Exercises { get; private set; }

    public CreateTrainingPage()
	{
		InitializeComponent();
        LoadExercises();
    }
    private void OnCheckBoxChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is CheckBox checkBox && checkBox.BindingContext is ExerciseViewModel exercise)
        {
            exercise.IsSelected = e.Value; 
        }
    }

    private void LoadExercises()
    {
        try
        {
            using (AppDbContext dbContext = new AppDbContext())
            {
                var exercises = dbContext.ExerciseEntities.Select(e => new ExerciseViewModel
                {
                    Id_Exercise = e.Id_Exercise,
                    Title = e.Title,
                    IsSelected = false
                }).ToList();

                Exercises = new ObservableCollection<ExerciseViewModel>(exercises);
                ExercisesList.ItemsSource = Exercises;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке упражнений: {ex.Message}");
        }
    }

    private async void AddTrainings(object sender, EventArgs e)
    {
        var title = TitleEntry.Text;
        var description = DescriptionEntry.Text;
        var date = MyDatePicker.Date;

        if (string.IsNullOrWhiteSpace(title))
        {
            await DisplayAlert("Ошибка", "Заголовок тренировки не может быть пустым!", "ОК");
            return;
        }else

        if (string.IsNullOrWhiteSpace(description))
        {
            await DisplayAlert("Ошибка", "Описание тренировки не может быть пустым!", "ОК");
            return;
        }

        try
        {
            using (AppDbContext dbContext = new AppDbContext())
            {
                var user = dbContext.Users.FirstOrDefault(u => u.Id_User == CurrentUser.Id_Cur_User);
                if (user == null)
                {
                    await DisplayAlert("Ошибка", "Пользователь не найден!", "ОК");
                    return;
                }

                var newTraining = new TrainingEntity
                {
                    Title = title,
                    Description = description,
                    DataCreate = date,
                    Id_User = CurrentUser.Id_Cur_User
                };

                dbContext.TrainingEntities.Add(newTraining);
                await dbContext.SaveChangesAsync();

                var selectedExercises = Exercises.Where(ex => ex.IsSelected).ToList();
               
                if (!selectedExercises.Any())
                {
                    await DisplayAlert("Ошибка", "Ни одно упражнение не выбрано!", "ОК");
                    return;
                }
                foreach (var exercise in selectedExercises)
                {
                    var dbExercise = dbContext.ExerciseEntities.Find(exercise.Id_Exercise);
                    if (dbExercise != null)
                    {
                        newTraining.Exercises.Add(dbExercise); 
                    }
                }

                await dbContext.SaveChangesAsync();

                Console.WriteLine($"Тренировка создана: {newTraining.Title}, Упражнений: {newTraining.Exercises.Count}");
                foreach (var exercise in newTraining.Exercises)
                {
                    Console.WriteLine($" - Упражнение: {exercise.Title}");
                }
                await DisplayAlert("Успех", "Тренировка добавлена!", "ОК");
                await AppShell.Current.GoToAsync(nameof(MenuPage), true);
            }
        }
        catch (DbUpdateException ex)
        {
            
            Console.WriteLine($"Ошибка при сохранении: {ex.InnerException?.Message}");

            await DisplayAlert("Ошибка", $"Не удалось сохранить данные: {ex.InnerException?.Message}", "OK");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
            await DisplayAlert("Ошибка", $"Произошла непредвиденная ошибка: {ex.Message}", "OK");
        }
    }

    private void GoBack(object sender, EventArgs e)
    {
        AppShell.Current.GoToAsync(nameof(MenuPage), true);
    }
}