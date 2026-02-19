using AddressBook.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace AddressBook.Services
{
    public class DataService
    {
        private string _filePath;

        public DataService()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "contacts.json");
        }

        public ObservableCollection<Contact> LoadContacts()
        {
            if (!File.Exists(_filePath))
                return new ObservableCollection<Contact>();

            try
            {
                string json = File.ReadAllText(_filePath);
                var contacts = JsonSerializer.Deserialize<ObservableCollection<Contact>>(json);
                if (contacts == null)
                    return new ObservableCollection<Contact>();
                return contacts;
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show("Ошибка загрузки: " + ex.Message);
                return new ObservableCollection<Contact>();
            }
        }

        public void SaveContacts(ObservableCollection<Contact> contacts)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(contacts, options);
                File.WriteAllText(_filePath, json);
                System.Windows.MessageBox.Show("Сохранено в: " + _filePath);
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show("Ошибка сохранения: " + ex.Message);
            }
        }
    }
}