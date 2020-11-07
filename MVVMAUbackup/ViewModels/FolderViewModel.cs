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
            _time = new ObservableCollection<TimeModel>();
            _backuptimer = new DispatcherTimer();
            _backuptimer.Interval = TimeSpan.FromHours(8);
        }
        #endregion

        #region Fields
        private ObservableCollection<FolderModel> _folders;
        private ObservableCollection<TimeModel> _time;
        private DispatcherTimer _backuptimer;
        #endregion

        //Properties
        #region
        public DispatcherTimer BackupTimer => _backuptimer;
        public ObservableCollection<FolderModel> Folders => _folders;
        public ObservableCollection<TimeModel> Time => _time;
        #endregion

        //Commands
        #region
        public ICommand Add =>  new RelayCommand(AddFolders, CanAddFolders);      
        public ICommand Remove =>  new RelayCommand(RemoveFolder, CanRemoveFolder);
        public ICommand TargetFolder => new RelayCommand(AddBackupFolder, CanAddBackupFolder);
        public ICommand TargetName => new RelayCommand(BackupFolderName);
        public ICommand AddTarget => new RelayCommand(AddBackupFolder, CanAddBackupFolder);
        public ICommand StartTimer => new RelayCommand(StartOrPauseTimer, CanStartTimer);
        
        #endregion

        //Methods
        #region
        private bool CanAddFolders()
        {
            if (_backuptimer.IsEnabled)
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
            if (_backuptimer.IsEnabled)
                return false;
            
            return true;
        }
        private void RemoveFolder(object FolderInfo)
        {
            var Folder = FolderInfo as FolderModel;
            _folders.Remove(Folder);
        }
        #endregion


        private bool CanAddBackupFolder() // Target
        {
            if (_backuptimer.IsEnabled)
                return false;

            return true;
        }

        private void AddBackupFolder(object parameter)
        {
            var OpenDialog = new FolderBrowserDialog();
            if (OpenDialog.ShowDialog() == DialogResult.OK)
            {
                if(_folders.Any(x => x.FilePath == FolderModel.Target))
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
            TimeSpan ElapsedTime;
            if (_backuptimer.IsEnabled)
            {
                ElapsedTime = _backuptimer.Interval;
                _backuptimer.Stop();
                return;
            }
            _backuptimer.Start();
            BackupProgress();
        }

        private void BackupProgress()
        {
            Time.Add(new TimeModel { ElapsedTime = BackupTimer.Interval });
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
    }
}
