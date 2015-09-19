using MahApps.Metro.Controls;
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
using System.IO;
using System.Threading;
using System.Windows.Threading;

namespace FlickrResearcher
{
    /// <summary>
    /// Логика взаимодействия для MoodWindow.xaml
    /// </summary>
    public partial class MoodWindow : MetroWindow
    {
        private FlickrClient client;
        private User user;
        private List<FlickrImage> _imagesList = null;
        private Thread thread;
        private DispatcherTimer timer;

        private System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
        private System.Diagnostics.StackTrace t = new System.Diagnostics.StackTrace();

        public MoodWindow()
        {
            InitializeComponent();
            Loaded += delegate { Load(); };
        }

        private void Load()
        {
            client = new FlickrClient();
            _imagesList = new List<FlickrImage>();

            user = new User("", "", TypeMoodUser.Cool);//User.CurrentUser;
            txtMood.Text = "Your mood is: " + user.TypeMood.ToString();
            imageMood.Source = user.LoadMood(user.TypeMood);

            //TestThread(); // Can load image to Imae on form
            TestDispatcherTimer(); // Can load image on form. SOme times load image togather
            //TestNewTask(); // Can load image to Imae on form

            //LoadFlickrImage();

            //this.imageFlickr1.Dispatcher.BeginInvoke(new Action(delegate() { imageFlickr1.Source = _imagesList[0]; }));
            //this.imageFlickr2.Dispatcher.BeginInvoke(new Action(delegate() { imageFlickr1.Source = _imagesList[1]; }));  
        }


        /// <summary>
        /// The time of tests you can see in ...FlickrResearcher\FlickrResearcher\bin\Debug\Analiz.txt
        /// </summary>
        #region Load images uisng task
        private void TestNewTask()
        {
            Task tsk = Task.Factory.StartNew(() =>
            {
                LoadFlickrImage();
                //Dispatcher.Invoke(() => imageFlickr1.Source = _imagesList[0]);
                Thread.Sleep(500);
            });
        }
        #endregion

        #region Load images using dispatcher timer
        private void TestDispatcherTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 3);
            timer.IsEnabled = true;
            timer.Tick += timer_Tick;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            LoadFlickrImage();
        }

        #endregion 

        #region Load images using new thread 
        private void TestThread()
        {
            thread = new Thread(new ThreadStart(LoadFlickrImage));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Name = "Flickr-Thread";
            thread.Priority = ThreadPriority.AboveNormal;
            thread.Start();
        }
        #endregion

        private void LoadFlickrImage()
        {
            watch.Start();
            _imagesList = client.GetImagesByTag(user.TypeMood, 2);
            watch.Stop();

            client.WriteToFile("MoodWindow.", watch.Elapsed.Milliseconds, _imagesList.Count);

            imageFlickr1.Source = _imagesList[0].Image;
            imageFlickr2.Source = _imagesList[1].Image;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            new FlickrWindow().Show();
            this.Close();
        }

        private void MoodFlickrWindow_Closed(object sender, EventArgs e)
        {
            timer.Stop();
            watch.Stop();
        }
    }
}
