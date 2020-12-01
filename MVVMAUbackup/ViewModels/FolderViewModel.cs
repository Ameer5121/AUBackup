using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using MVVMAUbackup.Models;
using MVVMAUbackup.Commands;
using MVVMAUbackup.Events;
using MVVMAUbackup.Animation;
using MVVMAUbackup.Serialization;
using System.Runtime.Serialization;

namespace MVVMAUbackup.ViewModels
{
    [Serializable]
    class FolderViewModel : ViewModelBase , ISerializable
    {
        #region Constructor
        public FolderViewModel()
        {
            _folders = new ObservableCollection<FolderModel>();
            _backupTimer = new DispatcherTimer();
            _backupTimer.Interval = TimeSpan.FromHours(8);
            _backupTimer.Tick += BackupFolders;
            _backupTimer.Tick += RestartInterval;
            _statusVM = new StatusViewModel();
            _animation = new Expand();
        }
        #endregion

        #region Fields
        private ObservableCollection<FolderModel> _folders;
        private static DispatcherTimer _backupTimer;
        private StatusViewModel _statusVM;
        private Expand _animation;
        private static Stopwatch StopWatch = new Stopwatch();
        private bool _isRunning;
        #endregion


        #region Properties
        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                _isRunning = value;
                OnPropertyChanged();
            }
        }
        public static DispatcherTimer BackupTimer => _backupTimer;
        public ObservableCollection<FolderModel> Folders => _folders;
        public StatusViewModel StatusVM => _statusVM;
        public Expand Animation => _animation;

        public event EventHandler<MessageEventArgs> DisplayAlert;

        public event EventHandler Exit;
        #endregion


        #region Commands
        public ICommand Add =>  new RelayCommand(AddFolders, CanAddFolders);      
        public ICommand Remove =>  new RelayCommand(RemoveFolder, CanRemoveFolder);
        public ICommand TargetFolder => new RelayCommand(AddBackupFolder, CanAddBackupFolder);
        public ICommand TargetName => new RelayCommand(BackupFolderName);
        public ICommand StartBackupProcess => new RelayCommand(StartTimer, CanStartTimer);
        public ICommand Save => new RelayCommand(Serialize);
        #endregion


        #region Methods
        private bool CanAddFolders()
        {
            if (IsRunning)
                return false;

            return true;
        }

        private void AddFolders(object ExtraInformation)
        {           
            var OpenDialog = new FolderBrowserDialog();
            if (OpenDialog.ShowDialog() == DialogResult.OK)
            {
                if(_folders.Any(x => x.FilePath == OpenDialog.SelectedPath))
                {
                    DisplayAlert?.Invoke(this, new MessageEventArgs { Message = "A folder of the same type already exists!" });
                    return;
                }                
               _folders.Add(new FolderModel { Name = Path.GetFileName(OpenDialog.SelectedPath), FilePath = OpenDialog.SelectedPath });
            }
        }


        private bool CanRemoveFolder()
        {
            if (IsRunning)
                return false;
            
            return true;
        }
        private void RemoveFolder(object FolderInfo)
        {
            var Folder = FolderInfo as FolderModel;
            _folders.Remove(Folder);
        }


        private bool CanAddBackupFolder() // Target
        {
            if (IsRunning)
                return false;

            return true;
        }

        private void AddBackupFolder(object parameter)
        {
            var OpenDialog = new FolderBrowserDialog();
            if (OpenDialog.ShowDialog() == DialogResult.OK)
            {
                if (_folders.Any(x => x.FilePath == FolderModel.Target))
                {
                    DisplayAlert?.Invoke(this, new MessageEventArgs { Message = "The Backup folder cannot be in the Folder list that you want to Backup!" });
                    return;
                }
                FolderModel.Target = OpenDialog.SelectedPath;
            }
        }

        private void BackupFolderName(object parameter)
        {
            MessageBox.Show(FolderModel.Target);
        }

        ///Backup:

        private bool CanStartTimer()
        {
            return _folders.Count > 0 && FolderModel.Target != null ? true : false;
        }

        private void StartTimer(object parameters)
        {         
            if (IsRunning)
            {
                StopWatch.Stop();
                PauseTimer(StopWatch);
                return;
            }
            IsRunning = true;
            _statusVM.StartProcess();
            _backupTimer.Start();
            StopWatch.Start();
        }
        private void PauseTimer(Stopwatch ElapsedTime)
        {
            IsRunning = false;
            _backupTimer.Stop();
            _backupTimer.Interval -= TimeSpan.FromSeconds(ElapsedTime.Elapsed.TotalSeconds);    
            _statusVM.PauseProcess();
            StopWatch.Reset();
        }

        private void BackupFolders(object sender, EventArgs e)
        {
             Task.Run(() =>
             {
                foreach (FolderModel Folder in _folders)
                {
                    var DirectoryName = Path.GetFileName(Folder.FilePath);
                    FileSystem.CreateDirectory($"{FolderModel.Target}\\{DirectoryName}");
                    FileSystem.CopyDirectory(Folder.FilePath, $"{FolderModel.Target}\\{DirectoryName}", true);
                }
            });
        }
        /// <summary>
        /// Resets the interval of the backup process after every cycle incase the user pauses it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RestartInterval(object sender, EventArgs e)
        {
            _backupTimer.Interval = TimeSpan.FromHours(8);
            StopWatch.Restart();
        }
        #endregion


        #region Serialization

        private void Serialize(object parameters)
        {
            if (IsRunning)
            {
                DisplayAlert?.Invoke(this, new MessageEventArgs { Message = "Please pause the backup process before exiting." });
                return;
            }
            Serializer.Serialize(this);
            Exit?.Invoke(this, EventArgs.Empty);
        }
            
        public FolderViewModel Deserialize()
        {
            FolderViewModel DeserializedClass = null;
            Serializer.DeSerialize(ref DeserializedClass);
            return DeserializedClass == null ? this : DeserializedClass;
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Folders", _folders);
            info.AddValue("Target", FolderModel.Target);
            info.AddValue("Status", _statusVM);
            info.AddValue("BackupTimerInterval", _backupTimer.Interval);
        }



        public FolderViewModel(SerializationInfo info, StreamingContext context) : this()
        {
            _folders = (ObservableCollection<FolderModel>)info.GetValue("Folders", typeof(ObservableCollection<FolderModel>));
            FolderModel.Target = (string)info.GetValue("Target", typeof(string));
            _statusVM = (StatusViewModel)info.GetValue("Status", typeof(StatusViewModel));
            _backupTimer.Interval = (TimeSpan)info.GetValue("BackupTimerInterval", typeof(TimeSpan));
        }
        #endregion



    }
}
