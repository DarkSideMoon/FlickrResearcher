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
using MahApps.Metro.Controls;
using FlickrResearcherLibrary;

namespace FlickrResearcher
{
    /// <summary>
    /// Логика взаимодействия для SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : MetroWindow
    {
        FlickrClient client;
        public SearchWindow()
        {
            InitializeComponent();
            client = new FlickrClient();
            this.btnOpenInBrowser.Click += new RoutedEventHandler(OpenInBrowser);
        }

        private void OpenInBrowser(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(client.WebUrl);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            List<FlickrImage> _listImages = null;
            string name = txtSearch.Text;

            int countImages = 0;
            int.TryParse(txtCountImages.Text, out countImages);
           

            // Search images by Flickr account
            if (radioUserPhotos.IsChecked == true && txtSearch.Text != "")
                _listImages = client.GetImagesByUserName(name);
            // Search images by tag 
            if (radioTagPhotos.IsChecked == true && txtSearch.Text != "" && txtCountImages.Text != "")
                _listImages = client.GetImagesByTag(name, countImages);

            if (_listImages != null)
                listBoxImages.ItemsSource = _listImages;
            else
                MessageBox.Show("Select search settings and write search parametr!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void listBoxImages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic value = listBoxImages.SelectedItem;

            if (value == null)
            {
                this.btnOpenInBrowser.Visibility = System.Windows.Visibility.Hidden;
                txtUrlImage.Text = "";
                return;
            }
            txtUrlImage.Text = value.WebUrl;
            client.WebUrl = value.WebUrl;
            this.btnOpenInBrowser.Visibility = System.Windows.Visibility.Visible;
        }

        private void radioTagPhotos_Checked(object sender, RoutedEventArgs e)
        {
            txtCountImages.Visibility = System.Windows.Visibility.Visible;
            txtCount.Visibility = System.Windows.Visibility.Visible;
        }

        private void radioUserPhotos_Checked(object sender, RoutedEventArgs e)
        {
            txtCountImages.Visibility = System.Windows.Visibility.Collapsed;
            txtCount.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            new FlickrWindow().Show();
            this.Close();
        }
    }
}
