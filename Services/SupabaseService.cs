namespace PetAdoptApp.Services;

public static class SupabaseService
{
    private const string SB_URL = "https://hfdubhxucmqtwqbaiisu.supabase.co";
    private const string SB_ANON_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImhmZHViaHh1Y21xdHdxYmFpaXN1Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDI5NzcwOTcsImV4cCI6MjA1ODU1MzA5N30.BuxxmSHydEi5LwtOnNpJ_gD6Q664kq8nf3bUnD_dMYU";

    public static readonly Supabase.Client SB = new(SB_URL, SB_ANON_KEY);
    public static Session? Session;
    public static bool IsAdmin { get; set; } = false;


}
