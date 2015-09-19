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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using FlickrResearcherLibrary;

namespace FlickrResearcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        User user;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            user = new User();
            comboBoxMood.ItemsSource = Enum.GetValues(typeof(TypeMoodUser));
        }

        private void btnOpenDeveloperContact_Click(object sender, RoutedEventArgs e)
        {
            // open link in linkd in 
        }

        private void btnAboutWindow_Click(object sender, RoutedEventArgs e)
        {
            new AboutWindow().Show();
        }

        private void btnNextStep_Click(object sender, RoutedEventArgs e)
        {
            this.Height = 350;
            btnNextStep.Visibility = System.Windows.Visibility.Collapsed;

            canvasMenu.Visibility = System.Windows.Visibility.Visible;
            btnNext.Visibility = System.Windows.Visibility.Visible;
        }

        private void comboBoxMood_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object objMood = comboBoxMood.SelectedItem;
            user.TypeMood = user.ChooseMood(objMood.ToString());
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            user.Name = txtName.Text;
            user.City = txtCity.Text;
            User.CurrentUser = user;

            new FlickrWindow().Show();
            this.Close();
        }
    }
}
