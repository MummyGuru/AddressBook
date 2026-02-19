using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Data;
using System.Windows.Input;
using AddressBook.Models;
using AddressBook.Services;
using AddressBook.Views;
using Microsoft.Win32;

namespace AddressBook.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private DataService _dataService;
        public ObservableCollection<Contact> Contacts { get; set; }
        public ICollectionView ContactsView { get; set; }

        private string _searchQuery;
        public string SearchQuery
        {
            get { return _searchQuery; }
            set
            {
                _searchQuery = value;
                ContactsView.Refresh();
                OnPropertyChanged("SearchQuery");
            }
        }

        private string _selectedCategory;
        public string SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                ContactsView.Refresh();
                OnPropertyChanged("SelectedCategory");
            }
        }

        public ObservableCollection<string> Categories { get; set; }

        private Contact _selectedContact;
        public Contact SelectedContact
        {
            get { return _selectedContact; }
            set
            {
                _selectedContact = value;
                OnPropertyChanged("SelectedContact");
            }
        }

        public MainViewModel()
        {
            _dataService = new DataService();
            Contacts = _dataService.LoadContacts();
            ContactsView = CollectionViewSource.GetDefaultView(Contacts);
            ContactsView.Filter = new Predicate<object>(FilterContacts);

            Categories = new ObservableCollection<string>();
            Categories.Add("Все");
            Categories.Add("Общее");
            Categories.Add("Друзья");
            Categories.Add("Работа");
            Categories.Add("Семья");
            Categories.Add("Клиенты");
            _selectedCategory = "Все";
            _searchQuery = "";

            AddCommand = new RelayCommand(AddContact);
            EditCommand = new RelayCommand(EditContact, CanEditDelete);
            DeleteCommand = new RelayCommand(DeleteContact, CanEditDelete);
            SaveCommand = new RelayCommand(SaveAll);
            LoadCommand = new RelayCommand(LoadAll);
            ExportCommand = new RelayCommand(ExportToCsv);
        }

        private bool FilterContacts(object obj)
        {
            Contact contact = obj as Contact;
            if (contact == null)
                return false;

            if (!string.IsNullOrEmpty(SearchQuery))
            {
                bool matchesSearch = contact.FirstName.IndexOf(SearchQuery, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                   contact.LastName.IndexOf(SearchQuery, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                   contact.Phone.IndexOf(SearchQuery, StringComparison.OrdinalIgnoreCase) >= 0;
                if (!matchesSearch)
                    return false;
            }

            if (SelectedCategory != "Все")
            {
                if (contact.Category != SelectedCategory)
                    return false;
            }

            return true;
        }

        public ICommand AddCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand LoadCommand { get; private set; }
        public ICommand ExportCommand { get; private set; }

        private void AddContact(object parameter)
        {
            Contact newContact = new Contact();
            ContactViewModel viewModel = new ContactViewModel(newContact, _dataService);
            ContactEditWindow window = new ContactEditWindow();
            window.DataContext = viewModel;

            bool? result = window.ShowDialog();
            if (result == true && viewModel.Contact != null)
            {
                Contacts.Add(viewModel.Contact);
            }
        }

        private void EditContact(object parameter)
        {
            if (SelectedContact == null)
                return;

            ContactViewModel viewModel = new ContactViewModel(SelectedContact, _dataService);
            ContactEditWindow window = new ContactEditWindow();
            window.DataContext = viewModel;
            window.ShowDialog();
        }

        private bool CanEditDelete(object parameter)
        {
            return SelectedContact != null;
        }

        private void DeleteContact(object parameter)
        {
            if (SelectedContact == null)
                return;

            System.Windows.MessageBoxResult res = System.Windows.MessageBox.Show(
                "Удалить контакт " + SelectedContact.FirstName + " " + SelectedContact.LastName + "?",
                "Подтверждение",
                System.Windows.MessageBoxButton.YesNo);

            if (res == System.Windows.MessageBoxResult.Yes)
            {
                Contacts.Remove(SelectedContact);
            }
        }

        private void SaveAll(object parameter)
        {
            _dataService.SaveContacts(Contacts);
            System.Windows.MessageBox.Show("Данные сохранены!", "Сохранение");
        }

        private void LoadAll(object parameter)
        {
            Contacts.Clear();
            var loadedContacts = _dataService.LoadContacts();
            foreach (var contact in loadedContacts)
            {
                Contacts.Add(contact);
            }
            ContactsView.Refresh();
            System.Windows.MessageBox.Show("Данные загружены! Контактов: " + Contacts.Count, "Загрузка");
        }

        private void ExportToCsv(object parameter)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "CSV файлы|*.csv";
            dialog.FileName = "contacts_" + DateTime.Now.ToString("yyyyMMdd") + ".csv";

            if (dialog.ShowDialog() == true)
            {
                string csv = "ID,Имя,Фамилия,Телефон,Email,Категория\n";
                foreach (Contact c in Contacts)
                {
                    csv += c.Id + "," + c.FirstName + "," + c.LastName + "," + c.Phone + "," + c.Email + "," + c.Category + "\n";
                }
                File.WriteAllText(dialog.FileName, csv, System.Text.Encoding.UTF8);
                System.Windows.MessageBox.Show("Экспорт выполнен!", "Экспорт");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}