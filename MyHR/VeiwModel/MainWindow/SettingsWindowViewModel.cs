using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Linq;
using System.Data.Entity;

namespace MyHR
{
    public class SettingsWindowViewModel : BaseViewModel
    {
        public string ServerName { get; set; }

        public string BaseName { get; set; }

        public ICommand AcceptChanges { get; set; }

        public ICommand AbortChanges { get; set; }

        public SettingsWindowViewModel(Window window)
        {
            AbortChanges = new RelayCommand(() => window.Close());
            AcceptChanges = new RelayCommand(() => RunMainWindow(window));

            if (ConnectionExist())
            {
                ShowMainWindow(window);
            }
        }

        private void ShowMainWindow(Window window)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            window.Close();
        }

        private bool ConnectionExist()
        {
            string connectionString = "";

            if (File.Exists("Settings.config"))
            {
                XDocument xdoc = XDocument.Load("Settings.config");
                foreach (XElement Element in xdoc.Element("connectionStrings").Elements("add"))
                {
                    XAttribute nameAttribute = Element.Attribute("connectionString");
                    connectionString = nameAttribute.Value;
                }
               
            }
            else
            {
                return false;
            }
            
            if(Database.Exists(connectionString))
            {
                return true;
            }
            return false;
        }

        private void RunMainWindow(Window window)
        {
            bool Abort = false;

            if (String.IsNullOrEmpty(ServerName))
            {
                MessageBox.Show("Укажите имя сервера");
                Abort = true;
            }

            if (String.IsNullOrEmpty(BaseName))
            {
                MessageBox.Show("Укажите имя базы данных");
                Abort = true;
            }

            if (!Abort)
            {
                FillSettingsFile();
                ShowMainWindow(window);
            }
        }

        private void FillSettingsFile()
        {
            List<add> connectionStrings = new List<add>();
            connectionStrings.Add(new add("ConnectionToDB", "Data Source="+ ServerName + ";Initial Catalog="+ BaseName + ";Integrated Security=True", "System.Data.SqlClient")) ;

            XmlDocument doc = new XmlDocument();

            XmlNode root = doc.CreateElement("connectionStrings");

            foreach( add Element in connectionStrings)
            {
                XmlNode add = doc.CreateElement("add");

                XmlAttribute name = doc.CreateAttribute("name");
                name.Value = Element.name;
                add.Attributes.Append(name);

                XmlAttribute connectionString = doc.CreateAttribute("connectionString");
                connectionString.Value = Element.connectionString;
                add.Attributes.Append(connectionString);

                XmlAttribute providerName = doc.CreateAttribute("providerName");
                providerName.Value = Element.providerName;
                add.Attributes.Append(providerName);

                root.AppendChild(add);
            }

            doc.AppendChild(root);

            using (FileStream fs = new FileStream("Settings.config", FileMode.OpenOrCreate))
            {
                doc.Save(fs);
            }
            
            
        }

        
        protected class add
        {
            public string name { get; set; }
            public string connectionString { get; set; }
            public string providerName { get; set; }

            public add(string name, string connectionString, string providerName)
            {
                this.name = name;
                this.connectionString = connectionString;
                this.providerName = providerName;

            }
        }
    }
}
