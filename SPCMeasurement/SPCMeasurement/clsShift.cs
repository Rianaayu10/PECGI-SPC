using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using C1.Win.C1List;

namespace SPCMeasurement
{
    class clsShift
    {
    }

    class clsShiftDB
    {
        private static clsConfig cfg = new clsConfig();
        private static clsConfigData cfd = cfg.uf_ReadConfig();
        private static string constr = cfd.ConnectionString;

        public static void FillCombo(C1.Win.C1List.C1Combo cbo, string FactoryCode, string ItemTypeCode, string LineCode, string ItemCheckCode)
        {

            cbo.DataMode = DataModeEnum.Normal;
            using (SqlConnection cn = new SqlConnection(constr))
            {
                cn.Open();
                string q;
                q = "select distinct rtrim(F.ShiftCode) ShiftCode " +
                    "From spc_ItemCheckByType T inner Join spc_MS_Frequency F on T.FrequencyCode = F.FrequencyCode " +
                    "where FactoryCode = @FactoryCode and ItemTypeCode = @ItemTypeCode and LineCode = @LineCode and ItemCheckCode = @ItemCheckCode  " +
                    "and T.ActiveStatus = 1 and F.ActiveStatus = 1  ";
                SqlCommand cmd = new SqlCommand(q, cn);
                cmd.Parameters.AddWithValue("FactoryCode", FactoryCode);
                cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode);
                cmd.Parameters.AddWithValue("LineCode", LineCode);
                cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode);                
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cbo.DataSource = dt;
                int n = dt.Rows.Count;
                cbo.ColumnHeaders = false;
                cbo.DataMode = DataModeEnum.Normal;
                cbo.ItemHeight = 18;                
                cbo.ExtendRightColumn = true;
                cbo.Splits[0].DisplayColumns[0].Width = 60;
                cbo.DropdownPosition = C1.Win.C1List.DropdownPositionEnum.LeftDown;
                cbo.ValueMember = "ShiftCode";
                cbo.DisplayMember = "ShiftCode";
                cbo.LimitToList = true;
            }
        }
    }
}
