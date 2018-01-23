using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Util.Log.Interface
{
    public interface ILog
    {
        void WriteEntry(string input, EventLogEntryType entryType);

        void WriteEntry(string input);

        void WriteEntry(Exception ex);
    }
}
