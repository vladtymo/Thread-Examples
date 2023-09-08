using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace FIle_Copy
{
    // клас, який містить необхідну інформацію про процес копіювання
    public class CopyFileInfo : INotifyPropertyChanged
    {
        private int progress;

        public string SourceFile { get; set; }
        public string DestFolder { get; set; }

        public string SourceFileName => Path.GetFileName(SourceFile);
        public string DestFolderName => new DirectoryInfo(DestFolder).Name;
        // прогрес виконання операції
        // до цієї властивості "біндиться" ProgressBar, тому викликаємо PropertyChanged
        public int Progress
        {
            get { return progress; }
            set
            {
                progress = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
