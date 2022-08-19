using DevExpress.XtraBars;
using DevExpress.XtraBars.FluentDesignSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using qqqq.forms;
using qqqq.models;
using DevExpress.XtraEditors;
using qqqq.utils;

namespace qqqq {
    public partial class MainForm : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm {

        #region 定义子窗体
        private Setting setting;
        private ImportFile importFile;
        private Adjust adjust;
        private Create create;
        #endregion


        #region 定义数据存储结构
        // 改线前后曲线表和坡度表
        public List<JD> JDsBak = new List<JD>();
        public List<BPD> BPDsAfter = new List<BPD>();
        public List<JD> JDsBefore = new List<JD>();
        public List<BPD> BPDsBefore = new List<BPD>();
        // 改线前后起终点
        public ZX qdAfter = new ZX();//起点
        public ZX zdAfter = new ZX();//终点
        public ZX qdBefore = new ZX();//起点
        public ZX zdBefore = new ZX();//终点
        // 里程集合
        public List<double> mile = new List<double>();
        // 改线前后三维坐标集合
        public List<Coordinate> coordinatesBefore = new List<Coordinate>();
        public List<Coordinate> coordinatesAfter = new List<Coordinate>();
        // 改线前后平曲线集合
        public List<Horizontal> horizontalsBefore = new List<Horizontal>();
        public List<Horizontal> horizontalsAfter = new List<Horizontal>();
        // 改线前后竖曲线集合
        public List<Vertical> verticalsBefore = new List<Vertical>();
        public List<Vertical> verticalsAfter = new List<Vertical>();
        // 偏差集合的集合
        public List<List<Deviation>> dev = new List<List<Deviation>>();
        // 文件路径
        public string quxianbiaoPath = "";
        public string podubiaoPath = "";
        public string milePath = "";

        public bool isImport { get; set; }
        #endregion


        public MainForm() {
            InitializeComponent();
            //Create header control that contains buttons
            FluentDesignFormControl fdfControl = new FluentDesignFormControl();
            //Create buttons
            SkinDropDownButtonItem skinItem = new SkinDropDownButtonItem();
            SkinPaletteDropDownButtonItem paletteItem =
                new SkinPaletteDropDownButtonItem();
            //Call BeginInit-EndInit methods to make sure
            //the header control is set up correctly
            fdfControl.BeginInit();
            this.FluentDesignFormControl = fdfControl; //bind header control with form
            this.Controls.Add(fdfControl); //add header control to form
            //BarItems require a BarManager to operate
            //Every FluentDesignFormControl has an internal BarManager
            //Add bar items to the Items collection to assign them to this manager
            fdfControl.Items.AddRange(new BarItem[] {
                skinItem, paletteItem,
            });
            //Items that the header control should display
            fdfControl.TitleItemLinks.AddRange(new BarItem[] {
                skinItem, paletteItem
            });
            fdfControl.EndInit();

            //FluentDesignFormContainer fills the entire client
            //area of a form and hosts all form controls
            FluentDesignFormContainer fdfContainer = new FluentDesignFormContainer();
            this.ControlContainer = fdfContainer;
            this.Controls.Add(fdfContainer);
            fdfContainer.Dock = DockStyle.Fill;
            fdfContainer.Controls.AddRange(new Control[] {
                //client area controls
            });

            this.isImport = false;
        }

        /// <summary>
        /// 导入数据页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void accordionControlElement3_Click(object sender, EventArgs e) {
            closeForm();
            importFile.Dock = DockStyle.Fill;
            importFile.Show();
            fluentDesignFormContainer1.Controls.Add(importFile);
        }

        /// <summary>
        /// 参数设置页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void accordionControlElement4_Click(object sender, EventArgs e) {
            closeForm();
            setting.Dock = DockStyle.Fill;
            setting.Show();
            fluentDesignFormContainer1.Controls.Add(setting);
        }

        /// <summary>
        /// 切换菜单栏预处理
        /// </summary>
        private void closeForm() {
            fluentDesignFormContainer1.Controls.Clear();
        }

        public static void updatePath() {

        }

        private void MainForm_Load(object sender, EventArgs e) {
            setting = new Setting();
            importFile = new ImportFile(this);
            adjust = new Adjust(this);
            create = new Create();
        }

        private void accordionControlElement5_Click(object sender, EventArgs e) {
            if (JDsBefore.Count == 0) {
                MessageBox.Show("未导入交点数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (BPDsBefore.Count == 0) {
                MessageBox.Show("未导入边坡点数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (mile.Count == 0) {
                MessageBox.Show("未导入里程数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            closeForm();
            adjust.Dock = DockStyle.Fill;
            adjust.Show();
            fluentDesignFormContainer1.Controls.Add(adjust);
        }

        private void accordionControlElement6_Click_1(object sender, EventArgs e) {
            if (dev.Count == 0) {
                MessageBox.Show("没有偏差数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try {
                SaveFileDialog fileDialog = new SaveFileDialog();
                fileDialog.Title = "请选择保存路径";
                fileDialog.Filter = "Text files(*.txt)|*.txt|CSV files(*.csv)|*.csv";
                fileDialog.FileName = "横垂偏差数据";
                if (fileDialog.ShowDialog() == DialogResult.OK) {
                    if (fileDialog.FilterIndex == 0) {
                        FileHelper.ExportDeviationTxt_2(dev[dev.Count - 1], fileDialog.FileName);

                    }
                    else {
                        FileHelper.ExportDeviationCsv(dev[dev.Count - 1], fileDialog.FileName);
                    }
                    MessageBox.Show("导出成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception) {
                MessageBox.Show("导出失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void accordionControlElement12_Click(object sender, EventArgs e) {
            if (coordinatesBefore.Count == 0) {
                MessageBox.Show("没有坐标数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try {
                SaveFileDialog fileDialog = new SaveFileDialog();
                fileDialog.Title = "请选择保存路径";
                fileDialog.Filter = "Text files(*.txt)|*.txt|CSV files(*.csv)|*.csv";
                fileDialog.FileName = "坐标数据";
                if (fileDialog.ShowDialog() == DialogResult.OK) {
                    if (fileDialog.FilterIndex == 0) {
                        FileHelper.ExportCoordinatesTxt(coordinatesBefore, fileDialog.FileName);

                    }
                    else {
                        FileHelper.ExportCoordinatesCsv(coordinatesBefore, fileDialog.FileName);
                    }
                    MessageBox.Show("导出成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception) {
                MessageBox.Show("导出失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void accordionControlElement13_Click(object sender, EventArgs e) {
            if (horizontalsBefore.Count == 0) {
                MessageBox.Show("没有平曲线数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try {
                SaveFileDialog fileDialog = new SaveFileDialog();
                fileDialog.Title = "请选择保存路径";
                fileDialog.Filter = "Text files(*.txt)|*.txt";
                fileDialog.FileName = "平曲线";
                if (fileDialog.ShowDialog() == DialogResult.OK) {
                    FileHelper.ExportHorizontalTxt(horizontalsBefore, fileDialog.FileName);
                    MessageBox.Show("导出成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception) {
                MessageBox.Show("导出失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void accordionControlElement14_Click(object sender, EventArgs e) {
            if (verticalsBefore.Count == 0) {
                MessageBox.Show("没有竖曲线数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try {
                SaveFileDialog fileDialog = new SaveFileDialog();
                fileDialog.Title = "请选择保存路径";
                fileDialog.Filter = "Text files(*.txt)|*.txt";
                fileDialog.FileName = "竖曲线";
                if (fileDialog.ShowDialog() == DialogResult.OK) {
                    FileHelper.ExportVerticalTxt(verticalsBefore, fileDialog.FileName);
                    MessageBox.Show("导出成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception) {
                MessageBox.Show("导出失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void accordionControlElement7_Click(object sender, EventArgs e) {
            closeForm();
            create.Dock = DockStyle.Fill;
            create.Show();
            fluentDesignFormContainer1.Controls.Add(create);
        }
    }
}
