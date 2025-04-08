namespace PetAdoptApp.Services;

public class AuthenticationService
{
    public async Task LoginAsync(string email, string password)
    {
        ValidateInputs(email, password);

        SupabaseService.Session = await SB.Auth.SignIn(email, password);
    }

    public async Task RegisterAsync(string email, string password, string firstName, string lastName)
    {
        ValidateInputs(email, password);
        SupabaseService.Session = await SB.Auth.SignUp(email, password);
        var profile = new Profile(SupabaseService.Session!.User!.Id!, firstName, lastName);
        await SB.From<Profile>().Insert(profile);
    }

    private static void ValidateInputs(string email, string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);
    }
}