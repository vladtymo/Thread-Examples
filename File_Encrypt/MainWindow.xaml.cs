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

namespace File_Encrypt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<EncryptProcessInfo> operations = new ObservableCollection<EncryptProcessInfo>();
        public MainWindow()
        {
            InitializeComponent();

            threadList.ItemsSource = operations;
            ThreadPool.SetMaxThreads(10, 10);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EncryptProcessInfo info = new EncryptProcessInfo()
            {
                FileName = fileNameTB.Text,
                OperationType = encrypt_rb.IsChecked.Value ? "Encrypt" : "Decrypt",
                Progress = 0
            };

            operations.Add(info);

            //Operation(info);

            //Thread thread = new Thread(Operation);
            //thread.Start(info);
            
            // IsBackground = true
            // Priority = Normal
            ThreadPool.QueueUserWorkItem(Operation, info);
        }

        private void Operation(object obj)
        {
            //MessageBox.Show("Id: " + Thread.CurrentThread.ManagedThreadId);
            EncryptProcessInfo info = obj as EncryptProcessInfo;
            if (info == null)
                return;

            // algorithm encrypt XOR
            // byte ^ key

            Random rnd = new Random();
            while(info.Progress < 99)
            {
                info.Progress += rnd.Next(5);
                Thread.Sleep(rnd.Next(500));
            }
            info.Progress = 100;
        }
    }
}
