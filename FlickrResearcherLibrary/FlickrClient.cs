using FlickrNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace FlickrResearcherLibrary
{
    public class FlickrClient
    {
        #region Privite Fileds
        private FlickrImage _flickImage = null;
        private Queue<FlickrImage> _flickrImages = null;
        private Queue quene;

        private string[] _badArr = new string[] { "sad", "bad mood", "sad mood" };
        private string[] _coolArr = new string[] { "cool", "greate", "nature", "rainbow", "sunshine" };
        private string[] _goodArr = new string[] { "sunny", "summer", "good", "good morning" };
        #endregion

        #region Properties
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string WebUrl { get; set; }
        public string Url { get; set; }
        #endregion

        #region Constructor
        public FlickrClient()
        {
            quene = new Queue();
            _flickrImages = new Queue<FlickrImage>();
            UserName = "Rob-Shanghai";
        }
        #endregion

        #region Methods
        /// <summary>
        /// Return info about image
        /// </summary>
        /// <returns></returns>
        public FlickrImage GetFlickImageInfo()
        {
            if (_flickrImages.Count != 0)
            {
                _flickImage = (FlickrImage)_flickrImages.Peek();
                _flickrImages.Dequeue();

                return _flickImage;
            }
            else
                return null;
        }

        /// <summary>
        /// Get photo url
        /// </summary>
        /// <returns></returns>
        public string GetPhotoUrl()
        {
            if (quene.Count != 0)
            {
                Url = (string)quene.Peek();
                quene.Dequeue();

                return Url;
            }
            else
            {
                GetPhotos();
                GetPhotoUrl();
            }
            return Url;
        }

        /// <summary>
        /// Get photos and insert them to Queue
        /// </summary>
        private void GetPhotos()
        {
            try
            {
                Flickr _flickr = FlickrManager.GetInstance();

                PhotoSearchOptions search = new PhotoSearchOptions();
                search.Extras = PhotoSearchExtras.AllUrls | PhotoSearchExtras.Description | PhotoSearchExtras.OwnerName;
                search.SortOrder = PhotoSearchSortOrder.Relevance;

                search.PerPage = 10; // Images that search 
                search.Page = 2; 
                search.Tags = "Kiev"; // By tag images search

                PhotoCollection image = _flickr.PhotosSearch(search);

                foreach (var value in image)
                {
                    //Add info about photo to Queue
                    _flickrImages.Enqueue(new FlickrImage()
                    {
                        Url = value.LargeUrl,
                        Title = value.Title,
                        PhotoId = value.PhotoId,
                        Description = value.Description,
                        OwnerName = value.OwnerName,
                        Views = value.Views,
                        WebUrl = value.WebUrl
                    });

                    //Add url to Queue in order to get image from web 
                    quene.Enqueue(value.LargeUrl);
                }
            }
            catch (System.Net.WebException)
            {
                MessageBox.Show("Check network connection", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// Get image bu url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public BitmapImage GetImageByUrl(string url)
        {
            if (url != null || url == "")
            {
                BitmapImage _bitmap = new BitmapImage();
                _bitmap.BeginInit();
                _bitmap.UriSource = new Uri(url, UriKind.Absolute);
                _bitmap.EndInit();
                return _bitmap;
            }
            else
            {
                MessageBox.Show("The url is empty!");
                return null;
            }
        }

        /// <summary>
        /// Get photo in user collection in Flickr using User Name 
        /// </summary>
        /// <param name="userName"></param>
        /// <value>Set the varible Url to photo url</value>
        public List<FlickrImage> GetImagesByUserName(string userName)
        {
            List<FlickrImage> _images = new List<FlickrImage>();
            try
            {
                UserName = userName;

                Flickr _flickr = FlickrManager.GetInstance();
                FoundUser userInfo = _flickr.PeopleFindByUserName(userName);// olivia dee ღ tosakan2000

                PhotoSearchOptions search = new PhotoSearchOptions();
                search.UserId = userInfo.UserId;
                search.Extras = PhotoSearchExtras.AllUrls;
                search.SortOrder = PhotoSearchSortOrder.Relevance;

                PhotoCollection image = _flickr.PhotosSearch(search);

                foreach (var value in image)
                {
                    Url = value.LargeUrl; //value.PhotosetThumbnailUrl
                    _images.Add(new FlickrImage() 
                    {
                        WebUrl = value.WebUrl,
                        Image = GetImageByUrl(Url),
                        Title = value.Title, 
                        Description = value.Description,
                        OwnerName = userInfo.UserName
                    });
                }

                return _images;
            }
            catch (FlickrNet.Exceptions.UserNotFoundException )
            {
                MessageBox.Show("The photos by user name "+ userName +" is not found!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return _images = null;
            }
            catch (System.Net.WebException)
            {
                MessageBox.Show("Check network connection", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return _images = null;
            }
        }

        /// <summary>
        /// Get photo by tag. Return image.
        /// </summary>
        /// <param name="type">Enter a type of users mood</param>
        /// <returns></returns>
        public List<FlickrImage> GetImagesByTag(object tag, int perPage = 5, int numPage = 1)
        {
            Random r = new Random();
            List<FlickrImage> _bitmap = new List<FlickrImage>();
            TypeMoodUser type = default(TypeMoodUser);

            try
            {
                Flickr _flickr = FlickrManager.GetInstance();

                PhotoSearchOptions search = new PhotoSearchOptions();
                search.Extras = PhotoSearchExtras.AllUrls | PhotoSearchExtras.Description | PhotoSearchExtras.OwnerName;
                search.SortOrder = PhotoSearchSortOrder.Relevance;

                //Get one photo
                search.PerPage = perPage;
                search.Page = numPage;

                if (tag is TypeMoodUser)
                {
                    type = (TypeMoodUser)tag;
                    switch (type)
                    {
                        case TypeMoodUser.Good:
                            search.Tags = _goodArr[r.Next(_goodArr.Length)];
                            break;
                        case TypeMoodUser.Bad:
                            search.Tags = _badArr[r.Next(_badArr.Length)];
                            break;
                        case TypeMoodUser.Cool:
                            search.Tags = _coolArr[r.Next(_coolArr.Length)];
                            break;
                        default:
                            break;
                    }
                }
                else
                    search.Tags = (string)tag;

                PhotoCollection image = _flickr.PhotosSearch(search);

                foreach (var value in image)
                {
                    Url = value.LargeUrl; //value.PhotosetThumbnailUrl
                    _bitmap.Add(new FlickrImage() 
                    {
                        WebUrl = value.WebUrl,
                        Image = GetImageByUrl(Url),
                        Title = value.Title, 
                        Description = value.Description,
                    });
                }            
                return _bitmap;
            }
            catch (System.Net.WebException)
            {
                MessageBox.Show("Check network connection", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        /// <summary>
        /// Write to file some results of loading images from Flickr
        /// File only for testing.
        /// </summary>
        /// <param name="time"></param>
        public void WriteToFile(string window, int time, int count)
        {
            string path = Environment.CurrentDirectory + "\\Analiz.txt";
            using (StreamWriter writer = File.AppendText(path))
            {
                writer.WriteLine(Environment.NewLine + "Window is: " + window + Environment.NewLine + 
                    "Load images: " + count + Environment.NewLine +
                    "Date time is: " + DateTime.Now + Environment.NewLine 
                    + "Backgroud thread load image im Milliseconds: " + time);
            }

        }
        #endregion
    }
}
