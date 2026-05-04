using Microsoft.Extensions.DependencyInjection;
using ProdjectClient.Maui.Pages;

namespace ExpensesTracker.MAUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new LoginView());
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}