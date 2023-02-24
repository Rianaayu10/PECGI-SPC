﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using C1.Win.C1List;

namespace SPCMeasurement
{
    class clsProcessGroup
    {
    }

    class clsProcessGroupDB
    {
        private static clsConfig cfg = new clsConfig();
        private static clsConfigData cfd = cfg.uf_ReadConfig();
        private static string constr = cfd.ConnectionString;

        public static void FillCombo(C1.Win.C1List.C1Combo cbo, string UserID, string FactoryCode)
        {

            cbo.DataMode = DataModeEnum.Normal;
            using (SqlConnection cn = new SqlConnection(constr))
            {
                cn.Open();
                string q = "sp_SPC_FillCombo";
                SqlCommand cmd = new SqlCommand(q, cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("ComboType", 0);
                cmd.Parameters.AddWithValue("UserID", UserID);
                cmd.Parameters.AddWithValue("FactoryCode", FactoryCode);
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
                cbo.ValueMember = "ProcessGroup";
                cbo.DisplayMember = "ProcessGroupName";
                cbo.LimitToList = true;
                if (dt.Rows.Count == 1)
                {
                    cbo.SelectedIndex = 0;
                }
            }
        }
    }
}