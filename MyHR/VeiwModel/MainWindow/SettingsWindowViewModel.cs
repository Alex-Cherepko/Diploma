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
            string connectionString = new ConnectionToDB(true).ConnectionString;


            if (string.IsNullOrEmpty(connectionString))
                return false;

            if (Database.Exists(connectionString))
            {
                return true;
            }
            return false;
        }

        private void RunMainWindow(Window window)
        {
            DataLogger Logger = new DataLogger();

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
                bool bdExists = false;
                FillSettingsFile();
                ConnectionToDB connectionToDB = new ConnectionToDB(false);
                using (EntityContext context = new EntityContext(connectionToDB.dbConnection, true))
                {
                    if (!Database.Exists(connectionToDB.ConnectionString))
                    {
                        try
                        {
                            context.Database.Initialize(false);
                        }
                        catch(Exception e)
                        {
                            Logger.WriteToLog(@"Не удалось создать базу данных");
                            Logger.WriteToLog(e.Message);
                        }

                    }
                    bdExists = context.Database.Exists();

                }

                if (bdExists)
                {
                    ShowMainWindow(window);
                }
                else
                { MessageBox.Show("Не удалось создать базу данных"); }
            }
        }

        private void FillSettingsFile()
        {
            List<ConnectionAttributes> connectionStringList = new List<ConnectionAttributes>
            {
                new ConnectionAttributes(BaseName, ServerName, "System.Data.SqlClient", @"Data Source = " + ServerName + "; Initial Catalog = " + BaseName + "; Integrated Security = True")
            };

            XmlDocument doc = new XmlDocument();

            XmlNode root = doc.CreateElement("connectionStrings");

            foreach (ConnectionAttributes Element in connectionStringList)
            {
                XmlNode add = doc.CreateElement("add");

                XmlAttribute baseName = doc.CreateAttribute("BaseName");
                baseName.Value = Element.DateBaseName;
                add.Attributes.Append(baseName);

                XmlAttribute serverName = doc.CreateAttribute("ServerName");
                serverName.Value = Element.ServerName;
                add.Attributes.Append(serverName);

                XmlAttribute providerName = doc.CreateAttribute("ProviderName");
                providerName.Value = Element.ProviderName;
                add.Attributes.Append(providerName);

                XmlAttribute connectionString = doc.CreateAttribute("ConnectionString");
                connectionString.Value = Element.ConnectionString;
                add.Attributes.Append(connectionString);
                root.AppendChild(add);
            }

            doc.AppendChild(root);

            try
            {
                using (FileStream fs = File.Open("C:\\ProgramData\\MyHR\\Settings.config", FileMode.OpenOrCreate, FileAccess.Write))
                { doc.Save(fs); }
            }
            catch
            {
                MessageBox.Show("Не удалось сохранить файл настроек");
            }

        }


        protected class ConnectionAttributes
        {
            public string DateBaseName { get; set; }
            public string ServerName { get; set; }
            public string ProviderName { get; set; }
            public string ConnectionString { get; set; }

            public ConnectionAttributes(string dbName, string serverName, string providerName, string connectionString)
            {
                DateBaseName = dbName;
                ServerName = serverName;
                ProviderName = providerName;
                ConnectionString = connectionString;
            }
        }
    }
}
