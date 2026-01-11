using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Contactly.Views;

// Dieser Code wurde mithilfe von ChatGPT erstellt
public partial class FaqPage : ContentPage
{

    public ObservableCollection<FaqItem> FaqItems { get; set; } = new ObservableCollection<FaqItem>();

    public FaqPage()
    {
        InitializeComponent();
        LoadFaqData();
        BindingContext = this;
    }

    private void LoadFaqData()
    {
        FaqItems.Add(new FaqItem 
        { 
            Question = "Wie füge ich Kontakte hinzu?", 
            Answer = "Gehe im Menü auf 'Hinzufügen' oder klicke auf das Plus-Symbol, falls vorhanden. Fülle das Formular aus und klicke auf Speichern." 
        });

        FaqItems.Add(new FaqItem 
        { 
            Question = "Sind meine Daten sicher?", 
            Answer = "Ja, deine Daten werden lokal in einer JSON-Datei auf deinem Gerät gespeichert. Es werden keine Daten ins Internet hochgeladen." 
        });

        FaqItems.Add(new FaqItem 
        { 
            Question = "Wie kann ich einen Kontakt löschen?", 
            Answer = "In der Kontaktliste findest du neben jedem Kontakt ein Mülleimer-Symbol. Klicke darauf und bestätige die Warnung." 
        });

        FaqItems.Add(new FaqItem 
        { 
            Question = "Kann ich die App im Dark Mode nutzen?", 
            Answer = "Ja! Contactly passt sich automatisch deinen Systemeinstellungen an. Wechsle einfach in Windows zu 'Dunkel'." 
        });
    }


    private void OnQuestionTapped(object sender, TappedEventArgs e)
    {

        var faqItem = e.Parameter as FaqItem;

        if (faqItem != null)
        {

            faqItem.IsVisible = !faqItem.IsVisible;
            

            faqItem.Rotation = faqItem.IsVisible ? 180 : 0;
        }
    }
    
    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}


public class FaqItem : INotifyPropertyChanged
{
    public string Question { get; set; }
    public string Answer { get; set; }

    private bool _isVisible;
    public bool IsVisible
    {
        get => _isVisible;
        set
        {
            _isVisible = value;
            OnPropertyChanged();
        }
    }

    private double _rotation;
    public double Rotation
    {
        get => _rotation;
        set
        {
            _rotation = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}