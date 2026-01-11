using Contactly.Models;
using Contactly.Services; // Wichtig für Delete/Save Funktionen
using Contact = Contactly.Models.Contact; // Verhindert Verwechslungen

namespace Contactly.Views;

// Wir nutzen IQueryAttributable, das ist stabiler als [QueryProperty]
public partial class ContactDetailPage : ContentPage, IQueryAttributable
{
    private Contact _contact;

    // Diese Property wird vom XAML "beobachtet" (Binding)
    public Contact Contact
    {
        get => _contact;
        set
        {
            _contact = value;
            OnPropertyChanged(); // Sagt der UI: "Daten aktualisieren!"
        }
    }

    public ContactDetailPage()
    {
        InitializeComponent();
        // WICHTIG: Damit XAML auf "Contact" zugreifen kann
        BindingContext = this; 
    }

    // Wird automatisch ausgeführt, wenn die Seite geöffnet wird
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Contact", out var value) && value is Contact contact)
        {
            Contact = contact;
        }
    }

    // Navigiert zur Bearbeiten-Seite
    private async void OnEditClicked(object sender, EventArgs e)
    {
        if (Contact == null) return;

        var navParam = new Dictionary<string, object>
        {
            { "Contact", Contact }
        };
        
        // Öffnet das Formular mit den Daten
        await Shell.Current.GoToAsync(nameof(ContactsFormPage), navParam);
    }

    // Löscht den Kontakt
    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (Contact == null) return;

        bool answer = await DisplayAlert("Löschen?", $"Möchtest du {Contact.FullName} wirklich löschen?", "Ja", "Nein");

        if (answer)
        {
            // Löschen im Service aufrufen
            ContactService.DeleteContact(Contact.Id);

            // Zurück zur Liste
            await Shell.Current.GoToAsync("..");
        }
    }

    // Zurück-Button
    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}