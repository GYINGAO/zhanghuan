using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using qqqq.models;

namespace qqqq.utils {
    /// <summary>
    /// 文件操作类
    /// </summary>
    public class FileHelper {
        /// <summary>
        /// 导出平曲线为tdt
        /// </summary>
        /// <param name="horizontal">平曲线对象数组</param>
        /// <param name="path">导出路径</param>
        public static void ExportHorizontalTxt(List<Horizontal> horizontal, string path) {
            using (StreamWriter sw = new StreamWriter(path)) {
                sw.WriteLine(" ------------------------------平曲线（对应设计中线）-------------------------------- \r\n\r\n" +
                 "里程(m)     主点类型     北坐标X(m)     东坐标Y(m)     半径(m)（左—右+）     缓和曲线/圆曲线长(m)     主点切线方位角(º)     超高(mm)     超高设置方式     顺坡距离     加宽值     加宽距离\r\n");
                foreach (var item in horizontal) {
                    sw.WriteLine(
                        item.mileage.ToString("#0.0000").PadRight(16) +
                        item.pType.PadRight(16) +
                        item.X.ToString("#0.0000").PadRight(16) +
                        item.Y.ToString("#0.0000").PadRight(16) +
                        item.R.ToString("#0.0000").PadRight(16) +
                        item.l.ToString("#0.0000").PadRight(16) +
                        item.A.ToString("#0.0000").PadRight(16) +
                        item.h.ToString("#0.0000").PadRight(16) +
                        item.hType.PadRight(16) +
                        item.sp.ToString("#0.0000").PadRight(16) +
                        item.widen.ToString("#0.0000").PadRight(16) +
                        item.jk.ToString("#0.00").PadRight(16)
                        );
                }
            }
        }

        /// <summary>
        /// 导出竖曲线为txt
        /// </summary>
        /// <param name="vertical">竖曲线对象数组</param>
        /// <param name="path">导出路径</param>
        public static void ExportVerticalTxt(List<Vertical> vertical, string path) {
            using (StreamWriter sw = new StreamWriter(path)) {
                sw.WriteLine(" ------------------------------竖曲线（对应设计中线）-------------------------------- \r\n\r\n" +
                 "里程(m)          主点类型          高程(m)          坡度(‰)          半径(m)（凸+凹—）          切线长(m)          竖曲线长(m)\r\n");
                foreach (var item in vertical) {
                    sw.WriteLine(
                        item.mileage.ToString("#0.0000").PadRight(16) +
                        item.pType.PadRight(16) +
                        item.H.ToString("#0.0000").PadRight(16) +
                        item.i.ToString("#0.0000").PadRight(16) +
                        item.R.ToString("#0.0000").PadRight(16) +
                        item.T.ToString("#0.0000").PadRight(16) +
                        item.vCurve.ToString("#0.0000").PadRight(16)
                        );
                }
            }
        }

        /// <summary>
        /// 导入曲线表
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>曲线表</returns>
        public static List<JD> ImportPlaneParam(string path, out ZX qd, out ZX zd) {
            //JDs对象用于盛放曲线表文件，做返回值使用
            List<JD> JDs = new List<JD>();
            ZX QD = new ZX();//起点
            ZX ZD = new ZX();//终点
            using (StreamReader sr = new StreamReader(path)) {
                //跳过前1行
                sr.ReadLine();
                string line;
                while (!string.IsNullOrEmpty(line = sr.ReadLine())) //读取，并且当读取不为空时
                {
                    string[] array = Regex.Split(line.Trim(), ","); //分割
                    if (array.Length == 4) //起点
                    {
                        QD.pName = "QD";
                        QD.mileage = Convert.ToDouble(array[3]);
                        QD.X = Convert.ToDouble(array[1]);
                        QD.Y = Convert.ToDouble(array[2]);
                    }
                    else if (array.Length == 3) //终点
                    {
                        ZD.pName = "ZD";
                        ZD.X = Convert.ToDouble(array[1]);
                        ZD.Y = Convert.ToDouble(array[2]);
                    }
                    else {
                        JD jd = new JD();
                        jd.pName = array[0];//点名
                        jd.X = Convert.ToDouble(array[1]);//X
                        jd.Y = Convert.ToDouble(array[2]);//Y
                        jd.a = Common.str2Degrees(array[3]);//转向角
                        jd.R = Convert.ToDouble(array[4]);//半径
                        jd.l1 = Convert.ToDouble(array[5]);//第一缓长
                        jd.l2 = string.IsNullOrWhiteSpace(array[6]) ? jd.l1 : Convert.ToDouble(array[6]);//第二缓长
                        jd.h = Convert.ToDouble(array[7]);//超高
                        jd.hType = string.IsNullOrWhiteSpace(array[8]) ? "WG" : array[8];//超高设置方式
                        jd.curveType = string.IsNullOrWhiteSpace(array[9]) ? 0 : Convert.ToInt32(array[9]);//曲线类型
                        jd.sp1 = string.IsNullOrWhiteSpace(array[10]) ? 0 : Convert.ToDouble(array[10]);//第一顺坡距离
                        jd.sp2 = string.IsNullOrWhiteSpace(array[11]) ? 0 : Convert.ToDouble(array[11]);//第二顺坡距离
                        jd.sp3 = string.IsNullOrWhiteSpace(array[12]) ? 0 : Convert.ToDouble(array[12]);//第三顺坡距离
                        jd.sp4 = string.IsNullOrWhiteSpace(array[13]) ? 0 : Convert.ToDouble(array[13]);//第四顺坡距离
                        jd.widening = string.IsNullOrWhiteSpace(array[14]) ? 0 : Convert.ToDouble(array[14]);//加宽
                        jd.jk1 = string.IsNullOrWhiteSpace(array[15]) ? 0 : Convert.ToDouble(array[15]);//第一加宽距离
                        jd.jk2 = string.IsNullOrWhiteSpace(array[16]) ? 0 : Convert.ToDouble(array[16]);//第二加宽距离
                        jd.jk3 = string.IsNullOrWhiteSpace(array[17]) ? 0 : Convert.ToDouble(array[17]);//第三加宽距离
                        jd.jk4 = string.IsNullOrWhiteSpace(array[18]) ? 0 : Convert.ToDouble(array[18]);//第四加宽距离
                        JDs.Add(jd);
                    }
                }
            }
            qd = QD;
            zd = ZD;
            return JDs;
        }
        /// <summary>
        /// 导入坡度表
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>坡度表</returns>
        public static List<BPD> ImportProfileParam(string path) {
            List<BPD> bpds = new List<BPD>();
            using (StreamReader sr = new StreamReader(path)) {
                //跳过前1行
                sr.ReadLine();
                string line;
                while (!string.IsNullOrEmpty(line = sr.ReadLine())) //读取，并且当读取不为空时
                {
                    string[] array = Regex.Split(line.Trim(), ","); //分割
                    BPD bpd = new BPD();
                    bpd.mileage = Convert.ToDouble(array[0]);
                    bpd.H = Convert.ToDouble(array[1]);
                    bpd.R = Convert.ToDouble(array[2]);
                    bpd.i = Convert.ToDouble(array[3]);
                    bpds.Add(bpd);
                }
            }
            return bpds;
        }
        /// <summary>
        /// 导入里程文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>里程list</returns>
        public static List<double> ImportMiles(string path) {
            List<double> miles = new List<double>();
            using (StreamReader sr = new StreamReader(path)) {
                //跳过首行
                sr.ReadLine();
                string line;
                while (!string.IsNullOrEmpty(line = sr.ReadLine())) //读取，并且当读取不为空时
                {
                    double mile = Convert.ToDouble(line.Trim());
                    miles.Add(mile);
                }
            }
            return miles;
        }


        public static void list2txt(List<JD> ls, string path) {
            using (StreamWriter sw = new StreamWriter(path)) {
                foreach (var item in ls) {
                    sw.WriteLine(
                        item.pName.ToString().PadRight(16) +
                        item.X.ToString("#0.0000").PadRight(16) +
                        item.Y.ToString("#0.0000").PadRight(16) +
                        item.a.ToString("#0.0000").PadRight(16) +
                        item.R.ToString("#0.0000").PadRight(16)
                        );
                }
            }
        }

        public static void list2txt(List<Horizontal> ls, string path) {
            using (StreamWriter sw = new StreamWriter(path)) {
                foreach (var item in ls) {
                    sw.WriteLine(
                        item.pType.ToString().PadRight(16) +
                        item.X.ToString("#0.0000").PadRight(16) +
                        item.Y.ToString("#0.0000").PadRight(16) +
                        item.mileage.ToString("#0.0000").PadRight(16) +
                        item.R.ToString("#0.0000").PadRight(16)
                        );
                }
            }
        }

        /// <summary>
        /// 导出坐标为txt
        /// </summary>
        /// <param name="coordinates">坐标对象数组</param>
        /// <param name="path">导出路径</param>
        public static void ExportCoordinatesTxt(List<Coordinate> coordinates, string path) {
            using (StreamWriter sw = new StreamWriter(path)) {
                sw.WriteLine(" ------------------------------生成里程的三维坐标-------------------------------- \r\n\r\n" +
                 "平面里程(m)    轨面里程(m)     中线北坐标X(m)  中线东坐标Y(m)  中线高程H(m)    左轨北坐标X(m)  左轨东坐标Y(m)  左轨高程H(m)    右轨北坐标X(m)  右轨东坐标Y(m)  右轨高程H(m) 切线方位角(rad)  点类型\r\n");
                foreach (var item in coordinates) {
                    sw.WriteLine(
                        item.mileage.ToString("#0.0000").PadRight(16) +
                        item.spaceMile.ToString("#0.0000").PadRight(16) +
                        item.X.ToString("#0.0000").PadRight(16) +
                        item.Y.ToString("#0.0000").PadRight(16) +
                        item.H.ToString("#0.0000").PadRight(16) +
                        item.Xl.ToString("#0.0000").PadRight(16) +
                        item.Yl.ToString("#0.0000").PadRight(16) +
                        item.Hl.ToString("#0.0000").PadRight(16) +
                        item.Xr.ToString("#0.0000").PadRight(16) +
                        item.Yr.ToString("#0.0000").PadRight(16) +
                        item.Hr.ToString("#0.0000").PadRight(16) +
                        item.A.ToString("#0.0000000000").PadRight(16) +//在生成三维坐标文件中加入任一点横向方位角
                        item.pType
                        );
                }
            }
        }

        /// <summary>
        /// 导出坐标为csv
        /// </summary>
        /// <param name="coordinates">坐标对象数组</param>
        /// <param name="path">导出路径</param>
        public static void ExportCoordinatesCsv(List<Coordinate> coordinates, string path) {
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8)) {

                sw.WriteLine(" ------------------------------生成里程的三维坐标-------------------------------- \r\n\r\n" +
                 "平面里程(m),轨面里程(m),中线北坐标X(m),中线东坐标Y(m),中线高程H(m),左轨北坐标X(m),左轨东坐标Y(m),左轨高程H(m),右轨北坐标X(m),右轨东坐标Y(m),右轨高程H(m),竖向方位角(rad),点类型\r\n");
                foreach (var item in coordinates) {
                    sw.WriteLine(
                        item.mileage.ToString("#0.0000") + "," +
                        item.spaceMile.ToString("#0.0000") + "," +
                        item.X.ToString("#0.0000") + "," +
                        item.Y.ToString("#0.0000") + "," +
                        item.H.ToString("#0.0000") + "," +
                        item.Xl.ToString("#0.0000") + "," +
                        item.Yl.ToString("#0.0000") + "," +
                        item.Hl.ToString("#0.0000") + "," +
                        item.Xr.ToString("#0.0000") + "," +
                        item.Yr.ToString("#0.0000") + "," +
                        item.Hr.ToString("#0.0000") + "," +
                        item.w.ToString("#0.0000000000").PadRight(16) +//在生成三维坐标文件中加入任一点竖向方位角
                        item.pType
                        );
                }
            }
        }

        /// <summary>
        /// 导出横垂偏差为txt
        /// </summary>
        /// <param name="deviations">横垂偏差对象数组</param>
        /// <param name="path">导出路径</param>
        public static void ExportDeviationTxt(List<Deviation> deviations, string path) {
            using (StreamWriter sw = new StreamWriter(path)) {
                sw.WriteLine("点号        反算里程(m)  中线横偏(mm)       中线垂偏(mm)  左轨横偏(mm)      左轨垂偏(mm)  右轨横偏(mm)      右轨垂偏(mm)");
                foreach (var item in deviations) {
                    sw.WriteLine(
                        item.ID.PadRight(12) +
                        item.designMile.ToString("#0.0000").PadRight(16) +
                        item.fym.ToString("#0.00").PadRight(16) +
                        item.fhm.ToString("#0.00").PadRight(16) +
                        item.fyl.ToString("#0.00").PadRight(16) +
                        item.fhl.ToString("#0.00").PadRight(16) +
                        item.fyr.ToString("#0.00").PadRight(16) +
                        item.fhr.ToString("#0.00").PadRight(16)
                        );
                }
            }
        }

        /// <summary>
        /// 导出横垂偏差为txt
        /// </summary>
        /// <param name="deviations">横垂偏差对象数组</param>
        /// <param name="path">导出路径</param>
        public static void ExportDeviationTxt_2(List<Deviation> deviations, string path) {
            using (StreamWriter sw = new StreamWriter(path)) {
                sw.WriteLine("反算里程(m)       中线横偏(mm)       中线垂偏(mm)       点类型");
                foreach (var item in deviations) {
                    sw.WriteLine(
                        item.designMile.ToString("#0.0000").PadRight(16) +
                        item.fym.ToString("#0.00").PadRight(16) +
                        item.fhm.ToString("#0.00").PadRight(16) +
                        item.ID
                        );
                }
            }
        }

        /// <summary>
        /// 导出横垂偏差为csv
        /// </summary>
        /// <param name="deviations">横垂偏差对象数组</param>
        /// <param name="path">导出路径</param>
        public static void ExportDeviationCsv(List<Deviation> deviations, string path) {
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8)) {
                sw.WriteLine("反算里程(m),中线横偏(mm),中线垂偏(mm),点类型");
                foreach (var item in deviations) {
                    sw.WriteLine(
                        item.designMile.ToString("#0.0000") + "," +
                        item.fym.ToString("#0.00") + "," +
                        item.fhm.ToString("#0.00") + "," +
                        item.ID
                        );
                }
            }
        }
    }
}
