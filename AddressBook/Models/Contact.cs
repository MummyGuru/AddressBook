using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace AddressBook.Models
{
    public class Contact : INotifyPropertyChanged, IDataErrorInfo
    {
        public int Id { get; set; }

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; OnPropertyChanged(nameof(FirstName)); }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; OnPropertyChanged(nameof(LastName)); }
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; OnPropertyChanged(nameof(Phone)); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }

        private string _photoPath;
        public string PhotoPath
        {
            get { return _photoPath; }
            set { _photoPath = value; OnPropertyChanged(nameof(PhotoPath)); }
        }

        private string _category;
        public string Category
        {
            get { return _category; }
            set { _category = value; OnPropertyChanged(nameof(Category)); }
        }

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; OnPropertyChanged(nameof(IsActive)); }
        }

        public DateTime CreatedDate { get; set; }

        public Contact()
        {
            _firstName = "";
            _lastName = "";
            _phone = "";
            _email = "";
            _photoPath = "";
            _category = "Общее";
            _isActive = true;
            CreatedDate = DateTime.Now;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public string Error => string.Empty;

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(FirstName):
                        if (string.IsNullOrWhiteSpace(FirstName))
                            return "Имя обязательно";
                        break;

                    case nameof(LastName):
                        if (!string.IsNullOrWhiteSpace(LastName) && LastName.Length < 2)
                            return "Фамилия должна содержать хотя бы 2 символа";
                        break;

                    case nameof(Email):
                        if (!string.IsNullOrWhiteSpace(Email) && !IsValidEmail(Email))
                            return "Некорректный формат email";
                        break;

                    case nameof(Phone):
                        if (!string.IsNullOrWhiteSpace(Phone) && !IsValidPhone(Phone))
                            return "Телефон может содержать только цифры, пробелы, '+' и '-'";
                        break;
                }
                return string.Empty;
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhone(string phone)
        {
            return Regex.IsMatch(phone, @"^[\d\s\+\-]+$");
        }
    }
}