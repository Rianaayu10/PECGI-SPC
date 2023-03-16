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
    public partial class frmGetValue : Form
    {
        public string FactoryCode = "";
        public string ItemType = "";
        public string Line = "";
        public string ItemCheckCode = "";
        public string ItemCheckCodeFrom = "";
        public string Shift = "";
        public string ProdDate = "";
        public int Sequence;
        public string UserID = "";
            
        public frmGetValue()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if(grid.Row < grid.Rows.Fixed)
            {
                MessageBox.Show("Please select Item Check!", "Select Item Check", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                DialogResult = DialogResult.None;
            }
            else
            {
                ItemCheckCodeFrom = grid[grid.Row, 1].ToString();
                DialogResult = DialogResult.OK;
            }            
        }

        private void frmGetValue_Load(object sender, EventArgs e)
        {
            
        }

        private void RefreshData()
        {
            List<clsItemCheck> result = clsItemCheckDB.GetPrevItemChek(FactoryCode, ItemType, Line, ItemCheckCode, ProdDate, Shift, Sequence, UserID);
            grid.DataSource = result;
            btnOK.Enabled = result.Count > 0;
        }

        private void frmGetValue_Shown(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}
