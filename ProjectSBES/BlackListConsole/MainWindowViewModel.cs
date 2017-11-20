using BlackListConsole.Command;
using Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackListConsole
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public AddCommand AddCommand { get; set; }
        public RemoveCommand RemoveCommand { get; set; }
        private List<BlackListElement> blackListItems = new List<BlackListElement>();
        private string processName, groupName, personName;
        private string processName2, groupName2, personName2;

        public MainWindowViewModel()
        {
            this.RemoveCommand = new RemoveCommand(this);
            this.AddCommand = new AddCommand(this);
            blackListItems = GetItemsFromFile();           
        }

        private static List<BlackListElement> GetItemsFromFile()
        {
            List<BlackListElement> listOfElements = new List<BlackListElement>();
            BlackListElement element;

            foreach (DictionaryEntry entry in BlackListConfiguration.ResourceSet)
            {
                element = new BlackListElement();
                element.Process = entry.Key.ToString();
                element.GroupPerson = entry.Value.ToString();
                listOfElements.Add(element);
            }

            return listOfElements;
        }

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public List<BlackListElement> BlackListItems
        {
            get
            {
                return blackListItems;
            }

            set
            {
                blackListItems = value;
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("BlackListItems"));
            }
        }

        public string ProcessName
        {
            get
            {
                return processName;
            }

            set
            {
                processName = value;
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("ProcessName"));
            }
        }

        public string GroupName
        {
            get
            {
                return groupName;
            }

            set
            {
                groupName = value;
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("GroupName"));
            }
        }

        public string PersonName
        {
            get
            {
                return personName;
            }

            set
            {
                personName = value;
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("PersonName"));
            }
        }

        public string ProcessName2
        {
            get
            {
                return processName2;
            }

            set
            {
                processName2 = value;
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("ProcessName2"));
            }
        }

        public string GroupName2
        {
            get
            {
                return groupName2;
            }

            set
            {
                groupName2 = value;
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("GroupName2"));
            }
        }

        public string PersonName2
        {
            get
            {
                return personName2;
            }

            set
            {
                personName2 = value;
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("PersonName2"));
            }
        }
    }
}
