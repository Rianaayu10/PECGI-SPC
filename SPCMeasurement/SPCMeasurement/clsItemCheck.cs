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
    class clsItemCheck
    {
        public string ItemCheckCode { get; set; }
        public string ItemCheck { get; set; }
        public string PrevItemCheck { get; set; }
        public string PrevValue { get; set; }
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
                q = "select I.ItemCheckCode, I.ItemCheckCode + ' - ' + I.ItemCheck ItemCheck, " +
                    "isnull(Measure2Cls, '0') Measure2Cls, " +
                    "isnull(T.PrevItemCheck, '') PrevItemCheck, " + 
                    "case when isnull(T.PrevItemCheck, '') = '' then 0 " +
                    "when isnull(T.PrevValue, '') = '1' then 1 " +
                    "else 2 end PrevValue " +
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
                cbo.Splits[0].DisplayColumns[3].Visible = false;
                cbo.Splits[0].DisplayColumns[4].Visible = false;
                cbo.ExtendRightColumn = true;                
                cbo.DropdownPosition = C1.Win.C1List.DropdownPositionEnum.LeftDown;
                cbo.ValueMember = "ItemCheckCode";
                cbo.DisplayMember = "ItemCheck";
                cbo.LimitToList = true;
            }
        }

        public static List<clsItemCheck> GetPrevItemChek(string FactoryCode, string ItemTypeCode, string Line, string ItemCheckCode, string ProdDate, string Shift, int Sequence, string UserID)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_SPC_GetPrevItemCheck", con);
                cmd.Parameters.AddWithValue("FactoryCode", FactoryCode);
                cmd.Parameters.AddWithValue("Line", Line);
                cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode);
                cmd.Parameters.AddWithValue("ProdDate", ProdDate);
                cmd.Parameters.AddWithValue("ShiftCode", Shift);
                cmd.Parameters.AddWithValue("SequenceNo", Sequence);
                cmd.Parameters.AddWithValue("UserID", UserID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader rd = cmd.ExecuteReader();
                List<clsItemCheck> result = new List<clsItemCheck>();
                while (rd.Read())
                {
                    clsItemCheck item = new clsItemCheck();
                    item.ItemCheckCode = rd["ItemCheckCode"].ToString();
                    item.ItemCheck = rd["ItemCheck"].ToString();
                    result.Add(item);
                }
                rd.Close();
                return result;
            }
        }

        public static clsItemCheck GetItemChek(string ItemCheckCode)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string q = "select T.*, I.ItemCheck from spc_ItemCheckByType T inner join spc_ItemCheckMaster I on T.ItemCheckCode = I.ItemCheckCode where T.ItemCheckCode = @ItemCheckCode";
                SqlCommand cmd = new SqlCommand(q, con);
                cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if(dt.Rows.Count > 0)
                {
                    clsItemCheck item = new clsItemCheck();
                    item.ItemCheckCode = dt.Rows[0]["ItemCheckCode"].ToString();
                    item.PrevItemCheck = dt.Rows[0]["PrevItemCheck"] + "";
                    item.PrevValue = dt.Rows[0]["PrevValue"] + "";
                    return item;
                } else
                {
                    return null;
                }
            }
        }
    }
}
