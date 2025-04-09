using PetAdoptApp.Models;
using System.Collections.ObjectModel;
using System.Windows;
using static Supabase.Postgrest.Constants;

namespace PetAdoptApp.Services;

public class FavoriteService
{
    public ObservableCollection<Pet> Favorites { get; private set; } = new();

    public void AddToFavorites(Pet pet)
    {
        if (!Favorites.Contains(pet))
        {
            Favorites.Add(pet);
        }
    }

    public void RemoveFromFavorites(Pet pet)
    {
        if (Favorites.Contains(pet))
        {
            Favorites.Remove(pet);
        }
    }

    public bool IsFavorite(Pet pet)
    {
        return Favorites.Contains(pet);
    }

    public async Task LoadFavoritesAsync(string userId)
    {
        try
        {
            var result = await SupabaseService.SB.From<Favorite>()
                .Filter("profile_id", Operator.Equals, userId)
                .Get();

            if (result.Models == null)
                return;

            var favoritePetIds = result.Models.Select(fav => fav.PetId).ToList();

            foreach (var petId in favoritePetIds)
            {
                var petResult = await SupabaseService.SB.From<Pet>()
                    .Filter("id", Operator.Equals, petId)
                    .Get();

                if (petResult.Models?.FirstOrDefault() is Pet pet)
                {
                    if (!Favorites.Contains(pet))
                    {
                        Favorites.Add(pet);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}