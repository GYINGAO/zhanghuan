using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using qqqq.models;
using DevExpress.XtraEditors.Controls;
using qqqq.utils;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Collections;
using System.Linq;

namespace qqqq.forms {
    public partial class Adjust : DevExpress.XtraEditors.XtraUserControl {
        public MainForm mainForm { get; set; }
        public DataTable JDs;
        public DataTable BPDs;
        List<List<JD>> JDsModify = new List<List<JD>>();
        List<List<BPD>> BPDsModify = new List<List<BPD>>();
        public Adjust(MainForm form) {
            InitializeComponent();
            this.mainForm = form;

        }

        private void calcError(List<JD> jds, List<BPD> bpds) {
            //平曲线表
            //List<Horizontal> horizontals = CalHelper.CalHorizontal(ref mainForm.JDsBefore, mainForm.qdBefore, mainForm.zdBefore);
            mainForm.horizontalsAfter = CalHelper.CalHorizontal(ref jds, mainForm.qdBefore, mainForm.zdBefore);
            //竖曲线表
            mainForm.verticalsAfter = CalHelper.CalVertical(bpds);
            //计算三维坐标
            mainForm.coordinatesAfter = CalHelper.CalculateCoors(mainForm.mile.ToArray(), mainForm.horizontalsAfter, mainForm.verticalsAfter);


            //横垂偏差集合
            //List<Deviation> deviations = CalHelper.CalDeviation(mainForm.coordinatesAfter, mainForm.horizontalsBefore, mainForm.verticalsBefore);
            //mainForm.dev.Add(deviations);

            List<Deviation> deviations = CalHelper.CalDeviation(mainForm.coordinatesAfter, mainForm.horizontalsBefore, mainForm.verticalsBefore);
            foreach (var item in deviations) {
                item.fym = Math.Abs(item.fym);
            }
            mainForm.dev.Add(deviations);
        }

        /// <summary>
        /// lsit2dt
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<JD> JDdt2list(DataTable dt) {
            List<JD> JDs = new List<JD>();
            for (int i = 0; i < dt.Rows.Count; i++) {
                JD jd = new JD();
                jd.pName = dt.Rows[i]["pName"].ToString();
                jd.X = Convert.ToDouble(dt.Rows[i]["X"]);
                jd.Y = Convert.ToDouble(dt.Rows[i]["Y"]);
                jd.R = Convert.ToDouble(dt.Rows[i]["R"]);
                jd.a = Convert.ToDouble(dt.Rows[i]["a"]);
                JDs.Add(jd);
            }
            return JDs;
        }

        private void paintError_hengxiang(string pname, double d, string type) {
            Draw.AddOneSeries(mainForm.dev[mainForm.dev.Count - 1], ctclR, $"{pname}-{type}:{d}");
            Draw.SetLegendAndXY(ctclR);
        }

        private void paintError_chuixiang(double d) {
            Draw.AddOneSeries_chuixiang(mainForm.dev[mainForm.dev.Count - 1], ctcl_C, $"高程变化{d}");
        }


        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Adjust_Load(object sender, EventArgs e) {
            if (JDsModify.Count == 0) {
                List<JD> jds = listClone<JD>(mainForm.JDsBefore);
                JDsModify.Add(jds);
            }

            JDs = new DataTable("JDsBefore");
            JDs.Columns.Add("pName", typeof(string));
            JDs.Columns.Add("X", typeof(double));
            JDs.Columns.Add("Y", typeof(double));
            JDs.Columns.Add("R", typeof(double));
            JDs.Columns.Add("a", typeof(double));
            for (int i = 0; i < mainForm.JDsBefore.Count; i++) {
                DataRow dr = JDs.NewRow();
                dr["pName"] = mainForm.JDsBefore[i].pName;
                dr["X"] = mainForm.JDsBefore[i].X;
                dr["Y"] = mainForm.JDsBefore[i].Y;
                dr["R"] = mainForm.JDsBefore[i].R;
                dr["a"] = Math.Round(mainForm.JDsBefore[i].a, 5);
                JDs.Rows.Add(dr);
            }
            gdclJD.DataSource = JDs;


            if (BPDsModify.Count == 0) {
                List<BPD> bpds = listClone<BPD>(mainForm.BPDsBefore);
                BPDsModify.Add(bpds);
            }
            BPDs = new DataTable("sqx");
            BPDs.Columns.Add("mileage", typeof(double));
            BPDs.Columns.Add("H", typeof(double));
            BPDs.Columns.Add("R", typeof(double));
            BPDs.Columns.Add("i", typeof(double));
            for (int i = 0; i < mainForm.BPDsBefore.Count; i++) {
                DataRow dr = BPDs.NewRow();
                dr["mileage"] = mainForm.BPDsBefore[i].mileage;
                dr["H"] = mainForm.BPDsBefore[i].H;
                dr["R"] = mainForm.BPDsBefore[i].R;
                dr["i"] = Math.Round(mainForm.BPDsBefore[i].i, 5);
                BPDs.Rows.Add(dr);
            }
            gdclBPD.DataSource = BPDs;

            //for (int i = 0; i < mainForm.JDsBefore.Count; i++) {
            //    gridView1.AddNewRow();
            //    for (int j = 0; j < gridView1.Columns.Count; j++) {
            //        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[j].FieldName, mainForm.JDsBefore[i]);
            //    }
            //}

            //gdclJD.DataSource = mainForm.JDsBefore;
        }


        /// <summary>
        /// list列表深复制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="List"></param>
        /// <returns></returns>
        public static List<T> listClone<T>(object List) {
            using (Stream objectStream = new MemoryStream()) {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, List);
                objectStream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(objectStream) as List<T>;
            }
        }



        /// <summary>
        /// 修改平曲线半径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repositoryItemButtonEdit1_ButtonClick(object sender, ButtonPressedEventArgs e) {
            //ButtonEdit editor = (ButtonEdit)sender;
            ////EditorButton button = e.Button;
            //int rowIndex = gridView1.FocusedRowHandle;
            //if (Convert.ToDouble(editor.EditValue) == JDsModify[JDsModify.Count - 1][rowIndex].R) {
            //    XtraMessageBox.Show("半径没有变化哟~", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
            //double derta = Math.Abs(JDsModify[0][rowIndex].R - Convert.ToDouble(editor.EditValue));
            //string jdName = gridView1.GetRowCellValue(rowIndex, "pName").ToString();

            //gridView1.CloseEditor();
            //gridView1.UpdateCurrentRow();
            //gridView1.CloseEditForm();

            //mainForm.JDsBefore[rowIndex].R = Convert.ToDouble(gridView1.GetRowCellValue(rowIndex, "R"));

            //JDsModify.Add(mainForm.JDsBefore);
            //calcError(mainForm.JDsBefore, mainForm.BPDsBefore);
            //paintError_hengxiang(jdName, derta, "R");

            ButtonEdit editor = (ButtonEdit)sender;
            //EditorButton button = e.Button;
            int rowIndex = gridView1.FocusedRowHandle;
            var d = Convert.ToDouble(editor.EditValue) - mainForm.JDsBefore[rowIndex].R;
            if (d == 0) {
                XtraMessageBox.Show("半径没有变化哟~", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string jdName = gridView1.GetRowCellValue(rowIndex, "pName").ToString();
            //var path = @"C:\Users\Rick\Desktop\偏差值.csv";
            var path = @"C:\Users\Administrator\Desktop\半径.csv";
            if (File.Exists(path)) {
                StreamWriter myWrite = File.AppendText(path);
                for (int i = 0; i < Convert.ToInt32(this.textEdit1.Text); i++) {
                    var derta = (i + 1) * d;
                    mainForm.JDsBefore[rowIndex].R += d;
                    calcError(mainForm.JDsBefore, mainForm.BPDsBefore);
                    List<Deviation> dev = mainForm.dev[mainForm.dev.Count - 1];
                    var max = dev.Max(t => t.fym);
                    var value = dev.LastOrDefault(t => t.fym == max);
                    var mile = value.designMile;
                    myWrite.WriteLine(
                        mainForm.JDsBefore[rowIndex].pName.ToString().PadRight(13) + "," +
                        mainForm.JDsBefore[rowIndex].R.ToString("#0.0000").PadRight(16) + "," +
                        d.ToString("#0.00").PadRight(20) + "," +
                        derta.ToString("#0.00").PadRight(20) + "," +
                        mile.ToString("#0.000").PadRight(16) + "," +
                        max.ToString("#0.0000").PadRight(16)
                        );
                }
                myWrite.Close();
            }
            else {
                FileStream mystream = new FileStream(path, FileMode.OpenOrCreate);
                StreamWriter myWrite = new StreamWriter(mystream);
                myWrite.WriteLine("点号,半径,半径变化量,半径累计变化量,最大值里程,最大值");
                for (int i = 0; i < Convert.ToInt32(this.textEdit1.Text); i++) {
                    var derta = (i + 1) * d;
                    mainForm.JDsBefore[rowIndex].R += d;
                    calcError(mainForm.JDsBefore, mainForm.BPDsBefore);
                    List<Deviation> dev = mainForm.dev[mainForm.dev.Count - 1];
                    var max = dev.Max(t => t.fym);
                    var value = dev.LastOrDefault(t => t.fym == max);
                    var mile = value.designMile;
                    myWrite.WriteLine(
                        mainForm.JDsBefore[rowIndex].pName.ToString().PadRight(13) + "," +
                        mainForm.JDsBefore[rowIndex].R.ToString("#0.0000").PadRight(16) + "," +
                        d.ToString("#0.00").PadRight(20) + "," +
                        derta.ToString("#0.00").PadRight(20) + "," +
                        mile.ToString("#0.000").PadRight(16) + "," +
                        max.ToString("#0.0000").PadRight(16)
                        );
                }
                myWrite.Close();
                mystream.Close();
            }

            MessageBox.Show("计算完成");
            // mainForm.JDsBefore[rowIndex].R = Convert.ToDouble(editor.EditValue) - d;
            gridView1.SetRowCellValue(rowIndex, "R", mainForm.JDsBefore[rowIndex].R);
        }


        /// <summary>
        /// 按下回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repositoryItemButtonEdit1_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                if (gridView1.FocusedColumn.FieldName == "R")
                    repositoryItemButtonEdit1_ButtonClick(sender, null);
                else if (gridView1.FocusedColumn.FieldName == "a")
                    repositoryItemButtonEdit3_ButtonClick(sender, null);
                else
                    return;
            }
        }



        /// <summary>
        /// 修改平曲线转向角
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repositoryItemButtonEdit3_ButtonClick(object sender, ButtonPressedEventArgs e) {
            //ButtonEdit editor = (ButtonEdit)sender;
            //int rowIndex = gridView1.FocusedRowHandle;
            //if (Convert.ToDouble(editor.EditValue) == Math.Round(JDsModify[JDsModify.Count - 1][rowIndex].a, 5)) {
            //    XtraMessageBox.Show("转向角没有变化哟~", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}

            //gridView1.CloseEditor();
            ////gridView1.UpdateCurrentRow();
            //gridView1.CloseEditForm();

            //double derta = Math.Floor(Math.Abs(JDsModify[0][rowIndex].a - Convert.ToDouble(editor.EditValue)) * 180 / Math.PI * 3600);
            //string jdName = gridView1.GetRowCellValue(rowIndex, "pName").ToString();
            //List<JD> jds = CalHelper.CalJD(JDsModify[JDsModify.Count - 1], mainForm.qdBefore, mainForm.zdBefore, rowIndex, Convert.ToDouble(editor.EditValue));
            //JDsModify.Add(jds);
            //calcError(jds, mainForm.BPDsBefore);
            //// 最后一次修改的偏差集合
            //List<Deviation> dev = mainForm.dev[mainForm.dev.Count - 1];
            //paintError_hengxiang(jdName, derta, "a");
            //double res = Common.GetAzimuth(dev[dev.Count - 2].fym / 1000, dev[dev.Count - 2].designMile, dev[dev.Count - 1].fym / 1000, dev[dev.Count - 1].designMile);
            //res = (res - Math.PI / 2) * 180 / Math.PI;
            //this.textEdit1.Text = res.ToString();


            ButtonEdit editor = (ButtonEdit)sender;


            int rowIndex = gridView1.FocusedRowHandle;
            var d = (Convert.ToDouble(editor.EditValue) - Convert.ToDouble(mainForm.JDsBefore[rowIndex].a));
            if (d == 0) {
                XtraMessageBox.Show("转向角没有变化哟~", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var path = @"C:\Users\Administrator\Desktop\转向角.csv";
            //var path = @"C:\Users\Rick\Desktop\转向角.csv";
            try {
                if (File.Exists(path)) {
                    File.Delete(path);
                }
                StreamWriter myWrite = File.AppendText(path);
                for (int i = 0; i < Convert.ToInt32(this.textEdit1.Text); i++) {
                    var derta = (i + 1) * d;
                    mainForm.JDsBefore[rowIndex].a += d;
                    List<JD> jds = CalHelper.CalJD(mainForm.JDsBefore, mainForm.qdBefore, mainForm.zdBefore, rowIndex, mainForm.JDsBefore[rowIndex].a);
                    calcError(jds, mainForm.BPDsBefore);
                    // 最后一次修改的偏差集合
                    List<Deviation> dev = mainForm.dev[mainForm.dev.Count - 1];

                    double res = Common.GetAzimuth(dev[dev.Count - 2].fym / 1000, dev[dev.Count - 2].designMile, dev[dev.Count - 1].fym / 1000, dev[dev.Count - 1].designMile);
                    res = (res - Math.PI / 2) * 180 / Math.PI;

                    myWrite.WriteLine(
                        mainForm.JDsBefore[rowIndex].pName.ToString().PadRight(13) + "," +
                        mainForm.JDsBefore[rowIndex].a.ToString("#0.0000").PadRight(16) + "," +
                        d.ToString("#0.00").PadRight(20) + "," +
                        derta.ToString("#0.00").PadRight(20) + "," +
                        res.ToString("#0.000").PadRight(16)
                        );
                }
                myWrite.Close();
                MessageBox.Show("计算完成");
                // mainForm.JDsBefore[rowIndex].R = Convert.ToDouble(editor.EditValue) - d;
                gridView1.SetRowCellValue(rowIndex, "a", mainForm.JDsBefore[rowIndex].a);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }

        }

        private double angle(int x1, int y1, int x2, int y2) {
            double res = 0;
            return res;
        }

        /// <summary>
        /// 修改竖曲线半径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonEdit_SQX_H_ButtonClick(object sender, ButtonPressedEventArgs e) {
            ButtonEdit editor = (ButtonEdit)sender;
            //EditorButton button = e.Button;
            int rowIndex = gridView2.FocusedRowHandle;
            if (Convert.ToDouble(editor.EditValue) == BPDsModify[BPDsModify.Count - 1][rowIndex].H) {
                XtraMessageBox.Show("高程没有变化哟~", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            double derta = Math.Abs(BPDsModify[0][rowIndex].H - Convert.ToDouble(editor.EditValue));


            gridView2.CloseEditor();
            gridView2.UpdateCurrentRow();
            gridView2.CloseEditForm();

            mainForm.BPDsBefore[rowIndex].H = Convert.ToDouble(gridView2.GetRowCellValue(rowIndex, "H"));
            BPDsModify.Add(mainForm.BPDsBefore);

            calcError(mainForm.JDsBefore, mainForm.BPDsBefore);
            paintError_chuixiang(Math.Round(derta, 3));
        }

        private void Adjust_VisibleChanged(object sender, EventArgs e) {
            if (this.Visible == true && mainForm.isImport) {

                ctclR.Series.Clear();
                ctcl_C.Series.Clear();

                JDsModify.Clear();
                BPDsModify.Clear();

                if (JDsModify.Count == 0) {
                    List<JD> jds = listClone<JD>(mainForm.JDsBefore);
                    JDsModify.Add(jds);
                }
                JDs = new DataTable("JDsBefore");
                JDs.Columns.Add("pName", typeof(string));
                JDs.Columns.Add("X", typeof(double));
                JDs.Columns.Add("Y", typeof(double));
                JDs.Columns.Add("R", typeof(double));
                JDs.Columns.Add("a", typeof(double));
                for (int i = 0; i < mainForm.JDsBefore.Count; i++) {
                    DataRow dr = JDs.NewRow();
                    dr["pName"] = mainForm.JDsBefore[i].pName;
                    dr["X"] = mainForm.JDsBefore[i].X;
                    dr["Y"] = mainForm.JDsBefore[i].Y;
                    dr["R"] = mainForm.JDsBefore[i].R;
                    dr["a"] = Math.Round(mainForm.JDsBefore[i].a, 5);
                    JDs.Rows.Add(dr);
                }
                gdclJD.DataSource = JDs;


                if (BPDsModify.Count == 0) {
                    List<BPD> bpds = listClone<BPD>(mainForm.BPDsBefore);
                    BPDsModify.Add(bpds);
                }
                BPDs = new DataTable("sqx");
                BPDs.Columns.Add("mileage", typeof(double));
                BPDs.Columns.Add("H", typeof(double));
                BPDs.Columns.Add("R", typeof(double));
                BPDs.Columns.Add("i", typeof(double));
                for (int i = 0; i < mainForm.BPDsBefore.Count; i++) {
                    DataRow dr = BPDs.NewRow();
                    dr["mileage"] = mainForm.BPDsBefore[i].mileage;
                    dr["H"] = mainForm.BPDsBefore[i].H;
                    dr["R"] = mainForm.BPDsBefore[i].R;
                    dr["i"] = Math.Round(mainForm.BPDsBefore[i].i, 5);
                    BPDs.Rows.Add(dr);
                }
                gdclBPD.DataSource = BPDs;
            }
        }
    }
}

