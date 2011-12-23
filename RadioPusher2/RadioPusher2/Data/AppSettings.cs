using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Threading;


namespace Data
{
    public class AppSettings
    {
        private string f_UserName;
        private string f_ServerAddress;
        private string f_Password;

        public string UserName
        {
            get { return f_UserName; }
            set { f_UserName = value; }
        }
        
        public string ServerAddress
        {
            get { return f_ServerAddress; }
            set { f_ServerAddress = value; }
        }

        public string Password
        {
            get { return f_Password; }
            set { f_Password = value; }
        }

        public AppSettings()
        {
        }

        public static AppSettings Load()
        {
            AppSettings retContainer;
            string loadPath = Path.Combine("Data","AppSettings");
            if (!File.Exists(loadPath))
            {
                retContainer = new AppSettings();
                retContainer.Save();
                return retContainer;
            }
            retContainer = new AppSettings();
//            lock (NewGUI.savelock) {
                XmlSerializer s = new XmlSerializer(typeof(AppSettings));
                TextReader w = new StreamReader(Path.Combine("Data", "AppSettings"));
                retContainer = (AppSettings)s.Deserialize(w);
                w.Close();
//            }
            return retContainer;
        }
        public bool Save()
        {
            XmlSerializer s = new XmlSerializer(typeof(AppSettings));
            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
            }
            try {
                //lock (NewGUI.savelock) {
                    TextWriter w = new StreamWriter(Path.Combine("Data", "AppSettings"));
                    s.Serialize(w, this);
                    w.Close();
   //             }
            } catch {
                // :(
#if DEBUG
                // Console.WriteLine("IO EXCEPTION IM AppSettingsCONTAINER!");
#endif
            }
            return true;
        }
    }
}
