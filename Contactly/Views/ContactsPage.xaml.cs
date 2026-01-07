using System.Collections.ObjectModel;
using Contactly.Models;

namespace Contactly.Models;

public partial class ContactsPage : ContentPage
{
    public ObservableCollection<Contact> Contacts { get; set; } = new ObservableCollection<Contact>();

    public ContactsPage()
	{
        InitializeComponent();

        ContactsCollection.ItemsSource = Contacts;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadContacts();
    }

    private void LoadContacts()
    {
        // Hier würdest du normalerweise aus der Datenbank laden.
        // Wir verhindern doppeltes Laden beim Zurücknavigieren:
        if (Contacts.Count > 0) return;

        var dummies = new List<Contact>
        {
            new Contact { FirstName = "Max", LastName = "Mustermann", Email = "max@test.ch", Phone = "+41 79 123 45 67", Company = "Muster AG" },
            new Contact { FirstName = "Sarah", LastName = "Connor", Email = "s.connor@sky.net", Phone = "+1 555 123 123", Company = "Resistance" },
            new Contact { FirstName = "Bruce", LastName = "Wayne", Email = "batman@wayne.com", Phone = "Secret", Company = "Wayne Ent." }
        };

        foreach (var contact in dummies)
        {
            Contacts.Add(contact);
        }
    }

    private void OnGridClicked(object sender, EventArgs e)
    {
        // 2 Spalten für Rasteransicht
        ContactsCollection.ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical);
    }

    private void OnListClicked(object sender, EventArgs e)
    {
        // Einfache Liste
        ContactsCollection.ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical);
    }
}