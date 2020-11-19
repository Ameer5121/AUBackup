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
using System.Runtime.Serialization;

namespace MVVMAUbackup.ViewModels
{
    [Serializable]
    class StatusViewModel : ISerializable
    {
        
        #region Constructor
        public StatusViewModel()
        {
            _statuses = new ObservableCollection<StatusModel>()
            {
                new StatusModel{
                    ElapsedTime = FolderViewModel.BackupTimer.Interval,
                    DateFinished = null,
                    BackupStatus = nameof(BackupProgress.NotStarted) 
                }
            };
            _updateProgress = new DispatcherTimer();
            _updateProgress.Interval = TimeSpan.FromSeconds(1);
            _updateProgress.Tick += UpdateElapsedTime;
        }
        #endregion

        #region Fields
        private ObservableCollection<StatusModel> _statuses;
        private DispatcherTimer _updateProgress;
        #endregion

        
        #region Properties
        public ObservableCollection<StatusModel> Statuses => _statuses;
        public DispatcherTimer UpdateProgress => _updateProgress;
        #endregion

        #region Methods
        private void AddStatus()
        {
            _statuses.Insert(0,
                 new StatusModel
                 {
                     ElapsedTime = FolderViewModel.BackupTimer.Interval,
                     DateFinished = null,
                     BackupStatus = nameof(BackupProgress.InProgress)
                 });
        }
        private void UpdateElapsedTime(object sender, EventArgs e)
        {
            var CurrentStatus = _statuses.First();
            if(CurrentStatus.ElapsedTime != TimeSpan.FromSeconds(0))
            {
                CurrentStatus.ElapsedTime -= TimeSpan.FromSeconds(1);
            }
        }

        public void PauseProcess()
        {
            var CurrentStatus = _statuses.First();
            CurrentStatus.BackupStatus = nameof(BackupProgress.Paused);
            _updateProgress.Stop();
        }
        public void StartProcess()
        {
            var CurrentStatus = _statuses.First();
            CurrentStatus.BackupStatus = nameof(BackupProgress.InProgress);
            _updateProgress.Start();
        }

        public void UpdateFinishedProcess()
        {
            var CurrentStatus = _statuses.First();
            CurrentStatus.DateFinished = DateTime.Now;
            CurrentStatus.BackupStatus = nameof(BackupProgress.Finished);
            AddStatus();
        }
        #endregion


        #region Serialization
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Statuses", _statuses);
        }

        public StatusViewModel(SerializationInfo info, StreamingContext context) : this()
        {
            _statuses = (ObservableCollection<StatusModel>)info.GetValue("Statuses", typeof(ObservableCollection<StatusModel>));

        }
        #endregion
    }
}
