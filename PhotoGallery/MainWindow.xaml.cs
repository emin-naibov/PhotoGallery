using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Text.Json;

namespace PhotoGallery
{
  
    public partial class MainWindow : Window
    {
       // public ObservableCollection<Image> Images { get; set; }
        public ObservableCollection<Folder> Folders { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            //Images = new ObservableCollection<Image>();
            Folders = new ObservableCollection<Folder>();
            FolderListBox.ItemsSource = Folders;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\Users";
            dialog.IsFolderPicker = true;
            dialog.ShowDialog();
            //if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            //{
            //    MessageBox.Show("You selected: " + dialog.FileName);
            //}
            //Folders.Add(Path.GetFileName(dialog.FileName), null);
            // Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            //string json = File.ReadAllText(@$"{dialog.FileName}");
            //Images = JsonSerializer.Deserialize<List<Account>>(json);
            Folder folder = new Folder(dialog.FileName);
            Folders.Add(folder);
        }
        public class Image
        {
            public Image(string path)
            {
                Photo = path;
                Name = Path.GetFileName(path);
                creatingTime = File.GetCreationTime(path).ToString();
                updatingTime = File.GetLastWriteTime(path).ToString();
            }

            public string Photo { get; set; }
            public string Name { get; set; }
            public string creatingTime { get; set; }
            public string updatingTime { get; set; }
        }
        public class Folder
        {
            public Folder(string url)
            {
                Url = Path.GetFileName(url);
                string[] file_paths = Directory.GetFiles(url);
                foreach (var item in file_paths)
                {
                    Image image = new Image(item);
                    Images.Add(image);
                }
               // Images = JsonSerializer.Deserialize<ObservableCollection<Image>>(url);
                //Images = images;
            }

            public string Url { get; set; }
            public ObservableCollection<Image> Images { get; set; } = new ObservableCollection<Image>();
        }
    }

}
