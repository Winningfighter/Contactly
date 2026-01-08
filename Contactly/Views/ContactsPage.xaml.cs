using System.Collections.ObjectModel;
using Contactly.Models;
using Contact = Contactly.Models.Contact;

namespace Contactly.Views;

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

    public void LoadContacts()
    {
        // Hier w�rdest du normalerweise aus der Datenbank laden.
        // Wir verhindern doppeltes Laden beim Zur�cknavigieren:
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
    
    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Contact selectedContact)
        {
            // Navigiere zur Detailseite und übergebe das Objekt
            var navParam = new Dictionary<string, object>
            {
                { "Contact", selectedContact }
            };
            
            // Auswahl aufheben, damit man nochmal draufklicken kann
            ContactsCollection.SelectedItem = null;

            await Shell.Current.GoToAsync(nameof(ContactDetailPage), navParam);
        }
    }

    // Wird ausgelöst beim Klick auf "Bearbeiten" Button
    private async void OnEditClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var contact = button?.CommandParameter as Contact;

        if (contact != null)
        {
            var navParam = new Dictionary<string, object>
            {
                { "Contact", contact }
            };
            await Shell.Current.GoToAsync(nameof(ContactsFormPage), navParam);
        }
    }

    // Wird ausgelöst beim Klick auf den Mülleimer
    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        var button = sender as ImageButton;
        var contact = button?.CommandParameter as Contact;

        if (contact != null)
        {
            bool answer = await DisplayAlert("Löschen?", $"Möchtest du {contact.FullName} wirklich löschen?", "Ja", "Nein");
            
            if (answer)
            {
                Contacts.Remove(contact); // Entfernt es sofort aus der UI
                // TODO: Hier auch aus Datenbank löschen!
            }
        }
    }

    public void OnGridClicked(object sender, EventArgs e)
    {
        // 2 Spalten f�r Rasteransicht
        ContactsCollection.ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical);
        
        // Buttons färben für Feedback
        BtnGrid.BackgroundColor = (Color)Application.Current.Resources["PrimaryButtonActive"];
        BtnGrid.TextColor = Colors.White;
        BtnList.BackgroundColor = (Color)Application.Current.Resources["SecondaryButton"];
        BtnList.TextColor = (Color)Application.Current.Resources["Font"];
    }

    public void OnListClicked(object sender, EventArgs e)
    {
        // Einfache Liste
        ContactsCollection.ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical);
        
        // Buttons färben für Feedback
        BtnList.BackgroundColor = (Color)Application.Current.Resources["PrimaryButtonActive"];
        BtnList.TextColor = Colors.White;
        BtnGrid.BackgroundColor = (Color)Application.Current.Resources["SecondaryButton"];
        BtnGrid.TextColor = (Color)Application.Current.Resources["Font"];
    
    }
}