using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace qqqq.forms {
    public partial class Create : DevExpress.XtraEditors.XtraUserControl {
        public Create() {
            InitializeComponent();
        }


        private void buttonEdit1_ButtonClick_1(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e) {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = "请选择项目路径";
            if (folder.ShowDialog() == DialogResult.OK) {
                this.buttonEdit1.EditValue = folder.SelectedPath;
            }


        }

        private void simpleButton1_Click_1(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(textEdit1.Text)) {
                MessageBox.Show("请输入项目名称", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(buttonEdit1.Text)) {
                MessageBox.Show("请选择项目路径", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBox.Show("新建成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
