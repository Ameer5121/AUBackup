using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVMAUbackup.ViewModels;

namespace MVVMAUbackup.Models
{
    class TimeModel : ViewModelBase
    {
        private TimeSpan _elapsedtime;
        private DateTime _datefinished;

        public TimeSpan ElapsedTime
        {
            get => _elapsedtime;
            set
            {
                _elapsedtime = value;
                OnPropertyChanged();
            }
        }
        public DateTime DateFinished
        {
            get => _datefinished;
            set
            {
                _datefinished = value;
                OnPropertyChanged();
            }
        }
    }
}
