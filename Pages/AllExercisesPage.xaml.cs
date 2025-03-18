using PetAdoptApp.Contexts;
using PetAdoptApp.Models;
using Microsoft.Maui.Controls;

namespace PetAdoptApp
{
    public partial class AllExercisesPage : ContentPage
    {
        public AllExercisesPage()
        {
            InitializeComponent();
            LoadExercises();
        }
        private void LoadExercises()
        {
            using (AppDbContext dbContext = new AppDbContext())
            {
                var exercises = dbContext.ExerciseEntities.ToList();
                ExercisesCollectionView.ItemsSource = exercises;
            }
        }

        private void GoBack(object sender, EventArgs e)
        {
            AppShell.Current.GoToAsync(nameof(MenuPage), true);
        }

        private async void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
        {
            var swipeItem = sender as SwipeItem;
            var exercise = swipeItem.BindingContext as ExerciseEntity;

            bool confirm = await DisplayAlert("Удаление", $"Вы уверены, что хотите удалить упражнение '{exercise.Title}'?", "Да", "Нет");
            if (confirm)
            {
                using (AppDbContext dbContext = new AppDbContext())
                {
                    dbContext.ExerciseEntities.Remove(exercise);
                    dbContext.SaveChanges();
                }
                LoadExercises(); 
            }
        }

       
    }
}