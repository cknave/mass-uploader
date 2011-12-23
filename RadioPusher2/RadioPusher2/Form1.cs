using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RadioPusher2.Data;
using Un4seen.Bass;
using Data;
using System.Text.RegularExpressions;
using NUtils;
using System.Web.Script.Serialization;
using System.Threading;
using Utils;
using System.IO;

namespace RadioPusher2
{


    public partial class Form1 : Form
    {
        int SelectedRow = -2;
        int SelectedCell = -2;
        BindingList<Track> ds = new BindingList<Track>();
        int handle = 0;
        int current_track_in_upload_queue = 0;
        public Form1()
        {
            InitializeComponent();
            //Load the settings
            config.loadconfig();
            trackBarVolume.Minimum = 1;
            trackBarVolume.Maximum = 100;

            //configure the grid
            ConfigureGrid();

            dataGridViewTracks.CellClick += new DataGridViewCellEventHandler(dataGridViewTracks_CellClick); //assign an event to the grid
            dataGridViewTracks.MouseDown += new MouseEventHandler(dataGridViewTracks_MouseDown);
            Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, System.IntPtr.Zero); //init bass
            try
            {
                trackBarVolume.Value = (int)(Bass.BASS_GetVolume() * 100); //get the current volume
            }
            catch
            {
            }
        }

        void dataGridViewTracks_MouseDown(object sender, MouseEventArgs e)
        {
            //windows is pretty damn retarded sometimes ;)
            //this is a hack (tm)
            if (e.Button == MouseButtons.Right)
            {
                dataGridViewTracks.ClearSelection();
                var hti = dataGridViewTracks.HitTest(e.X, e.Y);
                dataGridViewTracks.Rows[hti.RowIndex].Selected = true;
            }

        }

        void dataGridViewTracks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine(e.ColumnIndex);
            Console.WriteLine(e.RowIndex);
            //column -1 is the |> column, use it as play button.
            if (e.ColumnIndex == -1){
                try{
                    PlayMusic(ds[e.RowIndex].File);
                }catch{}
            }
        }

        private void PlayMusic(string filename)
        {
            //some bass code to play the track
            Bass.BASS_ChannelStop(handle);

            //for mp3 and ogg use special settings
            if (filename.ToLower().Contains(".mp3") || filename.ToLower().Contains(".ogg")){
                handle = Bass.BASS_StreamCreateFile(filename, 0, 0, BASSFlag.BASS_DEFAULT);
            }else{
                //this is for mods,xm,s3m.. you know.. formats for real men.
                handle = Bass.BASS_MusicLoad(filename, 0, 0, BASSFlag.BASS_DEFAULT, 44100);
            }
            Bass.BASS_ChannelPlay(handle, true);
        }

        private void StopMusic()
        {
            Bass.BASS_ChannelStop(handle);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //show the settings form
            SettingsForm sf = new SettingsForm();
            sf.ShowDialog();
            config.loadconfig();
        }

        private void buttonAddTracks_Click(object sender, EventArgs e)
        {
            //show the filechooser with some filter.. 
            OpenFileDialog ofp = new OpenFileDialog();
            ofp.RestoreDirectory = true;
            ofp.Multiselect = true;
            ofp.Filter  = "Mp3 Files (*.mp3)|*.mp3|"
            + "Ogg Files (*.ogg)|*.ogg|"
            + "ImpulseTracker Files (*.it)|*.it|"
            + "XM Files (*.xm)|*.xm|"
            + "S3M Files (*.s3m)|*.s3m|"
            + "MOD Files (*.mod)|*.mod|"
            + "MTM Files (*.mtm)|*.mtm|"
            + "UMX Files (*.umx)|*.umx|"
            + "All Files (*.*)|*.*";
            ofp.FileOk += new CancelEventHandler(ofp_FileOk);
            ofp.ShowDialog();
        }

        void ofp_FileOk(object sender, CancelEventArgs e)
        {
            //add the files to the grid
            //checkout Track.cs - because that's where the tagging magic happens
            OpenFileDialog ofp = (OpenFileDialog)sender;
            foreach (string filename in ofp.FileNames)
            {
                ds.Add(new Track(filename));
                
            }
        }

        private void trackBarVolume_ValueChanged(object sender, EventArgs e)
        {
            //change the volume.. sometimes stuff can be very loud :(
            float vol = (float)trackBarVolume.Value/100;
            Bass.BASS_SetVolume(vol);
        }

        private void buttonLookupArtist_Click(object sender, EventArgs e)
        {
            //lookup artist code

            //basic error check
            if(ds.Count==0){
                MessageBox.Show("Add some Tracks first");
                return;
            }

            BindingList<Track> copy = new BindingList<Track>(ds);
            int it = 0;
            foreach (Track track in copy)
            {
                if (track.ArtistID == 0){
                   //lookup time :)
                    NWebClient cw = new NWebClient(15000);
                    string json = cw.DownloadString(String.Format("{0}search/ajax/artist/?q={1}",config.hostname,null));
                }
                it++; 
            }
        }

        private void ConfigureGrid()
        {
            dataGridViewTracks.AutoGenerateColumns = false;
            

            DataGridViewTextBoxColumn ArtistID = new DataGridViewTextBoxColumn();
            ArtistID.DataPropertyName = "ArtistID";
            ArtistID.HeaderText = "ArtistID";
            ArtistID.Visible = true;
            ArtistID.Name = "ArtistID";
            ArtistID.Width = 50;
            ArtistID.ContextMenuStrip = contextMenuStripArtistSearch;


            DataGridViewTextBoxColumn ID3Artist = new DataGridViewTextBoxColumn();
            ID3Artist.DataPropertyName = "ID3Artist";
            ID3Artist.HeaderText = "ID3Artist";
            ID3Artist.Visible = false;
            ID3Artist.Name = "ID3Artist";

            DataGridViewTextBoxColumn File = new DataGridViewTextBoxColumn();
            File.DataPropertyName = "File";
            File.HeaderText = "File";
            File.Visible = true;
            File.Name = "File";
            File.ReadOnly = true;
            File.Width = 100;

            DataGridViewTextBoxColumn SongName = new DataGridViewTextBoxColumn();
            SongName.DataPropertyName = "SongName";
            SongName.HeaderText = "SongName";
            SongName.Visible = true;
            SongName.Name = "SongName";

            DataGridViewTextBoxColumn ReleaseYear = new DataGridViewTextBoxColumn();
            ReleaseYear.DataPropertyName = "ReleaseYear";
            ReleaseYear.HeaderText = "ReleaseYear";
            ReleaseYear.Visible = true;
            ReleaseYear.Name = "ReleaseYear";
            ReleaseYear.Width = 70;

            DataGridViewTextBoxColumn MixSongID = new DataGridViewTextBoxColumn();
            MixSongID.DataPropertyName = "MixSongID";
            MixSongID.HeaderText = "MixSongID";
            MixSongID.Visible = true;
            MixSongID.Name = "MixSongID";
            MixSongID.Width = 60;

            DataGridViewTextBoxColumn GroupsID = new DataGridViewTextBoxColumn();
            GroupsID.DataPropertyName = "GroupsID";
            GroupsID.HeaderText = "GroupsID";
            GroupsID.Visible = true;
            GroupsID.Name = "GroupsID";
            GroupsID.Width = 63;

            DataGridViewTextBoxColumn AlbumID = new DataGridViewTextBoxColumn();
            GroupsID.DataPropertyName = "AlbumID";
            GroupsID.HeaderText = "AlbumID";
            GroupsID.Visible = true;
            GroupsID.Name = "AlbumID";
            GroupsID.Width = 63;

            DataGridViewTextBoxColumn LabelID = new DataGridViewTextBoxColumn();
            LabelID.DataPropertyName = "LabelID";
            LabelID.HeaderText = "LabelID";
            LabelID.Visible = true;
            LabelID.Name = "LabelID";
            LabelID.Width = 63;

            DataGridViewTextBoxColumn Info = new DataGridViewTextBoxColumn();
            Info.DataPropertyName = "Info";
            Info.HeaderText = "Info";
            Info.Visible = true;
            Info.Name = "Info";
            Info.Width = 100;

            DataGridViewTextBoxColumn YoutubeVideoID = new DataGridViewTextBoxColumn();
            YoutubeVideoID.DataPropertyName = "YoutubeVideoID";
            YoutubeVideoID.HeaderText = "ytID";
            YoutubeVideoID.Visible = true;
            YoutubeVideoID.Name = "YoutubeVideoID";
            YoutubeVideoID.Width = 60;

            DataGridViewTextBoxColumn YoutubeStartOffset = new DataGridViewTextBoxColumn();
            YoutubeStartOffset.DataPropertyName = "YoutubeStartOffset";
            YoutubeStartOffset.HeaderText = "ytOffset";
            YoutubeStartOffset.Visible = true;
            YoutubeStartOffset.Name = "YoutubeStartOffset";
            YoutubeStartOffset.Width = 60;

            DataGridViewTextBoxColumn SourceID = new DataGridViewTextBoxColumn();
            SourceID.DataPropertyName = "SourceID";
            SourceID.HeaderText = "SourceID";
            SourceID.Visible = true;
            SourceID.Name = "SourceID";
            SourceID.Width = 60;

            DataGridViewTextBoxColumn PlatformID = new DataGridViewTextBoxColumn();
            PlatformID.DataPropertyName = "PlatformID";
            PlatformID.HeaderText = "PlatformID";
            PlatformID.Visible = true;
            PlatformID.Name = "PlatformID";
            PlatformID.Width = 60;

            DataGridViewTextBoxColumn PouetID = new DataGridViewTextBoxColumn();
            PouetID.DataPropertyName = "PouetID";
            PouetID.HeaderText = "PouetID";
            PouetID.Visible = true;
            PouetID.Name = "PouetID";
            PouetID.Width = 50;

            DataGridViewProgressColumn progress = new DataGridViewProgressColumn();
            progress.DataPropertyName = "Progress";
            progress.HeaderText = "Upload Progress";
            progress.ReadOnly = true;
            progress.Name = "Progress";
            progress.Visible = false;

            dataGridViewTracks.Columns.Add(progress);
            dataGridViewTracks.Columns.Add(ArtistID);
            dataGridViewTracks.Columns.Add(File);
            dataGridViewTracks.Columns.Add(SongName);
            dataGridViewTracks.Columns.Add(ReleaseYear);
            dataGridViewTracks.Columns.Add(MixSongID);
            dataGridViewTracks.Columns.Add(GroupsID);
            dataGridViewTracks.Columns.Add(AlbumID);
            dataGridViewTracks.Columns.Add(LabelID);
            dataGridViewTracks.Columns.Add(Info);
            dataGridViewTracks.Columns.Add(YoutubeVideoID);
            dataGridViewTracks.Columns.Add(YoutubeStartOffset);
            dataGridViewTracks.Columns.Add(SourceID);
            dataGridViewTracks.Columns.Add(PlatformID);
            dataGridViewTracks.Columns.Add(PouetID);

            dataGridViewTracks.DataSource = ds; //set the datasource to the Track Bindinglist

        }

        private void contextMenuStripArtistSearch_Opened(object sender, EventArgs e)
        {


            try{
                int rowIndex = dataGridViewTracks.SelectedCells[0].RowIndex;
                contextMenuStripArtistSearch.Items[0].Text = ds[rowIndex].ID3Artist;
                if(contextMenuStripArtistSearch.Items[0].Text.Trim().Equals("")){
                    return;
                }
                if (ds[rowIndex].ArtistID == 0)
                {
                    //lookup time :)

                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    jss.RegisterConverters(new JavaScriptConverter[] { new DynamicJsonConverter() });
                    NWebClient cw = new NWebClient(15000);
                    string json = cw.DownloadString(String.Format("{0}search/ajax/artist/?q={1}", config.hostname.Replace("/demovibes/", "/"), contextMenuStripArtistSearch.Items[0].Text));
                    dynamic artists = jss.Deserialize(json, typeof(object)) as dynamic;

                    //write a new contextmenu
                  //  contextMenuStripArtistSearch = new System.Windows.Forms.ContextMenuStrip();
                    contextMenuStripArtistSearch.Items.Clear();
                    contextMenuStripArtistSearch.Items.Add(toolStripTextBoxQuery);
                    contextMenuStripArtistSearch.Items.Add(toolStripSeparator1);                    
                    
                    foreach(var artist in artists.artists){
               //         Console.WriteLine(artist["id"]);

                        ToolStripMenuItem itm = new ToolStripMenuItem();
                        itm.Text = artist["handle"];
                        itm.Name = artist["id"].ToString();
                        itm.Tag = rowIndex;
                        itm.Click += new EventHandler(delegate
                        {
                            ds[(int)itm.Tag].ArtistID = Int32.Parse(itm.Name);
                            dataGridViewTracks.Refresh();
                        });
                        contextMenuStripArtistSearch.Items.Add(itm);
                    }
                 //   Console.WriteLine("artists.glossary.title: " + artists);
                    //Console.WriteLine("glossaryEntry.glossary.GlossDiv.title: " + glossaryEntry.glossary.GlossDiv.title);

                    
                }
                else
                {
                    contextMenuStripArtistSearch.Items.Clear();
                    ToolStripMenuItem itm = new ToolStripMenuItem();
                    itm.Text = "Copy to all fields in this column";                    
                    itm.Tag = rowIndex;
                    itm.Click += new EventHandler(delegate{
                        int aid = ds[rowIndex].ArtistID;
                        for(int i=0;i<ds.Count;i++){
                            ds[i].ArtistID = aid;
                        }
                        dataGridViewTracks.Refresh();
                    });
                    contextMenuStripArtistSearch.Items.Add(itm);
                }
            }catch(Exception ee){
                Console.WriteLine(ee.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != MessageBox.Show("Are you sure? Really? Check your Data twice :)", "Are you ready?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)){
                return;
            }
            MessageBox.Show("Uploading naoe!");
            buttonAddTracks.Enabled = false;
            buttonUpload.Enabled = false;
            dataGridViewTracks.Columns[0].Visible = true;
            dataGridViewTracks.Refresh();
            StartUploadProcess();
        }

        private void StartUploadProcess()
        {
            bool errors = false;
            foreach (Track track in ds)
            {
                if (track.ArtistID == 0)
                {
                    errors = true;
                }
            }

            if (errors)
            {
                MessageBox.Show("Sorry, you need to set the artistID's for all the tracks first - HINT: use right mouse button ;) !");
                if (InvokeRequired)
                {
                    //we are anonymous! 
                    BeginInvoke(new MethodInvoker(delegate()
                    {
                        buttonAddTracks.Enabled = true;
                        buttonUpload.Enabled = true;
                        dataGridViewTracks.Columns[0].Visible = false;
                        dataGridViewTracks.Refresh();
                        MessageBox.Show("Hey Sir! Looks like i'm through with the upload queue!");
                    }));
                }
                return;
            }

            new Thread(delegate()
            {
                NWebClient nwc = new NWebClient(15000);
                        string res = nwc.DownloadString(config.hostname.Replace("demovibes", "account/signin"));
                        Dictionary<string, string> kvp = new Dictionary<string, string>();
                        kvp.Add("next", "");
                        kvp.Add("username", config.username);
                        kvp.Add("password", config.password);
                        kvp.Add("blogin", "Sign+in");
                        res = nwc.PostAction(config.hostname.Replace("demovibes", "account/signin"), kvp);
                        if (res.Contains("Welcome, " + config.username)){ //are we logged in?
                            int z = 0;
                            foreach (Track track in ds){ //loop over all tracks

                                if (track.Progress == 100){ //this track has already been upped ;)
                                    z++;
                                    continue;
                                }

                                    //send the data
                                    kvp.Clear();
                                    if (track.ArtistID == 0 || track.ArtistID.Equals(""))
                                    {
                                        MessageBox.Show("no artist id set for track:" + track.SongName);
                                        break;
                                    }
                                    kvp.Add("title", track.SongName);
                                    kvp.Add("release_year", track.ReleaseYear.ToString());
                                    if (track.MixSongID > 0){
                                        kvp.Add("remix_of_id", "");
                                    }
                                    //TODO: add support for groups, labels and type
                                    // kvp.Add("groups[]", "");
                                    // kvp.Add("labels[]", "");
                                    kvp.Add("info", track.Info);
                                    //kvp.Add("type", null);
                                    if (track.PlatformID > 0){
                                        kvp.Add("platform", track.PlatformID.ToString());
                                    }
                                    kvp.Add("pouetid", track.PouetID.ToString());
                                    if (track.YoutubeVideoID > 0) { 
                                        kvp.Add("ytvidid", track.YoutubeVideoID.ToString());
                                                                  
                                    }
                                    kvp.Add("ytvidoffset", track.YoutubeStartOffset.ToString());         
                                    current_track_in_upload_queue = z; //does this work.. pretty weak stuff :S
                                    nwc.UploadProgress += new UploadProgressDelegate(nwc_UploadProgress);
                                    nwc.TimeOut = 0;
                                    res = nwc.PostMultipartData(config.hostname.Replace("demovibes/", "demovibes") + "/artist/" + track.ArtistID.ToString() + "/upload/", 
                                                                        kvp, 
                                                                        "file", 
                                                                        System.IO.File.ReadAllBytes(track.File)
                                                                        );
                                    if (!res.Contains("Preview:")){
                                        MessageBox.Show("Something went wrong when i tried to upload: " + track.File);
                                    }
                                if (InvokeRequired) { BeginInvoke(new MethodInvoker(delegate() { dataGridViewTracks.Refresh(); })); }
                                z++;
                            }
                        }else{
                            MessageBox.Show("Something is wrong with your configuration - i can't login");
                        }


                if (InvokeRequired){
                    //we are anonymous! 
                    BeginInvoke(new MethodInvoker(delegate() {
                        buttonAddTracks.Enabled = true;
                        buttonUpload.Enabled = true;
                        dataGridViewTracks.Columns[0].Visible = false;
                        dataGridViewTracks.Refresh();
                        MessageBox.Show("Hey Sir! Looks like i'm through with the upload queue!");
                    }));
                }
                
            }).Start();
        }

        void nwc_UploadProgress(long pos, long total)
        {
            int check = ds[current_track_in_upload_queue].Progress;
            long progress = (pos * 100) / total;
            if (check < Int32.Parse(progress.ToString()))
            {
                ds[current_track_in_upload_queue].Progress = Int32.Parse(progress.ToString());
                if (InvokeRequired) { BeginInvoke(new MethodInvoker(delegate() { dataGridViewTracks.Refresh(); })); }
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            StopMusic();

        }

    }



    public class config
    {
        public static string hostname = String.Empty;
        public static string username = string.Empty;
        public static string password = string.Empty;
        public static void loadconfig()
        {
            AppSettings appsettings = AppSettings.Load();
            hostname = appsettings.ServerAddress;
            username = appsettings.UserName;
            password = appsettings.Password;
        }
    }


}
