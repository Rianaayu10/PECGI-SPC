using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DirectPrint;

namespace SPCMeasurement
{
    public partial class frmMeasurement : Form
    {

        public string UserID;
        public string pFactoryCode;
        public string pProcessGroup;
        public string pLineGroup;
        public string pProcessCode;
        public string pLineCode;
        public string pItemType;
        public string pItemCheck;
        public string pShiftCode;
        public string pSeqNo;
        public string pProdDate;

        public frmLogin frmLogin;
        public bool IsInit = true;

        string ls_SQLHost = "", ls_SQLPort = "", ls_SQLDatabase = "", ls_SQLAuth = "", ls_SQLUserID = "", ls_SQLPassword = "";
        string ls_SQLConnTO = "", ls_SQLCmdTO = "";
        string ls_MEAS_Connection = "";
        string ls_COM_Port = "", ls_COM_BaudRate = "", ls_COM_DataBits = "", ls_COM_Parity = "", ls_COM_StopBits = "", ls_COM_RTSEnable = "", ls_COM_GetResultData = "", ls_Command = "";
        string ls_COM_Stable = "", ls_COM_FlowControl = "", ls_COM_DTREnable = "";
        string Measure2nd = "0";
        int getResultData = 0;

        string ls_SavedBarcodeNo = "", ls_SavedWeight = "";
        bool bolSaved = false;
        string ls_ErrMsg = "";
        bool bolSQLDBOK;

        bool bolExitOK;
        bool bolCOMCon = false;
        bool bolBarcodeOK = false, bolWeightOK = false;

        private bool InvokeInProgress = false;
        private bool stopInvoking = false;
        DateTime dtStartScale;
        double valScale, prevValScale;

        private const string USBPort = "USB";

        private void btnStop_Click(object sender, EventArgs e)
        {
            ShowMsg("");
            if (ComPort.IsOpen)
            {
                stopInvoking = true;
                uf_Disconnect();
            }
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            btnClose.Enabled = true;
            btnRead.Enabled = false;
            lblScaleCon.BackColor = Color.Red;
            lblScaleCon.Text = "Disconnected";
            lblScaleStatus.BackColor = Color.Red;
            lblScaleStatus.Text = "... ";
            ls_ErrMsg = "";
            txtScale.ReadOnly = true;
        }

        private bool ValidateValue2()
        {
            if(Measure2nd == "1" & opt2.Checked & grid.Rows.Count > grid.Rows.Fixed)
            {
                int SPCResultID = Convert.ToInt32(grid[1, "SPCResultID"]);
                int n = clsSPCResultDetailDB.CountValue2(SPCResultID);
                return n > 0;                
            } else
            {
                return true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ShowMsg("");
            if(ValidateValue2() == false)
            {
                ShowMsg("No more samples from Value 1", true);
                return;
            }
            if (!ValidSkill())
            {
                return;
            }
            string q = "'0082  +   1.2  N\n      +   3.4  N                \n";
            if(opt1.Checked)
            {
                q = "4.7 mg\n";
            } else
            {
                q = "1 mg\n";
            }

            frmValue frm = new frmValue();
            if(frm.ShowDialog() == DialogResult.OK)
            {
                double value = frm.Value;
                if (value > 0)
                {
                    InsertData(value);
                    bolWeightOK = false;
                    prevValScale = 0;
                }
            }
            frm.Dispose();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            uf_Disconnect();
            this.Close();            
        }

        private void cboFactory_TextChanged(object sender, EventArgs e)
        {
            if(IsInit)
            {
                return;
            }
            string FactoryCode;
            if (cboFactory.SelectedValue == null)
            {
                FactoryCode = "";
            } else {
                FactoryCode = cboFactory.SelectedValue.ToString();
            }
            clsProcessGroupDB.FillCombo(cboProcessGroup, UserID, FactoryCode);
            RefreshData();
        }

        private void frmMeasurement_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();            
        }

        private void cboProcessGroup_TextChanged(object sender, EventArgs e)
        {
            if (IsInit)
            {
                return;
            }
            string FactoryCode;
            if (cboFactory.SelectedValue == null)
            {
                FactoryCode = "";
            }
            else
            {
                FactoryCode = cboFactory.SelectedValue.ToString();
            }
            string ProcessGroup;
            if (cboProcessGroup.SelectedValue == null)
            {
                ProcessGroup = "";
            }
            else
            {
                ProcessGroup = cboProcessGroup.SelectedValue.ToString();
            }
            clsLineGroupDB.FillCombo(cboLineGroup, UserID, FactoryCode, ProcessGroup);
            RefreshData();
        }

        private void cboLineGroup_TextChanged(object sender, EventArgs e)
        {
            if (IsInit)
            {
                return;
            }
            string FactoryCode;
            if (cboFactory.SelectedValue == null)
            {
                FactoryCode = "";
            }
            else
            {
                FactoryCode = cboFactory.SelectedValue.ToString();
            }
            string ProcessGroup;
            if (cboProcessGroup.SelectedValue == null)
            {
                ProcessGroup = "";
            }
            else
            {
                ProcessGroup = cboProcessGroup.SelectedValue.ToString();
            }
            string LineGroup;
            if (cboLineGroup.SelectedValue == null)
            {
                LineGroup = "";
            }
            else
            {
                LineGroup = cboLineGroup.SelectedValue.ToString();
            }
            clsProcessDB.FillCombo(cboProcess, UserID, FactoryCode, ProcessGroup, LineGroup);
            RefreshData();
        }

        private void cboLine_TextChanged(object sender, EventArgs e)
        {
            if (IsInit)
            {
                return;
            }
            string FactoryCode;
            if (cboFactory.SelectedValue == null)
            {
                FactoryCode = "";
            }
            else
            {
                FactoryCode = cboFactory.SelectedValue.ToString();
            }
            string LineCode;
            if (cboLine.SelectedValue == null)
            {
                LineCode = "";
            } else
            {
                LineCode = cboLine.SelectedValue.ToString();
            }

            clsItemTypeDB.FillCombo(cboType, UserID, FactoryCode, LineCode);
            RefreshData();
        }

        private void cboProcess_TextChanged(object sender, EventArgs e)
        {
            if (IsInit)
            {
                return;
            }
            string FactoryCode;
            if (cboFactory.SelectedValue == null)
            {
                FactoryCode = "";
            }
            else
            {
                FactoryCode = cboFactory.SelectedValue.ToString();
            }
            string LineCode;
            if (cboLine.SelectedValue == null)
            {
                LineCode = "";
            }
            else
            {
                LineCode = cboLine.SelectedValue.ToString();
            }
            string ProcessCode;
            if (cboProcess.SelectedValue == null)
            {
                ProcessCode = "";
            }
            else
            {
                ProcessCode = cboProcess.SelectedValue.ToString();
            }
            clsLineDB.FillCombo(cboLine, UserID, FactoryCode, ProcessCode);
            RefreshData();
        }

        private void dtProd_ValueChanged(object sender, EventArgs e)
        {
            FillCboShift();
        }

        private void cboType_TextChanged(object sender, EventArgs e)
        {
            if (IsInit)
            {
                return;
            }
            string FactoryCode;
            if (cboFactory.SelectedValue == null)
            {
                FactoryCode = "";
            }
            else
            {
                FactoryCode = cboFactory.SelectedValue.ToString();
            }
            string LineCode;
            if (cboLine.SelectedValue == null)
            {
                LineCode = "";
            }
            else
            {
                LineCode = cboLine.SelectedValue.ToString();
            }
            string ItemTypeCode;
            if (cboType.SelectedValue == null)
            {
                ItemTypeCode = "";
            }
            else
            {
                ItemTypeCode = cboType.SelectedValue.ToString();
            }
            clsItemCheckDB.FillCombo(cboItemCheck, FactoryCode, LineCode, ItemTypeCode);
            RefreshData();
        }

        private void FillCboShift()
        {
            if (IsInit)
            {
                return;
            }
            string FactoryCode;
            if (cboFactory.SelectedValue == null)
            {
                FactoryCode = "";
            }
            else
            {
                FactoryCode = cboFactory.SelectedValue.ToString();
            }
            string LineCode;
            if (cboLine.SelectedValue == null)
            {
                LineCode = "";
            }
            else
            {
                LineCode = cboLine.SelectedValue.ToString();
            }
            string ItemTypeCode;
            if (cboType.SelectedValue == null)
            {
                ItemTypeCode = "";
            }
            else
            {
                ItemTypeCode = cboType.SelectedValue.ToString();
            }
            string ItemCheckCode;
            if (cboItemCheck.SelectedValue == null)
            {
                ItemCheckCode = "";
            }
            else
            {
                ItemCheckCode = cboItemCheck.SelectedValue.ToString();
            }
            string prevShift = "";
            if (cboShift.SelectedValue != null)
            {
                prevShift = cboShift.SelectedValue.ToString();
            }           
            clsShiftDB.FillCombo(cboShift, FactoryCode, ItemTypeCode, LineCode, ItemCheckCode);
            cboShift.SelectedValue = prevShift;
            clsDeviceDB.FillCombo(cboReg, FactoryCode, ItemTypeCode, LineCode, ItemCheckCode);
            if(cboReg.ListCount == 1)
            {
                cboReg.SelectedIndex = 0;
            }
        }

        private void FillCboDevice()
        {
            if (IsInit)
            {
                return;
            }
            string FactoryCode;
            if (cboFactory.SelectedValue == null)
            {
                FactoryCode = "";
            }
            else
            {
                FactoryCode = cboFactory.SelectedValue.ToString();
            }
            string LineCode;
            if (cboLine.SelectedValue == null)
            {
                LineCode = "";
            }
            else
            {
                LineCode = cboLine.SelectedValue.ToString();
            }
            string ItemTypeCode;
            if (cboType.SelectedValue == null)
            {
                ItemTypeCode = "";
            }
            else
            {
                ItemTypeCode = cboType.SelectedValue.ToString();
            }
            string ItemCheckCode;
            if (cboItemCheck.SelectedValue == null)
            {
                ItemCheckCode = "";
            }
            else
            {
                ItemCheckCode = cboItemCheck.SelectedValue.ToString();
            }
            string prevShift = "";
            if (cboShift.SelectedValue != null)
            {
                prevShift = cboShift.SelectedValue.ToString();
            }
            clsShiftDB.FillCombo(cboShift, FactoryCode, ItemTypeCode, LineCode, ItemCheckCode);
            cboShift.SelectedValue = prevShift;
        }

        private void cboItemCheck_TextChanged(object sender, EventArgs e)
        {
            Measure2nd = cboItemCheck.GetItemText(cboItemCheck.Row, 2).Trim();
            pnlValue.Visible = Measure2nd == "1";
            if(Measure2nd == "1")
            {
                opt1.Checked = true;                
            }
            ShowValue2(Measure2nd == "1");
            FillCboShift();
            RefreshData();
        }

        private void ShowValue2(bool show)
        {
            grid.Cols["Value1"].Visible = show;
            grid.Cols["Value2"].Visible = show;
        }

        private void cboShift_TextChanged(object sender, EventArgs e)
        {
            if (IsInit)
            {
                return;
            }
            string FactoryCode;
            if (cboFactory.SelectedValue == null)
            {
                FactoryCode = "";
            }
            else
            {
                FactoryCode = cboFactory.SelectedValue.ToString();
            }
            string LineCode;
            if (cboLine.SelectedValue == null)
            {
                LineCode = "";
            }
            else
            {
                LineCode = cboLine.SelectedValue.ToString();
            }
            string ItemTypeCode;
            if (cboType.SelectedValue == null)
            {
                ItemTypeCode = "";
            }
            else
            {
                ItemTypeCode = cboType.SelectedValue.ToString();
            }
            string ItemCheckCode;
            if (cboItemCheck.SelectedValue == null)
            {
                ItemCheckCode = "";
            }
            else
            {
                ItemCheckCode = cboItemCheck.SelectedValue.ToString();
            }
            string ShiftCode;
            if (cboShift.SelectedValue == null)
            {
                ShiftCode = "";
            }
            else
            {
                ShiftCode = cboShift.SelectedValue.ToString();
            }
            string ProdDate = dtProd.Value.ToString("yyyy-MM-dd");

            
            if(ShiftCode != "")
            {
                string PrevSeq = "";
                if (cboSeq.SelectedValue != null)
                {
                    PrevSeq = cboSeq.SelectedValue.ToString();
                }
                clsSequenceDB.FillCombo(cboSeq, FactoryCode, ItemTypeCode, LineCode, ItemCheckCode, ShiftCode);
                if (PrevSeq != "")
                {
                    cboSeq.SelectedValue = PrevSeq;
                }                
            }            
        }

        private void ShowMsg(string msg, bool isError = false)
        {
            if(isError == false)
            {
                stMsg.ForeColor = Color.Black;
                stMsg.BackColor = SystemColors.Control;
            } else
            {
                stMsg.ForeColor = Color.White;
                stMsg.BackColor = Color.Red;
            }
            stMsg.Text = msg;
        }

        private bool ValidInput()
        {
            if (!ValidSkill())
            {
                return false;
            }
            if (!ValidChartSetup())
            {
                return false;
            }
            return true;
        }

        private bool ValidFilter()
        {
            if (cboType.SelectedValue == null)
            {
                cboType.Focus();
                ShowMsg("Please select Item Type", true);
                return false;
            }
            if (cboItemCheck.SelectedValue == null)
            {
                cboItemCheck.Focus();
                ShowMsg("Please select Item Check", true);
                return false;
            }
            if (cboShift.SelectedValue == null)
            {
                cboShift.Focus();
                ShowMsg("Please select Shift Code", true);
                return false;
            }
            if (cboSeq.SelectedValue == null)
            {
                cboSeq.Focus();
                ShowMsg("Please select Sequence", true);
                return false;
            }
            return true;
        }

        private bool RefreshData()
        {
            ShowMsg("");
            try
            {
                string FactoryCode = cboFactory.SelectedValue + "";
                string ItemType = cboType.SelectedValue + "";
                string LineCode = cboLine.SelectedValue + "";
                string ItemCheck = cboItemCheck.SelectedValue + "";
                string ProcessGroup = cboProcessGroup.SelectedValue + "";
                string ProcessCode = cboProcess.SelectedValue + "";
                string LineGroup = cboLineGroup.SelectedValue + "";
                string ShiftCode = cboShift.SelectedValue + "";
                string SeqNo = cboSeq.SelectedValue + "";
                if(SeqNo == "")
                {
                    SeqNo = "0";
                }
                string ProdDate = dtProd.Value.ToString("yyyy-MM-dd");
                string RegNo = "";
                if (cboReg.SelectedValue != null)
                {
                    RegNo = cboReg.SelectedValue.ToString();
                }
                if (FactoryCode == "" | ProcessGroup == "" | LineGroup == "" | ProcessCode == "" | LineCode == "" | ItemType == "" | ItemCheck == "" | ShiftCode == "" | SeqNo == "" | SeqNo == "0")
                {
                    return false;
                }
                List<clsSPCResultDetail> result = clsSPCResultDetailDB.GetList(FactoryCode, ItemType, LineCode, ItemCheck, ProdDate, ShiftCode, Convert.ToInt32(SeqNo), 0, RegNo);
                grid.DataSource = result;
                grid.Row = grid.Rows.Count - 1;
                grid.ShowCell(grid.Rows.Count - 1, 0);
                SetRowColor();
                if (Measure2nd == "1" & grid.Rows.Count == grid.Rows.Fixed)
                {
                    opt1.Checked = true;
                }
                SaveCache(FactoryCode, ProcessGroup, LineGroup, ProcessCode, LineCode, ItemType, ItemCheck, ShiftCode, SeqNo, RegNo);
                if (result.Count > 0)
                {
                    clsSPCResultDetail detail = result[0];
                    ShowComplete(detail.SPCResultID);
                }
                else
                {
                    lblComplete.Text = "";
                }
                return true;
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message, true);
                return false;
            }
        }



        private void btnSearch_Click(object sender, EventArgs e)
        {
            ShowMsg("");
            if (!ValidFilter())
            {
                return;
            }
            if (!ValidInput())
            {                
                return;
            }
            RefreshData();
        }

        private void SaveCache(string FactoryCode, string ProcessGroup, string LineGroup, string ProcessCode, string LineCode, string ItemType, string ItemCheck, string ShiftCode, string SeqNo, string RegNo)
        {
            if(FactoryCode == "" | ProcessGroup == "" | LineGroup == "" | ProcessCode == "" | LineCode == "" | ItemType == "" | ItemCheck == "" | ShiftCode == "" | SeqNo == "" | SeqNo == "0")
            {
                return;
            }

            Microsoft.Win32.RegistryKey key;
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SPCMeasurement");
            key.SetValue("UserID", UserID);
            key.SetValue("FactoryCode", FactoryCode);
            key.SetValue("ProcessGroup", ProcessGroup);
            key.SetValue("LineGroup", LineGroup);
            key.SetValue("ProcessCode", ProcessCode);
            key.SetValue("LineCode", LineCode);
            key.SetValue("ItemType", ItemType);
            key.SetValue("ItemCheck", ItemCheck);
            key.SetValue("ShiftCode", ShiftCode);
            key.SetValue("SeqNo", SeqNo);
            key.SetValue("RegNo", RegNo);
            key.Close();
        }

        private void ReadParam()
        {
            if (pProcessGroup != "")
            {
                cboProcessGroup.SelectedValue = pProcessGroup;
            }
            if (pLineGroup != "")
            {
                cboLineGroup.SelectedValue = pLineGroup;
            }
            if (pProcessCode != "")
            {
                cboProcess.SelectedValue = pProcessCode;
            }
            if (pLineCode != "")
            {
                cboLine.SelectedValue = pLineCode;
            }
            if (pItemType != "")
            {
                cboType.SelectedValue = pItemType;
            }
            if (pItemCheck != "")
            {
                cboItemCheck.SelectedValue = pItemCheck;
            }
            if (pShiftCode != "")
            {
                cboShift.SelectedValue = pShiftCode;
            }
            if (pProdDate != "")
            {
                dtProd.Value = Convert.ToDateTime(pProdDate);
            }
            if (pSeqNo != "")
            {
                cboSeq.SelectedValue = pSeqNo;
                bool success = RefreshData();
                if(success)
                {
                    btnStart.PerformClick();
                }
            }
        }

        private void ReadCache()
        {
            string CacheUserID = "";
            string FactoryCode= "";  
            string ProcessGroup = "";  
            string LineGroup = "";  
            string ProcessCode = "";  
            string LineCode = "";  
            string ItemType = "";  
            string ItemCheck = "";  
            string ShiftCode = "";
            string SeqNo = "";
            string RegNo = "";

            Microsoft.Win32.RegistryKey key;
            key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SPCMeasurement");
            if(key != null)
            {
                CacheUserID = key.GetValue("UserID").ToString();
                FactoryCode = key.GetValue("FactoryCode").ToString();
                ProcessGroup = key.GetValue("ProcessGroup").ToString();
                LineGroup = key.GetValue("LineGroup").ToString();
                ProcessCode = key.GetValue("ProcessCode").ToString();
                LineCode = key.GetValue("LineCode").ToString();
                ItemType = key.GetValue("ItemType").ToString();
                ItemCheck = key.GetValue("ItemCheck").ToString();
                ShiftCode = key.GetValue("ShiftCode").ToString();
                SeqNo = key.GetValue("SeqNo").ToString();
                RegNo = key.GetValue("RegNo").ToString();
            }            
            key.Close();

            if (ProcessGroup != "")
            {
                cboProcessGroup.SelectedValue = ProcessGroup;
            }
            if (LineGroup != "")
            {
                cboLineGroup.SelectedValue = LineGroup;
            }
            if (ProcessCode != "")
            {
                cboProcess.SelectedValue = ProcessCode;
            }
            if (LineCode != "")
            {
                cboLine.SelectedValue = LineCode;
            }
            if (ItemType != "")
            {
                cboType.SelectedValue = ItemType;
            }
            if (ItemCheck != "")
            {
                cboItemCheck.SelectedValue = ItemCheck;
            }
            if (ShiftCode != "")
            {
                cboShift.SelectedValue = ShiftCode;
            }
            if (SeqNo != "")
            {
                cboSeq.SelectedValue = SeqNo;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                ShowMsg("");
                grid.Focus();
                btnPrint.Enabled = false;                
                clsPrint Printer = new clsPrint();
                Printer.set_PrinterFont("Courier New");
                Printer.set_PrinterFontSize(8);
                Printer.PrintString("Reg No          : " + cboReg.Text);
                Printer.PrintString("Description     : " + cboType.Text);
                Printer.PrintString("Machine Process : " + cboLine.Text);
                Printer.PrintString("Item Check      : " + cboItemCheck.Text);
                Printer.NewLine();

                double value = 0;
                int maxLen = 0;
                for (int i = 1; i <= grid.Rows.Count - 1; i++)
                {
                    value = Convert.ToDouble(grid[i, 3]);
                    string sValue = value.ToString("#####0.0000");
                    if (sValue.Length > maxLen)
                    {
                        maxLen = sValue.Length;
                    }
                }
                string numberFormat = "#####0.0000";
                numberFormat = numberFormat.Substring(numberFormat.Length - maxLen);
                for (int i = 1; i <= grid.Rows.Count - 1; i++)
                {
                    Printer.PrintTab(1, grid[i, 0].ToString());
                    value = Convert.ToDouble(grid[i, 3]);
                    int colValue = 4;
                    Printer.PrintNum(colValue, value, numberFormat);
                    DateTime RegisterDate = Convert.ToDateTime(grid[i, 5]);
                    int colDate = colValue + numberFormat.Length + 2;
                    Printer.PrintTab(colDate, RegisterDate.ToString("dd MMM yyyy HH:mm"));
                    Printer.NewLine();
                }
                Printer.NewLine();
                Printer.PrintString("User ID : " + UserID);
                Printer.PrintString("Print Date : " + DateTime.Now.ToString("dd MMM yyyy HH:mm"));
                Printer.EndDoc();
            } 
            catch (Exception ex)
            {
                ShowMsg(ex.Message, true);
            }
            finally
            {
                btnPrint.Enabled = true;
            }
        }

        private void frmMeasurement_Shown(object sender, EventArgs e)
        {
            stUser.Text = UserID;
            clsFactoryDB.FillCombo(cboFactory);
            opt1.BackColor = this.BackColor;
            opt2.BackColor = this.BackColor;
            IsInit = false;
            cboFactory.SelectedValue = pFactoryCode;            
            cboFactory.Focus();
            if(pProcessGroup != "")
            {
                ReadParam();
            } else
            {
                ReadCache();
            }            
        }

        private void grid_Click(object sender, EventArgs e)
        {

        }

        private void SetRowColor()
        {
            C1.Win.C1FlexGrid.CellStyle cs = grid.Styles.Add("red");
            cs.BackColor = System.Drawing.Color.Red;
            cs.ForeColor = System.Drawing.Color.White;

            C1.Win.C1FlexGrid.CellStyle csDel = grid.Styles.Add("delete");
            csDel.BackColor = System.Drawing.Color.Gray;
            csDel.ForeColor = System.Drawing.Color.Black;
            for (int i = 1; i <= grid.Rows.Count - 1; i++)
            {
                string Judgement = grid[i, "Judgement"].ToString().Trim();
                string DeleteStatus = grid[i, "DeleteStatus"].ToString().Trim();
                if (DeleteStatus == "1")
                {
                    grid.Rows[i].Style = csDel;
                } else if (Judgement == "NG")
                {
                    grid.Rows[i].Style = cs;
                }
            }
        }

        private bool ValidSkill()
        {
            string FactoryCode = cboFactory.SelectedValue.ToString();
            string ItemType = cboType.SelectedValue.ToString();
            string LineCode = cboLine.SelectedValue.ToString();

            bool AllowSkill = clsSPCResultDB.AllowSkill(UserID, FactoryCode, LineCode, ItemType);
            if(Environment.MachineName == "TOS56-ARI")
            {
                AllowSkill = true;
            }
            if(!AllowSkill)
            {
                ShowMsg("You do not have skill to input result for this item.", true);
            }
            return AllowSkill;
        }

        private bool ValidChartSetup()
        {
            string FactoryCode = cboFactory.SelectedValue.ToString();
            string ItemType = cboType.SelectedValue.ToString();
            string LineCode = cboLine.SelectedValue.ToString();
            string ItemCheck = cboItemCheck.SelectedValue.ToString();
            string ProdDate = dtProd.Value.ToString("yyyy-MM-dd");

            bool valid = clsSPCResultDB.ChartSetupExists(FactoryCode, ItemType, LineCode, ItemCheck, ProdDate);
            if (!valid)
            {
                ShowMsg("Chart Setup has not been set for this item", true);
            }
            return valid;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ShowMsg("");
            if(grid.Rows.Count > grid.Rows.Fixed)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to clear result?", "Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    int SPCResultID = Convert.ToInt32(grid[1, "SPCResultID"]);
                    clsSPCResultDetailDB.Delete(SPCResultID);
                    RefreshData();
                }
            }      
            else
            {
                ShowMsg("No data to delete", true);
            }      
        }

        private void txtScale_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtScale_Enter(object sender, EventArgs e)
        {
            txtScale.SelectAll();
        }

        public void InsertManual(string strValue)
        {
            string tmpVal = strValue.Trim();
            double value;
            if (double.TryParse(tmpVal, out value))
            {
                InsertData(value);
                prevValScale = 0;
                RefreshData();
            }
            else
            {
                ShowMsg("Invalid value!", true);
            }
        }

        private void txtScale_KeyPress(object sender, KeyPressEventArgs e)
        {
            ShowMsg("");
            if (e.KeyChar == (char)Keys.Return)
            {
                double value;
                string tmpVal = txtScale.Text.Trim();
                if (double.TryParse(tmpVal, out value))
                {
                    InsertData(value);
                    txtScale.Clear();                    
                    bolWeightOK = false;
                    prevValScale = 0;
                    RefreshData();
                } else
                {
                    txtScale.Focus();
                    txtScale.SelectAll();
                    ShowMsg("Invalid value!", true);
                }
            }
        }

        private void InsertData(double value)
        {
            clsSPCResult result = new clsSPCResult();
            result.FactoryCode = cboFactory.SelectedValue.ToString();
            result.ItemTypeCode = cboType.SelectedValue.ToString();
            result.LineCode = cboLine.SelectedValue.ToString();
            result.ItemCheckCode = cboItemCheck.SelectedValue.ToString();
            result.ProdDate = dtProd.Value.ToString("yyyy-MM-dd");
            result.ShiftCode = cboShift.SelectedValue.ToString();
            result.SequenceNo = Convert.ToInt32(cboSeq.SelectedValue.ToString());
            result.SubLotNo = "";
            result.Remark = "";
            result.RegisterUser = UserID;
            result.RegisterNo = cboReg.SelectedValue.ToString();
            result.Measure2nd = Measure2nd;
            clsSPCResultDB.Insert(result);

            clsSPCResultDetail detail = new clsSPCResultDetail();
            detail.SPCResultID = result.SPCResultID;
            if (Measure2nd == "1")
            {
                if (opt1.Checked)
                {
                    detail.Value1 = value;
                }
                else if (opt2.Checked)
                {
                    detail.Value2 = value;
                }
            }
            else
            {
                detail.Value = value;
            }
            detail.RegisterUser = UserID;
            clsSPCResultDetailDB.Insert(detail);

            string FactoryCode = cboFactory.SelectedValue.ToString();
            string ItemType = cboType.SelectedValue.ToString();
            string LineCode = cboLine.SelectedValue.ToString();
            string ItemCheck = cboItemCheck.SelectedValue.ToString();
            string ProcessGroup = cboProcessGroup.SelectedValue.ToString();
            string ProcessCode = cboProcess.SelectedValue.ToString();
            string LineGroup = cboLineGroup.SelectedValue.ToString();
            string ShiftCode = cboShift.SelectedValue.ToString();
            string SeqNo = cboSeq.SelectedValue.ToString();
            string RegNo = cboReg.SelectedValue.ToString();

            List<clsSPCResultDetail> resultdetail = clsSPCResultDetailDB.GetList(cboFactory.SelectedValue.ToString(), cboType.SelectedValue.ToString(), cboLine.SelectedValue.ToString(), cboItemCheck.SelectedValue.ToString(), dtProd.Value.ToString("yyyy-MM-dd"), cboShift.SelectedValue.ToString(), Convert.ToInt32(cboSeq.SelectedValue), 0, RegNo);
            grid.DataSource = resultdetail;
            grid.Row = grid.Rows.Count - 1;
            grid.ShowCell(grid.Rows.Count - 1, 0);
            SetRowColor();
            ShowComplete(detail.SPCResultID);
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            ShowMsg("");
            FrmConfig frmCfg = new FrmConfig();
            frmCfg.ls_Param = "";
            frmCfg.ShowDialog();
            frmCfg.Dispose();
            uf_GetConfig();
            uf_InitConnection();
        }

        private SerialPort ComPort = new SerialPort();

        public frmMeasurement(string userID, frmLogin parentForm, string factoryCode, string processGroup = "", string lineGroup = "", string processCode = "", string lineCode = "", string itemType = "", string itemCheck = "", string shiftCode = "", string seqNo = "", string prodDate = "", string server = "", string database = "")
        {
            UserID = userID;
            pFactoryCode = factoryCode;
            pProcessGroup = processGroup;
            pLineGroup = lineGroup;
            pProcessCode = processCode;
            pLineCode = lineCode;
            pItemType = itemType;
            pItemCheck = itemCheck;
            pShiftCode = shiftCode;
            pSeqNo = seqNo;
            pProdDate = prodDate;
            frmLogin = parentForm;
            InitializeComponent();
            lblArg.Text = UserID + "," + pFactoryCode + "," + pProcessGroup + "," + pLineGroup + "," + pLineCode + "," + pItemType + "," + pItemCheck + "," + pShiftCode + "," + pSeqNo + "," + pProdDate;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(opt1.Checked)
            {
                opt1.BackColor = Color.FromArgb(255, 192, 128);
            } else
            {
                opt1.BackColor = this.BackColor;
            }
        }

        private void opt2_CheckedChanged(object sender, EventArgs e)
        {
            if (opt2.Checked)
            {
                opt2.BackColor = Color.LightGreen;
            }
            else
            {
                opt2.BackColor = this.BackColor;
            }
        }

        private void cboSeq_TextChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void txtScale_EnabledChanged(object sender, EventArgs e)
        {

        }

        private void txtScale_ReadOnlyChanged(object sender, EventArgs e)
        {
            if (txtScale.ReadOnly)
            {
                txtScale.BackColor = Color.LightGray;
            }
            else
            {
                txtScale.BackColor = Color.White;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if(!ComPort.IsOpen)
                {
                    ShowMsg("Port is closed", true);
                    return;
                }
                //ComPort.Write("QX\r\n");
                ComPort.Write(ls_Command + "\r\n");
            }
            catch (Exception ex )
            {
                ShowMsg(ex.Message, true);
            }
        }

        private void Process()
        {

        }

        private bool SPCDetailExists()
        {
            return grid.Rows.Count > grid.Rows.Fixed;
        }

        private void lblComplete_Click(object sender, EventArgs e)
        {

        }

        private void lblComplete_TextChanged(object sender, EventArgs e)
        {
            if(lblComplete.Text.ToLower().StartsWith("complete"))
            {
                lblComplete.BackColor = Color.LimeGreen;
                lblComplete.Visible = true;
            } else if (lblComplete.Text.ToLower().StartsWith("incomplete"))
            {
                lblComplete.BackColor = Color.Orange;
                lblComplete.Visible = true;
            } else if (lblComplete.Text == "")
            {
                lblComplete.Visible = false;
            }
        }

        private void btnGetValue_Click(object sender, EventArgs e)
        {
            ShowMsg("");
            if(!ValidFilter())
            {
                return;
            }
            if(SPCDetailExists())
            {
                string msg = "Values already exist!\n" + "Are you sure you want to get values again?";
                DialogResult result = MessageBox.Show(msg, "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                if(result != DialogResult.OK)
                {
                    return;
                }
            }
            frmGetValue frm = new frmGetValue();
            frm.FactoryCode = cboFactory.SelectedValue.ToString();
            frm.ItemType = cboType.SelectedValue.ToString();
            frm.Line = cboLine.SelectedValue.ToString();
            frm.Shift = cboShift.SelectedValue.ToString();
            frm.ItemCheckCode = cboItemCheck.SelectedValue.ToString();
            frm.Sequence  = Convert.ToInt16(cboSeq.SelectedValue);
            frm.UserID = UserID;
            frm.ProdDate = dtProd.Value.ToString("yyyy-MM-dd");
            if(frm.ShowDialog() == DialogResult.OK)
            {
                string ItemCheckCodeFrom = frm.ItemCheckCodeFrom;
                int i = InsertPrevData(ItemCheckCodeFrom);
                RefreshData();
                opt2.Checked = true;
            }
        }

        private void frmMeasurement_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (btnRead.Enabled && e.KeyChar == Convert.ToChar(Keys.Space))
            {
                e.Handled = true;
                btnRead.PerformClick();
            }

        }

        private void btnClearValue2_Click(object sender, EventArgs e)
        {
            ShowMsg("");
            if (grid.Rows.Count > grid.Rows.Fixed)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to clear all value 2?", "Clear Value 2", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    int SPCResultID = Convert.ToInt32(grid[1, "SPCResultID"]);
                    clsSPCResultDetailDB.ClearValue2(SPCResultID);
                    RefreshData();
                }
            }
            else
            {
                ShowMsg("No data to delete", true);
            }
        }

        private void pnlValue_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pnlValue_VisibleChanged(object sender, EventArgs e)
        {
            if(pnlValue.Visible == false)
            {
                btnClearValue2.Visible = false;
            } else
            {
                btnClearValue2.Visible = btnClear.Visible;
            }
        }

        private void frmMeasurement_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F2 & e.Shift)
            {
                e.Handled = true;
                btnSave.Visible = !btnSave.Visible;
                btnClear.Visible = !btnClear.Visible;
                if(pnlValue.Visible)
                {
                    btnClearValue2.Visible = !btnClearValue2.Visible;
                } else
                {
                    btnClearValue2.Visible = false;
                }
                
                grid.Cols["SPCResultID"].Visible = !grid.Cols["SPCResultID"].Visible;
            } 
            else if(e.KeyCode == Keys.F1)
            {
                e.Handled = true;
                opt1.Checked = true;
            }
            else if (e.KeyCode == Keys.F2)
            {
                e.Handled = true;
                opt2.Checked = true;
            }
            else if (e.KeyCode == Keys.F3)
            {
                e.Handled = true;
                btnGetValue.PerformClick();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            ls_ErrMsg = "";
            ShowMsg("");
            if (!ValidFilter())
            {
                return;
            }
            if (cboReg.SelectedValue == null)
            {
                cboReg.Focus();
                ShowMsg("Please select Register No", true);
                return;
            }
            if (!ValidInput())
            {
                return;
            }

            if (ComPort.IsOpen)
            {
                stopInvoking = true;
                uf_Disconnect();
            }
            bolCOMCon = false;
            stopInvoking = false;
            uf_Connect();
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            if (bolCOMCon)
            {
                if(ls_COM_Port != USBPort)
                {
                    lblScaleCon.BackColor = Color.LimeGreen;
                    lblScaleCon.Text = "Connected";
                    lblScaleStatus.BackColor = Color.Orange;
                    lblScaleStatus.Text = "Waiting...";
                } else
                {
                    lblScaleCon.BackColor = Color.LimeGreen;
                    lblScaleCon.Text = "USB Mode";
                    lblScaleStatus.BackColor = Color.LimeGreen;
                    lblScaleStatus.Text = "...";
                }

            }
        }

        private void uf_Connect()
        {
            bool error = false;

            // Check if all settings have been selected

            int r = cboReg.SelectedIndex;
            ls_COM_Port = cboReg.GetItemText(r, "Port");
            ls_COM_BaudRate = cboReg.GetItemText(r, "BaudRate");
            ls_COM_Parity = cboReg.GetItemText(r, "Parity");
            ls_COM_DataBits = cboReg.GetItemText(r, "DataBits");
            ls_COM_StopBits = cboReg.GetItemText(r, "StopBits");
            ls_COM_GetResultData = cboReg.GetItemText(r, "GetResultData");
            ls_COM_FlowControl = cboReg.GetItemText(r, "FlowControl");
            ls_Command  = cboReg.GetItemText(r, "Command");            
            int.TryParse(ls_COM_GetResultData, out getResultData);

            ls_COM_RTSEnable = cboReg.GetItemText(r, "EnableRTS");
            ls_COM_DTREnable = cboReg.GetItemText(r, "EnableDTR");

            if (ls_COM_Port.Trim() == USBPort )
            {
                bolCOMCon = true;
                txtScale.ReadOnly = false;
                txtScale.Focus();
                return;
            }            
            else if (ls_COM_Port != "")
            {
                txtScale.ReadOnly = true;
                ComPort.PortName = ls_COM_Port;
                ComPort.BaudRate = int.Parse(ls_COM_BaudRate);      //convert Text to Integer
                ComPort.Parity = (Parity)Enum.Parse(typeof(Parity), ls_COM_Parity); //convert Text to Parity
                ComPort.DataBits = int.Parse(ls_COM_DataBits);        //convert Text to Integer
                ComPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), ls_COM_StopBits);  //convert Text to stop bits
                ComPort.RtsEnable = ls_COM_RTSEnable == "1";
                ComPort.DtrEnable = ls_COM_DTREnable == "1";
                ComPort.NewLine = "\n";
                if(ls_COM_FlowControl == "1")
                {
                    ComPort.Handshake = Handshake.XOnXOff;
                } 
                else if (ls_COM_FlowControl == "2")
                {
                    ComPort.Handshake = Handshake.RequestToSend;
                }
                else if (ls_COM_FlowControl == "3")
                {
                    ComPort.Handshake = Handshake.RequestToSendXOnXOff;
                } 
                else
                {
                    ComPort.Handshake = Handshake.None;
                }

                    try 
                {
                    ComPort.Open();
                    ComPort.DataReceived += SerialPortDataReceivedPassive;  //Check for received data. When there is data in the receive buffer,
                                                                            //it will raise this event, we need to subscribe to it to know when there is data
                    btnRead.Enabled = ls_Command != "";
                    bolCOMCon = true;
                }
                catch (UnauthorizedAccessException) { error = true; }
                catch (System.IO.IOException) { error = true; }
                catch (ArgumentException) { error = true; }

                if (error)
                {
                    ls_ErrMsg = "COM Port unavailable";
                    tsProgress.Text = ls_ErrMsg;
                    btnRead.Enabled = false;
                    lblScaleCon.BackColor = Color.Red;
                    lblScaleCon.Text = "ERROR";
                    lblScaleStatus.BackColor = Color.Red;
                    lblScaleStatus.Text = " ... ";
                }
            }
            else
            {
                //MessageBox.Show("Please select all the COM Serial Port Settings", "Serial Port Interface", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                ls_ErrMsg = "Serial Port Interface";
                tsProgress.Text = ls_ErrMsg;
                lblScaleCon.BackColor = Color.Red;
                lblScaleCon.Text = "ERROR";
                lblScaleStatus.BackColor = Color.Red;
                lblScaleStatus.Text = " ... ";
                btnRead.Enabled = false;
            }
            //if the port is open, Change the Connect button to disconnect, enable the send button.
            //and disable the groupBox to prevent changing configuration of an open port.
            if (ComPort.IsOpen)
            {
                //btnStart.Text = "Disconnect";
                //btnSend.Enabled = true;
                //if (!rdText.Checked & !rdHex.Checked)  //if no data mode is selected, then select text mode by default
                //{
                //    rdText.Checked = true;
                //}
                btnStart.Enabled = false;
                btnStop.Enabled = true;

            }
        }

        private void ShowCompleteThread(int SPCResultID)
        {
            lblComplete.Invoke(new EventHandler(delegate
            {
                clsSPCResultSample result = clsSPCResultDB.GetSampleSize(SPCResultID);
                if (result == null)
                {
                    lblComplete.Text = "";
                } else if (result.Remaining > 0)
                {
                    lblComplete.Text = "Incomplete: " + result.Remaining.ToString() + " remaining";
                }
                else
                {
                    lblComplete.Text = "Complete";
                }
            }));
        }

        private void ShowComplete(int SPCResultID)
        {
            clsSPCResultSample result = clsSPCResultDB.GetSampleSize(SPCResultID);
            if (result == null)
            {
                lblComplete.Text = "";
            }
            else if (result.Remaining > 0)
            {
                lblComplete.Text = "Incomplete: " + result.Remaining.ToString() + " remaining";
            }
            else
            {
                lblComplete.Text = "Complete";
            }
        }

        private void Log(LogMsgType msgtype, string msg)
        {
            rtfTerminal.Invoke(new EventHandler(delegate
            {
                rtfTerminal.SelectedText = string.Empty;
                rtfTerminal.SelectionFont = new Font(rtfTerminal.SelectionFont, FontStyle.Bold);
                rtfTerminal.AppendText(msg);
                rtfTerminal.ScrollToCaret();
            }));
        }

        private int InsertPrevData(string ItemCheckCodeFrom)
        {
            clsSPCResult result = new clsSPCResult();
            result.FactoryCode = cboFactory.SelectedValue.ToString();
            result.ItemTypeCode = cboType.SelectedValue.ToString();
            result.LineCode = cboLine.SelectedValue.ToString();
            result.ItemCheckCode = cboItemCheck.SelectedValue.ToString();
            result.ProdDate = dtProd.Value.ToString("yyyy-MM-dd");
            result.ShiftCode = cboShift.SelectedValue.ToString();
            result.SequenceNo = Convert.ToInt32(cboSeq.SelectedValue.ToString());
            result.RegisterUser = UserID;
            result.RegisterNo = cboReg.SelectedValue.ToString();
            int i = clsSPCResultDB.InsertPrevValue(result, ItemCheckCodeFrom);
            return i;
        }

        private void InsertRefreshData(double value)
        {
            grid.Invoke(new EventHandler(delegate
            {
                clsSPCResult result = new clsSPCResult();
                result.FactoryCode = cboFactory.SelectedValue.ToString();
                result.ItemTypeCode = cboType.SelectedValue.ToString();
                result.LineCode = cboLine.SelectedValue.ToString();
                result.ItemCheckCode = cboItemCheck.SelectedValue.ToString();
                result.ProdDate = dtProd.Value.ToString("yyyy-MM-dd");
                result.ShiftCode = cboShift.SelectedValue.ToString();
                result.SequenceNo = Convert.ToInt32(cboSeq.SelectedValue.ToString());
                result.SubLotNo = "";
                result.Remark = "";
                result.RegisterUser = UserID;
                result.RegisterNo = cboReg.SelectedValue.ToString();
                clsSPCResultDB.Insert(result);

                clsSPCResultDetail detail = new clsSPCResultDetail();
                detail.SPCResultID = result.SPCResultID;
                if(Measure2nd == "1")
                {
                    if(opt1.Checked)
                    {
                        detail.Value1 = value;
                    } 
                    else if(opt2.Checked)
                    {
                        detail.Value2 = value;
                    }                    
                } else
                {
                    detail.Value = value;
                }                
                detail.RegisterUser = UserID;
                clsSPCResultDetailDB.Insert(detail);

                string FactoryCode = cboFactory.SelectedValue.ToString();
                string ItemType = cboType.SelectedValue.ToString();
                string LineCode = cboLine.SelectedValue.ToString();
                string ItemCheck = cboItemCheck.SelectedValue.ToString();
                string ProcessGroup = cboProcessGroup.SelectedValue.ToString();
                string ProcessCode = cboProcess.SelectedValue.ToString();
                string LineGroup = cboLineGroup.SelectedValue.ToString();
                string ShiftCode = cboShift.SelectedValue.ToString();
                string SeqNo = cboSeq.SelectedValue.ToString();
                string RegNo = cboReg.SelectedValue.ToString();

                List<clsSPCResultDetail> resultdetail = clsSPCResultDetailDB.GetList(cboFactory.SelectedValue.ToString(), cboType.SelectedValue.ToString(), cboLine.SelectedValue.ToString(), cboItemCheck.SelectedValue.ToString(), dtProd.Value.ToString("yyyy-MM-dd"), cboShift.SelectedValue.ToString(), Convert.ToInt32(cboSeq.SelectedValue), 0, RegNo);
                grid.DataSource = resultdetail;
                grid.Row = grid.Rows.Count - 1;
                grid.ShowCell(grid.Rows.Count - 1, 0);
                SetRowColor();
                ShowCompleteThread(result.SPCResultID);
            }));
        }

        public enum LogMsgType { Incoming, Outgoing, Normal, Warning, Error };

        private void SerialPortDataReceivedPassive(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (stopInvoking != true)
                {
                    var serialPort = (SerialPort)sender;
                    if (!serialPort.IsOpen)
                    {
                        return;
                    }
                    ls_ErrMsg = "Reading scale data ...";
                    //var data = serialPort.ReadExisting();
                    var data = serialPort.ReadExisting();
                    Log(LogMsgType.Normal, data);
                    InvokeInProgress = true;
                    string[] ValArray = data.Split('\n');
                    string tmpVal = ValArray[0].Trim();
                    if (tmpVal.Length < getResultData)
                    {
                        return;
                    }
                    tmpVal = tmpVal.Substring(getResultData);
                    tmpVal = tmpVal.Replace("N", "");
                    tmpVal = tmpVal.Replace("mg", "");
                    if (tmpVal.Contains("+"))
                    {
                        int pos = tmpVal.IndexOf("+");
                        if(tmpVal.Length < pos + 1)
                        {
                            return;
                        }
                        tmpVal = tmpVal.Substring(pos + 1);
                    }
                    tmpVal = tmpVal.Trim();
                    double value = 0;
                    if (double.TryParse(tmpVal, out value))
                    {
                        if (value > 0)
                        {
                            InsertRefreshData(value);
                            prevValScale = 0;
                        }
                    }
                    InvokeInProgress = false;
                    Thread.Sleep(300);
                }
            }
            catch (System.IO.IOException error)
            {
                Console.WriteLine("IO Error >>" + error.Message.ToString());
                ls_ErrMsg = "IO Error >>" + error.Message.ToString();
                return;
            }
            catch (System.InvalidOperationException error)
            {
                Console.WriteLine("InvalidOperationException Error >>" + error.Message.ToString());
                ls_ErrMsg = "InvalidOperationException Error >>" + error.Message.ToString();
                return;
            }

        }

        private void SerialPortDataReceivedActive(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (stopInvoking != true)
                {
                    var serialPort = (SerialPort)sender;
                    if (!serialPort.IsOpen)
                    {
                        return;
                    }
                    ls_ErrMsg = "Reading scale data ...";
                    var tmpVal = serialPort.ReadLine();
                    Log(LogMsgType.Normal, tmpVal);
                    InvokeInProgress = true;                    
                    double value = SetText(tmpVal);
                    if (bolWeightOK & value > 0)
                    {
                        InsertRefreshData(value);
                        prevValScale = 0;
                    }
                    InvokeInProgress = false;
                    Thread.Sleep(300);
                }
            }
            catch (System.IO.IOException error)
            {
                Console.WriteLine("IO Error >>" + error.Message.ToString());
                ls_ErrMsg = "IO Error >>" + error.Message.ToString();
                return;
            }
            catch (System.InvalidOperationException error)
            {
                Console.WriteLine("InvalidOperationException Error >>" + error.Message.ToString());
                ls_ErrMsg = "InvalidOperationException Error >>" + error.Message.ToString();
                return;
            }

        }

        delegate double SetTextCallback(string text);
        private double SetText(string text)
        {
            if (this.txtScale.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
                return 0;
            }
            else
            {
                bolWeightOK = false;

                string tmpVal = text.Replace("N", "");
                tmpVal = tmpVal.Replace("mg", "");
                string[] ValArray = tmpVal.Split('\n');
                if(ValArray.Count() > 2)
                {
                    tmpVal = ValArray[0];
                }
                tmpVal = ValArray[0];
                if (tmpVal.Contains("+"))
                {
                    int pos = tmpVal.IndexOf("+");
                    tmpVal = tmpVal.Substring(pos + 1);
                }
                double nDec = 0;
                lblScaleStatus.BackColor = Color.Red;
                lblScaleStatus.Text = "Reading scale data ...";
                tmpVal = tmpVal.Replace("kg", "");
                tmpVal = tmpVal.Replace("-", "");
                tmpVal = tmpVal.Replace(Environment.NewLine, "");
                tmpVal = tmpVal.Trim();
                //valScale = int.Parse(tmpVal);
                if (double.TryParse(tmpVal, out nDec))
                {
                    this.txtScale.Text = nDec.ToString();
                }
                else
                {
                    this.txtScale.Text = "0.00";
                }
                valScale = nDec;
                if ((valScale != prevValScale) && (valScale > 0))
                {
                    dtStartScale = DateTime.Now;
                    prevValScale = valScale;
                }
                else if ((valScale == prevValScale) && (valScale > 0))
                {
                    TimeSpan ts = DateTime.Now.Subtract(dtStartScale);
                    if (ts.TotalSeconds > int.Parse(ls_COM_Stable))
                    {
                        lblScaleStatus.BackColor = Color.LimeGreen;
                        lblScaleStatus.Text = "STABLE";
                        Console.WriteLine("STABLE");
                        ls_ErrMsg = "Reading scale data - STABLE ";
                        bolWeightOK = true;
                    }
                }
                tsProgress.Text = ls_ErrMsg;
                return nDec;
            }

        }

        private double SetText2(string text)
        {
            if (this.txtScale.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
                return 0;
            }
            else
            {
                bolWeightOK = false;

                string tmpVal = text.Replace("N", "");
                tmpVal = tmpVal.Replace("mg", "");
                string[] ValArray = tmpVal.Split('\n');
                if (ValArray.Count() > 2)
                {
                    tmpVal = ValArray[1];
                }
                tmpVal = ValArray[0];
                if (tmpVal.Contains("+"))
                {
                    int pos = tmpVal.IndexOf("+");
                    tmpVal = tmpVal.Substring(pos + 1);
                }
                double nDec = 0;
                lblScaleStatus.BackColor = Color.Red;
                lblScaleStatus.Text = "Reading scale data ...";
                tmpVal = tmpVal.Replace("kg", "");
                tmpVal = tmpVal.Replace("-", "");
                tmpVal = tmpVal.Replace(Environment.NewLine, "");
                tmpVal = tmpVal.Trim();
                //valScale = int.Parse(tmpVal);
                if (double.TryParse(tmpVal, out nDec))
                {
                    this.txtScale.Text = nDec.ToString();
                }
                else
                {
                    this.txtScale.Text = "0.00";
                }
                valScale = nDec;
                if (valScale > 0)
                {
                    dtStartScale = DateTime.Now;
                    lblScaleStatus.BackColor = Color.LimeGreen;
                    lblScaleStatus.Text = "STABLE";
                    Console.WriteLine("STABLE");
                    ls_ErrMsg = "Reading scale data - STABLE ";
                    bolWeightOK = true;
                }
                tsProgress.Text = ls_ErrMsg;
                return nDec;
            }

        }

        private void uf_Disconnect()
        {
            txtScale.ReadOnly = true;
            if (ComPort.IsOpen)
            {
                ComPort.Close();
                //btnConnect.Text = "Connect";
                ////btnSend.Enabled = false;
                //groupBox1.Enabled = true;
                btnStart.Enabled = true;
                btnStop.Enabled = false;
            }            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            IsInit = true;
            lblComplete.Text = "";
            this.Text = "SPC Measurement ver " + Application.ProductVersion;
            ShowMsg("");
            txtScale.ReadOnly = true;
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            btnClose.Enabled = true;
            btnRead.Enabled = false;
            uf_GetConfig();
            uf_InitConnection();
            stopInvoking = false;
            bolExitOK = false;
            timerCurr.Enabled = true;
            lblScaleCon.BackColor = Color.Red;
            lblScaleCon.Text = "Disconnected";
            lblScaleStatus.BackColor = Color.Red;
            lblScaleStatus.Text = " ... ";
            ls_ErrMsg = "";
            txtScale.ReadOnly = true; 
        }

        private void uf_GetConfig()
        {
            clsConfigData cfgData = new clsConfigData();
            clsConfig cfg = new clsConfig();
            cfgData = cfg.uf_ReadConfig();

            if (cfgData != null)
            {
                ls_COM_Port = cfgData.COM_Port.Trim();
                ls_COM_BaudRate = cfgData.COM_BaudRate.ToString();
                ls_COM_DataBits = cfgData.COM_DataBits.Trim();
                ls_COM_Parity = cfgData.COM_Parity.Trim();
                ls_COM_StopBits = cfgData.COM_StopBits.Trim();
                ls_COM_Stable = cfgData.COM_Stable.Trim();
                ls_COM_RTSEnable = cfgData.COM_RTSEnable.Trim();

                if (string.IsNullOrEmpty(ls_COM_Stable) || ls_COM_Stable == "0")
                {
                    ls_COM_Stable = "3";
                }
                if(Program.pServer != "")
                {
                    ls_SQLHost = Program.pServer;
                } else {
                    ls_SQLHost = cfgData.SQL_Host.Trim();
                }                
                ls_SQLPort = cfgData.SQL_Port.ToString();
                if(Program.pDatabase != "")
                {
                    ls_SQLDatabase = Program.pDatabase;
                } else
                {
                    ls_SQLDatabase = cfgData.SQL_Database.Trim();
                }                
                ls_SQLConnTO = cfgData.SQL_DBTO.Trim();
                ls_SQLCmdTO = cfgData.SQL_CmdTO.Trim();

                ls_SQLAuth = cfgData.SQL_AuthType.Trim();

                if (ls_SQLAuth != "1")
                {
                    //SQL Server Auth Type
                    ls_SQLUserID = cfgData.SQL_UserID.Trim();
                    ls_SQLPassword = cfgData.SQL_Password.Trim();
                }
                else
                {
                    ls_SQLUserID = "";
                    ls_SQLPassword = "";
                }


            }
            else
            {
                ls_COM_Port = "";
                ls_COM_BaudRate = "";
                ls_COM_DataBits = "";
                ls_COM_Parity = "";
                ls_COM_StopBits = "";
                ls_COM_Stable = "2";

                ls_SQLHost = "";
                ls_SQLPort = "";
                ls_SQLDatabase = "";
                ls_SQLAuth = "";
                ls_SQLUserID = "";
                ls_SQLPassword = "";
                ls_SQLConnTO = "300";
                ls_SQLCmdTO = "3600";
                
            }
            stServer.Text = ls_SQLHost + "." + ls_SQLDatabase;
        }


        private void uf_InitConnection()
        {


            if (ls_SQLHost != "" && ls_SQLPort != "" && ls_SQLDatabase != "" && ls_SQLAuth != "" && ls_SQLUserID != "" && ls_SQLPassword != "")
            {



                if (ls_SQLPort != "1433")
                {
                    ls_MEAS_Connection = "Data Source=" + ls_SQLHost + "," + ls_SQLPort + ";";
                }
                else
                {
                    ls_MEAS_Connection = "Data Source=" + ls_SQLHost + ";";
                }
                ls_MEAS_Connection = ls_MEAS_Connection + "Initial Catalog=" + ls_SQLDatabase + ";";

                if (ls_SQLAuth == "1")
                {
                    ls_MEAS_Connection = ls_MEAS_Connection + "Integrated Security=SSPI;";
                }
                else
                {
                    ls_MEAS_Connection = ls_MEAS_Connection + "User Id=" + ls_SQLUserID + ";Password=" + ls_SQLPassword + ";";
                }
                ls_MEAS_Connection = ls_MEAS_Connection + "Connection Timeout=" + ls_SQLConnTO + ";";

            }
            else
            {
                ls_MEAS_Connection = "";
            }
            bolSQLDBOK = true;
            if (bolSQLDBOK == false)
            {
                //MessageBox.Show("Cannot connect to MEAS database !" + System.Environment.Line + ls_ErrMsg);
                tsProgress.Text = "Cannot connect to MEAS Database ";

            }
            else
            {
                //MessageBox.Show("Database ( MEAS and MIERUKA ) successfully connected !");
                //cLog.WriteLog(clsLogs.LogType.Info, true, "uf_InitConnection", "Check Connection DB", "-", "Database MEAS successfully connected");
            }

        }
    }
}
