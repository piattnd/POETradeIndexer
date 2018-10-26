using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Collections;

namespace POETradeIndexer
{
    public class poe_logger
    {
        private static string sSource = "POE Indexer";
        private static string sLog = "Application";
        private string logDir;
        private string logFile;
        public StreamWriter file { get; private set; }


        public poe_logger()
        {
            logDir = Environment.GetEnvironmentVariable("tmp");
            logFile = Path.Combine(logDir, "poeIndexLog.txt");
            file = new StreamWriter(logFile);
        }
        public static void logError(string message)
        {
            prepareEventLog();
            EventLog.WriteEntry(sSource, message, EventLogEntryType.Error);
        }

        public static void logInfo(string message)
        {
            prepareEventLog();
            EventLog.WriteEntry(sSource, message, EventLogEntryType.Information);
        }

        private static void prepareEventLog()
        {
            if (!EventLog.SourceExists(sSource))
                EventLog.CreateEventSource(sSource,sLog);
        }

        public void logInfoToText(string message)
        {
            file.WriteLine(message);
        }
    }
}
