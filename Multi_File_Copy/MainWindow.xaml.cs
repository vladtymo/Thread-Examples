using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Multi_File_Copy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<FileCopyInfo> files = new ObservableCollection<FileCopyInfo>();

        public MainWindow()
        {
            InitializeComponent();

            threadsListBox.ItemsSource = files;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FileCopyInfo info = new FileCopyInfo()
            {
                FileName = fileNameTB.Text,
                FolderName = folderNameTB.Text,
                Progress = 0
            };
            files.Add(info);

            //CopyFile(info); // freez

            //Thread newThread = new Thread(CopyFile);
            //newThread.Start(info);

            // Priority = Normal
            // IsBackground = true
            ThreadPool.QueueUserWorkItem(CopyFile, info);
        }

        private void CopyFile(object obj)
        {
            FileCopyInfo info = obj as FileCopyInfo;
            if (info == null)
                return;

            // set current set ID
            info.Id = Thread.CurrentThread.ManagedThreadId;

            // test progress
            Random rnd = new Random();
            while (info.Progress < 99)
            {
                info.Progress += rnd.Next(5);
                Thread.Sleep(rnd.Next(500));
            }
            info.Progress = 100;

            ///////// Copy file
            //string name = System.IO.Path.GetFileName(info.FileName);
            //string destPath = System.IO.Path.Combine(info.FolderName, name);
            // TODO:
        }
    }
}
