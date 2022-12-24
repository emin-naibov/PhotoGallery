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
        public ObservableCollection<Folder> Folders { get; set; } = new ObservableCollection<Folder>();
        public List<string> Paths { get; set; } = new List<string>();
        public MainWindow()
        {
            DataLoad();
            InitializeComponent();
            FolderListBox.ItemsSource = Folders;
        }
        public class Image
        {
            public Image(string path)
            {
                Photo = path;
                Name = Path.GetFileName(path);
                FileInfo fileInfo = new FileInfo(path);
                //creatingTime = fileInfo.LastWriteTime.ToString();  also an option
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
            }

            public string Url { get; set; }
            public ObservableCollection<Image> Images { get; set; } = new ObservableCollection<Image>();
        }
        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog=null;
            bool can_add = true;
            try
            {
                dialog = new CommonOpenFileDialog();
                dialog.InitialDirectory = "C:\\Users";
                dialog.IsFolderPicker = true;
                dialog.ShowDialog();


            }
            catch (Exception)
            {
                 MessageBox.Show("Choose Folder");
            }
            finally
            {
                DataSave();
            }

            foreach (var item in Folders)
            {
                if(Path.GetFileName(dialog.FileName)==item.Url)
                {
                    can_add = false;
                    MessageBox.Show("This folder already exists");
                }
            }
            if(can_add)
            {
            Folder folder = new Folder(dialog.FileName);
            Folders.Add(folder);
            Paths.Add(dialog.FileName);
            }

        }
        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            if(FolderListBox.SelectedIndex!=-1)
            {
                Folders.RemoveAt(FolderListBox.SelectedIndex);
            }
        }
        private void ImageListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new ImagesWindow(Folders[FolderListBox.SelectedIndex].Images[ImageListBox.SelectedIndex].Photo, this).Show();
        }
        public string GetNext()
        {
            if (ImageListBox.SelectedIndex < ImageListBox.Items.Count)
                ImageListBox.SelectedIndex++;
            return Folders[FolderListBox.SelectedIndex].Images[ImageListBox.SelectedIndex].Photo;
        }
        public string GetPrevious()
        {
            if (ImageListBox.SelectedIndex-1>=0)
                ImageListBox.SelectedIndex--;
            return Folders[FolderListBox.SelectedIndex].Images[ImageListBox.SelectedIndex].Photo;
        }

        public void DataLoad()
        {
            try
            {
                string json = File.ReadAllText("paths.json");
                Paths = JsonSerializer.Deserialize<List<string>>(json);
                foreach (var item in Paths)
                {
                    Folder folder = new Folder(item);
                    Folders.Add(folder);

                }
            }
            catch (Exception)
            {
                MessageBox.Show("Your Photo Gallery is empty");

            }
        }
        public void DataSave()
        {
            try
            {
               File.WriteAllText("paths.json", JsonSerializer.Serialize(Paths));

            }
            catch (Exception)
            {

                MessageBox.Show("Cant Save Data to Json");
            }
        }
    }

}
