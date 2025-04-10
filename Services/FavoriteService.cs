namespace PetAdoptApp.Services;

public class FavoriteService
{
    public ObservableCollection<Pet> Favorites { get; private set; } = [];

    public void AddToFavorites(Pet pet)
    {
        if (!Favorites.Any(f => f.Id == pet.Id))
        {
            Favorites.Add(pet);
        }
    }

    public void RemoveFromFavorites(Pet pet)
    {
        var favoritePet = Favorites.FirstOrDefault(f => f.Id == pet.Id);
        if (favoritePet != null)
        {
            Favorites.Remove(favoritePet);
        }
    }

    public bool IsFavorite(Pet pet)
    {
        return Favorites.Any(f => f.Id == pet.Id);
    }

    public void UpdateFavorites(IEnumerable<Pet> pets, string userId)
    {
        foreach (var pet in pets)
        {
            pet.IsFavorite = Favorites.Any(f => f.Id == pet.Id);
        }
    }
}