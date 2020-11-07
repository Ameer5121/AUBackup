using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVMAUbackup.ViewModels;

namespace MVVMAUbackup.Models
{
    class FolderModel : ViewModelBase
    {
        private string _name;
        private string _filepath;
        public static string Target = null;

        public string Name
        {
            get => _name;
            set 
            {
                _name = value;
                OnPropertyChanged(); 
            }
        }
        public string FilePath
        {
            get => _filepath;
            set 
            {
                _filepath = value;
                OnPropertyChanged(); 
            }
        }
    }
}
