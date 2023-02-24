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
    class clsLine
    {
    }

    class clsLineDB
    {
        private static clsConfig cfg = new clsConfig();
        private static clsConfigData cfd = cfg.uf_ReadConfig();
        private static string constr = cfd.ConnectionString;

        public static void FillCombo(C1.Win.C1List.C1Combo cbo, string UserID, string FactoryCode, string ProcessCode)
        {

            cbo.DataMode = DataModeEnum.Normal;
            using (SqlConnection cn = new SqlConnection(constr))
            {
                cn.Open();
                string q;
                q = "select distinct L.FactoryCode, L.ProcessCode, L.LineCode, L.LineCode + ' - ' + L.LineName as LineName \n";
                q = q +
                    "from MS_Line L inner join spc_ItemCheckByType I \n" +
                    "on L.FactoryCode = I.FactoryCode and L.LineCode = I.LineCode \n" +
                    "inner join spc_UserLine P on L.LineCode = P.LineCode \n" +
                    "where P.UserID = @UserID and P.AllowShow = 1 \n" +
                    "and I.ActiveStatus = 1 \n";
                if (FactoryCode!= "")
                {
                    q = q + "and L.FactoryCode = @FactoryCode ";
                }

                if (ProcessCode != "") {
                    q = q + "and L.ProcessCode = @ProcessCode ";
                }
                q = q + "order by LineCode";
                SqlCommand cmd = new SqlCommand(q, cn);
                cmd.Parameters.AddWithValue("ComboType", 2);
                cmd.Parameters.AddWithValue("UserID", UserID);
                cmd.Parameters.AddWithValue("FactoryCode", FactoryCode);
                cmd.Parameters.AddWithValue("ProcessCode", ProcessCode);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cbo.DataSource = dt;
                cbo.ColumnHeaders = false;
                cbo.DataMode = DataModeEnum.Normal;
                cbo.ItemHeight = 18;
                cbo.MaxDropDownItems = 10;
                cbo.Splits[0].DisplayColumns[0].Visible = false;
                cbo.Splits[0].DisplayColumns[1].Visible = false;
                cbo.Splits[0].DisplayColumns[2].Visible = false;
                cbo.ExtendRightColumn = true;
                cbo.DropdownPosition = C1.Win.C1List.DropdownPositionEnum.LeftDown;
                cbo.ValueMember = "LineCode";
                cbo.DisplayMember = "LineName";
                cbo.LimitToList = true;
            }
        }
    }
}
