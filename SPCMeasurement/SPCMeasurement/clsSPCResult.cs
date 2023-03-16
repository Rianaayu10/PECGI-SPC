using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPCMeasurement
{
    public class clsSPCResult
    {
        public int SPCResultID { get; set; }
        public string FactoryCode { get; set; }
        public string ItemTypeCode { get; set; }
        public string LineCode { get; set; }
        public string ItemCheckCode { get; set; }
        public string ProdDate { get; set; }
        public string ShiftCode { get; set; }
        public int SequenceNo { get; set; }
        public string SubLotNo { get; set; }
        public string Remark { get; set; }
        public string RegisterUser { get; set; }
        public string RegisterNo { get; set; }
        public string Measure2nd { get; set; }
    }  
}
