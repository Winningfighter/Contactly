using System.Text.Json;
using Contactly.Models;
using Contact = Contactly.Models.Contact;

namespace Contactly.Services;

public class ContactService
{
    private static string _filePath = Path.Combine(FileSystem.AppDataDirectory, "contacts.json");

    public static List<Contact> LoadContacts()
    {
        if (!File.Exists(_filePath))
            return new List<Contact>();
        
        string json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<Contact>>(json) ?? new List<Contact>();
    }

    public static void SaveContacts(Contact contact)
    {
        List<Contact> contacts = LoadContacts();
        
        var existing = contacts.FirstOrDefault(x => x.Id == contact.Id);

        if (existing != null)
        {
            contacts.Remove(existing);
            contacts.Add(contact);
        }
        else
        {
            contacts.Add(contact);
        }
        
        SaveAll(contacts);
    }

    public static void DeleteContact(string id)
    {
        List<Contact> contacts = LoadContacts();
        var contactsToRemove = contacts.FirstOrDefault(x => x.Id == id);

        if (contactsToRemove != null)
        {
            contacts.Remove(contactsToRemove);
            SaveAll(contacts);
        }
    }
    
    private static void SaveAll(List<Contact> contacts)
    {
        string json = JsonSerializer.Serialize(contacts);
        File.WriteAllText(_filePath, json);
    }
}