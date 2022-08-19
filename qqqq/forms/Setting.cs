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
    public partial class Setting : DevExpress.XtraEditors.XtraUserControl {
        public Setting() {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(textEdit1.Text)) {
                MessageBox.Show("请输入地球曲率半径", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(textEdit2.Text)) {
                MessageBox.Show("请输入设计高程面高程", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBox.Show("设置成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void textEdit2_EditValueChanged(object sender, EventArgs e) {

        }
    }
}
