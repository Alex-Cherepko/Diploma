using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyHR
{
    public class DataLogger
    {
        public DataLogger()
        {

        }

        public void WriteToLog(string strnLog)
        {
            string writePath = @"C:\ProgramData\MyHR\Log.txt";

            try
            {
                using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                {
                    sw.WriteLine(strnLog);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
