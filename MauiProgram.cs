using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using PetAdoptApp.Services;
using PetAdoptApp.Tabs;

namespace PetAdoptApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        }).UseMauiCommunityToolkit();

        builder.Services
            .AddScoped<AuthenticationService>();

        builder.Services.
            AddSingleton<FavoriteService>();

        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<FavoritePage>();
#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}