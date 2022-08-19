namespace qqqq.forms {
    partial class Create {
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
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.buttonEdit1 = new DevExpress.XtraEditors.ButtonEdit();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Primary;
            this.simpleButton1.Appearance.Options.UseBackColor = true;
            this.simpleButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.simpleButton1.Location = new System.Drawing.Point(273, 210);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(70, 3, 200, 3);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(146, 36);
            this.simpleButton1.TabIndex = 10;
            this.simpleButton1.Text = "确定";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click_1);
            // 
            // buttonEdit1
            // 
            this.buttonEdit1.Location = new System.Drawing.Point(158, 136);
            this.buttonEdit1.Margin = new System.Windows.Forms.Padding(3, 3, 70, 3);
            this.buttonEdit1.Name = "buttonEdit1";
            this.buttonEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.buttonEdit1.Properties.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.buttonEdit1.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.buttonEdit1.Size = new System.Drawing.Size(445, 20);
            this.buttonEdit1.TabIndex = 9;
            this.buttonEdit1.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.buttonEdit1_ButtonClick_1);
            // 
            // textEdit1
            // 
            this.textEdit1.Location = new System.Drawing.Point(158, 74);
            this.textEdit1.Margin = new System.Windows.Forms.Padding(3, 3, 70, 3);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Size = new System.Drawing.Size(445, 20);
            this.textEdit1.TabIndex = 8;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(85, 139);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(70, 3, 3, 3);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 7;
            this.labelControl3.Text = "项目位置";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(85, 77);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(70, 3, 3, 3);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 6;
            this.labelControl1.Text = "项目名称";
            // 
            // Create
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.buttonEdit1);
            this.Controls.Add(this.textEdit1);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl1);
            this.Name = "Create";
            this.Size = new System.Drawing.Size(713, 353);
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.ButtonEdit buttonEdit1;
        private DevExpress.XtraEditors.TextEdit textEdit1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}
