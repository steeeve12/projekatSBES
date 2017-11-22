using Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace BlackListConsole.Command
{
    public class AddCommand : BlackListCommand
    {
        private MainWindowViewModel view;
        private static object syncObj = new object();

        public AddCommand(MainWindowViewModel view)
        {
            this.view = view;
        }

        public override void Execute(object parameter)
        {
            if (parameter == null ||
                !(parameter is Object[]))
                return;

            Object[] parameters = parameter as Object[];

            if (parameters[0].ToString() == "")
            {
                MessageBox.Show("You need to fill process name!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                clearFields();

                return;
            }          

            if (parameters[1].ToString() == "" && parameters[2].ToString() == "")
            {
                MessageBox.Show("You need to fill group or person name!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                clearFields();

                return;
            }

            if(parameters[0].ToString().Length >= 20 || parameters[1].ToString().Length >= 20 || parameters[2].ToString().Length >= 20)
            {
                MessageBox.Show("20 characters is limit!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                clearFields();

                return;
            }

            if (!Regex.IsMatch(parameters[0].ToString(), @"^[a-zA-Z0-9]*$") || !Regex.IsMatch(parameters[1].ToString(), @"^[a-zA-Z0-9]*$") || !Regex.IsMatch(parameters[2].ToString(), @"^[a-zA-Z0-9]*$"))
            {
                MessageBox.Show("Only letters and numbers are allowed!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                clearFields();

                return;
            }
           
            if(parameters[0].ToString() == "null" || parameters[1].ToString() == "null" || parameters[2].ToString() == "null" || parameters[0].ToString() == "NULL" || parameters[1].ToString() == "NULL" || parameters[2].ToString() == "NULL")
            {
                MessageBox.Show("Null string is not allowed!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                clearFields();
                
                return;
            }


            bool contain = false;
            List<BlackListElement> copyOfOriginalList = new List<BlackListElement>();
            copyOfOriginalList.AddRange(view.BlackListItems);
            string path = "../../../Contracts/BlackListFile.resx";
            string blackListValue = "";

            foreach (var process in copyOfOriginalList)
            {
                if (process.Process == parameters[0].ToString())
                {
                    contain = true;
                   
                    ResXResourceReader reader = new ResXResourceReader(path);
                    foreach (DictionaryEntry node in reader)
                    {
                        if (parameters[0].ToString() == node.Key.ToString())
                        {
                            blackListValue = node.Value.ToString();
                            break;
                        }                        
                    }
                    //Za slucaj da nema grupa i korisnika za taj proces
                    if (blackListValue == "")
                        blackListValue += "!";

                    //Za slucaj da lista ne sadrzi datu grupu
                    if (!blackListValue.Contains(parameters[1].ToString()) && parameters[1].ToString() != "")
                    {
                        //Za slucaj da vec ima grupa u listi
                        if (blackListValue[0] != '!')
                            blackListValue = blackListValue.Insert(blackListValue.IndexOf("!"), "," + parameters[1].ToString());
                        //Za slucaj da nema grupa u listi
                        else if (blackListValue[0] == '!')
                            blackListValue = blackListValue.Insert(blackListValue.IndexOf("!"), parameters[1].ToString());
                    }
                    //Za slucaj da lista ne sadrzi datu osobu
                    if (!blackListValue.Contains(parameters[2].ToString()) && parameters[2].ToString() != "")
                    { 
                        //Za slucaj da vec ima osoba u listi
                        if ((blackListValue.IndexOf("!") != blackListValue.Length - 1))
                            blackListValue = blackListValue.Insert(blackListValue.IndexOf("!") + 1, parameters[2].ToString() + ",");
                        //Za slucaj da nema osoba u listi
                        else if ((blackListValue.IndexOf("!") == blackListValue.Length - 1))
                            blackListValue += parameters[2].ToString();
                    }

                    process.GroupPerson = blackListValue;
                    view.BlackListItems = copyOfOriginalList;

                    writeToBlackListFile(path, reader, parameters[0].ToString(), blackListValue);

                    clearFields();

                    break;
                }              
            }

            if (!contain)
            {
                MessageBox.Show("Process with that name doesn't exist!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                clearFields();

                return;
            }

        }

        private void writeToBlackListFile(string path, ResXResourceReader reader, string Key, string blackListValue)
        {
            lock (syncObj)
            {
                ResXResourceWriter rsxw = new ResXResourceWriter(path);

                if (File.Exists(path))
                {
                    foreach (DictionaryEntry node in reader)
                    {
                        if (Key == node.Key.ToString())
                        {
                            rsxw.AddResource(Key.ToString(), blackListValue);
                        }
                        else rsxw.AddResource(node.Key.ToString(), node.Value);
                    }
                }
                rsxw.Close();
            }
        }

        private void clearFields()
        {
            view.GroupName = "";
            view.PersonName = "";
            view.ProcessName = "";
        }
    }
}
