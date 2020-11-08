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
    class HistoryViewModel : ViewModelBase
    {
        #region Constructor
        public HistoryViewModel()
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
        private void AddStatus()
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
            
        }
        #endregion
    }
}
