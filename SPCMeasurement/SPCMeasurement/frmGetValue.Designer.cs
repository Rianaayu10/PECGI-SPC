
namespace SPCMeasurement
{
    partial class frmGetValue
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGetValue));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.grid = new C1.Win.C1FlexGrid.C1FlexGrid();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::SPCMeasurement.Properties.Resources.cancel;
            this.btnCancel.Location = new System.Drawing.Point(242, 144);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(83, 28);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Image = global::SPCMeasurement.Properties.Resources.tick;
            this.btnOK.Location = new System.Drawing.Point(153, 144);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(83, 28);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // grid
            // 
            this.grid.AllowEditing = false;
            this.grid.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid.AutoGenerateColumns = false;
            this.grid.ColumnInfo = "3,1,0,0,0,110,Columns:0{Name:\"ItemCheckCode\";}\t1{Width:70;Name:\"ItemCheckCode\";Ca" +
    "ption:\"ItemCheckCode\";Visible:False;}\t2{Name:\"ItemCheck\";Caption:\"Item Check\";}\t" +
    "";
            this.grid.ExtendLastCol = true;
            this.grid.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.None;
            this.grid.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grid.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None;
            this.grid.Location = new System.Drawing.Point(12, 10);
            this.grid.Name = "grid";
            this.grid.Rows.Count = 1;
            this.grid.Rows.DefaultSize = 22;
            this.grid.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
            this.grid.ShowCursor = true;
            this.grid.Size = new System.Drawing.Size(455, 125);
            this.grid.StyleInfo = resources.GetString("grid.StyleInfo");
            this.grid.TabIndex = 0;
            // 
            // frmGetValue
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(479, 181);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGetValue";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Get Values from Previous Measurement";
            this.Load += new System.EventHandler(this.frmGetValue_Load);
            this.Shown += new System.EventHandler(this.frmGetValue_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private C1.Win.C1FlexGrid.C1FlexGrid grid;
    }
}