using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using PetAdoptApp.Contexts;
using PetAdoptApp.Models;

namespace PetAdoptApp;

public partial class AllTrainingsPage : ContentPage
{
    private readonly AppDbContext _context = new();
    public ObservableCollection<TrainingEntity> Trainings { get; set; }
    public ObservableCollection<ExerciseEntity> ExerciseEntities { get; set; }

    public AllTrainingsPage()
    {
        InitializeComponent();
        LoadTrainings();
        BindingContext = this;
    }

    private void LoadTrainings()
    {
        var trainings = _context.TrainingEntities
                     .Where(t => t.Id_User == CurrentUser.Id_Cur_User)
                     .Include(t => t.Exercises)
                     .ToList();
        var exercises = _context.ExerciseEntities.ToList();

        Trainings = new ObservableCollection<TrainingEntity>(trainings);
        ExerciseEntities = new ObservableCollection<ExerciseEntity>(exercises);
       
        TrainingCV.ItemsSource = Trainings;

    }

    private async void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
    {
        if (sender is not SwipeItem swipeItem) return;
        if (swipeItem.BindingContext is not TrainingEntity training) return;

        bool confirm = await DisplayAlert("Удаление", $"Вы уверены, что хотите удалить тренировку '{training.Title}'?", "Да", "Нет");
        if (!confirm) return;

        _context.TrainingEntities.Remove(training);
        _context.SaveChanges();
        Trainings.Remove(training);
    }

    private void GoBack(object sender, EventArgs e)
    {
        AppShell.Current.GoToAsync(nameof(MenuPage), true);
    }
}