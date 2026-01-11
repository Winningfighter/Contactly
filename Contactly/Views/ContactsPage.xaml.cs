using System.Collections.ObjectModel;
using Contactly.Models;
using Contactly.Services;
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
        Contacts.Clear();
        var loadedContacts = ContactService.LoadContacts();

        foreach (var contact in loadedContacts)
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
                ContactService.DeleteContact(contact.Id);
                Contacts.Remove(contact);
            }
        }
    }

    public void OnGridClicked(object sender, EventArgs e)
    {
        var theme = Application.Current.RequestedTheme;

        // Rasteransicht setzen
        ContactsCollection.ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical);
    
        // Buttons färben
        UpdateButtons(isGrid: true, theme);
    }

    public void OnListClicked(object sender, EventArgs e)
    {
        var theme = Application.Current.RequestedTheme;

        // Listenansicht setzen
        ContactsCollection.ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical);
    
        // Buttons färben
        UpdateButtons(isGrid: false, theme);
    }
    
    private void UpdateButtons(bool isGrid, AppTheme theme)
    {
        var activeColor = (Color)Application.Current.Resources["PrimaryButtonActive"];
        var inactiveColor = theme == AppTheme.Dark 
            ? (Color)Application.Current.Resources["SecondaryButtonDark"] 
            : (Color)Application.Current.Resources["SecondaryButton"];
    
        var activeText = Colors.White;
        var inactiveText = theme == AppTheme.Dark 
            ? (Color)Application.Current.Resources["FontSecondary"] 
            : (Color)Application.Current.Resources["Font"];

        if (isGrid)
        {
            BtnGrid.BackgroundColor = activeColor;
            BtnGrid.TextColor = activeText;
            BtnList.BackgroundColor = inactiveColor;
            BtnList.TextColor = inactiveText;
        }
        else
        {
            BtnList.BackgroundColor = activeColor;
            BtnList.TextColor = activeText;
            BtnGrid.BackgroundColor = inactiveColor;
            BtnGrid.TextColor = inactiveText;
        }
    }
}