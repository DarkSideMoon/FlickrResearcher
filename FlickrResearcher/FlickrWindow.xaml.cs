using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FlickrResearcherLibrary;
using MahApps.Metro.Controls;
using System.Threading;

namespace FlickrResearcher
{
    /// <summary>
    /// Логика взаимодействия для FlickrWindow.xaml
    /// </summary>
    public partial class FlickrWindow : MetroWindow
    {
        private System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
        private System.Windows.Threading.DispatcherTimer timer;
        private FlickrClient flickr;
        
        public FlickrWindow()
        {
            InitializeComponent();

            timer = new System.Windows.Threading.DispatcherTimer();
            flickr = new FlickrClient();
        }

        /// <summary>
        /// Load personal information and image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainGrid_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Tick += RefreshPhoto;
            timer.Interval = new TimeSpan(0, 0, 2);
            timer.Start();

            User user = User.CurrentUser;
            txtName.Text = "Your name is : " + user.Name;
            txtCity.Text = "You live in: " + user.City;
            txtMood.Text = "Your mood is: " + user.TypeMood.ToString();


        }

        /// <summary>
        /// Load photos from web service "Flickr". Load photos every 15 sec
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshPhoto(object sender, EventArgs e)
        {
            flickr.GetPhotoUrl();
            watch.Start();
            image.Source = flickr.GetImageByUrl(flickr.Url);
            watch.Stop();
            watch.Reset();

            FlickrImage imageInfo = flickr.GetFlickImageInfo();
            txtDescription.Text = "Description: " + imageInfo.Description;
            txtPhotoId.Text = "Photo id: " + imageInfo.PhotoId.ToString();
            txtOwenerName.Text = "Owner name is: " + imageInfo.OwnerName;
            txtTitle.Text = "Title: " + imageInfo.Title;

            if (imageInfo.Views == null)
                txtViews.Text = "Views is: 0";
            else
                txtViews.Text = "Views is: " + imageInfo.Views;

            if (imageInfo.Tags != null)
                foreach (var item in imageInfo.Tags)
                    txtTags.Text = "Tags: " + item.ToString();
            else
                txtTags.Text = "Tags: nothing to show";

        }

        private void menuItemMoodImage_Click(object sender, RoutedEventArgs e)
        {
            new MoodWindow().Show();
            this.Close();
        }

        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            timer.Stop();
        }

        private void menuItemSearchImage_Click(object sender, RoutedEventArgs e)
        {
            new SearchWindow().Show();
            this.Close();
        }
    }
}
