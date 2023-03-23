using C1.Win.C1List;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPCMeasurement
{
    class clsDevice
    {
        public string FactoryCode { get; set; }
        public string RegistrationNo { get; set; }
        public string Description { get; set; }
        public string ToolName { get; set; }
        public string ToolFunction { get; set; }
        public string ActiveStatus { get; set; }
    }

    class clsDeviceDB
    {
        private static clsConfig cfg = new clsConfig();
        private static clsConfigData cfd = cfg.uf_ReadConfig();
        private static string constr = cfd.ConnectionString;

        public static void FillCombo(C1.Win.C1List.C1Combo cbo, string FactoryCode, string ItemTypeCode, string LineCode, string ItemCheckCode)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string q;
                q = "select D.RegistrationNo, D.Description, \n" +
                    "D.BaudRate, D.DataBits, Case D.Parity when '0' then 'None' when '1' then 'Even' when '2' then 'Odd' else 'Both' end Parity, \n" +
                    "D.StopBits, D.StableCondition, D.GetResultData, D.Port, isnull(D.EnableRts, '0') EnableRTS, isnull(D.Command, '') Command, isnull(FlowControl, '') FlowControl, isnull(D.EnableDTR, '0') EnableDTR " +
                    "from spc_MS_Device D inner join spc_ItemCheckByType I on D.RegistrationNo = I.RegistrationNo and D.FactoryCode = I.FactoryCode  " +
                    "where D.FactoryCode = @FactoryCode and I.ItemTypeCode = @ItemTypeCode and I.LineCode = @LineCode and I.ItemCheckCode = @ItemCheckCode and D.ActiveStatus = '1'";

                SqlCommand cmd = new SqlCommand(q, con);
                cmd.Parameters.AddWithValue("FactoryCode", FactoryCode);
                cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode);
                cmd.Parameters.AddWithValue("LineCode", LineCode);
                cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode);                
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cbo.DataSource = dt;
                cbo.ColumnHeaders = false;
                cbo.DataMode = DataModeEnum.Normal;
                cbo.ItemHeight = 18;
                cbo.MaxDropDownItems = 10;
                cbo.Splits[0].DisplayColumns[0].Width = 80;
                for(int i = 2; i < cbo.Splits[0].DisplayColumns.Count; i++)
                {
                    cbo.Splits[0].DisplayColumns[i].Visible = false;
                }
                cbo.DropDownWidth = 230;
                cbo.ExtendRightColumn = true;
                cbo.DropdownPosition = C1.Win.C1List.DropdownPositionEnum.LeftDown;
                cbo.ValueMember = "RegistrationNo";
                cbo.DisplayMember = "RegistrationNo";
                cbo.LimitToList = true;
            }
        }
    }
}
