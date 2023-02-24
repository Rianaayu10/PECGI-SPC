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
    class clsFactory
    {
    }

    class clsFactoryDB
    {
        private static clsConfig cfg = new clsConfig();
        private static clsConfigData cfd = cfg.uf_ReadConfig();
        private static string constr = cfd.ConnectionString;

        public static void FillCombo(C1.Win.C1List.C1Combo cbo)
        {

            cbo.DataMode = DataModeEnum.Normal;
            using(SqlConnection cn = new SqlConnection(constr))
            {
                cn.Open();
                string q = "select FactoryCode, FactoryName from MS_Factory ";
                SqlCommand cmd = new SqlCommand(q, cn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cbo.DataSource = dt;
                cbo.ColumnHeaders = false;
                cbo.DataMode = DataModeEnum.Normal;
                cbo.ItemHeight = 18;
                cbo.Splits[0].DisplayColumns[0].Visible = false;
                cbo.Splits[0].DisplayColumns[1].Width = 60;
                cbo.ExtendRightColumn = true;                
                cbo.DropdownPosition = C1.Win.C1List.DropdownPositionEnum.LeftDown;
                cbo.ValueMember = "FactoryCode";
                cbo.DisplayMember = "FactoryName";
                cbo.LimitToList = true;
            }
        }
    }
}
