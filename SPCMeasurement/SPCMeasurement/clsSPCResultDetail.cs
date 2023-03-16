using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPCMeasurement
{
    public class clsSPCResultDetail
    {
        public int SPCResultID { get; set; }
        public int SeqNo { get; set; }
        public double Value { get; set; }
        public double Value1 { get; set; }
        public double Value2 { get; set; }

        public string Remark { get; set; }
        public string Judgement { get; set; }
        public string DeleteStatus { get; set; }
        public string RegisterUser { get; set; }
        public string RegisterNo { get; set; }
        public int ValueIndex { get; set; }
        public DateTime RegisterDate { get; set; }
    }

    public class clsSPCResultDetailDB
    {
        private static clsConfig cfg = new clsConfig();
        private static clsConfigData cfd = cfg.uf_ReadConfig();
        private static string constr = cfd.ConnectionString;

        public static int Insert(clsSPCResultDetail Detail)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_SPCResultDetail_Ins", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("SPCResultID", Detail.SPCResultID);
                cmd.Parameters.AddWithValue("Value", Detail.Value);
                if(Detail.Value1 > 0)
                {
                    cmd.Parameters.AddWithValue("Value1", Detail.Value1);
                } else if (Detail.Value2 > 0)
                {
                    cmd.Parameters.AddWithValue("Value2", Detail.Value2);
                }                               
                cmd.Parameters.AddWithValue("RegisterUser", Detail.RegisterUser);

                int i = cmd.ExecuteNonQuery();
                cmd.Dispose();
                return i;
            }
        }

        public static int Delete(int SPCResultID, string Measure2nd = "")
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string q;
                //if (Measure2nd == "1")
                //{
                //    q = "Delete spc_Result \n" +
                //        "from spc_Result R inner join spc_ResultTemp T \n" +
                //        "on T.FactoryCode = R.FactoryCode and T.ItemTypeCode = R.ItemTypeCode and T.LineCode = R.LineCode and T.ItemCheckCode = R.ItemCheckCode \n" +
                //        "and T.ProdDate = R.ProdDate and T.ShiftCode = R.ShiftCode and T.SequenceNo = R.SequenceNo \n" +
                //        "where T.SPCResultID = @SPCResultID	\n\n" +
                //        "Delete spc_ResultDetailTemp where SPCResultID = @SPCResultID \n\n" +
                //        "Delete spc_ResultTemp where SPCResultID = @SPCResultID \n\n";
                //} else
                //{
                //    q = "Delete spc_ResultDetail where SPCResultID = @SPCResultID\n" +
                //        "Delete spc_Result where SPCResultID = @SPCResultID\n";
                //}
                q = "Delete spc_ResultDetail where SPCResultID = @SPCResultID\n" +
                    "Delete spc_Result where SPCResultID = @SPCResultID\n";
                SqlCommand cmd = new SqlCommand(q, con);
                cmd.Parameters.AddWithValue("SPCResultID", SPCResultID);
                int i = cmd.ExecuteNonQuery();
                cmd.Dispose();
                return i;
            }
        }

        public static int CountValue2(int SPCResultID)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string q = "select count(*) from spc_ResultDetail where SPCResultID = @SPCResultID and Value1 is not Null and Value2 is Null";
                SqlCommand cmd = new SqlCommand(q, con);
                cmd.Parameters.AddWithValue("SPCResultID", SPCResultID);
                int i = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Dispose();
                return i;
            }
        }

        public static List<clsSPCResultDetail> GetList(string FactoryCode, string ItemTypeCode, string Line, string ItemCheckCode, string ProdDate, string Shift, int Sequence, int VerifiedOnly, string RegisterNo)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_SPCResultDetail", con);
                cmd.Parameters.AddWithValue("FactoryCode", FactoryCode);
                cmd.Parameters.AddWithValue("Line", Line);
                cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode);
                cmd.Parameters.AddWithValue("ProdDate", ProdDate);
                cmd.Parameters.AddWithValue("ShiftCode", Shift);
                cmd.Parameters.AddWithValue("SequenceNo", Sequence);
                cmd.Parameters.AddWithValue("VerifiedOnly", VerifiedOnly);                
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader rd = cmd.ExecuteReader();
                List<clsSPCResultDetail> result = new List<clsSPCResultDetail>();
                while(rd.Read())
                {
                    if (rd["Value"] == System.DBNull.Value)
                    {
                        rd.Close();
                        break;
                    }
                    clsSPCResultDetail detail = new clsSPCResultDetail();
                    detail.SPCResultID = Convert.ToInt32(rd["SPCResultID"]);
                    detail.SeqNo = Convert.ToInt32(rd["SeqNo"]);
                    detail.Value = Convert.ToDouble(rd["Value"]);
                    detail.Value1 = Convert.ToDouble(rd["Value1"]);
                    detail.Value2 = Convert.ToDouble(rd["Value2"]);
                    detail.DeleteStatus = rd["DeleteStatus"] + "";
                    detail.Judgement = rd["Judgement"] + "";
                    detail.Remark = rd["Remark"] + "";
                    detail.RegisterUser = rd["RegisterUser"].ToString();
                    detail.RegisterNo = rd["RegisterNo"].ToString();
                    if (rd["RegisterDate"] != System.DBNull.Value)
                    {
                        detail.RegisterDate = Convert.ToDateTime(rd["RegisterDate"]);
                    }                        
                    result.Add(detail);
                }
                rd.Close();
                return result;
            }
        }

    }
}
