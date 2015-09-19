using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FlickrResearcherLibrary
{
    /// <summary>
    /// To get info about image on flickr
    /// </summary>
    public class FlickrImage
    {
        public string Title { get; set; }
        public string PhotoId { get; set; }
        public string Description { get; set; }
        public string OwnerName { get; set; }
        public string WebUrl { get; set; }
        public int? Views { get; set; }
        public string Url { get; set; }
        public System.Collections.ObjectModel.Collection<string> Tags { get; set; }
        public BitmapImage Image { get; set; }
    }
}
