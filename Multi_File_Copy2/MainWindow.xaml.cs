using IOExtensions;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Windows;

namespace FIle_Copy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<CopyFileInfo> operations;
        public MainWindow()
        {
            InitializeComponent();

            operations = new ObservableCollection<CopyFileInfo>();
            // даємо можливість "біндиться" до параметрів операції копіювання
            operationsList.ItemsSource = operations;
        }

        // запуск копіювання файла
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CopyFileInfo info = new CopyFileInfo()
            {
                // встановлюємо дані для копіювання
                SourceFile = filePathTB.Text,
                DestFolder = folderPathTB.Text
            };
            operations.Add(info);

            // створюємо новий потік
            //Thread copyThread = new Thread(CopyFile);
            // запускаємо потік, передаючи необхідні дані 
            //copyThread.Start(info);
            ThreadPool.QueueUserWorkItem(CopyFile, info);
        }

        int c = 10;
        private void CopyFile(object data)
        {
            // приводимо object до типу CopyFileInfo, вразі невдачі - return
            //CopyFileInfo info = data as CopyFileInfo;
            //if (info == null) return;
            // коротший запис
            if (!(data is CopyFileInfo info)) return;

            // формуємо ім'я файла копії - name_copy_mmss.extension
            string copyFileName = $"{Path.GetFileNameWithoutExtension(info.SourceFile)}_copy_{c++}{Path.GetExtension(info.SourceFile)}";
            // формуємо шлях до файла копії
            string destFile = Path.Combine(info.DestFolder, copyFileName);

            // 1 - manual
            try
            {
                // відкриваємо потік для читання файла
                using (FileStream sourceFs = new FileStream(info.SourceFile, FileMode.Open, FileAccess.Read, FileShare.Read))//File.OpenRead(info.SourceFile))
                {
                    // відкриваємо потік для запису файла копії
                    FileStream destFs = new FileStream(destFile, FileMode.OpenOrCreate, FileAccess.Write);
                    // встановлюємо початковий розмір файла копії
                    destFs.SetLength(sourceFs.Length);

                    long sourceFileLength = sourceFs.Length;    // розмір файла
                    byte[] buffer = new byte[1024 * 16];        // буфер розміром 8Kb
                    long readBytes = 0;                         // поточна к-сть зчитаних байт
                    long totalReadBytes = 0;                    // загальна к-сть зчитаних байт

                    do
                    {
                        // зчитуємо дані з файла в буфер та зберігаємо к-сть зчитаних байт
                        readBytes = sourceFs.Read(buffer, 0, buffer.Length);
                        // записуємо зчитані дані в файл копію
                        destFs.Write(buffer, 0, (int)readBytes);
                        // оновлюємо загальну к-сть зчитаних байтів
                        totalReadBytes += readBytes;
                        // оновлюємо значення прогресу операції (%)
                        info.Progress = (int)(totalReadBytes / (sourceFileLength / 100.0));

                    } while (readBytes > 0); // повторюємо поки дані для читання існують

                    // закриваємо потік файла копії
                    destFs.Close();
                }

                // 2 - File.Copy()
                // копіювання файла методом класа File.Copy()
                //File.Copy(info.SourceFile, destFile, true);

                // 3 - using FileTransferManager
                //FileTransferManager.CopyWithProgress(info.SourceFile, destFile, (obj) =>
                //{
                //    if (obj.Percentage >= 1)
                //        info.Progress = (int)obj.Percentage;
                //}, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
