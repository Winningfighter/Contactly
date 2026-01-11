using Contactly.Views;

namespace Contactly
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ContactDetailPage), typeof(ContactDetailPage));
            Routing.RegisterRoute(nameof(ContactsFormPage), typeof(ContactsFormPage));
            Routing.RegisterRoute(nameof(Views.FaqPage), typeof(Views.FaqPage));
            Routing.RegisterRoute(nameof(InfoPage), typeof(InfoPage));
        }
        
        private void OnExitClicked(object sender, EventArgs e)
        {
            Application.Current.Quit();
        }
        
        private async void OnFaqClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(Views.FaqPage));
        }
        
        private async void OnInfoClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(InfoPage));
            
        }
    }
}
