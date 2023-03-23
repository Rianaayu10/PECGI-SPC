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
    public partial class frmValue : Form
    {
        public double Value;
        public frmValue()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            double v;
            if(double.TryParse(txtScale.Text, out v))
            {
                Value = v;
                this.DialogResult = DialogResult.OK;
            } else
            {
                this.DialogResult = DialogResult.None;
            }         
        }

        private void txtScale_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
