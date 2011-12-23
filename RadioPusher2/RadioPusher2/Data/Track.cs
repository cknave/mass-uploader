using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RadioPusher2.Data
{
    public class Track
    {
        /*
        TagLib.File file = TagLib.File.Create ("/home/schneipp/Music/skank.mp3");
		//f_duration = file.Properties.Duration.Milliseconds.ToString ();
		string f_artist = file.Tag.FirstAlbumArtist;
		string f_title = file.Tag.Title;
		string f_album = file.Tag.Album;
		string f_trackno = file.Tag.Track.ToString ();	
         */
        private int f_progress = 0;
        private int f_ArtistID = 0;
        private string f_File = String.Empty;
        private string f_SongName = String.Empty;
        private int f_ReleaseYear = 1990;
        private int f_MixSongID = 0;
        private int f_AlbumID = 0;
        private List<int> f_GroupsIDs = new List<int>();
        private List<int> f_LabelIDs = new List<int>();
        private string f_Info = String.Empty;
        private int f_SourceID = 0;
        private int f_PlatformID = 0;
        private int f_PouetID = 0;
        private int f_YoutubeVideoID = 0;
        private int f_YoutubeStartOffset = 0;
        private string f_ID3Artist = String.Empty;
        

        public int Progress
        {
            get { return f_progress; }
            set { f_progress = value; }
        }

        public int AlbumID
        {
            get { return f_AlbumID; }
            set { f_AlbumID = value; }
        }


        public string ID3Artist
        {
            get { return f_ID3Artist; }
            set { f_ID3Artist = value; }
        }

        public Track(string FilePath)
        {
            File = FilePath;
            
            //f_duration = file.Properties.Duration.Milliseconds.ToString ();
            try
            {
                TagLib.File file = TagLib.File.Create(FilePath);
                SongName = file.Tag.Title;
                //the artist part is hard..
                try
                {
                    ID3Artist = file.Tag.Artists[0];
                }catch{
                    /*
                    string artist = string.Empty;
                    try{
                        artist = Regex.Match(file.Name, @"\(?<m>.+?)\-").Groups["m"].Value.Trim();
                    }catch{
                        try
                        {
                            artist = file.Tag.Composers[0];
                        }
                        catch
                        {
                        }
                    }
                    ID3Artist = artist;
                     */ 
                }
                
                ReleaseYear = (int)file.Tag.Year;
                Info = file.Tag.Comment;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
           // = file.Tag.Album;
           // = file.Tag.Track.ToString();	
        }

        public int ArtistID
        {
            get { return f_ArtistID; }
            set { f_ArtistID = value; }
        }
        public string File
        {
            get { return f_File; }
            set { f_File = value; }
        }

        public string SongName
        {
            get { return f_SongName; }
            set { f_SongName = value; }
        }

        public int ReleaseYear
        {
            get { return f_ReleaseYear; }
            set { f_ReleaseYear = value; }
        }
        public int MixSongID
        {
            get { return f_MixSongID; }
            set { f_MixSongID = value; }
        }

        public List<int> GroupsIDs
        {
            get { return f_GroupsIDs; }
            set { f_GroupsIDs = value; }
        }

        public List<int> LabelIDs
        {
            get { return f_LabelIDs; }
            set { f_LabelIDs = value; }
        }

        public string Info
        {
            get { return f_Info; }
            set { f_Info = value; }
        }

        public int SourceID
        {
            get { return f_SourceID; }
            set { f_SourceID = value; }
        }

        public int PlatformID
        {
            get { return f_PlatformID; }
            set { f_PlatformID = value; }
        }

        public int PouetID
        {
            get { return f_PouetID; }
            set { f_PouetID = value; }
        }

        public int YoutubeVideoID
        {
            get { return f_YoutubeVideoID; }
            set { f_YoutubeVideoID = value; }
        }

        public int YoutubeStartOffset
        {
            get { return f_YoutubeStartOffset; }
            set { f_YoutubeStartOffset = value; }
        }


    }
}
