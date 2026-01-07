using Contactly.Models;

namespace Contactly.Views;

[QueryProperty(nameof(Contact), "Contact")]
public partial class ContactDetailPage : ContentPage
{
    private Models.Contact _contact;
    public Models.Contact Contact
    {
        set
        {
            _contact = value;
            BindData();
        }
    }

    public ContactDetailPage()
	{
		InitializeComponent();
	}

    private void BindData()
    {
        if (_contact == null) return;

        HeaderNameLabel.Text = $"{_contact.FirstName} {_contact.LastName}";
        DetailFirstName.Text = _contact.FirstName;
        DetailLastName.Text = _contact.LastName;
        DetailEmail.Text = _contact.Email;
        DetailPhone.Text = _contact.Phone;
        DetailCompany.Text = _contact.Company;
        DetailNotes.Text = _contact.Notes;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}