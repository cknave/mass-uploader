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
using System.Text.RegularExpressions;

namespace RadioPusher2
{
    public partial class SearchForm : Form
    {
        string SearchUrl = String.Empty;
        public string mKey = String.Empty;
        public string mValue = String.Empty;
        public string ResultKey = String.Empty;
        public string ResultValue = String.Empty;
        public string NodeKey = String.Empty;
        string query = String.Empty;
        public List<int> SelectionList = new List<int>();
        public List<string> SelectionListValues = new List<string>();
        bool searchinprogress = false;
        bool MultiSelectionMode = false;
        List<searchDS> ds = new List<searchDS>();
        List<searchDS> searchds = new List<searchDS>();

        NWebClient nwc = null;

        public SearchForm(bool multiple,NWebClient wc, string searchurl,string initialQuery, string matchKeyOrInnerRegex, string matchValueOrOuterRegex,string nodekey,List<int> values)
        {
            InitializeComponent();
            MultiSelectionMode = multiple;
            nwc = wc;
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
            mKey = matchKeyOrInnerRegex;
            mValue = matchValueOrOuterRegex;
            query = initialQuery;
            textBoxSearch.Text = query;
            NodeKey = nodekey;

            if (!MultiSelectionMode) {
                textBoxSelection.Visible = false;
                buttonOK.Visible = false;
            } else {
                
                _PerformSearch();
                
                if (values != null) {
                    new Thread(delegate() {
                        while (searchinprogress) {
                            Thread.Sleep(300);
                        }
                        //we are anonymous! (refresh the grid please);
                        BeginInvoke(new MethodInvoker(delegate() {
                            foreach (searchDS s in ds) {
                                int val = Int32.Parse(s.mKey.ToString());
                                if (values.Contains(val)) {
                                    SelectionList.Add(val);
                                    SelectionListValues.Add(s.mValue.ToString());
                                    textBoxSelection.Text += s.mKey.ToString() + ",";
                                    textBoxSelectionValues.Text += s.mValue.ToString() + ",";
                                }
                            }
                        }));

                    }).Start();
                   
                }

                
            }

        }

        private void dataGridViewSearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (!MultiSelectionMode) {
                    ResultKey = dataGridViewSearch.Rows[e.RowIndex].Cells[0].Value.ToString();
                } else {
                    Int32 tmpval = Int32.Parse(dataGridViewSearch.Rows[e.RowIndex].Cells[0].Value.ToString());
                    if (SelectionList.Contains(tmpval)) {
                        SelectionList.Remove(tmpval);                        
                    } else {
                        SelectionList.Add(tmpval);
                    }
                    textBoxSelection.Text = "";
                    foreach (int val in SelectionList) {
                        textBoxSelection.Text += val.ToString() + ",";
                    }

                }
            }catch(Exception ee){
                MessageBox.Show(ee.Message);
            }

            try{
                if (!MultiSelectionMode) {
                    ResultValue = dataGridViewSearch.Rows[e.RowIndex].Cells[1].Value.ToString();
                } else {
                    string tmpval = dataGridViewSearch.Rows[e.RowIndex].Cells[1].Value.ToString();
                    if (SelectionListValues.Contains(tmpval)) {
                        SelectionListValues.Remove(tmpval);
                    } else {
                        SelectionListValues.Add(tmpval);
                    }
                    textBoxSelectionValues.Text = "";
                    foreach (string val in SelectionListValues) {
                        textBoxSelectionValues.Text += val.ToString() + ",";
                    }
                }
            }catch (Exception ee){
                MessageBox.Show(ee.Message);
            }
            if (!MultiSelectionMode) {
                this.Close();
            }
        }

     

        private void textBoxSearch_KeyUp(object sender, KeyEventArgs e)
        {
            try {
                if (e.KeyData == Keys.Return) {
                    if (!MultiSelectionMode) {
                   
                            _PerformSearch();
                    
                    } else {
                        dataGridViewSearch.DataSource = ds;
                        searchds.Clear();
                        foreach (searchDS d in ds) {
                            if (d.mValue.ToLower().Contains(textBoxSearch.Text.ToLower().Trim())) {
                                searchds.Add(d);
                            }
                        }
                    }
                    dataGridViewSearch.DataSource = searchds;
                    dataGridViewSearch.Refresh();
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        private void _PerformSearch()
        {
            ds.Clear();
            string query = textBoxSearch.Text.Trim();
            new Thread(delegate() {
                try {
                    searchinprogress = true;
                    if (SearchUrl.Contains("REGEX:")) {
                        string tmpurl = SearchUrl.Replace("REGEX:", "");
                        string res = nwc.DownloadString(tmpurl);

                        if (!mValue.Trim().Equals("")) {
                            //outer regex
                            res = Regex.Match(res, mValue, RegexOptions.Singleline).Groups["match"].Value;
                        }

                        MatchCollection mc = Regex.Matches(res, mKey, RegexOptions.Singleline);
                        foreach (Match m in mc) {
                            ds.Add(new searchDS(m.Groups["key"].Value, m.Groups["value"].Value));
                        }
                    } else {
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        jss.RegisterConverters(new JavaScriptConverter[] { new DynamicJsonConverter() });

                        string json = nwc.DownloadString(String.Format("{0}{1}", SearchUrl, query)).Replace(NodeKey, "artists");
                        dynamic artists = jss.Deserialize(json, typeof(object)) as dynamic;


                        foreach (var artist in artists.artists) {

                            string key = String.Empty;
                            try {
                                key = (String)artist[mKey];
                            } catch {
                            }
                            if (key.Equals("")) {
                                try {
                                    Int32 tmp = (Int32)artist[mKey];
                                    key = tmp.ToString();
                                } catch {
                                    Console.WriteLine("parseroorrr");
                                }
                            }
                            string value = String.Empty;
                            try {
                                value = (String)artist[mValue];
                            } catch { }
                            if (value.Equals("")) {
                                try {
                                    Int32 tmp = (Int32)artist[mValue];
                                    value = tmp.ToString();
                                } catch {
                                    Console.WriteLine("parseroorrr");
                                }
                            }

                            ds.Add(new searchDS(key, value));
                        }
                    }
                    if (InvokeRequired) {
                        //we are anonymous! (refresh the grid please);
                        BeginInvoke(new MethodInvoker(delegate() {
                            dataGridViewSearch.DataSource = ds;
                            dataGridViewSearch.Refresh();
                        }));
                    }
                    searchinprogress = false;
                } catch { searchinprogress = false; }
            }).Start();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Close();
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
