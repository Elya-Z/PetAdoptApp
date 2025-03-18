using PetAdoptApp.Contexts;

namespace PetAdoptApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            AppDbContext dbContext = new AppDbContext();
            dbContext.Database.EnsureCreated();
            AppDbContext.Initialize(dbContext);

        }
    }
}
