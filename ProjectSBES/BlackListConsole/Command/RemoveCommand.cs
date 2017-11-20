using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BlackListConsole.Command
{
    public class RemoveCommand : BlackListCommand
    {
        private MainWindowViewModel view;
        private static object syncObj = new object();

        public RemoveCommand(MainWindowViewModel view)
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

                    //Za slucaj da nema grupa i osoba za taj proces na black listi
                    if (blackListValue == "")
                        break;

                    //Za grupe
                    if (blackListValue.Contains(parameters[1].ToString()) && parameters[1].ToString() != "")
                    {                                            
                        //ako je jedina grupa
                        if (blackListValue.IndexOf(parameters[1].ToString()) == 0 && blackListValue[parameters[1].ToString().Length] == '!')
                            blackListValue = blackListValue.Remove(blackListValue.IndexOf(parameters[1].ToString()), parameters[1].ToString().Length);
                        //ako se nalazi na pocetku grupa
                        else if (blackListValue.IndexOf(parameters[1].ToString()) == 0)
                            blackListValue = blackListValue.Remove(blackListValue.IndexOf(parameters[1].ToString()), parameters[1].ToString().Length + 1);
                        //ako se nalazi na kraju ili u sredini
                        else if (blackListValue[blackListValue.IndexOf(parameters[1].ToString()) - 1] == ',')
                            blackListValue = blackListValue.Remove(blackListValue.IndexOf(parameters[1].ToString()) - 1, parameters[1].ToString().Length + 1);                     
                    }
                    //Za osobe
                    if (blackListValue.Contains(parameters[2].ToString()) && parameters[2].ToString() != "")
                    {                      
                        //Ako je jedina osoba
                        if(blackListValue[blackListValue.IndexOf(parameters[2].ToString()) - 1] == '!' && blackListValue.IndexOf(parameters[2].ToString()) + parameters[2].ToString().Length == blackListValue.Length)
                        {
                            blackListValue = blackListValue.Remove(blackListValue.IndexOf(parameters[2].ToString()), parameters[2].ToString().Length);
                        }
                        // ako se nalazi na kraju ili u sredini
                        else if(blackListValue[blackListValue.IndexOf(parameters[2].ToString()) - 1] != '!')
                        {
                            blackListValue = blackListValue.Remove(blackListValue.IndexOf(parameters[2].ToString()) - 1, parameters[2].ToString().Length + 1);
                        }
                        // ako se nalazi na pocetku, a nije jedina osoba
                        else if(blackListValue[blackListValue.IndexOf(parameters[2].ToString()) - 1] == '!')
                        {
                            blackListValue = blackListValue.Remove(blackListValue.IndexOf(parameters[2].ToString()), parameters[2].ToString().Length + 1);
                        }
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
            view.GroupName2 = "";
            view.PersonName2 = "";
            view.ProcessName2 = "";
        }
    }
}
