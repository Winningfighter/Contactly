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