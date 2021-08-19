using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace MyHR
{
    public class ConnectionToDB
    {
        public string ConnectionString { get; set; } = "";
        public DbConnection dbConnection { get; set; } = null;

        private string BaseName;
        private string ServerName;

        public ConnectionToDB(bool OnlyConectionString)
        {
            if (!Directory.Exists("C:\\ProgramData\\MyHR"))
            {
                Directory.CreateDirectory("C:\\ProgramData\\MyHR");
            }

            if (File.Exists("C:\\ProgramData\\MyHR\\Settings.config"))
            {

                XDocument xdoc = XDocument.Load("C:\\ProgramData\\MyHR\\Settings.config");
                foreach (XElement Element in xdoc.Element("connectionStrings").Elements("add"))
                {
                    XAttribute BaseNameAttribute = Element.Attribute("BaseName");
                    BaseName = BaseNameAttribute.Value;
                    XAttribute ServerNameAttribute = Element.Attribute("ServerName");
                    ServerName = ServerNameAttribute.Value;
                    XAttribute ConnectionStringAttribute = Element.Attribute("ConnectionString");
                    ConnectionString = ConnectionStringAttribute.Value;
                    
                }

                if (!OnlyConectionString)
                {
                    dbConnection = GetSqlConn4DbName(ServerName, BaseName);
                }

            }
        }

        public DbConnection GetSqlConn4DbName(string dataSource, string dbName)
        {
            var sqlConnStringBuilder = new SqlConnectionStringBuilder
            {
                DataSource = dataSource,
                IntegratedSecurity = true,
                MultipleActiveResultSets = true
            };
            // NOW MY PROVIDER FACTORY OF CHOICE, switch providers here 
            var sqlConnFact = new SqlConnectionFactory(sqlConnStringBuilder.ConnectionString);
            var sqlConn = sqlConnFact.CreateConnection(dbName);
            return sqlConn;
        }
    }
}
