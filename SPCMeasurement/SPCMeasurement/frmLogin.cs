using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPCMeasurement
{
    public partial class frmLogin : Form
    {
        public frmLogin(string pArgs)
        {
            InitializeComponent();
            lblArg.Text = pArgs;
            lblVer.Text = "ver " + Application.ProductVersion;
        }

        private bool ValidConnection()
        {
            clsConfig cfg = new clsConfig();
            clsConfigData cfd = cfg.uf_ReadConfig();
            string constr = cfd.ConnectionString;
            string errmsg = cfg.TestConnection(constr);
            if(errmsg != "")
            {
                MessageBox.Show(errmsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void ShowConfig()
        {
            FrmConfig frmCfg = new FrmConfig();
            frmCfg.ls_Param = "";
            frmCfg.ShowDialog();
            frmCfg.Dispose();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string userID = txtUser.Text.Trim();
            if(ValidConnection() == false)
            {
                Cursor.Current = Cursors.Default;                
                return;
            }
            Cursor.Current = Cursors.Default;
            clsUser User = clsUserDB.GetData(userID);
            if(User == null)
            {
                txtUser.Focus();
                txtUser.SelectAll();
                MessageBox.Show("Invalid User ID or password", "Login", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            } else if (User.Password != txtPassword.Text)
            {
                txtPassword.Focus();
                txtPassword.SelectAll();
                MessageBox.Show("Invalid User ID or password", "Login", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            bool allowUpdate = clsUserDB.AllowUpdate(userID, "B020");
            if(!allowUpdate)
            {
                MessageBox.Show("You do not have privilege to input result", "Login", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            frmMeasurement frm = new frmMeasurement(userID, this, User.FactoryCode, "","","","","","");
            this.Hide();
            txtPassword.Text = "";
            frm.Show();
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            txtPassword.SelectAll();
            this.AcceptButton = btnLogin;
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            this.AcceptButton = null;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            if(Environment.MachineName == "TOS56-ARI")
            {
                txtUser.Text = "admintos";
                txtPassword.Text = "Pecgi22";
            }
        }

        private void frmLogin_Shown(object sender, EventArgs e)
        {
            txtUser.Focus();
        }

        private void txtUser_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (txtUser.Text.Trim() != "")
            {
                if(e.KeyChar == '\r')
                {
                    e.Handled = true;
                    txtPassword.Focus();
                }
            }
        }

        private void frmLogin_Activated(object sender, EventArgs e)
        {
            txtUser.Focus();
        }

        private void txtUser_Enter(object sender, EventArgs e)
        {
            txtUser.SelectAll();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            ShowConfig();
            txtUser.Focus();
        }
    }
}
