using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PhotoGallery
{
   
    public partial class ImagesWindow : Window
    {
        public MainWindow mainWindow { get; set; }
        public ImagesWindow(string path,MainWindow mainWindow)
        {
            InitializeComponent();
            DataContext = this;
            this.mainWindow = mainWindow;
            MyImage.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
        }
        public void GetNext()
        {

        }

        private void PrevButtonClick(object sender, RoutedEventArgs e)
        {
            string url = mainWindow.GetPrevious();
            MyImage.Source = new BitmapImage(new Uri(url, UriKind.Absolute));
        }

        private void NextButtonClick(object sender, RoutedEventArgs e)
        {
            string url=mainWindow.GetNext();
            MyImage.Source = new BitmapImage(new Uri(url, UriKind.Absolute));
        }
    }
}
