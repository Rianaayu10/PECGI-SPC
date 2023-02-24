using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPCMeasurement
{
    public class clsConfigData
    {
        public string COM_Port { get; set; }
        public string COM_BaudRate { get; set; }
        public string COM_DataBits { get; set; }
        public string COM_Parity { get; set; }
        public string COM_StopBits { get; set; }
        public string COM_Handshake { get; set; }

        public string COM_RTSEnable { get; set; }
        public string COM_Stable { get; set; }

        public string SQL_Host { get; set; }
        public string SQL_Port { get; set; }
        public string SQL_Database { get; set; }
        public string SQL_AuthType { get; set; }

        public string SQL_UserID { get; set; }
        public string SQL_Password { get; set; }
        public string SQL_DBTO { get; set; }
        public string SQL_CmdTO { get; set; }

        public string Meas_Interval { get; set; }

        public string Meas_Data_Interval { get; set; }
        public string RT_Interval { get; set; }

        public string RT_Data_Interval { get; set; }
        public string ConnectionString { get; set; }
    }
}
