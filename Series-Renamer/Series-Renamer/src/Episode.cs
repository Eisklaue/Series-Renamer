using System;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Series_Renamer.src
{
    public class Episode
    {
        public int id { get; set; }
        public string name { get; set; }
        public string series { get; set; }
        public string season { get; set; }
        public string fullPath { get; set; }
        public string path { get; }
        public string episodeNr { get; set; } 
        public string episode { get; set; } 
        public string folder { get; set; }
        
        public Episode (string path)
        {
            this.fullPath = path;
            this.path = new DirectoryInfo(Path.GetDirectoryName(fullPath)).ToString();

            getName();
            getEpisode();
            getSerie();

            this.folder = ConfigurationManager.AppSettings["SerieFolder"]+"\\"+this.series + "\\" +this.season;            
        }

        public Episode(string path, int id)
        {
            this.fullPath = path;
            this.path = new DirectoryInfo(Path.GetDirectoryName(fullPath)).ToString();
            this.id = id;

            getName();
            getEpisode();
            getSerie();

            this.folder = ConfigurationManager.AppSettings["SerieFolder"] + "\\" + this.series + "\\" + this.season;
        }

        private void getEpisode()
        {
            Regex regex = new Regex(@"S(?<season>\d{1,2})E(?<episode>\d{1,2})");
            Match match = regex.Match(this.fullPath);
            
            if (match.Success)
            {
                this.season = "Staffel " + match.Groups["season"].Value;
                this.episodeNr = match.Groups["episode"].Value;
                this.episode = match.Groups["season"].Value + match.Groups["episode"].Value;
            }

        }

        private void getName()
        {
            this.name = Path.GetFileName(this.fullPath);
        }

        private void getSerie()
        {   //get everything till Season/Episode Information: (?:(?!\.S\d{1,2}E\d{1,2}).)*
            Regex regex = new Regex(@"(?<serie>(?<=Z:\\JDownload\\)(.*)(?=\.S\d{1,2}E\d{1,2}))");
            Match match = regex.Match(this.fullPath);

            if (match.Success)
            {
                
                this.series = match.Groups["serie"].Value.Replace('.',' ');
            }
        }


    }
}
