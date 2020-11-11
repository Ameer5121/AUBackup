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
using System.Runtime.Serialization;

namespace MVVMAUbackup.ViewModels
{
    [Serializable]
    class FolderViewModel : ISerializable
    {
     

        #region Constructor
        public FolderViewModel()
        {
            _folders = new ObservableCollection<FolderModel>();
            _backupTimer = new DispatcherTimer();
            _backupTimer.Interval = TimeSpan.FromSeconds(8);
            _backupTimer.Tick += BackupFolders;
            _statusVM = new StatusViewModel();
        }
        #endregion

        #region Fields
        private ObservableCollection<FolderModel> _folders;
        private static DispatcherTimer _backupTimer;
        private StatusViewModel _statusVM;
        #endregion


        #region Properties
        public static DispatcherTimer BackupTimer => _backupTimer;
        public ObservableCollection<FolderModel> Folders => _folders;
        public StatusViewModel StatusVM => _statusVM;
        #endregion


        #region Commands
        public ICommand Add =>  new RelayCommand(AddFolders, CanAddFolders);      
        public ICommand Remove =>  new RelayCommand(RemoveFolder, CanRemoveFolder);
        public ICommand TargetFolder => new RelayCommand(AddBackupFolder, CanAddBackupFolder);
        public ICommand TargetName => new RelayCommand(BackupFolderName);
        public ICommand StartBackupProcess => new RelayCommand(StartTimer, CanStartTimer);

        #endregion


        #region Methods
        private bool CanAddFolders()
        {
            if (_backupTimer.IsEnabled)
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
                    MessageBox.Show("A folder of the same type already exists!");
                    return;
                }                
               _folders.Add(new FolderModel { Name = Path.GetFileName(OpenDialog.SelectedPath), FilePath = OpenDialog.SelectedPath });        
            }
        }


        private bool CanRemoveFolder()
        {
            if (_backupTimer.IsEnabled)
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
            if (_backupTimer.IsEnabled)
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
                    MessageBox.Show("The Backup folder cannot be in the Folder list that you want to Backup!");
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
            var StopWatch = new Stopwatch();
            if (_backupTimer.IsEnabled)
            {
                PauseTimer(StopWatch);
                StopWatch.Reset();
                return;
            }
            _backupTimer.Start();
            StopWatch.Start();
            _statusVM.StartProcess();
        }

        private void PauseTimer(Stopwatch ElapsedTime)
        {

            _backupTimer.Interval = ElapsedTime.Elapsed;    
            _statusVM.PauseProcess();
            _backupTimer.Stop();
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
        #endregion


        #region Serialization
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach(FolderModel Folder in _folders)
            {
                info.AddValue(Folder.Name, Folder);
            }
        }

        public FolderViewModel(SerializationInfo info, StreamingContext context)
        {

        }
        #endregion



    }
}
