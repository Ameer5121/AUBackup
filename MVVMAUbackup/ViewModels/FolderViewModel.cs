using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using MVVMAUbackup.Models;
using MVVMAUbackup.Commands;

namespace MVVMAUbackup.ViewModels
{
    class FolderViewModel
    {
     

        #region Constructor
        public FolderViewModel()
        {
            _folders = new ObservableCollection<FolderModel>();
            _backupTimer = new DispatcherTimer();
            _backupTimer.Interval = TimeSpan.FromHours(8);
            history = new HistoryViewModel();
        }
        #endregion

        #region Fields
        private ObservableCollection<FolderModel> _folders;
        private static DispatcherTimer _backupTimer;
        private HistoryViewModel history;
        #endregion


        #region Properties
        public static DispatcherTimer BackupTimer => _backupTimer;
        public ObservableCollection<FolderModel> Folders => _folders;
        public HistoryViewModel History => history;
        #endregion


        #region Commands
        public ICommand Add =>  new RelayCommand(AddFolders, CanAddFolders);      
        public ICommand Remove =>  new RelayCommand(RemoveFolder, CanRemoveFolder);
        public ICommand TargetFolder => new RelayCommand(AddBackupFolder, CanAddBackupFolder);
        public ICommand TargetName => new RelayCommand(BackupFolderName);
        public ICommand AddTarget => new RelayCommand(AddBackupFolder, CanAddBackupFolder);
        public ICommand StartTimer => new RelayCommand(StartOrPauseTimer, CanStartTimer);

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

        private void StartOrPauseTimer(object parameters)
        {
            TimeSpan CurrentTick = TimeSpan.FromSeconds(0);
            if (_backupTimer.IsEnabled)
            {
                CurrentTick = _backupTimer.Interval;
                history.UpdateProgress.Stop();
                _backupTimer.Stop();
                return;
            }
            _backupTimer.Interval = CurrentTick;
            _backupTimer.Start();
        }

        private void BackupFolders()
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


    }
}
