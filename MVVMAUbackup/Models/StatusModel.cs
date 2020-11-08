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
        #region Constructor
        public StatusModel()
        {
            _elapsedTime = new TimeSpan();
        }
        #endregion

        #region Fields
        private TimeSpan _elapsedTime;
        private DateTime? _dateFinished;
        private string _backupStatus;
        #endregion

        #region Properties
        public DateTime? DateFinished
        {
            get => _dateFinished;
            set
            {
                _dateFinished = value;
                OnPropertyChanged();
            }
        }
        public TimeSpan ElapsedTime
        {
            get => _elapsedTime;
            set
            {
                _elapsedTime = value;
                OnPropertyChanged();
            }
        }
        public string BackupStatus
        {
            get => _backupStatus;
            set
            {
                _backupStatus = value;
                OnPropertyChanged();
            }
        }
        #endregion

    }
}
