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
    class clsItemType
    {
    }

    class clsItemTypeDB
    {
        private static clsConfig cfg = new clsConfig();
        private static clsConfigData cfd = cfg.uf_ReadConfig();
        private static string constr = cfd.ConnectionString;

        public static void FillCombo(C1.Win.C1List.C1Combo cbo, string UserID, string FactoryCode, string LineCode)
        {

            cbo.DataMode = DataModeEnum.Normal;
            using (SqlConnection cn = new SqlConnection(constr))
            {
                cn.Open();
                string q = "select distinct I.ItemTypeCode, M.Description " +
                    "From spc_ItemCheckByType I inner Join MS_ItemType M on I.ItemTypeCode = M.ItemTypeCode  " +
                    "inner join spc_UserLine UL on I.LineCode = UL.LineCode and UL.AllowUpdate = 1 " +
                    "inner join MS_ItemDetail D on M.Description = D.Item_Code and I.LineCode = D.LineCode and I.FactoryCode = D.FactoryCode " +
                    "Where I.FactoryCode = @FactoryCode and ActiveStatus = 1 " +
                    "and UL.UserID = @UserID and I.LineCode = @LineCode ";


                q = "SELECT DISTINCT A.ItemTypeCode, A.Description \n" +
                    "From MS_ItemType A \n" +
                    "INNER JOIN MS_TypeDetail TD ON A.ItemTypeCode = TD.ItemTypeCode \n" +
                    "INNER Join spc_ItemCheckByType B ON A.ItemTypeCode = B.ItemTypeCode And B.FactoryCode = TD.FactoryCode And B.ActiveStatus = '1' \n" +
                    "INNER Join spc_UserLine C ON B.LineCode = C.LineCode And C.AppID = 'SPC' AND C.AllowShow = '1'  \n" +
                    "AND C.UserID = @UserID \n" +
                    "WHERE TD.FactoryCode = @FactoryCode AND TD.LineCode = @LineCode \n" +
                    "GROUP BY A.ItemTypeCode, A.Description \n" +
                    "ORDER BY Description ";
                SqlCommand cmd = new SqlCommand(q, cn);
                cmd.Parameters.AddWithValue("FactoryCode", FactoryCode);
                cmd.Parameters.AddWithValue("UserID", UserID);
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
                cbo.ExtendRightColumn = true;
                cbo.DropdownPosition = C1.Win.C1List.DropdownPositionEnum.LeftDown;
                cbo.ValueMember = "ItemTypeCode";
                cbo.DisplayMember = "Description";
                cbo.LimitToList = true;
            }
        }
    }
}
