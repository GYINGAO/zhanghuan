namespace qqqq.forms {
    partial class Setting {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.textEdit2 = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit2.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(151, 76);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(87, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "地球曲率半径/m";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(139, 132);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(99, 14);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "设计高程面高程/m";
            // 
            // textEdit1
            // 
            this.textEdit1.EditValue = "6371000";
            this.textEdit1.Location = new System.Drawing.Point(269, 73);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Size = new System.Drawing.Size(166, 20);
            this.textEdit1.TabIndex = 2;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Primary;
            this.simpleButton1.Appearance.Options.UseBackColor = true;
            this.simpleButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.simpleButton1.Location = new System.Drawing.Point(237, 193);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(124, 33);
            this.simpleButton1.TabIndex = 4;
            this.simpleButton1.Text = "确定";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // textEdit2
            // 
            this.textEdit2.EditValue = "0.000";
            this.textEdit2.Location = new System.Drawing.Point(269, 129);
            this.textEdit2.Name = "textEdit2";
            this.textEdit2.Size = new System.Drawing.Size(166, 20);
            this.textEdit2.TabIndex = 3;
            this.textEdit2.EditValueChanged += new System.EventHandler(this.textEdit2_EditValueChanged);
            // 
            // Setting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.textEdit1);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.textEdit2);
            this.Name = "Setting";
            this.Size = new System.Drawing.Size(578, 336);
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit2.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit textEdit1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.TextEdit textEdit2;
    }
}
