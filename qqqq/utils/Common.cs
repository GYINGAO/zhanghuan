using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace qqqq.utils {
    /// <summary>
    /// 公共计算类
    /// </summary>
    public class Common {
        /// <summary>
        /// 计算方位角
        /// </summary>
        /// <param name="x1">原点X</param>
        /// <param name="y1">原点Y</param>
        /// <param name="x2">方向点X</param>
        /// <param name="y2">方向点Y</param>
        /// <returns>方位角</returns>
        internal static double GetAzimuth(double x1, double y1, double x2, double y2) {
            if ((x2 - x1) != 0 && (y2 - y1) != 0) {
                double A = Math.Abs(Math.Atan(Math.Abs(y2 - y1) / Math.Abs(x2 - x1)));
                if ((x2 - x1) > 0 && (y2 - y1) > 0)//第一象限
                { A = Math.Atan(Math.Abs(y2 - y1) / Math.Abs(x2 - x1)); }
                if ((x2 - x1) < 0 && (y2 - y1) > 0)//第二象限
                { A = Math.PI - A; }
                if ((x2 - x1) < 0 && (y2 - y1) < 0)//第三象限
                { A = Math.PI + A; }
                if ((x2 - x1) > 0 && (y2 - y1) < 0)//第四象限
                { A = 2 * Math.PI - A; }
                return A;
            }
            else {
                double A = 0.0;
                if (x2 > x1 && y2 == y1)//与X正轴平行
                { A = 0; }
                if (x2 < x1 && y2 == y1)//与X负轴平行
                { A = Math.PI; }
                if (x2 == x1 && y2 > y1)//与Y正轴平行
                { A = Math.PI / 2; }
                if (x2 == x1 && y2 < y1)//与Y负轴平行
                { A = 3 * Math.PI / 2; }
                if (x2 == x1 && y2 == y1) { }//两点重合
                return A;
            }
        }
        /// <summary>
        /// 牛顿迭代法计算
        /// </summary>
        /// <param name="R"></param>
        /// <param name="l0"></param>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <returns></returns>
        internal static double Newton(double R, double l0, double x0, double y0)//原始数据半径，缓和曲线长，实测坐标
        {
            double xnn = x0, xn = x0;
            R = Math.Abs(R);
            do {
                xn = xnn;
                double a = (xn - x0 - Math.Pow(xn, 2) * y0 / (2.0 * R * l0) + Math.Pow(xn, 4) * x0 / (8.0 * R * R * l0 * l0) - Math.Pow(xn, 5) / (15.0 * R * R * l0 * l0)
                    + Math.Pow(xn, 6) * y0 / (48.0 * R * R * R * l0 * l0 * l0) - Math.Pow(xn, 8) * x0 / (384.0 * R * R * R * R * l0 * l0 * l0 * l0)
                    + Math.Pow(xn, 9) / (945.0 * R * R * R * R * l0 * l0 * l0 * l0) - 19 * Math.Pow(xn, 13) / (483840.0 * R * R * R * R * R * R * l0 * l0 * l0 * l0 * l0 * l0));
                double b = (1.0 - Math.Pow(xn, 1) * y0 / (R * l0)
                    + 3 * Math.Pow(xn, 3) * x0 / (8.0 * R * R * l0 * l0) - Math.Pow(xn, 4) / (3.0 * R * R * l0 * l0)
                    + Math.Pow(xn, 5) * y0 / (8.0 * R * R * R * l0 * l0 * l0) - Math.Pow(xn, 7) * x0 / (48.0 * R * R * R * R * l0 * l0 * l0 * l0)
                    + Math.Pow(xn, 8) / (105.0 * R * R * R * R * l0 * l0 * l0 * l0));
                xnn = xn - a / b;
            }
            while (Math.Abs(xnn - xn) > 0.000005);//保证截断误差小于0.005mm
            return xnn;
        }

        /// <summary>
        /// 将字符串类型的度分秒转换成度
        /// </summary>
        /// <param name="dfm">度分秒</param>
        /// <returns>度</returns>
        internal static double str2Degrees(string dfm) {
            Regex regex = new Regex("'|’|‘|′");
            string[] arr = regex.Split(dfm);
            int degrees = Convert.ToInt32(arr[0]);
            double minutes = Convert.ToDouble(arr[1]);
            double seconds = Convert.ToDouble(arr[2]);
            int sign = Math.Sign(degrees);
            //例 -0'26'37 的情况
            if (sign == 0 && arr[0].IndexOf("-") > -1) {
                return -(minutes / 60 + seconds / 3600);
            }
            //例 0'26'37 的情况
            if (sign == 0) {
                return degrees + minutes / 60 + seconds / 3600;
            }
            //其他情况
            return sign * (Math.Abs(degrees) + minutes / 60 + seconds / 3600);
        }

        public static DataTable gridView2dt(GridView gv) {
            DataTable dt = new DataTable();
            // 列强制转换
            for (int count = 0; count < gv.Columns.Count; count++) {
                DataColumn dc = new DataColumn(gv.Columns[count].Name.ToString());
                dt.Columns.Add(dc);
            }

            // 循环行
            for (int i = 0; i < gv.RowCount; i++) {
                DataRowView row = (DataRowView)gv.GetRow(i);
                DataRow dr = dt.NewRow();
                for (int countsub = 0; countsub < gv.Columns.Count; countsub++) {
                    dr[countsub] = Convert.ToString(row.Row.ItemArray[countsub]);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
