using Contact = Contactly.Models.Contact;

namespace Contactly.Views;

[QueryProperty(nameof(ContactToEdit), "Contact")]
public partial class ContactsFormPage : ContentPage
{

    public ContactsFormPage()
    {
        InitializeComponent();
    }

    private Contact _currentContact;

	public Contact ContactToEdit
	{
		set
		{
			_currentContact = value;
			LoadContactData();
		}
	}


	private void LoadContactData()
	{
		if (_currentContact != null)
		{
			PageTitleLabel.Text = "Kontakt Bearbeiten";
			ActionBtn.Text = "Speichern";

			FirstNameEntry.Text = _currentContact.FirstName;
			LastNameEntry.Text = _currentContact.LastName;
			EmailEntry.Text = _currentContact.Email;
			PhoneEntry.Text = _currentContact.Phone;
			CompanyEntry.Text = _currentContact.Company;
			NotesEditor.Text = _currentContact.Notes;
		}
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

            await DisplayAlert("Erfolg", "Kontakt wurde gespeichert", "OK");
            await Shell.Current.GoToAsync(".."); 
        }
	}

	private async void OnCancelClicked(object sender, EventArgs e)
	{
        // NotesEditor.Text = Shell.Current.ToString();
		bool goBack = await DisplayAlert("Achtung", "Deine Änderungen werden verworfen wenn du jetzt abbrichst!", "Trotzdem Verlassen", "Zurück");
		if (goBack) await Shell.Current.GoToAsync("..");
    }

    private bool ValidateForm()
    {
        bool isValid = true;

        // Reset Errors
        ErrFirstName.IsVisible = false;
        ErrLastName.IsVisible = false;
        ErrEmail.IsVisible = false;
        ErrPhone.IsVisible = false;

        // Vorname Prüfung
        if (string.IsNullOrWhiteSpace(FirstNameEntry.Text))
        {
            ErrFirstName.IsVisible = true;
            isValid = false;
        }

        // Nachname Prüfung
        if (string.IsNullOrWhiteSpace(LastNameEntry.Text))
        {
            ErrLastName.IsVisible = true;
            isValid = false;
        }

        // Email Prüfung (Einfach)
        if (!string.IsNullOrWhiteSpace(EmailEntry.Text) && !EmailEntry.Text.Contains("@"))
        {
            ErrEmail.IsVisible = true;
            isValid = false;
        }

        // Telefon Prüfung (Nur als Beispiel: muss Zahlen enthalten)
        if (!string.IsNullOrWhiteSpace(PhoneEntry.Text) && !PhoneEntry.Text.Any(char.IsDigit))
        {
	        ErrPhone.Text = "Geben Sie eine korrekte Telefonnummer an!";
            ErrPhone.IsVisible = true;
            isValid = false;
        }

        if (!string.IsNullOrWhiteSpace(PhoneEntry.Text) && PhoneEntry.Text.Any(char.IsLetter))
        {
	        ErrPhone.Text = "Telefonnummer darf keine Buchstaben enthalten.";
	        ErrPhone.IsVisible = true;
	        isValid = false;
        }

        return isValid;
    }
}