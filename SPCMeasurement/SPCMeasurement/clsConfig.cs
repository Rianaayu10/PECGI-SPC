using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using IniParser;
using IniParser.Model;
using System.Data.SqlClient;

namespace SPCMeasurement
{
    public class clsConfig
    {
        string ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini");

        public string TestConnection(string constr)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();
                    con.Close();
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public bool Uf_WriteConfig(clsConfigData cData) 
        {
            //clsConfigData cData = new clsConfigData();
            bool ret = false;
            try
            {
                var parser = new FileIniDataParser();
                IniData cfgdata = new IniData(); //= parser.ReadFile(ConfigPath);

                //COM Serials 
                cfgdata["COM"]["Port"] = cData.COM_Port.Trim();
                cfgdata["COM"]["BaudRate"] = cData.COM_BaudRate.ToString();
                cfgdata["COM"]["DataBits"] = cData.COM_DataBits.Trim();
                cfgdata["COM"]["Parity"] = cData.COM_Parity.Trim();
                cfgdata["COM"]["StopBits"] = cData.COM_StopBits.Trim();
                cfgdata["COM"]["Handshake"] = cData.COM_Handshake.Trim();
                cfgdata["COM"]["StableTime"] = cData.COM_Stable.Trim();
                cfgdata["COM"]["RTSEnable"] = cData.COM_RTSEnable.Trim();


                //SQL SERVER CONNECTION
                cfgdata["SPC"]["Host"] = cData.SQL_Host.Trim();
                cfgdata["SPC"]["Port"] = cData.SQL_Port.ToString();
                cfgdata["SPC"]["AuthType"] = cData.SQL_AuthType;
                cfgdata["SPC"]["Database"] = cData.SQL_Database.Trim();
                cfgdata["SPC"]["UserID"] = cData.SQL_UserID.Trim();
                cfgdata["SPC"]["Password"] = cData.SQL_Password.Trim();
                cfgdata["SPC"]["DBTO"] = cData.SQL_DBTO.Trim();
                cfgdata["SPC"]["CMDTO"] = cData.SQL_CmdTO.Trim();


                parser.WriteFile(ConfigPath, cfgdata);

                ret = true;
                return ret;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return ret;
            }
            finally
            {
                
            }
        }

        public clsConfigData uf_ReadConfig()
        {
            try
            {
                string dbAuth ="";
                if (!File.Exists(ConfigPath))
                {
                    //BUAT FILE KOSONG JIKA BELUM ADA FILE
                    var rparser = new FileIniDataParser();
                    IniData rdata = new IniData(); //= parser.ReadFile(ConfigPath);

                    //int PGPort = 5432;
                    int SQLPort = 1433;
                    //int IFInterval = 2;
                    clsEncryption fEnc = new clsEncryption();

                    //COM Serials 

                    rdata["COM"]["Port"] = fEnc.Encrypt("COM1");
                    rdata["COM"]["BaudRate"] = fEnc.Encrypt("2400");
                    rdata["COM"]["DataBits"] = fEnc.Encrypt("7");
                    rdata["COM"]["Parity"] = fEnc.Encrypt("None");
                    rdata["COM"]["StopBits"] = fEnc.Encrypt("1");
                    rdata["COM"]["Handshake"] = fEnc.Encrypt("0");
                    rdata["COM"]["RTSEnable"] = fEnc.Encrypt("0");
                    rdata["COM"]["StableTime"] = fEnc.Encrypt("2");


                    //SQL SERVER CONNECTION
                    rdata["SPC"]["Host"] = fEnc.Encrypt("localhost");
                    rdata["SPC"]["Port"] = fEnc.Encrypt(SQLPort.ToString());
                    rdata["SPC"]["AuthType"] = fEnc.Encrypt("0");
                    rdata["SPC"]["Database"] = fEnc.Encrypt("master");
                    rdata["SPC"]["UserID"] = fEnc.Encrypt("sa");
                    rdata["SPC"]["Password"] = fEnc.Encrypt("password");
                    rdata["SPC"]["DBTO"] = fEnc.Encrypt("300");
                    rdata["SPC"]["CMDTO"] = fEnc.Encrypt("300");                    

                    rparser.WriteFile(ConfigPath, rdata);
                }


                clsEncryption fDec = new clsEncryption();
                clsConfigData cData = new clsConfigData();
                var parser = new FileIniDataParser();
                IniData cfgdata = parser.ReadFile(ConfigPath);

                //COM Serials 
                cData.COM_Port = fDec.Decrypt(cfgdata["COM"]["Port"]);
                cData.COM_BaudRate = fDec.Decrypt(cfgdata["COM"]["BaudRate"]);
                cData.COM_DataBits = fDec.Decrypt(cfgdata["COM"]["DataBits"]);
                cData.COM_Parity = fDec.Decrypt(cfgdata["COM"]["Parity"]);
                cData.COM_StopBits = fDec.Decrypt(cfgdata["COM"]["StopBits"]);
                cData.COM_Handshake = fDec.Decrypt(cfgdata["COM"]["Handshake"]);
                cData.COM_RTSEnable = fDec.Decrypt(cfgdata["COM"]["RTSEnable"]);
                cData.COM_Stable = fDec.Decrypt(cfgdata["COM"]["StableTime"]);

                //if (cfgdata["MEAS"]["CMDTO"] == null)
                //{
                //    cData.PG_CmdTO = "720";
                //}
                //else
                //{
                //    cData.PG_CmdTO = fDec.Decrypt(cfgdata["MEAS"]["CMDTO"]);
                //}



                //SQL SERVER CONNECTION
                cData.SQL_Host = fDec.Decrypt(cfgdata["SPC"]["Host"]);
                if (cfgdata["SPC"]["Port"] == null)
                {
                    cData.SQL_Port = "1433";
                }
                else
                {
                    cData.SQL_Port = fDec.Decrypt(cfgdata["SPC"]["Port"]);
                }

                dbAuth = cfgdata["SPC"]["AuthType"];
                if (dbAuth  == null)
                {
                    cData.SQL_AuthType = "0";
                    cData.SQL_UserID = fDec.Decrypt(cfgdata["SPC"]["UserID"]);
                    cData.SQL_Password = fDec.Decrypt(cfgdata["SPC"]["Password"]);
                }
                else if(fDec.Decrypt(dbAuth) == "0")
                {
                    cData.SQL_AuthType = "0";
                    cData.SQL_UserID = fDec.Decrypt(cfgdata["SPC"]["UserID"]);
                    cData.SQL_Password = fDec.Decrypt(cfgdata["SPC"]["Password"]);
                }
                else
                {
                    cData.SQL_AuthType = cfgdata["SPC"]["AuthType"];
                    cData.SQL_UserID = "";
                    cData.SQL_Password = "";
                }
                cData.SQL_Database = fDec.Decrypt(cfgdata["SPC"]["Database"]);

                if (cfgdata["SPC"]["DBTO"] == null)
                {
                    cData.SQL_DBTO = "300";
                }
                else
                {
                    cData.SQL_DBTO = fDec.Decrypt(cfgdata["SPC"]["DBTO"]);
                }
                if (cfgdata["SPC"]["CMDTO"] == null)
                {
                    cData.SQL_CmdTO = "720";
                }
                else
                {
                    cData.SQL_CmdTO = fDec.Decrypt(cfgdata["SPC"]["CMDTO"]);
                }

                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();                
                if(Program.pServer != "")
                {
                    builder.DataSource = Program.pServer;
                } else
                {
                    builder.DataSource = cData.SQL_Host;
                }
                if (Program.pDatabase != "")
                {
                    builder.InitialCatalog = Program.pDatabase;
                }
                else
                {
                    builder.InitialCatalog = cData.SQL_Database;
                }
                builder.IntegratedSecurity = cData.SQL_AuthType == "1";
                builder.UserID = cData.SQL_UserID;
                builder.Password = cData.SQL_Password;
                cData.ConnectionString = builder.ConnectionString;
                return cData;                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return null;
            }
            finally
            {

            }
            
        }

    }
}
