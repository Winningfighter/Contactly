using System.Collections.ObjectModel;
using Contactly.Models;
using Contactly.Services;
using Contact = Contactly.Models.Contact;

namespace Contactly.Views;

public partial class SearchPage : ContentPage
{

    private List<Contact> _allContacts = new();
    
    // Die gefilterte Liste für die Anzeige
    public ObservableCollection<Contact> FilteredContacts { get; set; } = new();

    public SearchPage()
    {
        InitializeComponent();

        // Verbinde die CollectionView mit der gefilterten Liste
        SearchResultsCollection.ItemsSource = FilteredContacts;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _allContacts = ContactService.LoadContacts();
    }

    // Event: called on every key stroke
    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue?.ToLower() ?? "";

        // 1. Liste leeren
        FilteredContacts.Clear();

        // Zustand: Suchfeld ist leer (Mockup 3)
        if (string.IsNullOrWhiteSpace(searchText))
        {
            NoResultsLabel.IsVisible = false;
            SearchResultsCollection.IsVisible = false;
            return;
        }

        // filter (search for "Vorname", "Nachname" or "Firma")
        if (_allContacts != null)
        {
            var results = _allContacts.Where(c => 
                (c.FirstName != null && c.FirstName.ToLower().Contains(searchText)) || 
                (c.LastName != null && c.LastName.ToLower().Contains(searchText)) ||
                (c.Company != null && c.Company.ToLower().Contains(searchText))
            ).ToList();
            
            foreach (var contact in results)
            {
                FilteredContacts.Add(contact);
            }
        }

        // Zustand: Keine Treffer
        if (FilteredContacts.Count == 0)
        {
            NoResultsLabel.IsVisible = true;  // Zeige orangenen Text
            SearchResultsCollection.IsVisible = false;
        }
        // Zustand: Treffer gefunden
        else
        {
            NoResultsLabel.IsVisible = false; // Verstecke Warnung
            SearchResultsCollection.IsVisible = true;
        }
    }

    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Contact selectedContact)
        {
            var navParam = new Dictionary<string, object>
            {
                { "Contact", selectedContact }
            };

            SearchResultsCollection.SelectedItem = null;
            await Shell.Current.GoToAsync(nameof(ContactDetailPage), navParam);
        }
    }
}