using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMAUbackup.Models
{
    public enum BackupProgress
    {
        NotStarted,
        InProgress,
        Paused,
        Finished
    }
}
