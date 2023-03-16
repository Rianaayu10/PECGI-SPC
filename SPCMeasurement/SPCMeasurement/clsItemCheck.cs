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
    class clsItemCheck
    {
    }

    class clsItemCheckDB
    {
        private static clsConfig cfg = new clsConfig();
        private static clsConfigData cfd = cfg.uf_ReadConfig();
        private static string constr = cfd.ConnectionString;

        public static void FillCombo(C1.Win.C1List.C1Combo cbo, string FactoryCode, string LineCode, string ItemTypeCode)
        {

            cbo.DataMode = DataModeEnum.Normal;
            using (SqlConnection cn = new SqlConnection(constr))
            {
                cn.Open();
                string q;
                q = "select I.ItemCheckCode, I.ItemCheckCode + ' - ' + I.ItemCheck ItemCheck, isnull(Measure2Cls, '0') Measure2Cls " +
                    "from spc_ItemCheckMaster I inner join spc_ItemCheckByType T on I.ItemCheckCode = T.ItemCheckCode " +
                    "where T.ItemCheckCode is not Null ";
                if (FactoryCode != "") {
                    q = q + "and T.FactoryCode = @FactoryCode ";
                } 
                if (ItemTypeCode != "")
                {
                    q = q + "and ItemTypeCode = @ItemTypeCode ";
                }
                if (LineCode != "") { }
                {
                    q = q + "and LineCode = @LineCode ";
                }
                q = q + "and T.ActiveStatus = '1' ";
                SqlCommand cmd = new SqlCommand(q, cn);
                cmd.Parameters.AddWithValue("FactoryCode", FactoryCode);
                cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode);
                cmd.Parameters.AddWithValue("LineCode", LineCode);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cbo.DataSource = dt;
                int n = dt.Rows.Count;
                cbo.ColumnHeaders = false;
                cbo.DataMode = DataModeEnum.Normal;
                cbo.ItemHeight = 18;
                cbo.MaxDropDownItems = 10;
                cbo.Splits[0].DisplayColumns[0].Visible = false;
                cbo.Splits[0].DisplayColumns[2].Visible = false;
                cbo.ExtendRightColumn = true;                
                cbo.DropdownPosition = C1.Win.C1List.DropdownPositionEnum.LeftDown;
                cbo.ValueMember = "ItemCheckCode";
                cbo.DisplayMember = "ItemCheck";
                cbo.LimitToList = true;
            }
        }
    }
}
