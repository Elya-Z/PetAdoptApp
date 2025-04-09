namespace PetAdoptApp.Services;

public class AuthenticationService
{
    public static Profile? Profile { get; private set; }
    public async Task LoginAsync(string email, string password)
    {
        ValidateInputs(email, password);
        SupabaseService.Session = await SB.Auth.SignIn(email, password);
        Profile = (await SB.From<Profile>()
            .Where(p => p.UserId == SupabaseService.Session!.User!.Id)
            .Get()).Model;
    }

    public async Task RegisterAsync(string email, string password, string firstName, string lastName)
    {
        ValidateInputs(email, password);
        SupabaseService.Session = await SB.Auth.SignUp(email, password);

        var profile = new Profile(SupabaseService.Session!.User!.Id!, firstName, lastName);
        var response = await SB.From<Profile>().Insert(profile);
        Profile = response.Model;
    }

    private static void ValidateInputs(string email, string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);
    }
}