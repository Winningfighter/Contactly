using Contactly.Models;
using Contactly.Views;
using Microsoft.Extensions.Logging;

namespace Contactly
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            Routing.RegisterRoute("search", typeof(SearchPage));
            Routing.RegisterRoute("add", typeof(AddPage));
            Routing.RegisterRoute("contactsFormPage", typeof(ContactsFormPage));
            Routing.RegisterRoute("contacts", typeof(ContactsPage));

            return builder.Build();
        }
    }
}
