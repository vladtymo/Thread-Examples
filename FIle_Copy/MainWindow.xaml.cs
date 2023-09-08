using System;
using System.Collections.Generic;
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

namespace FIle_Copy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CopyFileInfo CopyInfo { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            CopyInfo = new CopyFileInfo();
            // даємо можливість "біндиться" до параметрів операції копіювання
            this.DataContext = CopyInfo;
        }

        // запуск копіювання файла
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // створюємо новий потік
            Thread copyThread = new Thread(CopyFile);

            // встановлюємо дані для копіювання
            CopyInfo.SourceFile = @"C:\Users\Vlad\Desktop\test.iso";
            CopyInfo.DestFolder = @"C:\Users\Vlad\Desktop";

            // запускаємо потік, передаючи необхідні дані 
            copyThread.Start(CopyInfo);
        }

        private void CopyFile(object data)
        {
            // приводимо object до типу CopyFileInfo, вразі невдачі - return
            CopyFileInfo info = data as CopyFileInfo;
            if (info == null) return;
            // коротший запис
            //if (!(data is CopyFileInfo info)) return;

            // формуємо ім'я файла копії - name_copy_mmss.extension
            string copyFileName = $"{Path.GetFileNameWithoutExtension(info.SourceFile)}_copy_{DateTime.Now.ToString("mmss")}{Path.GetExtension(info.SourceFile)}";
            // формуємо шлях до файла копії
            string destFile = Path.Combine(info.DestFolder, copyFileName);

            // відкриваємо потік для читання файла
            using (FileStream sourceFs = File.OpenRead(info.SourceFile))
            {
                // відкриваємо потік для запису файла копії
                FileStream destFs = new FileStream(destFile, FileMode.OpenOrCreate, FileAccess.Write);
                // встановлюємо початковий розмір файла копії
                destFs.SetLength(sourceFs.Length);

                long sourceFileLength = sourceFs.Length;    // розмір файла
                byte[] buffer = new byte[1024 * 8];        // буфер розміром 8Kb
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

            // копіювання файла методом класа File.Copy()
            //File.Copy(info.SourceFile, destFile, true);
        }
    }
}
