using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVMAUbackup.ViewModels;

namespace MVVMAUbackup.Models
{
    class StatusModel : ViewModelBase
    {
        private TimeSpan _elapsedTime;
        private DateTime _dateFinished;
        private bool _isBackupProcessRunning;

        public TimeSpan ElapsedTime
        {
            get => _elapsedTime;
            set
            {
                _elapsedTime = value;
                OnPropertyChanged();
            }
        }
        public DateTime DateFinished
        {
            get => _dateFinished;
            set
            {
                _dateFinished = value;
                OnPropertyChanged();
            }
        }
        public bool IsBackupProcessRunning
        {
            get => _isBackupProcessRunning;
            set
            {
                _isBackupProcessRunning = value;
                OnPropertyChanged();
            }
        }
    }
}
