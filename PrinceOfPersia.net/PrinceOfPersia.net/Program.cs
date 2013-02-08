using System;
using System.Diagnostics;

namespace PrinceOfPersia
{
    static class Program
    {


        static string sSource;
        static string sLog;
        static string sEvent;


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            sSource = "PoP.net";
            sLog = "Application";
            sEvent = "Error Event";

            try
            {

            using (Game game = new Game())
            {
                game.Run();
            }


            }
            catch (Exception ex)
            {
                if (!EventLog.SourceExists(sSource))
                    EventLog.CreateEventSource(sSource, sLog);

                EventLog.WriteEntry(sSource, ex.ToString());
                EventLog.WriteEntry(sSource, ex.StackTrace.ToString(),
                EventLogEntryType.Error, 234);
            }
        }
    }
}

