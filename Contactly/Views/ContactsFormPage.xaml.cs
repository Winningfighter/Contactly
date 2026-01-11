using Contact = Contactly.Models.Contact;
using Contactly.Services;

namespace Contactly.Views;

// Dieser Code wurde mithile von ChatGPT erstellt, da ich Unterstützung bei der Logik brauchte.
public partial class ContactsFormPage : ContentPage, IQueryAttributable
{
    private string state;
    private Contact _currentContact;

    public ContactsFormPage()
    {
        InitializeComponent();
        state = "add"; // Standard-Status
    }

    // Wird beim Navigieren aufgerufen (Parameter verarbeiten)
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Contact", out var value) && value is Contact contact)
        {
            // BEARBEITEN
            _currentContact = contact;
            state = "edit";
        }
        else
        {
            // HINZUFÜGEN
            _currentContact = null;
            state = "add";
        }
    }

    // WICHTIG: UI-Updates erst hier machen, wenn die Seite wirklich angezeigt wird!
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Hier entscheiden wir final, was angezeigt wird
        if (state == "edit" && _currentContact != null)
        {
            LoadContactData();
        }
        else
        {
            // Sicherheitsnetz: Wenn State "add" ist, muss alles leer sein.
            ResetForm();
        }
    }

    private void LoadContactData()
    {
        PageTitleLabel.Text = "Kontakt Bearbeiten";
        ActionBtn.Text = "Speichern";

        if (_currentContact != null)
        {
            FirstNameEntry.Text = _currentContact.FirstName;
            LastNameEntry.Text = _currentContact.LastName;
            EmailEntry.Text = _currentContact.Email;
            PhoneEntry.Text = _currentContact.Phone;
            CompanyEntry.Text = _currentContact.Company;
            NotesEditor.Text = _currentContact.Notes;
            FavoriteSwitch.IsToggled = _currentContact.IsFavorite;
            if (_currentContact.ContactType == "Geschäftlich")
                RadioBusiness.IsChecked = true;
            else
                RadioPrivate.IsChecked = true;
            TrustSlider.Value = _currentContact.TrustLevel;
            CategoryPicker.SelectedItem = _currentContact.Category;
            if (_currentContact.BirthDate.HasValue)
            {
                BirthDatePicker.Date = _currentContact.BirthDate.Value;
            }

        }
        
        ResetValidationLabels();
    }

    private void ResetForm()
    {
        PageTitleLabel.Text = "Hinzufügen";
        ActionBtn.Text = "Hinzufügen";

        // Felder leeren
        FirstNameEntry.Text = string.Empty;
        LastNameEntry.Text = string.Empty;
        EmailEntry.Text = string.Empty;
        PhoneEntry.Text = string.Empty;
        CompanyEntry.Text = string.Empty;
        NotesEditor.Text = string.Empty;
        FavoriteSwitch.IsToggled = false;
        RadioPrivate.IsChecked = true;
        TrustSlider.Value = 5;
        CategoryPicker.SelectedItem = null;
        BirthDatePicker.Date = DateTime.Today;

        // Sicherstellen, dass _currentContact auch wirklich weg ist
        _currentContact = null;
        
        ResetValidationLabels();
    }

    private void ResetValidationLabels()
    {
        ErrFirstName.IsVisible = false;
        ErrLastName.IsVisible = false;
        ErrEmail.IsVisible = false;
        ErrPhone.IsVisible = false;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (ValidateForm())
        {
            if (_currentContact == null) _currentContact = new Contact();

            _currentContact.FirstName = FirstNameEntry.Text;
            _currentContact.LastName = LastNameEntry.Text;
            _currentContact.Email = EmailEntry.Text;
            _currentContact.Phone = PhoneEntry.Text;
            _currentContact.Company = CompanyEntry.Text;
            _currentContact.Notes = NotesEditor.Text;
            _currentContact.IsFavorite = FavoriteSwitch.IsToggled;
            if (RadioBusiness.IsChecked) 
                _currentContact.ContactType = "Geschäftlich";
            else 
                _currentContact.ContactType = "Privat";
            _currentContact.TrustLevel = TrustSlider.Value;
            _currentContact.Category = CategoryPicker.SelectedItem?.ToString();
            _currentContact.BirthDate = BirthDatePicker.Date;

            // Speichern
            ContactService.SaveContacts(_currentContact);

            await DisplayAlert("Erfolg", "Kontakt wurde gespeichert", "OK");

            // Zurücksetzen und Navigieren
            state = "add"; 
            ResetForm(); // Direktes Leeren für den Fall, dass die Page gecached bleibt

            if (state == "add") 
                await Shell.Current.GoToAsync("//contacts"); 
            else
                await Shell.Current.GoToAsync(".."); 
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        // Wenn man im Add-Modus ist und nichts eingetippt hat, direkt raus
        if (state == "add" && string.IsNullOrWhiteSpace(FirstNameEntry.Text) && string.IsNullOrWhiteSpace(LastNameEntry.Text))
        {
            await Shell.Current.GoToAsync("//contacts");
            return;
        }

        bool goBack = await DisplayAlert("Achtung", "Änderungen verwerfen?", "Ja", "Nein");
        if (!goBack) return;

        ResetForm(); // Sauber hinterlassen

        if (state == "add")
            await Shell.Current.GoToAsync("//contacts");
        else
            await Shell.Current.GoToAsync("..");
    }

    private bool ValidateForm()
    {
        bool isValid = true;
        ResetValidationLabels();

        if (string.IsNullOrWhiteSpace(FirstNameEntry.Text)) { ErrFirstName.IsVisible = true; isValid = false; }
        if (string.IsNullOrWhiteSpace(LastNameEntry.Text)) { ErrLastName.IsVisible = true; isValid = false; }
        
        if (!string.IsNullOrWhiteSpace(EmailEntry.Text) && !EmailEntry.Text.Contains("@")) 
        { 
            ErrEmail.IsVisible = true; isValid = false; 
        }

        if (!string.IsNullOrWhiteSpace(PhoneEntry.Text) && !PhoneEntry.Text.Any(char.IsDigit))
        {
             ErrPhone.Text = "Ungültige Nummer";
             ErrPhone.IsVisible = true; 
             isValid = false;
        }

        return isValid;
    }
}