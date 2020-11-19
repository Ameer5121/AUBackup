using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace MVVMAUbackup.ViewModels
{
    [Serializable]
    class ViewModelBase : INotifyPropertyChanged
    {
   
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void Exit() { Environment.Exit(0); }

    }
}
