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
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraBars.FluentDesignSystem;
using qqqq.utils;
using qqqq.models;
using DevExpress.XtraCharts;
using DevExpress.Utils;

namespace qqqq.forms {
    public partial class ImportFile : DevExpress.XtraEditors.XtraUserControl {

        public string quxianbiao { get; set; }
        public string podubiaoPath { get; set; }
        public string milePath { get; set; }

        public MainForm mainForm { get; set; }
        public ImportFile(MainForm form) {
            InitializeComponent();

            // 给按钮绑定事件
            btnEdit_QX.ButtonClick += btnEdit_QX_ButtonClick;
            btnEdit_PD.ButtonClick += btnEdit_PD_ButtonClick;
            btnEdit_mile.ButtonClick += btnEdit_mile_ButtonClick;

            // 接收主窗体
            mainForm = form;

            // 实例化buttonEdit
            //btnEdit_PD.EditValue = "";
            //btnEdit_QX.EditValue = "";

        }

        /// <summary>
        /// 导入里程文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_mile_ButtonClick(object sender, ButtonPressedEventArgs e) {
            if (mainForm.mile.Count != 0) {
                mainForm.isImport = true;
            }
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Title = "请选择里程文件";
            fileDialog.Filter = "Text files (*.txt)|*.txt|All files|*.*";

            if (fileDialog.ShowDialog() == DialogResult.OK) {

                mainForm.mile.Clear();
                try {
                    mainForm.mile = FileHelper.ImportMiles(fileDialog.FileName);
                }
                catch (Exception) {
                    MessageBox.Show("文件格式有误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                btnEdit_mile.EditValue = fileDialog.FileName;
                mainForm.coordinatesBefore = CalHelper.CalculateCoors(mainForm.mile.ToArray(), mainForm.horizontalsBefore, mainForm.verticalsBefore);
            }
        }

        /// <summary>
        /// 导入坡度表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_PD_ButtonClick(object sender, ButtonPressedEventArgs e) {
            if (mainForm.BPDsBefore.Count != 0) {
                mainForm.isImport = true;
            }
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Title = "请选择坡度表";
            fileDialog.Filter = "Text files (*.txt)|*.txt|All files|*.*";

            if (fileDialog.ShowDialog() == DialogResult.OK) {

                mainForm.BPDsBefore.Clear();
                try {
                    mainForm.BPDsBefore = FileHelper.ImportProfileParam(fileDialog.FileName);
                }
                catch (Exception) {
                    MessageBox.Show("文件格式有误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                btnEdit_PD.EditValue = fileDialog.FileName;
                if (!string.IsNullOrEmpty(btnEdit_QX.Text)) {
                    calcCoordinatesBefore();
                    paintPQX();
                    paintSQX();
                }
                if (!string.IsNullOrEmpty(btnEdit_mile.Text)) {

                }
            }
        }

        private void paintSQX() {
            ctclSQX.Series.Clear();
            Draw.AddSQXSeries(mainForm.verticalsBefore, ctclSQX, "主点");
            Draw.AddBPDSeries(mainForm.BPDsBefore, ctclSQX, "变坡点");
        }

        private void SetLab(string quxianbiaoPath, string podubiaoPath, string milePath) {
            this.quxianbiao = quxianbiaoPath;
            this.podubiaoPath = podubiaoPath;
            this.milePath = milePath;
        }


        /// <summary>
        /// 导入曲线表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_QX_ButtonClick(object sender, ButtonPressedEventArgs e) {
            //btnEdit editor = sender as btnEdit;
            //if (e.Button.Kind == ButtonPredefines.OK) {
            //    //...
            //}
            //if (e.Button.Kind == ButtonPredefines.Delete) {
            //    //...
            //}

            if (mainForm.JDsBefore.Count != 0) {
                mainForm.isImport = true;
            }
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Title = "请选择曲线表";
            fileDialog.Filter = "Text files (*.txt)|*.txt|All files|*.*";

            if (fileDialog.ShowDialog() == DialogResult.OK) {

                mainForm.JDsBefore.Clear();
                try {
                    mainForm.JDsBefore = FileHelper.ImportPlaneParam(fileDialog.FileName, out mainForm.qdBefore, out mainForm.zdBefore);
                }
                catch (Exception ex) {
                    MessageBox.Show("文件格式有误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw ex;
                    return;
                }
                btnEdit_QX.EditValue = fileDialog.FileName;
                if (!string.IsNullOrEmpty(btnEdit_PD.Text)) {
                    calcCoordinatesBefore();
                    paintPQX();
                    paintSQX();
                }
            }
        }

        /// <summary>
        /// 改线前三维坐标计算
        /// </summary>
        private void calcCoordinatesBefore() {
            mainForm.horizontalsBefore.Clear();
            mainForm.verticalsBefore.Clear();
            mainForm.coordinatesBefore.Clear();


            mainForm.horizontalsBefore = CalHelper.CalHorizontal(ref mainForm.JDsBefore, mainForm.qdBefore, mainForm.zdBefore);//平曲线表
            mainForm.verticalsBefore = CalHelper.CalVertical(mainForm.BPDsBefore);//竖曲线表

        }

        private void paintPQX() {
            ctclPQX.Series.Clear();
            Draw.AddHorizontalSeries(mainForm.horizontalsBefore, ctclPQX, "主点");
            Draw.SetHorizontalLegendAndXY(ctclPQX);
            Draw.AddJDSeries(mainForm.JDsBefore, ctclPQX, "交点");

            //Series series = new Series("交点", ViewType.ScatterLine);

            //// 下一句为定义线上点标识，有他才能设置线上点
            //((LineSeriesView)series.View).MarkerVisibility = DefaultBoolean.True;
            ////设置线上点标识为圆形
            //((LineSeriesView)series.View).LineMarkerOptions.Kind = MarkerKind.Circle;
            //////设置点标识为红色
            ////((LineSeriesView)series.View).LineMarkerOptions.Color = Color.Red;
            ////((LineSeriesView)series.View).LineMarkerOptions.Size = 2;
            ////设置线型类型为虚线
            ////((LineSeriesView)series.View).LineStyle.DashStyle = DashStyle.Dash;

            //SeriesPoint sp;
            //for (int i = 0; i < mainForm.JDsBefore.Count; i++) {

            //    sp = new SeriesPoint(mainForm.JDsBefore[i].Y, mainForm.JDsBefore[i].X);
            //    //设置主点颜色
            //    series.Points.Add(sp);
            //}
            //ctclPQX.Series.Add(series);
        }

        private void ImportFile_Load(object sender, EventArgs e) {


        }
    }
}
