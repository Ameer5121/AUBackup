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
    class StatusViewModel : ViewModelBase
    {
        #region Constructor
        public StatusViewModel()
        {
            _time = new ObservableCollection<StatusModel>()
            {
                new StatusModel{
                    ElapsedTime = FolderViewModel.BackupTimer.Interval,
                    DateFinished = null,
                    BackupStatus = nameof(BackupProgress.NotStarted) 
                }
            };
            
            _updateProgress = new DispatcherTimer();
            _updateProgress.Interval = TimeSpan.FromSeconds(1);
            _updateProgress.Tick += UpdateStatus;
        }
        #endregion

        #region Fields
        private ObservableCollection<StatusModel> _time;
        private DispatcherTimer _updateProgress;
        #endregion

        
        #region Properties
        public ObservableCollection<StatusModel> Time => _time;
        public DispatcherTimer UpdateProgress => _updateProgress;
        #endregion

        #region Methods
        public void AddStatus()
        {
            _time.Add(
                 new StatusModel
                 {
                     ElapsedTime = FolderViewModel.BackupTimer.Interval,
                     DateFinished = null,
                     BackupStatus = nameof(BackupProgress.InProgress)
                 });
        }
        private void UpdateStatus(object sender, EventArgs e)
        {
            var CurrentStatus = _time.Last();
            if(CurrentStatus.ElapsedTime != TimeSpan.FromSeconds(0))
            {
                CurrentStatus.ElapsedTime -= TimeSpan.FromSeconds(1);
            }
            else
            {
                CurrentStatus.DateFinished = DateTime.Now;
                CurrentStatus.BackupStatus = nameof(BackupProgress.Finished);
                AddStatus();
            }
        }
        #endregion
    }
}
