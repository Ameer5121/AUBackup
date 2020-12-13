using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVMAUbackup.ViewModels;


namespace MVVMAUbackup.Models
{
    [Serializable]
    class FolderModel
    {
        private string _name;
        private string _filepath;
        public static string Target;

        public string Name
        {
            get => _name;
            set 
            {
                _name = value;
            }
        }
        public string FilePath
        {
            get => _filepath;
            set 
            {
                _filepath = value;
            }
        }
    }
}
