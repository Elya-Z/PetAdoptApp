using PetAdoptApp.Contexts;

namespace PetAdoptApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //MainPage = new AppShell();
            AppDbContext dbContext = new AppDbContext();
            dbContext.Database.EnsureCreated();
            AppDbContext.Initialize(dbContext);
        }
        

        protected override Window CreateWindow(IActivationState? activationState) {
            return new Window(new AppShell());
        }
    }
}
