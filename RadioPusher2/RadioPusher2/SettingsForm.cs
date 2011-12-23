using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Data;
using System.Threading;
using Utils;

namespace RadioPusher2
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            AppSettings appsettings = AppSettings.Load();
            textBoxU.Text = appsettings.UserName;
            textBoxP.Text = appsettings.Password;
            textBoxServer.Text = appsettings.ServerAddress;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            AppSettings appsettings = new AppSettings();
            appsettings.UserName = textBoxU.Text;
            appsettings.Password = textBoxP.Text;
            appsettings.ServerAddress = textBoxServer.Text;

            new Thread(delegate()
            {
                NWebClient nwc = new NWebClient(15000);
                string res = nwc.DownloadString(textBoxServer.Text.Replace("demovibes", "account/signin"));
                Dictionary<string, string> kvp = new Dictionary<string, string>();
                kvp.Add("next", "");
                kvp.Add("username", textBoxU.Text);
                kvp.Add("password", textBoxP.Text);
                kvp.Add("blogin", "Sign+in");
                res = nwc.PostAction(textBoxServer.Text.Replace("demovibes", "account/signin"), kvp);
                if (res.Contains("Welcome, " + textBoxU.Text))
                {
                    MessageBox.Show("Login OK - storing data");
                    if (InvokeRequired)
                    {
                        BeginInvoke(new MethodInvoker(delegate()
                        {
                            appsettings.Save();
                            Close();
                        }));
                    }
                }
                else
                {
                    MessageBox.Show("Can't login - something wrong with your data");
                }
                //next=&username=rams&password=521Vt1y6Ec&blogin=Sign+in


            }).Start();


        }
    }
}
