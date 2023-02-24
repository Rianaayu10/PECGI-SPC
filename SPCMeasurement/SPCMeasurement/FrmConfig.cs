using SPCMeasurement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace SPCMeasurement
{
    public partial class FrmConfig : Form
    {
        public string ls_Param;
        public FrmConfig()
        {
            InitializeComponent();
        }

        private void updatePorts()
        {
            // Retrieve the list of all COM ports on your Computer
            string[] ports = SerialPort.GetPortNames();
            cboCOM.Items.Clear();
            foreach (string port in ports)
            {
                cboCOM.Items.Add(port);
            }
        }

        private void uf_display()
        {
            clsConfigData cfgData = new clsConfigData();
            clsConfig cfg = new clsConfig();
            cfgData = cfg.uf_ReadConfig();

            if (cfgData != null)
            {
                cboCOM.Text = cfgData.COM_Port.Trim();
                cboBaud.Text = cfgData.COM_BaudRate.ToString();
                cboDataBits.Text = cfgData.COM_DataBits.Trim();
                cboParity.Text = cfgData.COM_Parity.Trim();
                cboStopBits.Text = cfgData.COM_StopBits.Trim();
                txtStableTime.Text = cfgData.COM_Stable.Trim();
                chkRTS.Checked = cfgData.COM_RTSEnable.Trim() == "1";

                txtSQLHost.Text = cfgData.SQL_Host.Trim();
                txtSQLPort.Text = cfgData.SQL_Port.ToString();
                txtSQLDB.Text = cfgData.SQL_Database.Trim();
                txtSQLDBTimeOut.Text = cfgData.SQL_DBTO.Trim();
                txtSQLCmdTimeOut.Text = cfgData.SQL_CmdTO.Trim();
                if (cfgData.SQL_AuthType.Trim() != "1")
                {
                    cboSQLAuth.SelectedIndex = 0;
                    txtSQLUser.Text = cfgData.SQL_UserID.Trim();
                    txtSQLUser.Enabled = true;
                    txtSQLPwd.Text = cfgData.SQL_Password.Trim();
                    txtSQLPwd.Enabled = true;
                }
                else
                {
                    cboSQLAuth.SelectedIndex = 0;
                    txtSQLUser.Text = "";
                    txtSQLUser.Enabled = false;
                    txtSQLPwd.Text = "";
                    txtSQLPwd.Enabled = false;
                }

            }
            else
            {
                cboCOM.Text = "";
                cboBaud.Text = "2400";
                cboDataBits.Text = "7";
                cboParity.Text = "None";
                cboStopBits.Text = "1";
                txtStableTime.Text = "2";

                txtSQLHost.Text = "";
                txtSQLPort.Text = "1433";
                txtSQLDB.Text = "";
                txtSQLUser.Text = "";
                txtSQLPwd.Text = "";
                txtSQLDBTimeOut.Text = "300";
                txtSQLCmdTimeOut.Text = "300";
                cboSQLAuth.SelectedIndex = 0;
                txtSQLUser.Enabled = true;
                txtSQLPwd.Enabled = true;
            }

        }
        private void uf_Init()
        {
            cboCOM.Text = "";
            cboBaud.Text = "2400";
            cboDataBits.Text = "7";
            cboParity.Text = "None";
            cboStopBits.Text = "1";
            txtStableTime.Text = "2";

            txtSQLHost.Text = "";
            txtSQLPort.Text = "";
            txtSQLDB.Text = "";
            txtSQLUser.Text = "";
            txtSQLPwd.Text = "";
            txtSQLDBTimeOut.Text = "300";
            txtSQLCmdTimeOut.Text = "300";


            cboCOM.TabIndex = 0;
            cboBaud.TabIndex = 1;
            cboDataBits.TabIndex = 2;
            cboParity.TabIndex = 3;
            cboStopBits.TabIndex = 4;
            txtStableTime.TabIndex = 5;
            btnCOM.TabIndex = 6;

            txtSQLHost.TabIndex = 8;
            txtSQLPort.TabIndex = 9;
            txtSQLDB.TabIndex = 10;
            txtSQLUser.TabIndex = 11;
            txtSQLPwd.TabIndex = 12;
            txtSQLDBTimeOut.TabIndex = 13;
            txtSQLCmdTimeOut.TabIndex = 14;
            btnSQLTest.TabIndex = 15;


            btnApply.TabIndex = 19;
            btnClose.TabIndex = 20;


            cboSQLAuth.Items.Clear();
            cboSQLAuth.Items.Add("SQL Server Authentication");
            cboSQLAuth.Items.Add("Windows Authentication");
            cboSQLAuth.SelectedIndex = 0;
        }

        private bool uf_ValidasiAll()
        {
            bool bRet = false;

            if (cboCOM.Text.Trim() == "")
            {
                cboCOM.Focus();
                MessageBox.Show("Please Input valid COM Port !");
            }
            else if (cboBaud.Text.Trim() == "")
            {
                cboBaud.Focus();
                MessageBox.Show("Please Input valid COM Baud Rate  ( default 2400 ) !");
            }
            else if (cboDataBits.Text.Trim() == "")
            {
                cboDataBits.Focus();
                MessageBox.Show("Please Input valid COM DataBits !");
            }
            else if (cboParity.Text.Trim() == "")
            {
                cboParity.Focus();
                MessageBox.Show("Please Input valid COM Parity  !");
            }
            else if (cboStopBits.Text.Trim() == "")
            {
                cboStopBits.Focus();
                MessageBox.Show("Please Input valid COM StopBits !");
            }
            else if (txtStableTime.Text.Trim() == "")
            {
                txtStableTime.Focus();
                MessageBox.Show("Please Input valid Stable Time !");
            }
            else if (txtSQLHost.Text.Trim() == "")
            {
                txtSQLHost.Focus();
                MessageBox.Show("Please Input SQL Server Host !");
            }
            else if (cboSQLAuth.SelectedIndex == 0 && txtSQLUser.Text.Trim() == "")
            {
                txtSQLUser.Focus();
                MessageBox.Show("Please Input User ID !");
            }
            else if (cboSQLAuth.SelectedIndex == 0 && txtSQLPwd.Text.Trim() == "")
            {
                txtSQLPwd.Focus();
                MessageBox.Show("Please Input Password !");
            }            
            else
            {
                bRet = true;
            }

            return bRet;
        }

        

        private bool uf_ValidasiConnection()
        {
            bool bRet = false;

            if (txtSQLHost.Text.Trim() == "")
            {
                txtSQLHost.Focus();
                MessageBox.Show("Please Input Server Name !");
            }
            else if (cboSQLAuth.SelectedIndex == 0 && txtSQLUser.Text.Trim() == "")
            {
                txtSQLUser.Focus();
                MessageBox.Show("Please Input User ID !");
            }
            else if (cboSQLAuth.SelectedIndex == 0 && txtSQLPwd.Text.Trim() == "")
            {
                txtSQLPwd.Focus();
                MessageBox.Show("Please Input Password !");
            }            
            else
            {
                bRet = true;
            }

            return bRet;
        }

        private void txtPGPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
            //    (e.KeyChar != '.'))
            //{
            //    e.Handled = true;
            //}

            //// only allow one decimal point
            //if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            //{
            //    e.Handled = true;
            //}

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtSQLPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
            //    (e.KeyChar != '.'))
            //{
            //    e.Handled = true;
            //}

            //// only allow one decimal point
            //if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            //{
            //    e.Handled = true;
            //}

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtIFInterval_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
            //    (e.KeyChar != '.'))
            //{
            //    e.Handled = true;
            //}

            //// only allow one decimal point
            //if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            //{
            //    e.Handled = true;
            //}

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public bool uf_CekConSQLServer(string connectionString)
        {
            bool canConnect;
            var conn = new OleDbConnection(connectionString);
            try
            {

                conn.Open();

                MessageBox.Show("Database connection successful", "Test Connection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                canConnect = true;
            }
            catch (OleDbException sqlEx)
            {
                MessageBox.Show(sqlEx.Message, "Database Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                canConnect = false;
            }
            finally
            {
                conn.Close();
            }
            return canConnect;
        }

        private void btnSQLTest_Click(object sender, EventArgs e)
        {
            string sqlConstring = "";


            if (uf_ValidasiConnection() == true)
            {
                sqlConstring = "Provider=SQLOLEDB;"; //Data Source=" + txtSQLHost.Text.Trim() + ";Port=" + txtPGPort.Text.Trim() + ";Initial Catalog=" + txtSQLDB.Text.Trim() + "; User Id=" + txtSQLUser.Text.Trim() + ";Password=" + txtSQLPwd.Text.Trim() + ";";

                if (txtSQLPort.Text.Trim() != "1433")
                {
                    sqlConstring = sqlConstring + "Data Source=" + txtSQLHost.Text.Trim() + "," + txtSQLPort.Text.Trim() + ";";
                }
                else
                {
                    sqlConstring = sqlConstring + "Data Source=" + txtSQLHost.Text.Trim() + ";";
                }
                sqlConstring = sqlConstring + "Initial Catalog=" + txtSQLDB.Text.Trim() + ";";

                if (cboSQLAuth.SelectedIndex == 1)
                {
                    sqlConstring = sqlConstring + "Integrated Security=SSPI;";
                }
                else
                {
                    sqlConstring = sqlConstring + "User Id=" + txtSQLUser.Text.Trim() + ";Password=" + txtSQLPwd.Text.Trim() + ";";
                }

                if (txtSQLDBTimeOut.Text.Trim() == "")
                {
                    sqlConstring = sqlConstring + "Connection Timeout=300;";
                }
                else
                {
                    sqlConstring = sqlConstring + "Connection Timeout=" + txtSQLDBTimeOut.Text.Trim() + ";";
                }

                


                uf_CekConSQLServer(sqlConstring);
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (uf_ValidasiAll() == true)
            {

                clsEncryption fEnc = new clsEncryption();


                clsConfigData cfgData = new clsConfigData();
                cfgData.COM_Port = fEnc.Encrypt(cboCOM.Text.Trim());
                cfgData.COM_BaudRate = fEnc.Encrypt(cboBaud.Text.Trim());
                cfgData.COM_DataBits = fEnc.Encrypt(cboDataBits.Text.Trim());
                cfgData.COM_Parity = fEnc.Encrypt(cboParity.Text.Trim());
                cfgData.COM_StopBits = fEnc.Encrypt(cboStopBits.Text.Trim());
                cfgData.COM_Handshake = fEnc.Encrypt("0");
                if(chkRTS.Checked)
                {
                    cfgData.COM_RTSEnable = fEnc.Encrypt("1");
                } else
                {
                    cfgData.COM_RTSEnable = fEnc.Encrypt("0");
                }
                if (string.IsNullOrEmpty(txtStableTime.Text) || txtStableTime.Text == "0")
                {
                    cfgData.COM_Stable = fEnc.Encrypt("2");
                }
                else
                {
                    cfgData.COM_Stable = fEnc.Encrypt(txtStableTime.Text.Trim());
                }



                cfgData.SQL_Host = fEnc.Encrypt(txtSQLHost.Text.Trim());
                cfgData.SQL_Port = fEnc.Encrypt(txtSQLPort.Text.Trim());
                cfgData.SQL_Database = fEnc.Encrypt(txtSQLDB.Text.Trim());
                cfgData.SQL_AuthType = fEnc.Encrypt(cboSQLAuth.SelectedIndex.ToString());
                cfgData.SQL_UserID = fEnc.Encrypt(txtSQLUser.Text.Trim());
                cfgData.SQL_Password = fEnc.Encrypt(txtSQLPwd.Text.Trim());
                if (string.IsNullOrEmpty(txtSQLDBTimeOut.Text) || txtSQLDBTimeOut.Text == "0")
                {
                    cfgData.SQL_DBTO = fEnc.Encrypt("300");
                }
                else
                {
                    cfgData.SQL_DBTO = fEnc.Encrypt(txtSQLDBTimeOut.Text.Trim());
                }

                if (string.IsNullOrEmpty(txtSQLCmdTimeOut.Text) || txtSQLCmdTimeOut.Text == "0")
                {
                    cfgData.SQL_CmdTO = fEnc.Encrypt("720");
                }
                else
                {
                    cfgData.SQL_CmdTO = fEnc.Encrypt(txtSQLCmdTimeOut.Text.Trim());
                }



                clsConfig cfg = new clsConfig();
                if (cfg.Uf_WriteConfig(cfgData) == true)
                {
                    MessageBox.Show("Apply configuration successful", "Apply", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Apply configuration failed", "Apply", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void FrmConfig_Load(object sender, EventArgs e)
        {
            uf_Init();
            uf_display();
            
            if (string.IsNullOrEmpty(ls_Param) || ls_Param == "")
            {
                Console.WriteLine("No Parameter ");
            }
            else
            {
                MessageBox.Show(" Parameter >> " + ls_Param);
            }
        }

        private void txtMeasLimit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void chkViewPassSQL_CheckedChanged(object sender, EventArgs e)
        {
            txtSQLPwd.PasswordChar = chkViewPassSQL.Checked ? '\0' : '*';
        }


        private void txtCmdTimeOut_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtDBTimeOut_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtSQLDBTimeOut_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtSQLCmdTimeOut_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnCOM_Click(object sender, EventArgs e)
        {
            updatePorts();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void txtStableTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtStableTime_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
