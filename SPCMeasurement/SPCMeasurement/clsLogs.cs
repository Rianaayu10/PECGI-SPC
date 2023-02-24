using System;
using System.IO;

namespace SPCMeasurement
{
    class clsLogs
    {
        public class LogType
        {
            public LogType(string value) { Value = value; }

            public string Value { get; set; }

            public static LogType Process { get { return new LogType("Process"); } }
            public static LogType Trace { get { return new LogType("Trace"); } }
            public static LogType Debug { get { return new LogType("Debug"); } }
            public static LogType Info { get { return new LogType("Info"); } }
            public static LogType Warning { get { return new LogType("Warning"); } }
            public static LogType Error { get { return new LogType("Error"); } }
        }

        public void WriteLog(LogType pLogType, bool pDBStatus, string pProcessType, string pProcessName, string pFunction, string pErrMessage)
        {
            try
            {
                //if (pDBStatus == true)
                //{
                //    WriteToDB(clsLog.LogType.Error, pProcessType, pProcessName, pFunction, pErrMessage);
                //}
                WriteToFiles(clsLogs.LogType.Error, pProcessType, pProcessName, pFunction, pErrMessage);

            }

            catch
            {

            }
        }

        private void WriteToFiles(LogType pLogType, string pProcessType, string pProcessName, string pFunction, string pErrMessage)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Log\\" + pLogType.Value + "\\" + DateTime.Now.ToString("yyyy") + "\\" + DateTime.Now.ToString("MMM") + "\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string sFilename = path + DateTime.Now.ToString("yyyyMMdd") + '.' + pProcessName.Replace(" ", "") + ".log";

            StreamWriter objWriter;

            if (File.Exists(sFilename))
            {
                objWriter = new StreamWriter(sFilename, append: true);
                objWriter.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t" + pProcessType + "\t" + pProcessName + "\t" + pFunction + "\t" + pErrMessage, true);
            }
            else
            {
                objWriter = new StreamWriter(sFilename);
                objWriter.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t" + pProcessType + "\t" + pProcessName + "\t" + pFunction + "\t" + pErrMessage);
            }

            objWriter.Close();
            objWriter.Dispose();

        }
    }
}
