namespace Contactly.Models;

public class Contact
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Company { get; set; }
    public string Notes { get; set; }
    
    public bool IsFavorite { get; set; }        // Favorit (Ja/Nein)
    public string ContactType { get; set; }     // "Privat" oder "Geschäftlich"
    public double TrustLevel { get; set; }      // Vertrauen (0-10)
    public string Category { get; set; }        // Kategorie
    public DateTime? BirthDate { get; set; }    // Geburtstag (Nullable, falls leer)

    public string FullName => $"{FirstName} {LastName}";

    public string Initials
    {
        get
        {
            char first = string.IsNullOrWhiteSpace(FirstName) ? '?' : FirstName[0];
            char last = string.IsNullOrWhiteSpace(LastName) ? '?' : LastName[0];
            return $"{first}{last}".ToUpper();
        }
    }
}