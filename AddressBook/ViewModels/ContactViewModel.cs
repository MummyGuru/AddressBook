using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using AddressBook.Models;
using AddressBook.Services;
using Microsoft.Win32;

namespace AddressBook.ViewModels
{
    public class ContactViewModel : INotifyPropertyChanged
    {
        private DataService _dataService;
        public Contact Contact { get; set; }

        public ObservableCollection<string> Categories { get; set; }

        public ContactViewModel(Contact contact, DataService dataService)
        {
            Contact = contact;
            _dataService = dataService;

            Categories = new ObservableCollection<string>();
            Categories.Add("Общее");
            Categories.Add("Друзья");
            Categories.Add("Работа");
            Categories.Add("Семья");
            Categories.Add("Клиенты");

            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
            BrowsePhotoCommand = new RelayCommand(BrowsePhoto);
        }

        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand BrowsePhotoCommand { get; private set; }

        private bool CanSave(object parameter)
        {
            return !string.IsNullOrEmpty(Contact.FirstName);
        }

        private void Save(object parameter)
        {
            System.Windows.Window window = null;
            foreach (System.Windows.Window w in System.Windows.Application.Current.Windows)
            {
                if (w.IsActive)
                {
                    window = w;
                    break;
                }
            }
            if (window != null)
                window.DialogResult = true;
        }

        private void Cancel(object parameter)
        {
            System.Windows.Window window = null;
            foreach (System.Windows.Window w in System.Windows.Application.Current.Windows)
            {
                if (w.IsActive)
                {
                    window = w;
                    break;
                }
            }
            if (window != null)
                window.DialogResult = false;
        }

        private void BrowsePhoto(object parameter)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp";
            dialog.Title = "Выберите фото";

            if (dialog.ShowDialog() == true)
            {
                Contact.PhotoPath = dialog.FileName;
                OnPropertyChanged("Contact");
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