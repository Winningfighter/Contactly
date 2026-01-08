using System.Collections.ObjectModel;
using Contactly.Models;

using Contact = Contactly.Models.Contact;

namespace Contactly.Views;

public partial class SearchPage : ContentPage
{
// Die "Datenbank" (Alle Kontakte)
    private List<Contact> _allContacts;
    
    // Die gefilterte Liste für die Anzeige
    public ObservableCollection<Contact> FilteredContacts { get; set; } = new();

    public SearchPage()
    {
        InitializeComponent();
        
        // Simuliere Daten laden (Das käme später aus deiner DB)
        LoadDummyData();

        // Verbinde die CollectionView mit der gefilterten Liste
        SearchResultsCollection.ItemsSource = FilteredContacts;
    }

    private void LoadDummyData()
    {
        _allContacts = new List<Contact>
        {
            new Contact { FirstName = "Max", LastName = "Mustermann", Email = "max@mustermann.ch", Phone = "+41 79 123 45 67", Company = "Musterfirma" },
            new Contact { FirstName = "Anna", LastName = "Beispiel", Email = "anna@test.ch", Phone = "+41 78 999 88 77", Company = "Test AG" },
            new Contact { FirstName = "Hans", LastName = "Müller", Email = "hans@mueller.de", Phone = "+49 170 123456", Company = "Bau GmbH" },
            new Contact { FirstName = "Julia", LastName = "Schmidt", Email = "j.schmidt@web.de", Phone = "+41 44 555 66 66", Company = "Schmidt & Co" }
        };
    }

    // Event: Wird bei jedem Tastendruck ausgelöst
    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue?.ToLower() ?? "";

        // 1. Liste leeren
        FilteredContacts.Clear();

        // Zustand: Suchfeld ist leer (Mockup 3)
        if (string.IsNullOrWhiteSpace(searchText))
        {
            NoResultsLabel.IsVisible = false;
            return; // Nichts anzeigen
        }

        // 2. Filtern (Suche nach Vorname, Nachname oder Firma)
        var results = _allContacts.Where(c => 
            c.FirstName.ToLower().Contains(searchText) || 
            c.LastName.ToLower().Contains(searchText) ||
            c.Company.ToLower().Contains(searchText)
        ).ToList();

        // 3. Ergebnisse hinzufügen
        foreach (var contact in results)
        {
            FilteredContacts.Add(contact);
        }

        // Zustand: Keine Treffer (Mockup 10)
        if (FilteredContacts.Count == 0)
        {
            NoResultsLabel.IsVisible = true;  // Zeige orangenen Text
            SearchResultsCollection.IsVisible = false;
        }
        // Zustand: Treffer gefunden (Mockup 4)
        else
        {
            NoResultsLabel.IsVisible = false; // Verstecke Warnung
            SearchResultsCollection.IsVisible = true;
        }
    }
}