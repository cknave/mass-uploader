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


    public partial class MainForm : Form
    {
        int SelectedRow = -2;
        int SelectedCell = -2;
        BindingList<Track> ds = new BindingList<Track>();
        int handle = 0;
        int current_track_in_upload_queue = 0;
        TextWriter _writer = null;
        NWebClient nwc = new NWebClient(15000); //webclient
        public MainForm()
        {
            InitializeComponent();
            //Load the settings
            _writer = new TextBoxStreamWriter(textboxDebug);
            // Redirect the out Console stream
            Console.SetOut(_writer);
            Console.WriteLine("Output Console Activated");


            try
            {
                config.loadconfig();
                trackBarVolume.Minimum = 1;
                trackBarVolume.Maximum = 100;

                //configure the grid
                ConfigureGrid();

                dataGridViewTracks.CellClick += new DataGridViewCellEventHandler(dataGridViewTracks_CellClick); //assign an event to the grid
                dataGridViewTracks.MouseDown += new MouseEventHandler(dataGridViewTracks_MouseDown);
                Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, System.IntPtr.Zero); //init bass
                trackBarVolume.Value = (int)(Bass.BASS_GetVolume() * 100); //get the current volume
                new Thread(delegate() {
                    if (!_Login()) {
                        MessageBox.Show("Your login settings are not correct - i can't login");
                        SettingsForm sf = new SettingsForm();
                        sf.ShowDialog();
                        config.loadconfig();
                        _Login();
                    } else {
                        Console.WriteLine("Login Complete - session ready");
                    }
                }).Start();
            }
            catch(Exception exp)
            {
                Console.WriteLine("Exception: " + exp.Message);
            }



        }

        void dataGridViewTracks_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                //windows is pretty damn retarded sometimes ;)
                //this is a hack (tm)
                if (e.Button == MouseButtons.Right)
                {
                    dataGridViewTracks.ClearSelection();
                    var hti = dataGridViewTracks.HitTest(e.X, e.Y);
                    dataGridViewTracks.Rows[hti.RowIndex].Selected = true;
                    if (e.Button == MouseButtons.Right)
                    {
                        if (hti.ColumnIndex == 1)
                        { //artist row clicked
                            if (ds[hti.RowIndex].ArtistID == 0){
                                SearchForm sf = new SearchForm(false,nwc,String.Format("{0}search/ajax/artist/?q=", config.hostname.Replace("/demovibes/", "/")), ds[hti.RowIndex].ID3Artist, "id", "handle", "artists",null);
                                sf.ShowDialog(); 
                                try
                                {
                                    ds[hti.RowIndex].ArtistID = Int32.Parse(sf.ResultKey);
                                }
                                catch
                                {
                                    ds[hti.RowIndex].ArtistID = 0;
                                }
                            }
                            else
                            {
                                contextMenuStripArtistSearch.Show(new Point(MousePosition.X, MousePosition.Y));
                            }
                        }

                        if (hti.ColumnIndex > 0 && hti.ColumnIndex != 8 && hti.ColumnIndex != 14 && hti.ColumnIndex != 7)
                        {
                            SelectedCell = hti.ColumnIndex;
                            if (hti.ColumnIndex == 1) { /* artist */        if (ds[hti.RowIndex].ArtistID > 0)             {ShowCopyContextMenu(hti.ColumnIndex); } }
                            if(hti.ColumnIndex == 4) { /* release year */   if (ds[hti.RowIndex].ReleaseYear > 0)          {ShowCopyContextMenu(hti.ColumnIndex); } }
                            if(hti.ColumnIndex == 5) { /* mix song id */    if (ds[hti.RowIndex].MixSongID > 0)            {ShowCopyContextMenu(hti.ColumnIndex); } }
                            if(hti.ColumnIndex == 6) { /* album id */       if (ds[hti.RowIndex].AlbumID > 0)              {ShowCopyContextMenu(hti.ColumnIndex); } }
                          //  if(hti.ColumnIndex == 7) { /* label id */       if (ds[hti.RowIndex].LabelIDs == 0)             {ShowCopyContextMenu(hti.ColumnIndex); } }
                          //  if(hti.ColumnIndex == 9) { /* info */           if (!ds[hti.RowIndex].Info.Equals(""))                 {ShowCopyContextMenu(hti.ColumnIndex); } }

                            if(hti.ColumnIndex == 9) { /* ytID */          if (ds[hti.RowIndex].YoutubeVideoID > 0)       {ShowCopyContextMenu(hti.ColumnIndex); } }
                            if(hti.ColumnIndex == 10) { /* ytOffset */      if (ds[hti.RowIndex].YoutubeStartOffset > 0)   {ShowCopyContextMenu(hti.ColumnIndex); } }
                            if(hti.ColumnIndex == 11) { /* sourceID */      if (ds[hti.RowIndex].SourceID > 0)             { ShowCopyContextMenu(hti.ColumnIndex); } }
                            if(hti.ColumnIndex == 12) { /* platformID */    if (ds[hti.RowIndex].PlatformID > 0)           {ShowCopyContextMenu(hti.ColumnIndex); } }
                            if (hti.ColumnIndex == 13) { /* pouetID */      if (ds[hti.RowIndex].PouetID > 0)              { ShowCopyContextMenu(hti.ColumnIndex); } }
                        }
                        if (hti.ColumnIndex == 1){ //artist row clicked
                            if (ds[hti.RowIndex].ArtistID == 0){
                                SearchForm sf = new SearchForm(false,nwc,String.Format("{0}search/ajax/artist/?q=", config.hostname.Replace("/demovibes/", "/")), ds[hti.RowIndex].ID3Artist, "id", "handle", "artists",null);
                                    sf.ShowDialog();
                                try{
                                    ds[hti.RowIndex].ArtistID = Int32.Parse(sf.ResultKey);
                                }catch{
                                    ds[hti.RowIndex].ArtistID = 0;
                                }
                            }
                        }   
                        if (hti.ColumnIndex == 5) { // mix song id selected
                            if (ds[hti.RowIndex].MixSongID == 0) {
                                SearchForm sf = new SearchForm(false,nwc,String.Format("{0}search/ajax/song/?q=", config.hostname.Replace("/demovibes/", "/")), ds[hti.RowIndex].SongName, "id", "title", "songs",null);
                                sf.ShowDialog();
                                try {
                                    ds[hti.RowIndex].MixSongID = Int32.Parse(sf.ResultKey);
                                } catch {
                                    ds[hti.RowIndex].MixSongID = 0;
                                }
                            }
                        }

                        if (hti.ColumnIndex == 11) { //sourceID
                            if (ds[hti.RowIndex].SourceID == 0) {
                                SearchForm sf = new SearchForm(false, nwc, String.Format("REGEX:{0}demovibes/artist/1/upload/", config.hostname.Replace("/demovibes/", "/")), "", "<option value=\"(?<key>.+?)\">(?<value>.+?)</option>", "<select name=\"type\" id=\"id_type\">(?<match>.+?)</select", "", ds[hti.RowIndex].GroupsIDs);
                                sf.ShowDialog();
                                try {
                                    ds[hti.RowIndex].SourceID = Int32.Parse(sf.ResultKey);
                                } catch {
                                    ds[hti.RowIndex].SourceID = 0;
                                }
                            }
                        }


                        if (hti.ColumnIndex == 12) { //platform
                            if (ds[hti.RowIndex].PlatformID == 0) {
                                SearchForm sf = new SearchForm(false, nwc, String.Format("REGEX:{0}demovibes/artist/1/upload/", config.hostname.Replace("/demovibes/", "/")), "", "<option value=\"(?<key>.+?)\">(?<value>.+?)</option>", "<select name=\"platform\" id=\"id_platform\">(?<match>.+?)</select", "", null);
                                sf.ShowDialog();
                                try {
                                    ds[hti.RowIndex].PlatformID = Int32.Parse(sf.ResultKey);
                                } catch {
                                    ds[hti.RowIndex].PlatformID = 0;
                                }
                            }
                        }
                        //<select multiple="multiple" name="labels" id="id_labels">
                        if (hti.ColumnIndex == 7) { //platform
                           // if (ds[hti.RowIndex].PlatformID == 0) {
                                SearchForm sf = new SearchForm(true, nwc, String.Format("REGEX:{0}demovibes/artist/1/upload/", config.hostname.Replace("/demovibes/", "/")), "", "<option value=\"(?<key>.+?)\">(?<value>.+?)</option>", "<select multiple=\"multiple\" name=\"labels\" id=\"id_labels\">(?<match>.+?)</select", "", ds[hti.RowIndex].LabelIDs);
                                sf.ShowDialog();
                                try {
                                    ds[hti.RowIndex].LabelIDs.Clear();
                                    ds[hti.RowIndex].LabelIDs.AddRange(sf.SelectionList);
                                } catch {
                                 
                                }
                         //   }
                        }

                        if (hti.ColumnIndex == 14) {
//                            if (ds[hti.RowIndex].GroupsIDs == 0) {
                            //<select multiple="multiple" name="groups" id="id_groups"><option value="269">1oo%</option><option value="81">2000 A.D.</option>
                            SearchForm sf = new SearchForm(true, nwc, String.Format("REGEX:{0}demovibes/artist/1/upload/", config.hostname.Replace("/demovibes/", "/")), "", "<option value=\"(?<key>.+?)\">(?<value>.+?)</option>", "<select multiple=\"multiple\" name=\"groups\" id=\"id_groups\">(?<match>.+?)</select", "", ds[hti.RowIndex].GroupsIDs);
                                sf.ShowDialog();

                                try {
                                    ds[hti.RowIndex].GroupsIDs.Clear();
                                    ds[hti.RowIndex].GroupsIDs.AddRange(sf.SelectionList);
                                } catch {
                                }
 //                           }
                                

                        }
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine("Exception: " + exp.Message);
            }

        }

        void ShowCopyContextMenu(int columnIndex)
        {
            contextMenuStripArtistSearch.Show(new Point(MousePosition.X, MousePosition.Y));
        }

        void dataGridViewTracks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //column -1 is the |> column, use it as play button.
           try{
            if (e.ColumnIndex == -1){
                
                    PlayMusic(ds[e.RowIndex].File);
            }
           }
           catch (Exception exp) { Console.WriteLine("Exception: " + exp.Message); }

        }

        private void PlayMusic(string filename)
        {
            try
            {
                //some bass code to play the track
                Bass.BASS_ChannelStop(handle);

                //for mp3 and ogg use special settings
                if (filename.ToLower().Contains(".mp3") || filename.ToLower().Contains(".ogg"))
                {
                    handle = Bass.BASS_StreamCreateFile(filename, 0, 0, BASSFlag.BASS_DEFAULT);
                }
                else
                {
                    //this is for mods,xm,s3m.. you know.. formats for real men.
                    handle = Bass.BASS_MusicLoad(filename, 0, 0, BASSFlag.BASS_DEFAULT, 44100);
                }
                Bass.BASS_ChannelPlay(handle, true);
            }
            catch (Exception exp)
            {
                Console.WriteLine("Exception: " + exp.Message);
            }
        }

        private void StopMusic()
        {
            try
            {
                Bass.BASS_ChannelStop(handle);
            }
            catch (Exception exp)
            {
                Console.WriteLine("Exception: " + exp.Message);
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //show the settings form
                SettingsForm sf = new SettingsForm();
                sf.ShowDialog();
                config.loadconfig();
            }
            catch (Exception exp)
            {
                Console.WriteLine("Exception: " + exp.Message);
            }
        }

        private void buttonAddTracks_Click(object sender, EventArgs e)
        {
            //show the filechooser with some filter.. 
            OpenFileDialog ofp = new OpenFileDialog();
            ofp.RestoreDirectory = true;
            ofp.Multiselect = true;
            ofp.Filter  = "Mp3 Files (*.mp3)|*.mp3|"
            + "Ogg Files (*.ogg)|*.ogg|"
            + "Flac Files (*.flac)|*.flac|"
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
            try
            {
                //add the files to the grid
                //checkout Track.cs - because that's where the tagging magic happens
                OpenFileDialog ofp = (OpenFileDialog)sender;
                foreach (string filename in ofp.FileNames)
                {
                    ds.Add(new Track(filename));

                }
            }
            catch (Exception exp)
            {
                Console.WriteLine("Exception: " + exp.Message);
            }
        }

        private void trackBarVolume_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                //change the volume.. sometimes stuff can be very loud :(
                float vol = (float)trackBarVolume.Value / 100;
                Bass.BASS_SetVolume(vol);
            }
            catch (Exception exp)
            {
                Console.WriteLine("Exception: " + exp.Message);
            }
        }

        private void buttonLookupArtist_Click(object sender, EventArgs e)
        {
            try
            {
                //lookup artist code

                //basic error check
                if (ds.Count == 0)
                {
                    MessageBox.Show("Add some Tracks first");
                    return;
                }

                BindingList<Track> copy = new BindingList<Track>(ds);
                int it = 0;
                foreach (Track track in copy)
                {
                    if (track.ArtistID == 0)
                    {
                        //lookup time :)
                       
                        string json = nwc.DownloadString(String.Format("{0}search/ajax/artist/?q={1}", config.hostname, null));
                    }
                    it++;
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine("Exception: " + exp.Message);
            }
        }

        private void ConfigureGrid()
        {
            try
            {
                dataGridViewTracks.AutoGenerateColumns = false;


                DataGridViewTextBoxColumn ArtistID = new DataGridViewTextBoxColumn();
                ArtistID.DataPropertyName = "ArtistID";
                ArtistID.HeaderText = "ArtistID";
                ArtistID.Visible = true;
                ArtistID.Name = "ArtistID";
                ArtistID.Width = 50;


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


                DataGridViewListColumn GroupsID = new DataGridViewListColumn();
                GroupsID.DataPropertyName = "GroupsIDs";
                GroupsID.HeaderText = "GroupsIDs";
                GroupsID.Visible = true;
                GroupsID.Name = "GroupsIDs";
                GroupsID.Width = 96;

                DataGridViewTextBoxColumn AlbumID = new DataGridViewTextBoxColumn();
                AlbumID.DataPropertyName = "AlbumID";
                AlbumID.HeaderText = "AlbumID";
                AlbumID.Visible = false;
                AlbumID.Name = "AlbumID";
                AlbumID.Width = 63;

                DataGridViewListColumn LabelID = new DataGridViewListColumn();
                LabelID.DataPropertyName = "LabelIDs";
                LabelID.HeaderText = "LabelIDs";
                LabelID.Visible = true;
                LabelID.Name = "LabelIDs";
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
                
                dataGridViewTracks.Columns.Add(AlbumID);
                dataGridViewTracks.Columns.Add(LabelID);
                dataGridViewTracks.Columns.Add(Info);
                dataGridViewTracks.Columns.Add(YoutubeVideoID);
                dataGridViewTracks.Columns.Add(YoutubeStartOffset);
                dataGridViewTracks.Columns.Add(SourceID);
                dataGridViewTracks.Columns.Add(PlatformID);
                dataGridViewTracks.Columns.Add(PouetID);
                dataGridViewTracks.Columns.Add(GroupsID);
                dataGridViewTracks.DataSource = ds; //set the datasource to the Track Bindinglist
            }
            catch (Exception exp)
            {
                Console.WriteLine("Exception: " + exp.Message);
            }
        }


        private void contextMenuStripArtistSearch_Opened(object sender, EventArgs e)
        {

            try{
                int rowIndex = dataGridViewTracks.SelectedCells[0].RowIndex;

                contextMenuStripArtistSearch.Visible = true;
                contextMenuStripArtistSearch.Items.Clear();
                ToolStripMenuItem itm = new ToolStripMenuItem();
                itm.Text = "Copy to all fields in this row";                    
                itm.Tag = rowIndex;
                itm.Click += new EventHandler(delegate{
                    var aid =ds[rowIndex].ArtistID; 
                    switch (SelectedCell) {
                        case 1:
                            aid = ds[rowIndex].ArtistID;
                            for (int i = 0; i < ds.Count; i++) { ds[i].ArtistID = aid; }
                        break;
                        case 4:
                            aid = ds[rowIndex].ReleaseYear;
                            for (int i = 0; i < ds.Count; i++) { ds[i].ReleaseYear = aid; }
                        break;
                        case 5:
                            aid = ds[rowIndex].MixSongID;
                            for (int i = 0; i < ds.Count; i++) { ds[i].MixSongID = aid; }
                        break;
                        case 6:
                            aid = ds[rowIndex].AlbumID;
                            for (int i = 0; i < ds.Count; i++) { ds[i].AlbumID = aid; }
                        break;
                       /* case 8:
                            aid = ds[rowIndex].LabelIDs;
                        break; */
                       /* case 9:
                            aid = ds[rowIndex].Info;
                        break; */
                        case 9:
                            aid = ds[rowIndex].YoutubeVideoID;
                            for (int i = 0; i < ds.Count; i++) { ds[i].YoutubeVideoID = aid; }
                        break;
                        case 10:
                            aid = ds[rowIndex].YoutubeStartOffset;
                            for (int i = 0; i < ds.Count; i++) { ds[i].YoutubeStartOffset = aid; }
                        break;
                        case 11:
                            aid = ds[rowIndex].SourceID;
                            for (int i = 0; i < ds.Count; i++) { ds[i].SourceID = aid; }
                        break;
                        case 12:
                            aid = ds[rowIndex].PlatformID;
                            for (int i = 0; i < ds.Count; i++) { ds[i].PlatformID = aid; }
                        break;
                        case 13:
                            aid = ds[rowIndex].PouetID;
                            for (int i = 0; i < ds.Count; i++) { ds[i].PouetID = aid; }
                        break;
                    }
                    
                    dataGridViewTracks.Refresh();
                });
                contextMenuStripArtistSearch.Items.Add(itm);
                ToolStripMenuItem itm2 = new ToolStripMenuItem();
                itm2.Text = "Copy to all rows in this column below this field";
                itm2.Tag = rowIndex;
                itm2.Click += new EventHandler(delegate{
                    var aid = ds[rowIndex].ArtistID;
                    switch (SelectedCell) {
                        case 1:
                            aid = ds[rowIndex].ArtistID;
                            for (int i = rowIndex; i < ds.Count; i++) { ds[i].ArtistID = aid; }
                            break;
                        case 4:
                            aid = ds[rowIndex].ReleaseYear;
                            for (int i = rowIndex; i < ds.Count; i++) { ds[i].ReleaseYear = aid; }
                            break;
                        case 5:
                            aid = ds[rowIndex].MixSongID;
                            for (int i = rowIndex; i < ds.Count; i++) { ds[i].MixSongID = aid; }
                            break;
                        case 6:
                            aid = ds[rowIndex].AlbumID;
                            for (int i = rowIndex; i < ds.Count; i++) { ds[i].AlbumID = aid; }
                            break;
                        /* case 8:
                             aid = ds[rowIndex].LabelIDs;
                         break; */
                        /* case 9:
                             aid = ds[rowIndex].Info;
                         break; */
                        case 9:
                            aid = ds[rowIndex].YoutubeVideoID;
                            for (int i = rowIndex; i < ds.Count; i++) { ds[i].YoutubeVideoID = aid; }
                            break;
                        case 10:
                            aid = ds[rowIndex].YoutubeStartOffset;
                            for (int i = rowIndex; i < ds.Count; i++) { ds[i].YoutubeStartOffset = aid; }
                            break;
                        case 11:
                            aid = ds[rowIndex].SourceID;
                            for (int i = rowIndex; i < ds.Count; i++) { ds[i].SourceID = aid; }
                            break;
                        case 12:
                            aid = ds[rowIndex].PlatformID;
                            for (int i = rowIndex; i < ds.Count; i++) { ds[i].PlatformID = aid; }
                            break;
                        case 13:
                            aid = ds[rowIndex].PouetID;
                            for (int i = rowIndex; i < ds.Count; i++) { ds[i].PouetID = aid; }
                            break;
                    }
                    dataGridViewTracks.Refresh();
                });
                contextMenuStripArtistSearch.Items.Add(itm2);
                
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

        private bool _Login(){
            string res = nwc.DownloadString(config.hostname.Replace("demovibes", "account/signin"));
            Dictionary<string, string> kvp = new Dictionary<string, string>();
            kvp.Add("next", "");
            kvp.Add("username", config.username);
            kvp.Add("password", config.password);
            kvp.Add("blogin", "Sign+in");
            res = nwc.PostAction(config.hostname.Replace("demovibes", "account/signin"), kvp);
            if (res.Contains("Welcome, " + config.username)) { //are we logged in?
                return true;
            }
            return false;
        }

        private void StartUploadProcess()
        {
            try
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
            }
            catch (Exception exp)
            {
                Console.WriteLine("Exception: " + exp.Message);
            }

            new Thread(delegate()
            {
                try
                {
                    Dictionary<string, string> kvp = new Dictionary<string, string>();
                    string res = string.Empty;
                    if (_Login())
                    { //are we logged in?
                        int z = 0;
                        foreach (Track track in ds)
                        { //loop over all tracks

                            if (track.Progress == 100)
                            { //this track has already been upped ;)
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
                            if (track.MixSongID > 0)
                            {
                                kvp.Add("remix_of_id", "");
                            }
                            //TODO: add support for groups, labels and type
                            // kvp.Add("groups[]", "");
                            // kvp.Add("labels[]", "");
                            kvp.Add("info", track.Info);
                            //kvp.Add("type", null);
                            if (track.PlatformID > 0)
                            {
                                kvp.Add("platform", track.PlatformID.ToString());
                            }
                            kvp.Add("pouetid", track.PouetID.ToString());
                            if (track.YoutubeVideoID > 0)
                            {
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
                            if (!res.Contains("Preview:"))
                            {
                                MessageBox.Show("Something went wrong when i tried to upload: " + track.File);
                            }
                            if (InvokeRequired) { BeginInvoke(new MethodInvoker(delegate() { dataGridViewTracks.Refresh(); })); }
                            z++;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Something is wrong with your configuration - i can't login");
                    }


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
                }
                catch (Exception exp)
                {
                    Console.WriteLine("Exception: " + exp.Message);
                }
            }).Start();
        }

        void nwc_UploadProgress(long pos, long total)
        {
            try
            {
                int check = ds[current_track_in_upload_queue].Progress;
                long progress = (pos * 100) / total;
                if (check < Int32.Parse(progress.ToString()))
                {
                    ds[current_track_in_upload_queue].Progress = Int32.Parse(progress.ToString());
                    if (InvokeRequired) { BeginInvoke(new MethodInvoker(delegate() { dataGridViewTracks.Refresh(); })); }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine("Exception: " + exp.Message);
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            StopMusic();

        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void dataGridViewTracks_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    //GOT FILEZ! 
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    List<string> filetypes = new List<string>();
                    filetypes.Add(".s3m");
                    filetypes.Add(".xm");
                    filetypes.Add(".it");
                    filetypes.Add(".mp3");
                    filetypes.Add(".mp4");
                    filetypes.Add(".m4a");
                    filetypes.Add(".ogg");
                    filetypes.Add(".mod");
                    filetypes.Add(".mtm");
                    filetypes.Add(".umx");
                    filetypes.Add(".it");
                    foreach (string filename in files)
                    {
                        FileInfo f = new FileInfo(filename);
                        bool match = false;
                        foreach (string ft in filetypes)
                        {
                            if (ft.Equals(f.Extension))
                            {
                                match = true;
                            }
                        }
                        if (match)
                        {
                            ds.Add(new Track(filename));
                        }

                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine("Exception: " + exp.Message);
            }
        }

        private void dataGridViewTracks_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    e.Effect = DragDropEffects.Move;
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine("Exception: " + exp.Message);
            }
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
