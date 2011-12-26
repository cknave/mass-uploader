using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using RadioPusher2.Data;
using Utils;
using System.Threading;

namespace RadioPusher2
{
    public partial class SearchForm : Form
    {
        string SearchUrl = String.Empty;
        public string mKey = String.Empty;
        public string mValue = String.Empty;
        public string ResultKey = String.Empty;
        public string ResultValue = String.Empty;
        string query = String.Empty;
        List<searchDS> ds = new List<searchDS>();

        public SearchForm(string searchurl,string initialQuery, string matchKey, string matchValue)
        {
            InitializeComponent();
            DataGridViewTextBoxColumn keyID = new DataGridViewTextBoxColumn();
            keyID.DataPropertyName = "mKey";
            keyID.HeaderText = "mKey";
            keyID.Visible = false;
            keyID.Name = "mKey";
            keyID.Width = 50;

            DataGridViewTextBoxColumn valueID = new DataGridViewTextBoxColumn();
            valueID.DataPropertyName = "mValue";
            valueID.HeaderText = "mValue";
            valueID.Visible = true;
            valueID.Name = "mValue";
            valueID.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            valueID.ReadOnly = true;
            dataGridViewSearch.Columns.Add(keyID);
            dataGridViewSearch.Columns.Add(valueID);


            dataGridViewSearch.CellClick += new DataGridViewCellEventHandler(dataGridViewSearch_CellClick);

            SearchUrl = searchurl;
            mKey = matchKey;
            mValue = matchValue;
            query = initialQuery;
            textBoxSearch.Text = query;
        }

        private void dataGridViewSearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                ResultKey = dataGridViewSearch.Rows[e.RowIndex].Cells[0].Value.ToString();
            }catch(Exception ee){
                MessageBox.Show(ee.Message);
            }

            try
            {
                ResultValue = dataGridViewSearch.Rows[e.RowIndex].Cells[1].Value.ToString();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }

            this.Close();

        }



        private void textBoxSearch_KeyUp(object sender, KeyEventArgs e)
        {
    

            if (e.KeyData == Keys.Return){
                ds.Clear();

                new Thread(delegate()
                {
                    try
                    {
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    jss.RegisterConverters(new JavaScriptConverter[] { new DynamicJsonConverter() });
                    NWebClient cw = new NWebClient(15000);
                    string json = cw.DownloadString(String.Format("{0}search/ajax/artist/?q={1}", config.hostname.Replace("/demovibes/", "/"), textBoxSearch.Text.Trim()));
                    dynamic artists = jss.Deserialize(json, typeof(object)) as dynamic;


                    foreach (var artist in artists.artists)
                    {

                        string key = String.Empty;
                        try{
                            key = (String)artist[mKey];
                        }catch{
                        }
                        if (key.Equals("")){
                            try{
                                Int32 tmp = (Int32)artist[mKey];
                                key = tmp.ToString();
                            }catch{
                                Console.WriteLine("parseroorrr");
                            }
                        }
                        string value = String.Empty;
                        try
                        {
                            value = (String)artist[mValue];
                        }catch{}
                        if (value.Equals(""))
                        {
                            try{
                                Int32 tmp = (Int32)artist[mValue];
                                value = tmp.ToString();
                            }catch{
                                Console.WriteLine("parseroorrr");
                            }
                        }

                        ds.Add(new searchDS(key, value));
                        /*
                        ToolStripMenuItem itm = new ToolStripMenuItem();
                        itm.Text = artist["handle"];
                        itm.Name = artist["id"].ToString();
                        itm.Tag = rowIndex;
                        itm.Click += new EventHandler(delegate
                        {
                            ds[(int)itm.Tag].ArtistID = Int32.Parse(itm.Name);
                            dataGridViewSearch.Refresh();
                        });
                        contextMenuStripArtistSearch.Items.Add(itm);
                         */
                    }
                    if (InvokeRequired)
                    {
                        //we are anonymous! (refresh the grid please);
                        BeginInvoke(new MethodInvoker(delegate()
                        {
                            dataGridViewSearch.DataSource = ds;
                            dataGridViewSearch.Refresh();
                        }));
                    }
                            }catch{}
                }).Start();
            }
    
        }
    }



    //own class for further dev.
    public class searchDS
    {
        public String mKey
        {
            get;
            set;
        }
        public String mValue
        {
            get;
            set;
        }

        public searchDS(string key,String value)
        {
            mKey = key;
            mValue = value;
        }
        public searchDS()
        {
        }
    }
}
