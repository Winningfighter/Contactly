namespace Contactly.Views;

public partial class InfoPage : ContentPage
{
    public InfoPage()
    {
        InitializeComponent();
    }

    // Öffnet den Browser
    private async void OnWebsiteClicked(object sender, EventArgs e)
    {
        try
        {
            Uri uri = new Uri("https://github.com/Winningfighter/Contactly");
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception)
        {
            await DisplayAlert("Fehler", "Link konnte nicht geöffnet werden.", "OK");
        }
    }

    private async void OnCloseClicked(object sender, EventArgs e)
    {
        // Navigiert zurück
        await Shell.Current.GoToAsync("..");
    }
}