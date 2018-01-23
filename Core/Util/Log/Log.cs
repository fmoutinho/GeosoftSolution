using Core.Util.Log.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Util.Log
{
    public class Log:ILog
    {
        string LOG_NAME = ConfigurationManager.AppSettings["LOG_NAME"]; //Nome da entrada no Visualizador de Eventos do Windows 
        string SOURCE = ConfigurationManager.AppSettings["SOURCE"]; //Nome da fonte (source) com que serão gravados os logs. 

        public Log()
        {
            //verifica se o log já existe, se não existe então cria;  
            if (EventLog.SourceExists(SOURCE) == false)
                EventLog.CreateEventSource(SOURCE, LOG_NAME);
        }

        public void WriteEntry(string input, EventLogEntryType entryType)
        {
            //grava o texto na fonte de logs com o nome que      definimos para a constante SOURCE.  
            EventLog.WriteEntry(SOURCE, input, entryType);
        }

        public void WriteEntry(string input)
        {
            //loga um simples evento com a categoria de informação.  
            WriteEntry(input, EventLogEntryType.Information);
        }

        public void WriteEntry(Exception ex)
        {
            //loga a ocorrência de uma excessão com a categoria de erro.  
            WriteEntry(ex.ToString(), EventLogEntryType.Error);
        }
    }
}
