using System;
using System.ComponentModel;

namespace AddressBook.Models
{
    public class Contact : INotifyPropertyChanged, IDataErrorInfo
    {
        public int Id { get; set; }

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; OnPropertyChanged("FirstName"); }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; OnPropertyChanged("LastName"); }
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; OnPropertyChanged("Phone"); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged("Email"); }
        }

        private string _photoPath;
        public string PhotoPath
        {
            get { return _photoPath; }
            set { _photoPath = value; OnPropertyChanged("PhotoPath"); }
        }

        private string _category;
        public string Category
        {
            get { return _category; }
            set { _category = value; OnPropertyChanged("Category"); }
        }

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; OnPropertyChanged("IsActive"); }
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
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public string Error
        {
            get { return ""; }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "FirstName")
                {
                    if (string.IsNullOrEmpty(FirstName))
                        return "Имя обязательно";
                }
                return "";
            }
        }
    }
}