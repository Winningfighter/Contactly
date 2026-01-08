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
        }
    }
}
