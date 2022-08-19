using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using qqqq.models;

namespace qqqq.utils {
    /// <summary>
    /// 计算类
    /// </summary>
    public class CalHelper {
        /// <summary>
        /// 计算平曲线
        /// </summary>
        /// <param name="JDs">曲线表</param>
        /// <param name="qd">起点对象</param>
        /// <param name="zd">终点对象</param>
        /// <returns>平曲线list</returns>
        public static List<Horizontal> CalHorizontal(ref List<JD> JDs, ZX qd, ZX zd) {

            List<Horizontal> horizontals = new List<Horizontal>();
            double[] A = new double[JDs.Count + 1];//交点方位角数组
            double[] d = new double[JDs.Count + 1];//交点间距数组
            #region 求方位角及交点间距
            A[0] = Common.GetAzimuth(qd.X, qd.Y, JDs[0].X, JDs[0].Y);
            d[0] = Math.Sqrt(Math.Pow(JDs[0].Y - qd.Y, 2) + Math.Pow(JDs[0].X - qd.X, 2));
            for (int i = 1; i < JDs.Count; i++) {
                //求取方位角
                A[i] = Common.GetAzimuth(
                  JDs[i - 1].X,
                  JDs[i - 1].Y,
                  JDs[i].X,
                  JDs[i].Y
                );
                //求交点间距
                d[i] = Math.Sqrt(Math.Pow(JDs[i].Y - JDs[i - 1].Y, 2) + Math.Pow(JDs[i].X - JDs[i - 1].X, 2));
            }
            JD lastJd = JDs[JDs.Count - 1];//获得最后一个交点的值（不是终点）
            A[A.Length - 1] = Common.GetAzimuth(lastJd.X, lastJd.Y, zd.X, zd.Y);//最后一个方位角
            d[d.Length - 1] = Math.Sqrt(Math.Pow(zd.Y - lastJd.Y, 2) + Math.Pow(zd.X - lastJd.X, 2));//最后一个点间距
            #endregion
            double[] a = new double[JDs.Count];//转向角
            //曲线要素
            double[] L = new double[JDs.Count];//曲线长
            double[] T1 = new double[JDs.Count];
            double[] T2 = new double[JDs.Count];
            double[] p1 = new double[JDs.Count];//圆曲线内移量
            double[] p2 = new double[JDs.Count];
            double[] m1 = new double[JDs.Count];//切垂距
            double[] m2 = new double[JDs.Count];
            #region 求曲线要素
            for (int i = 0; i < JDs.Count; i++) {
                JD JD = JDs[i];
                a[i] = A[i + 1] - A[i]; //求转角
                //使得该回头曲线的转向角绝对值始终大于180
                if (JD.curveType == 1 && Math.Abs(a[i]) < Math.PI && a[i] < 0) {
                    a[i] += 2 * Math.PI;
                }
                if (JD.curveType == 1 && a[i] < Math.PI && a[i] > 0) {
                    a[i] -= 2 * Math.PI;
                }
                JDs[i].a = a[i];
                //反向曲线情况
                if (JD.curveType == 2) a[i] += Math.PI;
                //修正南北向线性转向角出错
                else if (JD.curveType != 1 && a[i] > 0 && Math.Abs(a[i]) >= Math.PI)
                    a[i] -= 2 * Math.PI;
                else if (JD.curveType != 1 && a[i] < 0 && Math.Abs(a[i]) >= Math.PI)
                    a[i] += 2 * Math.PI;
                //转折点情况（曲线要素的计算）
                if (JD.R < 0.001) L[i] = T2[i] = T1[i] = p2[i] = p1[i] = m2[i] = m1[i] = 0.0;
                //回头曲线情况
                else if (JD.R > 0.001 && Math.Abs(a[i]) > Math.PI) {
                    p1[i] = Math.Pow(JD.l1, 2) / (24 * JD.R) -
                            Math.Pow(JD.l1, 4) / (2688 * Math.Pow(JD.R, 3));
                    m1[i] = JD.l1 / 2 - Math.Pow(JD.l1, 3) / (240 * JD.R * JD.R);
                    T1[i] = (JD.R + p1[i]) * Math.Tan(Math.PI - Math.Abs(a[i]) / 2) - m1[i];
                    T2[i] = T1[i];
                    L[i] = JD.R * (Math.Abs(a[i]) - (JD.l1 + JD.l2) / 2 / JD.R) + JD.l1 + JD.l2;
                }
                //正常情况
                else if (JD.R > 0.001 && Math.Abs(a[i]) < Math.PI) {
                    p1[i] = Math.Pow(JD.l1, 2) / (24 * JD.R) -
                            Math.Pow(JD.l1, 4) / (2688 * Math.Pow(JD.R, 3));
                    p2[i] = Math.Pow(JD.l2, 2) / (24 * JD.R) -
                            Math.Pow(JD.l2, 4) / (2688 * Math.Pow(JD.R, 3));
                    m1[i] = JD.l1 / 2 - Math.Pow(JD.l1, 3) / (240 * JD.R * JD.R);
                    m2[i] = JD.l2 / 2 - Math.Pow(JD.l2, 3) / (240 * JD.R * JD.R);
                    T1[i] = (JD.R + p1[i]) * Math.Tan(Math.Abs(a[i] / 2)) +
                            m1[i] - (p1[i] - p2[i]) / Math.Sin(Math.Abs(a[i]));
                    T2[i] = (JD.R + p2[i]) * Math.Tan(Math.Abs(a[i] / 2)) +
                            m2[i] + (p1[i] - p2[i]) / Math.Sin(Math.Abs(a[i]));
                    L[i] = JD.R * (Math.Abs(a[i]) - (JD.l1 + JD.l2) / 2 / JD.R) + JD.l1 + JD.l2;
                }

            }
            #endregion
            //重新求取方位角，用于验证
            if (A[0] < 0) A[0] += 2 * Math.PI;
            for (int j = 1; j < JDs.Count + 1; j++) {
                A[j] = A[j - 1] + a[j - 1];
                A[j] = A[j] <= 0 ? A[j] + 2 * Math.PI : A[j];
                A[j] = A[j] >= 2 * Math.PI ? A[j] - 2 * Math.PI : A[j];
            }
            //主点坐标
            double[] ZH_X = new double[JDs.Count];
            double[] HY_X = new double[JDs.Count];
            double[] YH_X = new double[JDs.Count];
            double[] HZ_X = new double[JDs.Count];
            double[] ZH_Y = new double[JDs.Count];
            double[] HY_Y = new double[JDs.Count];
            double[] YH_Y = new double[JDs.Count];
            double[] HZ_Y = new double[JDs.Count];
            //主点方位角
            double[] ZH_A = new double[JDs.Count];
            double[] HY_A = new double[JDs.Count];
            double[] YH_A = new double[JDs.Count];
            double[] HZ_A = new double[JDs.Count];
            //主点里程
            double[] ZH_m = new double[JDs.Count];
            double[] HY_m = new double[JDs.Count];
            double[] YH_m = new double[JDs.Count];
            double[] HZ_m = new double[JDs.Count];
            //独立坐标系坐标
            double x_hy = 0;
            double y_hy = 0;
            double x_yh = 0;
            double y_yh = 0;

            #region 对起点进行处理
            JD plane1st = JDs[0];
            if (plane1st.R < 0.001) {
                ZH_X[0] = HY_X[0] = YH_X[0] = HZ_X[0] = plane1st.X;
                ZH_Y[0] = HY_Y[0] = YH_Y[0] = HZ_Y[0] = plane1st.Y;
                ZH_A[0] = HY_A[0] = A[0];
                YH_A[0] = HZ_A[0] = A[1];
                ZH_m[0] = HY_m[0] = YH_m[0] = HZ_m[0] = qd.mileage + d[0];
            }
            //回头曲线问题
            else if (plane1st.R > 0.001 && Math.Abs(a[0]) > Math.PI) {
                if (T1[0] > 0) {
                    ZH_X[0] = plane1st.X + T1[0] * Math.Cos(A[0]);
                    ZH_Y[0] = plane1st.Y + T1[0] * Math.Sin(A[0]);
                    HZ_X[0] = plane1st.X + T2[0] * Math.Cos(A[1] + Math.PI);
                    HZ_Y[0] = plane1st.Y + T2[0] * Math.Sin(A[1] + Math.PI);
                }
                else if (T1[0] < 0) {
                    ZH_X[0] = plane1st.X + Math.Abs(T1[0]) * Math.Cos(A[0] + Math.PI);
                    ZH_Y[0] = plane1st.Y + Math.Abs(T1[0]) * Math.Sin(A[0] + Math.PI);
                    HZ_X[0] = plane1st.X + Math.Abs(T2[0]) * Math.Cos(A[1]);
                    HZ_Y[0] = plane1st.Y + Math.Abs(T2[0]) * Math.Sin(A[1]);
                }
                ZH_A[0] = A[0];
                HZ_A[0] = A[1];
                //求HY,YH点在独立坐标系里的坐标
                x_hy = plane1st.l1 -
                       Math.Pow(plane1st.l1, 3) / (40 * Math.Pow(plane1st.R, 2)) +
                       Math.Pow(plane1st.l1, 5) / (3456 * Math.Pow(plane1st.R, 4));
                y_hy = Math.Pow(plane1st.l1, 2) / (6 * plane1st.R) -
                       Math.Pow(plane1st.l1, 4) / (336 * Math.Pow(plane1st.R, 3)) +
                       Math.Pow(plane1st.l1, 6) / (42240 * Math.Pow(plane1st.R, 5));
                y_hy = Math.Sign(a[0]) * y_hy;
                x_yh = plane1st.l2 -
                       Math.Pow(plane1st.l2, 3) / (40 * Math.Pow(plane1st.R, 2)) +
                       Math.Pow(plane1st.l2, 5) / (3456 * Math.Pow(plane1st.R, 4));
                y_yh = Math.Pow(plane1st.l2, 2) / (6 * plane1st.R) -
                       Math.Pow(plane1st.l2, 4) / (336 * Math.Pow(plane1st.R, 3)) +
                       Math.Pow(plane1st.l2, 6) / (42240 * Math.Pow(plane1st.R, 5));
                y_yh = -Math.Sign(a[0]) * y_yh;
                //求HY,YH点在线路坐标系里的坐标及方位角
                HY_X[0] = ZH_X[0] + x_hy * Math.Cos(ZH_A[0]) - y_hy * Math.Sin(ZH_A[0]);
                HY_Y[0] = ZH_Y[0] + x_hy * Math.Sin(ZH_A[0]) + y_hy * Math.Cos(ZH_A[0]);
                HY_A[0] = ZH_A[0] + (Math.Sign(a[0]) * plane1st.l1) / (2 * plane1st.R);
                YH_X[0] = HZ_X[0] +
                          x_yh * Math.Cos(HZ_A[0] + Math.PI) -
                          y_yh * Math.Sin(HZ_A[0] + Math.PI);
                YH_Y[0] = HZ_Y[0] +
                          x_yh * Math.Sin(HZ_A[0] + Math.PI) +
                          y_yh * Math.Cos(HZ_A[0] + Math.PI);
                YH_A[0] = HZ_A[0] - (Math.Sign(a[0]) * plane1st.l2) / (2 * plane1st.R);
                //求取主点的里程
                ZH_m[0] = qd.mileage + d[0] + T1[0];
                HY_m[0] = ZH_m[0] + plane1st.l1;
                YH_m[0] = ZH_m[0] + L[0] - plane1st.l2;
                HZ_m[0] = ZH_m[0] + L[0];
            }
            else//最常见情况
            {
                ZH_X[0] = plane1st.X + T1[0] * Math.Cos(A[0] + Math.PI);
                ZH_Y[0] = plane1st.Y + T1[0] * Math.Sin(A[0] + Math.PI);
                HZ_X[0] = plane1st.X + T2[0] * Math.Cos(A[1]);
                HZ_Y[0] = plane1st.Y + T2[0] * Math.Sin(A[1]);
                ZH_A[0] = A[0];
                HZ_A[0] = A[1];
                //求HY,YH点在独立坐标系里的坐标
                x_hy = plane1st.l1 -
                       Math.Pow(plane1st.l1, 3) / (40 * Math.Pow(plane1st.R, 2)) +
                       Math.Pow(plane1st.l1, 5) / (3456 * Math.Pow(plane1st.R, 4));
                y_hy = Math.Pow(plane1st.l1, 2) / (6 * plane1st.R) -
                       Math.Pow(plane1st.l1, 4) / (336 * Math.Pow(plane1st.R, 3)) +
                       Math.Pow(plane1st.l1, 6) / (42240 * Math.Pow(plane1st.R, 5));
                y_hy = Math.Sign(a[0]) * y_hy;
                x_yh = plane1st.l2 -
                       Math.Pow(plane1st.l2, 3) / (40 * Math.Pow(plane1st.R, 2)) +
                       Math.Pow(plane1st.l2, 5) / (3456 * Math.Pow(plane1st.R, 4));
                y_yh = Math.Pow(plane1st.l2, 2) / (6 * plane1st.R) -
                       Math.Pow(plane1st.l2, 4) / (336 * Math.Pow(plane1st.R, 3)) +
                       Math.Pow(plane1st.l2, 6) / (42240 * Math.Pow(plane1st.R, 5));
                y_yh = -Math.Sign(a[0]) * y_yh;
                //求HY,YH点在线路坐标系里的坐标及方位角
                HY_X[0] = ZH_X[0] + x_hy * Math.Cos(ZH_A[0]) - y_hy * Math.Sin(ZH_A[0]);
                HY_Y[0] = ZH_Y[0] + x_hy * Math.Sin(ZH_A[0]) + y_hy * Math.Cos(ZH_A[0]);
                HY_A[0] = ZH_A[0] + (Math.Sign(a[0]) * plane1st.l1) / (2 * plane1st.R);
                YH_X[0] = HZ_X[0] +
                          x_yh * Math.Cos(HZ_A[0] + Math.PI) -
                          y_yh * Math.Sin(HZ_A[0] + Math.PI);
                YH_Y[0] = HZ_Y[0] +
                          x_yh * Math.Sin(HZ_A[0] + Math.PI) +
                          y_yh * Math.Cos(HZ_A[0] + Math.PI);
                YH_A[0] = HZ_A[0] - (Math.Sign(a[0]) * plane1st.l2) / (2 * plane1st.R);
                //求取主点的里程
                ZH_m[0] = qd.mileage + d[0] - T1[0];
                HY_m[0] = ZH_m[0] + plane1st.l1;
                YH_m[0] = ZH_m[0] + L[0] - plane1st.l2;
                HZ_m[0] = ZH_m[0] + L[0];
            }
            #endregion
            #region 求主点坐标里程
            for (int i = 1; i < JDs.Count; i++) {
                JD JD = JDs[i];
                //转折点情况
                if (JD.R < 0.001) {
                    ZH_X[i] = HY_X[i] = YH_X[i] = HZ_X[i] = JD.X;
                    ZH_Y[i] = HY_Y[i] = YH_Y[i] = HZ_Y[i] = JD.Y;
                    ZH_A[i] = HY_A[i] = A[i];
                    YH_A[i] = HZ_A[i] = A[i + 1];
                    if (Math.Abs(a[i - 1]) > Math.PI) {
                        ZH_m[i] = HY_m[i] = YH_m[i] = HZ_m[i] = HZ_m[i - 1] + d[i] + T2[i - 1];
                    }
                    else {
                        ZH_m[i] = HY_m[i] = YH_m[i] = HZ_m[i] = HZ_m[i - 1] + d[i] - T2[i - 1];
                    }
                }
                //回头曲线情况
                else if (JD.R > 0.001 && Math.Abs(a[i]) > Math.PI) {
                    //求ZH,HZ点坐标
                    if (T1[i] > 0) {
                        ZH_X[i] = JD.X + T1[i] * Math.Cos(A[i]);
                        ZH_Y[i] = JD.Y + T1[i] * Math.Sin(A[i]);
                        HZ_X[i] = JD.X + T2[i] * Math.Cos(A[i + 1] + Math.PI);
                        HZ_Y[i] = JD.Y + T2[i] * Math.Sin(A[i + 1] + Math.PI);
                    }
                    else if (T1[i] < 0) {
                        ZH_X[i] = JD.X + Math.Abs(T1[i]) * Math.Cos(A[i] + Math.PI);
                        ZH_Y[i] = JD.Y + Math.Abs(T1[i]) * Math.Sin(A[i] + Math.PI);
                        HZ_X[i] = JD.X + Math.Abs(T2[i]) * Math.Cos(A[i + 1]);
                        HZ_Y[i] = JD.Y + Math.Abs(T2[i]) * Math.Sin(A[i + 1]);
                    }
                    ZH_A[i] = A[i];
                    HZ_A[i] = A[i + 1];
                    //求HY,YH点在独立坐标系里的坐标
                    x_hy = JD.l1 -
                           Math.Pow(JD.l1, 3) / (40 * Math.Pow(JD.R, 2)) +
                           Math.Pow(JD.l1, 5) / (3456 * Math.Pow(JD.R, 4));
                    y_hy = Math.Pow(JD.l1, 2) / (6 * JD.R) -
                           Math.Pow(JD.l1, 4) / (336 * Math.Pow(JD.R, 3)) +
                           Math.Pow(JD.l1, 6) / (42240 * Math.Pow(JD.R, 5));
                    y_hy = Math.Sign(a[i]) * y_hy;
                    x_yh = JD.l2 -
                           Math.Pow(JD.l2, 3) / (40 * Math.Pow(JD.R, 2)) +
                           Math.Pow(JD.l2, 5) / (3456 * Math.Pow(JD.R, 4));
                    y_yh = Math.Pow(JD.l2, 2) / (6 * JD.R) -
                           Math.Pow(JD.l2, 4) / (336 * Math.Pow(JD.R, 3)) +
                           Math.Pow(JD.l2, 6) / (42240 * Math.Pow(JD.R, 5));
                    y_yh = -Math.Sign(a[i]) * y_yh;
                    //求HY,YH点在线路坐标系里的坐标及方位角
                    HY_X[i] = ZH_X[i] + x_hy * Math.Cos(ZH_A[i]) - y_hy * Math.Sin(ZH_A[i]);
                    HY_Y[i] = ZH_Y[i] + x_hy * Math.Sin(ZH_A[i]) + y_hy * Math.Cos(ZH_A[i]);
                    HY_A[i] = ZH_A[i] + (Math.Sign(a[i]) * JD.l1) / (2 * JD.R);
                    YH_X[i] = HZ_X[i] +
                              x_yh * Math.Cos(HZ_A[i] + Math.PI) -
                              y_yh * Math.Sin(HZ_A[i] + Math.PI);
                    YH_Y[i] = HZ_Y[i] +
                              x_yh * Math.Sin(HZ_A[i] + Math.PI) +
                              y_yh * Math.Cos(HZ_A[i] + Math.PI);
                    YH_A[i] = HZ_A[i] - (Math.Sign(a[i]) * JD.l2) / (2 * JD.R);
                    //求取主点的里程
                    ZH_m[i] = HZ_m[i - 1] - d[i] - T2[i - 1] + T1[i];
                    HY_m[i] = ZH_m[i] + JD.l1;
                    YH_m[i] = ZH_m[i] + L[i] - JD.l2;
                    HZ_m[i] = ZH_m[i] + L[i];
                }
                //无缓圆曲线但需超高顺坡的情况
                else if (JD.l1 < 0.001 && JD.l2 < 0.001 && JD.curveType == 3 && JD.R > 0.001) {
                    ZH_X[i] = JD.X + T1[i] * Math.Cos(A[i] + Math.PI);
                    ZH_Y[i] = JD.Y + T1[i] * Math.Sin(A[i] + Math.PI);
                    HZ_X[i] = JD.X + T2[i] * Math.Cos(A[i + 1]);
                    HZ_Y[i] = JD.Y + T2[i] * Math.Sin(A[i + 1]);
                    ZH_A[i] = A[i];
                    HZ_A[i] = A[i + 1];
                    //求HY,YH点在线路坐标系里的坐标及方位角
                    HY_X[i] = JD.X + T1[i] * Math.Cos(A[i] + Math.PI);
                    HY_Y[i] = JD.Y + T1[i] * Math.Sin(A[i] + Math.PI);
                    HY_A[i] = ZH_A[i];
                    YH_X[i] = JD.X + T2[i] * Math.Cos(A[i + 1]);
                    YH_Y[i] = JD.Y + T2[i] * Math.Sin(A[i + 1]);
                    YH_A[i] = HZ_A[i];
                    //求取主点的里程
                    if (Math.Abs(a[i - 1]) > Math.PI) {
                        ZH_m[i] = HZ_m[i - 1] + d[i] + T2[i - 1] - T1[i];
                    }
                    else {
                        ZH_m[i] = HZ_m[i - 1] + d[i] - T2[i - 1] - T1[i];
                    }
                    HY_m[i] = ZH_m[i];
                    YH_m[i] = ZH_m[i] + L[i];
                    HZ_m[i] = ZH_m[i] + L[i];
                }
                //无特殊曲线情况
                else {
                    //求ZH,HZ点坐标
                    ZH_X[i] = JD.X + T1[i] * Math.Cos(A[i] + Math.PI);
                    ZH_Y[i] = JD.Y + T1[i] * Math.Sin(A[i] + Math.PI);
                    HZ_X[i] = JD.X + T2[i] * Math.Cos(A[i + 1]);
                    HZ_Y[i] = JD.Y + T2[i] * Math.Sin(A[i + 1]);
                    ZH_A[i] = A[i];
                    HZ_A[i] = A[i + 1];
                    //求HY,YH点在独立坐标系里的坐标
                    x_hy = JD.l1 -
                           Math.Pow(JD.l1, 3) / (40 * Math.Pow(JD.R, 2)) +
                           Math.Pow(JD.l1, 5) / (3456 * Math.Pow(JD.R, 4));
                    y_hy = Math.Pow(JD.l1, 2) / (6 * JD.R) -
                           Math.Pow(JD.l1, 4) / (336 * Math.Pow(JD.R, 3)) +
                           Math.Pow(JD.l1, 6) / (42240 * Math.Pow(JD.R, 5));
                    y_hy = Math.Sign(a[i]) * y_hy;
                    x_yh = JD.l2 -
                           Math.Pow(JD.l2, 3) / (40 * Math.Pow(JD.R, 2)) +
                           Math.Pow(JD.l2, 5) / (3456 * Math.Pow(JD.R, 4));
                    y_yh = Math.Pow(JD.l2, 2) / (6 * JD.R) -
                           Math.Pow(JD.l2, 4) / (336 * Math.Pow(JD.R, 3)) +
                           Math.Pow(JD.l2, 6) / (42240 * Math.Pow(JD.R, 5));
                    y_yh = -Math.Sign(a[i]) * y_yh;
                    //求HY,YH点在线路坐标系里的坐标及方位角
                    HY_X[i] = ZH_X[i] + x_hy * Math.Cos(ZH_A[i]) - y_hy * Math.Sin(ZH_A[i]);
                    HY_Y[i] = ZH_Y[i] + x_hy * Math.Sin(ZH_A[i]) + y_hy * Math.Cos(ZH_A[i]);
                    HY_A[i] = ZH_A[i] + (Math.Sign(a[i]) * JD.l1) / (2 * JD.R);
                    YH_X[i] = HZ_X[i] +
                              x_yh * Math.Cos(HZ_A[i] + Math.PI) -
                              y_yh * Math.Sin(HZ_A[i] + Math.PI);
                    YH_Y[i] = HZ_Y[i] +
                              x_yh * Math.Sin(HZ_A[i] + Math.PI) +
                              y_yh * Math.Cos(HZ_A[i] + Math.PI);
                    YH_A[i] = HZ_A[i] - (Math.Sign(a[i]) * JD.l2) / (2 * JD.R);
                    //求取主点的里程
                    if (Math.Abs(a[i - 1]) > Math.PI) {
                        ZH_m[i] = HZ_m[i - 1] - d[i] + T2[i - 1] - T1[i];
                    }
                    else {
                        ZH_m[i] = HZ_m[i - 1] + d[i] - T2[i - 1] - T1[i];
                    }
                    HY_m[i] = ZH_m[i] + JD.l1;
                    YH_m[i] = ZH_m[i] + L[i] - JD.l2;
                    HZ_m[i] = ZH_m[i] + L[i];
                }
            }
            #endregion
            #region 添加数据
            Horizontal QD = new Horizontal();
            QD.mileage = qd.mileage;//里程
            QD.pType = "ZX";//主点类型
            QD.X = qd.X;//北坐标
            QD.Y = qd.Y;//东坐标
            QD.R = 0;//半径
            QD.l = 0;//缓长/圆曲线长
            QD.A = A[0];//切线方位角 弧度值
            horizontals.Add(QD);
            //根据平曲线要素判断主点类型
            for (int i = 0; i < JDs.Count; i++) {
                //转点情况
                if (JDs[i].R < 0.001) {
                    Horizontal ZD1 = new Horizontal();
                    ZD1.mileage = ZH_m[i];//里程
                    ZD1.pType = "ZD1";//主点类型 转点
                    ZD1.X = ZH_X[i];//北坐标
                    ZD1.Y = ZH_Y[i];//东坐标
                    ZD1.R = 0;//半径
                    ZD1.l = 0;//缓长/圆曲线长
                    ZD1.A = ZH_A[i];//切线方位角 弧度值
                    horizontals.Add(ZD1);

                    Horizontal ZD2 = new Horizontal();
                    ZD2.mileage = HZ_m[i];//里程
                    ZD2.pType = "ZD";//主点类型 转点
                    ZD2.X = HZ_X[i];//北坐标
                    ZD2.Y = HZ_Y[i];//东坐标
                    ZD2.R = 0;//半径
                    ZD2.l = 0;//缓长/圆曲线长
                    ZD2.A = HZ_A[i];//切线方位角 弧度值
                    horizontals.Add(ZD2);
                }
                //无缓圆曲线情况
                else if ((JDs[i].l1 < 0.001 && JDs[i].l2 < 0.001 && JDs[i].h < 0.001 && JDs[i].R > 0.001) || JDs[i].curveType == 3) {
                    Horizontal ZY = new Horizontal();
                    ZY.mileage = HY_m[i];//里程
                    ZY.pType = "ZY";//主点类型 转点
                    ZY.X = HY_X[i];//北坐标
                    ZY.Y = HY_Y[i];//东坐标
                    ZY.R = JDs[i].R * Math.Sign(a[i]);//半径
                    ZY.l = L[i] - JDs[i].l1 - JDs[i].l2;//缓长/圆曲线长
                    ZY.A = HY_A[i];//切线方位角 弧度值
                    horizontals.Add(ZY);

                    Horizontal YZ = new Horizontal();
                    YZ.mileage = YH_m[i];//里程
                    YZ.pType = "YZ";//主点类型 转点
                    YZ.X = YH_X[i];//北坐标
                    YZ.Y = YH_Y[i];//东坐标
                    YZ.R = JDs[i].R * Math.Sign(a[i]);//半径
                    YZ.l = JDs[i].l2;//缓长/圆曲线长
                    YZ.A = YH_A[i];//切线方位角 弧度值
                    horizontals.Add(YZ);
                }
                //复曲线情况 第一条曲线
                else if (JDs[i].l1 > 0.001 && JDs[i].l2 < 0.001 && Math.Abs(d[i + 1] - T2[i] - T1[i + 1]) < 0.001) {
                    Horizontal ZH = new Horizontal();
                    ZH.mileage = ZH_m[i];//里程
                    ZH.pType = "ZH";//主点类型 转点
                    ZH.X = ZH_X[i];//北坐标
                    ZH.Y = ZH_Y[i];//东坐标
                    ZH.R = 0;//半径
                    ZH.l = JDs[i].l1;//缓长/圆曲线长
                    ZH.A = ZH_A[i];//切线方位角 弧度值
                    horizontals.Add(ZH);

                    Horizontal HY = new Horizontal();
                    HY.mileage = HY_m[i];//里程
                    HY.pType = "HY";//主点类型 转点
                    HY.X = HY_X[i];//北坐标
                    HY.Y = HY_Y[i];//东坐标
                    HY.R = JDs[i].R * Math.Sign(a[i]);//半径
                    HY.l = L[i] - JDs[i].l1 - JDs[i].l2;//缓长/圆曲线长
                    HY.A = HY_A[i];//切线方位角 弧度值
                    horizontals.Add(HY);
                }
                //复曲线情况 YY点
                else if (JDs[i].l1 < 0.001 && JDs[i].l2 < 0.001 && JDs[i].h > 0.1 && JDs[i].R > 0.001 && JDs[i].curveType != 3) {

                    Horizontal YY = new Horizontal();
                    YY.mileage = HY_m[i];//里程
                    YY.pType = "YY";//主点类型 转点
                    YY.X = HY_X[i];//北坐标
                    YY.Y = HY_Y[i];//东坐标
                    YY.R = JDs[i].R * Math.Sign(a[i]);//半径
                    YY.l = L[i] - JDs[i].l1 - JDs[i].l2;//缓长/圆曲线长
                    YY.A = HY_A[i];//切线方位角 弧度值
                    horizontals.Add(YY);
                }
                //复曲线情况 第二条曲线
                else if (JDs[i].l1 < 0.001 && JDs[i].l2 > 0.001 && Math.Abs(d[i] - T2[i - 1] - T1[i]) < 0.001) {
                    Horizontal YY = new Horizontal();
                    YY.mileage = HY_m[i];//里程
                    YY.pType = "YY";//主点类型 转点
                    YY.X = HY_X[i];//北坐标
                    YY.Y = HY_Y[i];//东坐标
                    YY.R = JDs[i].R * Math.Sign(a[i]);//半径
                    YY.l = L[i] - JDs[i].l1 - JDs[i].l2;//缓长/圆曲线长
                    YY.A = HY_A[i];//切线方位角 弧度值
                    horizontals.Add(YY);

                    Horizontal YH = new Horizontal();
                    YH.mileage = YH_m[i];//里程
                    YH.pType = "YH";//主点类型 转点
                    YH.X = YH_X[i];//北坐标
                    YH.Y = YH_Y[i];//东坐标
                    YH.R = JDs[i].R * Math.Sign(a[i]);//半径
                    YH.l = JDs[i].l2;//缓长/圆曲线长
                    YH.A = YH_A[i];//切线方位角 弧度值
                    horizontals.Add(YH);

                    Horizontal HZ = new Horizontal();
                    HZ.mileage = HZ_m[i];//里程
                    HZ.pType = "HZ";//主点类型 转点
                    HZ.X = HZ_X[i];//北坐标
                    HZ.Y = HZ_Y[i];//东坐标
                    HZ.R = 0;//半径
                    HZ.l = 0;//缓长/圆曲线长
                    HZ.A = HZ_A[i];//切线方位角 弧度值
                    horizontals.Add(HZ);
                }
                //正常情况
                else {
                    Horizontal ZH = new Horizontal();
                    ZH.mileage = ZH_m[i];//里程
                    ZH.pType = "ZH";//主点类型 转点
                    ZH.X = ZH_X[i];//北坐标
                    ZH.Y = ZH_Y[i];//东坐标
                    ZH.R = 0;//半径
                    ZH.l = JDs[i].l1;//缓长/圆曲线长
                    ZH.A = ZH_A[i];//切线方位角 弧度值
                    horizontals.Add(ZH);

                    Horizontal HY = new Horizontal();
                    HY.mileage = HY_m[i];//里程
                    HY.pType = "HY";//主点类型 转点
                    HY.X = HY_X[i];//北坐标
                    HY.Y = HY_Y[i];//东坐标
                    HY.R = JDs[i].R * Math.Sign(a[i]);//半径
                    HY.l = L[i] - JDs[i].l1 - JDs[i].l2;//缓长/圆曲线长
                    HY.A = HY_A[i];//切线方位角 弧度值
                    horizontals.Add(HY);

                    Horizontal YH = new Horizontal();
                    YH.mileage = YH_m[i];//里程
                    YH.pType = "YH";//主点类型 转点
                    YH.X = YH_X[i];//北坐标
                    YH.Y = YH_Y[i];//东坐标
                    YH.R = JDs[i].R * Math.Sign(a[i]);//半径
                    YH.l = JDs[i].l2;//缓长/圆曲线长
                    YH.A = YH_A[i];//切线方位角 弧度值
                    horizontals.Add(YH);

                    Horizontal HZ = new Horizontal();
                    HZ.mileage = HZ_m[i];//里程
                    HZ.pType = "HZ";//主点类型 转点
                    HZ.X = HZ_X[i];//北坐标
                    HZ.Y = HZ_Y[i];//东坐标
                    HZ.R = 0;//半径
                    HZ.l = 0;//缓长/圆曲线长
                    HZ.A = HZ_A[i];//切线方位角 弧度值
                    horizontals.Add(HZ);
                }
            }
            //添加终点  ？？？再想想
            double ZDm = Math.Abs(a[JDs.Count - 1]) > Math.PI
                        ? HZ_m[JDs.Count - 1] + d[JDs.Count] + T2[JDs.Count - 1]
                        : HZ_m[JDs.Count - 1] + d[JDs.Count] - T2[JDs.Count - 1];

            Horizontal ZD = new Horizontal();
            ZD.mileage = ZDm;//里程
            ZD.pType = "ZX";//主点类型
            ZD.X = zd.X;//北坐标
            ZD.Y = zd.Y;//东坐标
            ZD.R = 0;//半径
            ZD.l = 0;//缓长/圆曲线长
            ZD.A = A[A.Length - 1];//切线方位角 弧度值
            horizontals.Add(ZD);
            #endregion

            return horizontals;
        }

        /// <summary>
        /// 计算竖曲线
        /// </summary>
        /// <param name="profile">坡度表</param>
        /// <returns>竖曲线list</returns>
        public static List<Vertical> CalVertical(List<BPD> profile) {
            int nums = profile.Count; //坡度表数组长度
            List<Vertical> verticalcurve = new List<Vertical>(); //待返回的竖曲线数组
            double[] slopes = new double[nums]; //坡度‰
            double[] slopeA = new double[nums]; //坡度角
            double[] ZY_m = new double[nums]; //ZY里程
            double[] ZY_H = new double[nums]; //ZY高程
            double[] YZ_m = new double[nums]; //YZ里程
            double[] YZ_H = new double[nums]; //YZ高程
            double[] T = new double[nums]; //切线长
            double[] S = new double[nums]; //竖曲线长
            double[] at = new double[nums]; //凹凸性，凸为1，凹为-1，竖曲线半径为0时等于0

            for (int i = 0; i < nums - 1; i++) {
                slopes[i] =
                  ((profile[i + 1].H - profile[i].H) /
                    (profile[i + 1].mileage - profile[i].mileage)) *
                  1000;
                slopeA[i] = Math.Atan(slopes[i] / 1000);
            }
            //计算第一个和最后一个竖曲线
            T[0] = S[0] = T[nums - 1] = S[nums - 1] = 0;
            at[0] = at[nums - 1] = 0;
            ZY_m[0] = YZ_m[0] = profile[0].mileage;
            ZY_H[0] = YZ_H[0] = profile[0].H;
            ZY_m[nums - 1] = YZ_m[nums - 1] = profile[nums - 1].mileage;
            ZY_H[nums - 1] = YZ_H[nums - 1] = profile[nums - 1].H;
            //添加第一个竖曲线
            Vertical QD = new Vertical();
            QD.mileage = ZY_m[0];//里程
            QD.pType = "ZY";//主点类型
            QD.H = ZY_H[0];//高程
            //todo:这里对坡度的处理可能会有问题
            QD.i = 1000;//坡度 ‰
            QD.R = profile[0].R * at[0];//半径 凸+凹-
            QD.T = T[0];//切线长
            QD.vCurve = S[0];//竖曲线长
            verticalcurve.Add(QD);

            Vertical SD1 = new Vertical();
            SD1.mileage = YZ_m[0];
            SD1.pType = "YZ";
            SD1.H = YZ_H[0];
            SD1.i = slopes[0];
            SD1.R = profile[0].R * at[0];
            SD1.T = T[0];
            SD1.vCurve = 0;
            verticalcurve.Add(SD1);

            //计算竖曲线
            for (int i = 1; i < nums - 1; i++) {
                double absIcha = 0;
                BPD bpd = profile[i];
                if (bpd.R < 0.1) at[i] = 0;
                else if (bpd.R > 0.1 && slopeA[i] - slopeA[i - 1] < 0) at[i] = 1;
                else if (bpd.R > 0.1 && slopeA[i] - slopeA[i - 1] > 0) at[i] = -1;
                absIcha = Math.Abs(slopeA[i] - slopeA[i - 1]);
                T[i] = bpd.R * Math.Tan(absIcha / 2);
                S[i] = bpd.R * absIcha;
                //计算竖曲线里程高程
                ZY_m[i] = bpd.mileage - T[i] * Math.Cos(slopeA[i - 1]);
                ZY_H[i] = bpd.H - (T[i] * Math.Cos(slopeA[i - 1]) * slopes[i - 1]) / 1000;
                YZ_m[i] = bpd.mileage + T[i] * Math.Cos(slopeA[i]);
                YZ_H[i] = bpd.H + (T[i] * Math.Cos(slopeA[i]) * slopes[i]) / 1000;
                //添加竖曲线
                Vertical ZY = new Vertical();
                ZY.mileage = ZY_m[i];
                ZY.pType = "ZY";
                ZY.H = ZY_H[i];
                ZY.i = slopes[i - 1];
                ZY.R = bpd.R * at[i];
                ZY.T = T[i];
                ZY.vCurve = S[i];
                verticalcurve.Add(ZY);

                Vertical YZ = new Vertical();
                YZ.mileage = YZ_m[i];
                YZ.pType = "YZ";
                YZ.H = YZ_H[i];
                YZ.i = slopes[i];
                YZ.R = bpd.R * at[i];
                YZ.T = T[i];
                YZ.vCurve = 0;
                verticalcurve.Add(YZ);
            }
            //添加终点
            Vertical ZD = new Vertical();
            ZD.mileage = ZY_m[nums - 1];
            ZD.pType = "ZY";
            ZD.H = ZY_H[nums - 1];
            ZD.i = slopes[nums - 2];
            ZD.R = profile[nums - 1].R * at[nums - 1];
            ZD.T = T[nums - 1];
            ZD.vCurve = S[nums - 1];
            verticalcurve.Add(ZD);

            Vertical SD2 = new Vertical();
            SD2.mileage = YZ_m[nums - 1];
            SD2.pType = "YZ";
            SD2.H = YZ_H[nums - 1];
            //todo:这里对坡度的处理可能会有问题
            SD2.i = 1000;
            SD2.R = profile[nums - 1].R * at[nums - 1];
            SD2.T = T[nums - 1];
            SD2.vCurve = 0;
            verticalcurve.Add(SD2);

            return verticalcurve;
        }

        /// <summary>
        /// 计算任意里程的轨枕三维坐标
        /// </summary>
        /// <param name="miles">任意里程数组</param>
        /// <param name="horizontal">平曲线对象数组</param>
        /// <param name="vertical">竖曲线对象数组</param>
        /// <param name="settings">设置对象</param>
        /// <returns>坐标对象数组</returns>
        public static List<Coordinate> CalculateCoors(double[] miles, List<Horizontal> horizontal, List<Vertical> vertical) {
            double midInterval = 1.505;
            int m = 0, n = 0;//平曲线索引，竖曲线索引
            double h, hl = 0, hr = 0, widen, widenR = 0, widenL = 0; //,超高,左轨超高,右轨超高,加宽值，左加宽值，右加宽值
            double H = 0, Hl = 0, Hr = 0, X = 0, Xl = 0, Xr = 0, Y = 0, Yl = 0, Yr = 0; //该里程处的中线高程,左轨高程，右轨高程，中线X，左轨X，右轨X，中线Y，左轨Y，右轨Y,该点所处区段类型
            string pType = "";
            double w = 0, a = 0;//竖象限角和切线方位角
            double spaceMile = 0;//空间里程
            List<Coordinate> coordinates = new List<Coordinate>();//要输出的任意里程坐标高程的数组
            for (int i = 0; i < miles.Length; i++) {
                double mile = miles[i];//当前轨枕里程
                //计算该里程区段
                for (int j = 0; j < horizontal.Count - 1; j++) {
                    //分段计算（n个五大桩点有n-1段）
                    if (mile > horizontal[j].mileage && mile < horizontal[j + 1].mileage) {
                        //在(i,i+1)区段
                        m = j;
                        break;
                    }
                    if (Math.Abs(mile - horizontal[j].mileage) < 0.001) {
                        //在主点上
                        m = j;
                        break;
                    }
                }
                Horizontal ZD1 = horizontal[m], //小里程主点
                           ZD2 = horizontal[m + 1]; //大里程主点
                Horizontal ZD0 = new Horizontal(), ZD3 = new Horizontal();
                if (m > 0) {
                    ZD0 = horizontal[m - 1]; //ZD1小里程主点
                }
                if (m < horizontal.Count - 2) {
                    ZD3 = horizontal[m + 2]; //ZD2大里程主点
                }
                double ZD1spMile = ZD1.mileage + ZD1.sp; //ZD1超高顺坡里程
                double ZD2spMile = ZD2.mileage + ZD2.sp; //ZD2超高顺坡里程
                double ZD1jkMile = ZD1.mileage + ZD1.jk; //ZD1加宽顺坡里程
                double ZD2jkMile = ZD2.mileage + ZD2.jk; //ZD2加宽顺坡里程
                #region 先统一计算超高和加宽，若有特殊情况则重新计算
                //计算超高
                h = (ZD1.h + (mile - ZD1spMile) / (ZD2spMile - ZD1spMile) * (ZD2.h - ZD1.h)) / 1000;
                //计算加宽
                widen = (ZD1.widen + Math.Abs(mile - ZD1jkMile) / (ZD2jkMile - ZD1jkMile) * (ZD2.jk - ZD1.jk)) / 1000;
                #endregion
                #region 计算中线高程
                //寻找该里程所在的竖曲线区间
                for (int j = 0; j < vertical.Count - 1; j++) {
                    //分段计算（n个竖曲线主点有n-1段）
                    if (mile > vertical[j].mileage && vertical[j + 1].mileage > mile) {
                        //判断在哪一段曲线上，在竖曲线主点之间时，取前一个【前进方向】
                        n = j;
                        break;
                    }
                    if (Math.Abs(mile - vertical[j].mileage) < 0.001) {
                        //在竖曲线主点时
                        n = j;
                        break;
                    }
                }
                Vertical SD1 = vertical[n], //小里程竖曲线主点
                         SD2 = vertical[n + 1]; //大里程竖曲线主点
                //在直线段时
                if (SD1.pType == "YZ" && SD2.pType == "ZY") {
                    H = SD1.H + (SD1.i * (mile - SD1.mileage)) / 1000;
                    w = Math.PI / 2 - Math.Abs(Math.Atan(SD1.i / 1000)); //竖象限角
                    double num = n / 2;
                    int bpi = Convert.ToInt32(Math.Floor(num)); //变坡点索引
                }
                //在竖曲线段时
                else if (SD1.pType == "ZY" && SD2.pType == "YZ" && Math.Abs(SD1.R) > 0.001) {
                    double k = SD1.i / 1000;
                    double L0 = SD1.mileage + SD1.R * k * Math.Sqrt(1 / (1 + k * k)); //圆心里程
                    double H0 = SD1.H - SD1.R * Math.Sqrt(1 / (1 + k * k)); //圆心高程
                    //任意点中线高程
                    //曲线右偏
                    if (SD1.R > 0) {
                        H = H0 + Math.Sqrt(Math.Pow(SD1.R, 2) - Math.Pow(mile - L0, 2));
                    }
                    //曲线左偏
                    else if (SD1.R < 0) {
                        H = H0 - Math.Sqrt(Math.Pow(SD1.R, 2) - Math.Pow(mile - L0, 2));
                    }
                    w = Math.PI / 2.0 - Math.Abs(Math.Atan2(H - H0, mile - L0) - (Math.Sign(SD1.R) * Math.PI) / 2); //竖象限角
                    //w = Math.PI / 2.0 - Math.Abs(Math.Atan2(Math.Abs(H - H0), Math.Abs(mile - L0))); //竖象限角,自己理解的，应该没问题
                }
                //与竖曲线主点重合
                else if (Math.Abs(mile - SD1.mileage) < 0.001 && Math.Abs(SD1.R) < 0.001) {
                    H = SD1.H;
                    w = Math.PI / 2.0 - Math.Abs(Math.Atan(SD1.i / 1000)); //竖象限角
                    double num = n / 2;
                }
                #endregion
                //计算左右轨坐标
                #region 点在直线上
                if (ZD1.pType == "ZX" || ZD1.pType == "HZ" || ZD1.pType == "YZ" || ZD1.pType == "ZD") {
                    //ZY_YZ型曲线直线段顺坡情况
                    //重新计算顺坡段超高
                    //1.该里程位于YZ大里程顺坡段
                    if (ZD1.pType == "YZ" && ZD1.sp > 0 && mile < ZD1spMile) {
                        h = ZD1.h * (ZD1spMile - mile) / Math.Abs(ZD1.sp) / 1000;
                    }
                    //2.该里程位于ZY小里程顺坡段
                    else if (ZD2.pType == "ZY" && ZD2.sp < 0 && mile > ZD2spMile) {
                        h = ZD2.h * (mile - ZD2spMile) / Math.Abs(ZD2.sp) / 1000;
                    }
                    //重新计算顺坡段加宽
                    //1.该里程位于YZ大里程顺坡段
                    if (ZD1.pType == "YZ" && ZD1.jk > 0 && mile < ZD1jkMile) {
                        widen = ZD1.widen * (ZD1jkMile - mile) / Math.Abs(ZD1.jk) / 1000;
                    }
                    //2.该里程位于ZY小里程顺坡段
                    else if (ZD2.pType == "ZY" && ZD2.jk < 0 && mile > ZD2jkMile) {
                        widen = ZD2.widen * (mile - ZD2jkMile) / Math.Abs(ZD2.jk) / 1000;
                    }
                    //非常规顺坡情况 重新计算顺坡段超高
                    //ZD0:YH ZD1:HZ, ZD2:ZH, ZD3:HY
                    //当ZH点sp为负时，需在直线段顺坡 即：顺坡起点 => 点P => ZH 顺坡起点:ZD2spMile, 顺坡终点ZD3.mileage + ZD3.sp
                    if (ZD2.pType == "ZH" && ZD2.sp < 0 && mile > ZD2spMile) {
                        h = ZD3.h * (mile - ZD2spMile) / (ZD3.mileage + ZD3.sp - ZD2spMile) / 1000;
                    }
                    //当HZ点sp为正时，需在直线段顺坡 即：HZ => 点P => 顺坡终点 顺坡起点:ZD0.mileage + ZD0.sp, 顺坡终点ZD1spMile
                    if (ZD1.pType == "HZ" && ZD1.sp > 0 && mile < ZD1spMile) {
                        h = ZD0.h * (ZD1spMile - mile) / (ZD1spMile - (ZD0.mileage + ZD0.sp)) / 1000;
                    }
                    //重新计算加宽 同上
                    if (ZD2.pType == "ZH" && ZD2.jk < 0 && mile > ZD2jkMile) {
                        widen = ZD3.widen * (mile - ZD2jkMile) / (ZD3.mileage + ZD3.jk - ZD2jkMile) / 1000;
                    }
                    //当HZ点sp为正时，需在直线段顺坡 即：HZ => 点P => 顺坡终点 顺坡起点:ZD0.mileage + ZD0.jk, 顺坡终点ZD1spMile
                    if (ZD1.pType == "HZ" && ZD1.jk > 0 && mile < ZD1jkMile) {
                        widen = ZD0.widen * (ZD1jkMile - mile) / (ZD1jkMile - (ZD0.mileage + ZD0.jk)) / 1000;
                    }
                    //计算左右轨加宽
                    if (mile < ZD1jkMile) {
                        //该里程位于大里程顺坡段
                        if (ZD1.R > 0) {
                            //半超高 加宽为内外轨各一半
                            if (ZD1.hType == "LR") {
                                widenL = widen / 2;
                                widenR = widenL;
                            }
                            //全超高顺坡 内轨加宽外轨不变
                            else if (ZD1.hType == "WG") {
                                widenL = 0;
                                widenR = widen;
                            }
                        }
                        else if (ZD1.R < 0) {
                            if (ZD1.hType == "LR") {
                                widenL = widen / 2;
                                widenR = widenL;
                            }
                            else if (ZD1.hType == "WG") {
                                widenL = widen;
                                widenR = 0;
                            }
                        }
                    }
                    else if (mile > ZD2jkMile) {
                        //该里程位于小里程顺坡段
                        if (ZD2.R > 0) {
                            if (ZD2.hType == "LR") {
                                widenL = widen / 2;
                                widenR = widenL;
                            }
                            else if (ZD2.hType == "WG") {
                                widenL = 0;
                                widenR = widen;
                            }
                        }
                        else if (ZD2.R < 0) {
                            if (ZD2.hType == "LR") {
                                widenL = widen / 2;
                                widenR = widenL;
                            }
                            else if (ZD2.hType == "WG") {
                                widenL = widen;
                                widenR = 0;
                            }
                        }
                    }
                    else {
                        widenL = widenR = 0; //非直线段顺坡，加宽都为0
                    }
                    //计算中线及左右轨坐标
                    X = ZD1.X + (mile - ZD1.mileage) * Math.Cos(ZD1.A); //中线北坐标
                    Y = ZD1.Y + (mile - ZD1.mileage) * Math.Sin(ZD1.A); //中线东坐标
                    Xl = X +
                         (Math.Sqrt(Math.Pow(midInterval + widenL, 2) - Math.Pow(h, 2)) / 2) *
                         Math.Cos(ZD1.A - Math.PI / 2); //左轨北坐标
                    Yl = Y +
                         (Math.Sqrt(Math.Pow(midInterval + widenL, 2) - Math.Pow(h, 2)) / 2) *
                         Math.Sin(ZD1.A - Math.PI / 2); //左轨东坐标
                    Xr = X +
                         (Math.Sqrt(Math.Pow(midInterval + widenR, 2) - Math.Pow(h, 2)) / 2) *
                         Math.Cos(ZD1.A + Math.PI / 2); //右轨北坐标
                    Yr = Y +
                         (Math.Sqrt(Math.Pow(midInterval + widenR, 2) - Math.Pow(h, 2)) / 2) *
                         Math.Sin(ZD1.A + Math.PI / 2); //右轨东坐标
                    pType = "Z";
                    a = ZD1.A; //切线方位角
                    //ZY_YZ型曲线直线段顺坡情况
                    //计算左右轨超高
                    if (mile < ZD1spMile) {
                        //该里程位于YZ大里程顺坡段
                        if (ZD1.R > 0) {
                            //半超高顺坡
                            if (ZD1.hType == "LR") {
                                hl = h / 2;
                                hr = -hl;
                            }
                            //全超高顺坡
                            else if (ZD1.hType == "WG") {
                                hl = h;
                                hr = 0;
                            }
                        }
                        else if (ZD1.R < 0) {
                            if (ZD1.hType == "LR") {
                                hr = h / 2;
                                hl = -hr;
                            }
                            else if (ZD1.hType == "WG") {
                                hr = h;
                                hl = 0;
                            }
                        }
                    }
                    else if (mile > ZD2spMile) {
                        //该里程位于ZY小里程顺坡段
                        if (ZD2.R > 0) {
                            if (ZD2.hType == "LR") {
                                hl = h / 2;
                                hr = -hl;
                            }
                            else if (ZD2.hType == "WG") {
                                hl = h;
                                hr = 0;
                            }
                        }
                        else if (ZD2.R < 0) {
                            if (ZD2.hType == "LR") {
                                hr = h / 2;
                                hl = -hr;
                            }
                            else if (ZD2.hType == "WG") {
                                hr = h;
                                hl = 0;
                            }
                        }
                    }
                    else {
                        hl = hr = 0; //非直线段顺坡，超高都为0
                    }
                }
                #endregion
                #region 该里程位于第一缓和曲线上
                else if (ZD1.pType == "ZH" && ZD2.pType == "HY") {
                    double d = mile - ZD1.mileage;
                    double x = d -
                               Math.Pow(d, 5) / (40 * Math.Pow(ZD2.R * ZD1.l, 2)) +
                               Math.Pow(d, 9) / (3456 * Math.Pow(ZD2.R * ZD1.l, 4));
                    double y = Math.Pow(d, 3) / (6 * Math.Abs(ZD2.R) * ZD1.l) -
                               Math.Pow(d, 7) / (336 * Math.Abs(Math.Pow(ZD2.R * ZD1.l, 3)));
                    y = Math.Sign(ZD2.R) * y;
                    X = ZD1.X + x * Math.Cos(ZD1.A) - y * Math.Sin(ZD1.A); //中线北坐标
                    Y = ZD1.Y + x * Math.Sin(ZD1.A) + y * Math.Cos(ZD1.A); //中线东坐标
                    //非常规顺坡情况：当该里程位于缓和曲线还未开始顺坡段，即ZH => 点P => 顺坡起点
                    //重新计算超高
                    if (mile < ZD1spMile) {
                        h = 0;
                    }
                    //非常规顺坡情况：当该里程位于缓和曲线顺坡结束段，即顺坡终点 => 点P => HY点
                    else if (mile > ZD2spMile) {
                        h = ZD2.h;
                    }
                    //非常规顺坡情况：重新计算加宽 原理同超高
                    if (mile < ZD1jkMile) {
                        widen = 0;
                    }
                    else if (mile > ZD2jkMile) {
                        widen = ZD2.widen;
                    }
                    // 计算左右轨超高
                    if (ZD2.R > 0) {
                        if (ZD2.hType == "LR") {
                            hl = h / 2;
                            hr = -hl;
                        }
                        else if (ZD2.hType == "WG") {
                            hl = h;
                            hr = 0;
                        }
                    }
                    else if (ZD2.R < 0) {
                        if (ZD2.hType == "LR") {
                            hr = h / 2;
                            hl = -hr;
                        }
                        else if (ZD2.hType == "WG") {
                            hr = h;
                            hl = 0;
                        }
                    }
                    // 计算左右轨加宽
                    if (ZD2.R > 0) {
                        if (ZD2.hType == "LR") {
                            widenL = widen / 2;
                            widenR = widenL;
                        }
                        else if (ZD2.hType == "WG") {
                            widenL = 0;
                            widenR = widen;
                        }
                    }
                    else if (ZD2.R < 0) {
                        if (ZD2.hType == "LR") {
                            widenL = widen / 2;
                            widenR = widenL;
                        }
                        else if (ZD2.hType == "WG") {
                            widenL = widen;
                            widenR = 0;
                        }
                    }
                    //计算左右轨坐标
                    Xl = X +
                         Math.Sqrt(Math.Pow(midInterval + widenL, 2) - Math.Pow(h, 2)) / 2 *
                         Math.Cos(ZD1.A + (d * d) / (2 * ZD2.R * ZD1.l) - Math.PI / 2); //左轨北坐标
                    Yl = Y +
                         Math.Sqrt(Math.Pow(midInterval + widenL, 2) - Math.Pow(h, 2)) / 2 *
                         Math.Sin(ZD1.A + (d * d) / (2 * ZD2.R * ZD1.l) - Math.PI / 2); //左轨东坐标
                    Xr = X +
                         Math.Sqrt(Math.Pow(midInterval + widenR, 2) - Math.Pow(h, 2)) / 2 *
                         Math.Cos(ZD1.A + (d * d) / (2 * ZD2.R * ZD1.l) + Math.PI / 2); //右轨北坐标
                    Yr = Y +
                         Math.Sqrt(Math.Pow(midInterval + widenR, 2) - Math.Pow(h, 2)) / 2 *
                         Math.Sin(ZD1.A + (d * d) / (2 * ZD2.R * ZD1.l) + Math.PI / 2); //右轨东坐标
                    pType = "H1";
                    a = ZD1.A + (d * d) / (2 * ZD2.R * ZD1.l); //切线方位角
                }
                #endregion
                #region 该里程位于第二缓和曲线上
                else if (ZD1.pType == "YH" && ZD2.pType == "HZ") {
                    double d = ZD2.mileage - mile;
                    double x = d -
                               Math.Pow(d, 5) / (40 * Math.Pow(ZD1.R * ZD1.l, 2)) +
                               Math.Pow(d, 9) / (3456 * Math.Pow(ZD1.R * ZD1.l, 4));
                    double y = Math.Pow(d, 3) / (6 * Math.Abs(ZD1.R) * ZD1.l) -
                               Math.Pow(d, 7) / (336 * Math.Abs(Math.Pow(ZD1.R * ZD1.l, 3)));
                    y = -Math.Sign(ZD1.R) * y;
                    X = ZD2.X + x * Math.Cos(ZD2.A + Math.PI) - y * Math.Sin(ZD2.A + Math.PI); //中线北坐标
                    Y = ZD2.Y + x * Math.Sin(ZD2.A + Math.PI) + y * Math.Cos(ZD2.A + Math.PI); //中线东坐标
                    //非常规顺坡情况：当该里程位于缓和曲线还未开始顺坡段，即YH => 点P => 顺坡起点
                    //重新计算超高
                    if (mile < ZD1spMile) {
                        h = ZD1.h;
                    }
                    //非常规顺坡情况：当该里程位于缓和曲线顺坡结束段，即顺坡终点 => 点P => HZ点
                    else if (mile > ZD2spMile) {
                        h = 0;
                    }
                    //非常规顺坡情况：重新计算加宽 原理同超高
                    if (mile < ZD1jkMile) {
                        widen = ZD1.widen;
                    }
                    else if (mile > ZD2spMile) {
                        widen = 0;
                    }
                    // 计算左右轨超高
                    if (ZD1.R > 0) {
                        if (ZD1.hType == "LR") {
                            hl = h / 2;
                            hr = -hl;
                        }
                        else if (ZD1.hType == "WG") {
                            hl = h;
                            hr = 0;
                        }
                    }
                    else if (ZD1.R < 0) {
                        if (ZD1.hType == "LR") {
                            hr = h / 2;
                            hl = -hr;
                        }
                        else if (ZD1.hType == "WG") {
                            hr = h;
                            hl = 0;
                        }
                    }
                    // 计算左右轨加宽
                    if (ZD1.R > 0) {
                        if (ZD1.hType == "LR") {
                            widenL = widen / 2;
                            widenR = widenL;
                        }
                        else if (ZD1.hType == "WG") {
                            widenL = 0;
                            widenR = widen;
                        }
                    }
                    else if (ZD1.R < 0) {
                        if (ZD1.hType == "LR") {
                            widenL = widen / 2;
                            widenR = widenL;
                        }
                        else if (ZD1.hType == "WG") {
                            widenL = widen;
                            widenR = 0;
                        }
                    }
                    //计算左右轨坐标
                    Xl = X +
                         Math.Sqrt(Math.Pow(midInterval + widenL, 2) - Math.Pow(h, 2)) / 2 *
                         Math.Cos(ZD2.A - (d * d) / (2 * ZD1.R * ZD1.l) - Math.PI / 2); //左轨北坐标
                    Yl = Y +
                         Math.Sqrt(Math.Pow(midInterval + widenL, 2) - Math.Pow(h, 2)) / 2 *
                         Math.Sin(ZD2.A - (d * d) / (2 * ZD1.R * ZD1.l) - Math.PI / 2); //左轨东坐标
                    Xr = X +
                         Math.Sqrt(Math.Pow(midInterval + widenR, 2) - Math.Pow(h, 2)) / 2 *
                         Math.Cos(ZD2.A - (d * d) / (2 * ZD1.R * ZD1.l) + Math.PI / 2); //右轨北坐标
                    Yr = Y +
                         Math.Sqrt(Math.Pow(midInterval + widenR, 2) - Math.Pow(h, 2)) / 2 *
                         Math.Sin(ZD2.A - (d * d) / (2 * ZD1.R * ZD1.l) + Math.PI / 2); //右轨东坐标
                    pType = "H2";
                    a = ZD2.A - (d * d) / (2 * ZD1.R * ZD1.l);
                }
                #endregion
                #region 该里程位于圆曲线端
                else if (ZD1.pType == "HY" || ZD1.pType == "YY" || ZD1.pType == "ZY") {
                    double d = mile - ZD1.mileage;
                    double x = Math.Abs(ZD1.R) * Math.Sin(d / Math.Abs(ZD1.R));
                    double y = Math.Abs(ZD1.R) * (1 - Math.Cos(d / Math.Abs(ZD1.R)));
                    y = Math.Sign(ZD1.R) * y;
                    X = ZD1.X + x * Math.Cos(ZD1.A) - y * Math.Sin(ZD1.A); //中线北坐标
                    Y = ZD1.Y + x * Math.Sin(ZD1.A) + y * Math.Cos(ZD1.A); //中线东坐标
                    //非常规顺坡情况 重新计算超高
                    // ZD0:ZH, ZD1:HY, ZD2:YH, ZD3: HZ
                    //1.点P位于圆曲线小里程顺坡段 顺坡起点 => HY => 点P => 顺坡终点
                    //顺坡起点:ZD0.mileage + ZD0.sp  顺坡终点:ZD1spMile
                    if (ZD1.pType == "HY" && ZD1.sp > 0 && mile < ZD1spMile) {
                        double spStart = ZD0.mileage + ZD0.sp; //顺坡起点里程
                        h = ZD1.h * (mile - spStart) / (ZD1spMile - spStart) / 1000;
                    }
                    //2.点P位于圆曲线大里程顺坡段 顺坡起点 => 点P => YH => 顺坡终点
                    //顺坡起点:ZD2spMile  顺坡终点:ZD3.mileage + ZD3.sp
                    if (ZD2.pType == "YH" && ZD2.sp < 0 && mile > ZD2spMile) {
                        double spEnd = ZD3.mileage + ZD3.sp; //顺坡终点
                        h = ZD2.h * (spEnd - mile) / (spEnd - ZD2spMile) / 1000;
                    }
                    //重新计算加宽 同上
                    if (ZD1.pType == "HY" && ZD1.jk > 0 && mile < ZD1jkMile) {
                        double spStart = ZD0.mileage + ZD0.jk; //顺坡起点里程
                        widen = ZD1.widen * (mile - spStart) / (ZD1jkMile - spStart) / 1000;
                    }
                    if (ZD2.pType == "YH" && ZD2.jk < 0 && mile > ZD2jkMile) {
                        double spEnd = ZD3.mileage + ZD3.jk; //顺坡终点
                        widen = ZD2.widen * (spEnd - mile) / (spEnd - ZD2jkMile) / 1000;
                    }
                    //计算左右轨加宽
                    if (ZD1.R > 0) {
                        if (ZD1.hType == "LR") {
                            widenL = widen / 2;
                            widenR = widenL;
                        }
                        else if (ZD1.hType == "WG") {
                            widenL = 0;
                            widenR = widen;
                        }
                    }
                    else if (ZD1.R < 0) {
                        if (ZD1.hType == "LR") {
                            widenR = widen / 2;
                            widenL = widenR;
                        }
                        else if (ZD1.hType == "WG") {
                            widenR = 0;
                            widenL = widen;
                        }
                    }
                    //计算左右轨坐标
                    Xl = X +
                         Math.Sqrt(Math.Pow(midInterval + widenL, 2) - Math.Pow(h, 2)) / 2 *
                         Math.Cos(ZD1.A + d / ZD1.R - Math.PI / 2); //左轨北坐标
                    Yl = Y +
                         Math.Sqrt(Math.Pow(midInterval + widenL, 2) - Math.Pow(h, 2)) / 2 *
                         Math.Sin(ZD1.A + d / ZD1.R - Math.PI / 2); //左轨东坐标
                    Xr = X +
                         Math.Sqrt(Math.Pow(midInterval + widenR, 2) - Math.Pow(h, 2)) / 2 *
                         Math.Cos(ZD1.A + d / ZD1.R + Math.PI / 2); //右轨北坐标
                    Yr = Y +
                         Math.Sqrt(Math.Pow(midInterval + widenR, 2) - Math.Pow(h, 2)) / 2 *
                         Math.Sin(ZD1.A + d / ZD1.R + Math.PI / 2); //右轨东坐标
                    //计算左右轨超高
                    if (ZD1.R > 0) {
                        if (ZD1.hType == "LR") {
                            hl = h / 2;
                            hr = -hl;
                        }
                        else if (ZD1.hType == "WG") {
                            hl = h;
                            hr = 0;
                        }
                    }
                    else if (ZD1.R < 0) {
                        if (ZD1.hType == "LR") {
                            hr = h / 2;
                            hl = -hr;
                        }
                        else if (ZD1.hType == "WG") {
                            hr = h;
                            hl = 0;
                        }
                    }
                    pType = "Y";
                    a = ZD1.A + d / ZD1.R;
                }
                #endregion
                //计算左右轨高程
                Hl = H + hl;
                Hr = H + hr;
                coordinates.Add(new Coordinate(
                    mile, //里程
                    spaceMile, //空间里程
                    X, //中线X
                    Y, //中线Y
                    H, //中线高程
                    Xl, //左轨X
                    Yl, //左轨Y
                    Hl, //左轨高程
                    Xr, //右轨X
                    Yr, //右轨Y
                    Hr,//右轨高程
                    a, w, pType //切线方位角，竖象限角，点类型
                    ));
            }
            return coordinates;
        }

        /// <summary>
        /// 计算实测点的里程（里程反算）
        /// </summary>
        /// <param name="points">实测点对象list</param>
        /// <param name="horizontal">平曲线对象list</param>
        /// <returns>里程list<double></returns>
        public static double[] CalPointMile(List<Coordinate> points, List<Horizontal> horizontal) {
            List<double> mileages = new List<double>(); //待返回的测量点或CP3的里程
            for (int i = 0; i < points.Count; i++) {
                Coordinate point = points[i]; //当前循环的测量点或CP3
                //寻找距离最近的五大桩点P,索引为P_i,距离为minDistance
                double minDistance = 1000000; int P_i = 0;
                for (int j = 0; j < horizontal.Count; j++) {
                    Horizontal curve = horizontal[j];
                    double temp = Math.Sqrt(Math.Pow(point.X - curve.X, 2) + Math.Pow(point.Y - curve.Y, 2)); //测量点到主点的距离
                    if (temp < minDistance) {
                        minDistance = temp;
                        P_i = j;//找到距离最近的那个主点的索引号
                    }
                }
                // 判断轨道设计中线点B所在的P位置关系。
                // 夹角a=点P切线方向PB的方位角-PO的方位角（以P为原点）
                double a = 0;
                double pb = Common.GetAzimuth(
                  horizontal[P_i].X,
                  horizontal[P_i].Y,
                  point.X,
                  point.Y
                ); //PB的方位角
                double po = horizontal[P_i].A; //PO的方位角
                //南北向曲线特殊情况
                if (
                  pb < Math.PI / 2 &&
                  pb > 0 &&
                  po > (Math.PI * 3) / 2 &&
                  po < 2 * Math.PI
                ) {
                    a = 2 * Math.PI - po + pb;
                }
                //正常情况
                else {
                    a = Math.Abs(pb - po); //取了绝对值
                }
                //当a=90°，这时点P与点B重合，点P就是找到的点
                if (Math.Abs(a - Math.PI / 2) < 0.0000000005) {
                    mileages.Add(horizontal[P_i].mileage);
                    continue;
                }
                //判断点B在P大里程还是小里程
                else if (a > Math.PI / 2) P_i = P_i - 1;
                Horizontal ZD = horizontal[P_i]; //点P
                Horizontal ZD2 = horizontal[P_i + 1]; //点P大里程反向的主点
                //计算点B的里程
                double mile = 0, BX = 0, BY = 0; //点B的里程，点B的坐标
                //点B在直线上
                if (
                  ZD.pType == "ZX" ||
                  ZD.pType == "HZ" ||
                  ZD.pType == "YZ" ||
                  ZD.pType == "ZD"
                ) {
                    double k = Math.Tan(ZD.A); //斜率
                    double b = ZD.Y - k * ZD.X; //常数项
                    BX = (point.X + k * point.Y - k * b) / (1 + k * k);
                    BY = (k * point.X + k * k * point.Y + b) / (1 + k * k);
                    mile = ZD.mileage + Math.Sqrt(Math.Pow(ZD.X - BX, 2) + Math.Pow(ZD.Y - BY, 2));
                }
                //点B在第一缓和曲线上
                else if (ZD.pType == "ZH" && ZD2.pType == "HY") {
                    //缓圆点处
                    //实测点O在ZH独立坐标系下坐标
                    double x0 = 0, y0 = 0;
                    if (ZD2.R < 0) {
                        //左偏
                        x0 = (point.X - ZD.X) * Math.Cos(ZD.A) + (point.Y - ZD.Y) * Math.Sin(ZD.A);
                        y0 = (point.X - ZD.X) * Math.Sin(ZD.A) - (point.Y - ZD.Y) * Math.Cos(ZD.A);
                    }
                    if (ZD2.R > 0) {
                        //右偏
                        x0 = (point.X - ZD.X) * Math.Cos(ZD.A) + (point.Y - ZD.Y) * Math.Sin(ZD.A);
                        y0 = -(point.X - ZD.X) * Math.Sin(ZD.A) + (point.Y - ZD.Y) * Math.Cos(ZD.A);
                    }
                    double l = Common.Newton(ZD2.R, ZD.l, x0, y0);
                    mile = ZD.mileage + l;
                }
                //点B在第二缓和曲线上
                else if (ZD.pType == "YH" && ZD2.pType == "HZ") {
                    //点O在HZ独立坐标系下坐标
                    double x0 = 0, y0 = 0;
                    if (ZD.R < 0) {
                        //左偏
                        x0 = (point.X - ZD2.X) * Math.Cos(ZD2.A + Math.PI) +
                             (point.Y - ZD2.Y) * Math.Sin(ZD2.A + Math.PI);
                        y0 = -(point.X - ZD2.X) * Math.Sin(ZD2.A + Math.PI) +
                             (point.Y - ZD2.Y) * Math.Cos(ZD2.A + Math.PI);
                    }
                    if (ZD.R > 0) {
                        //右偏
                        x0 = (point.X - ZD2.X) * Math.Cos(ZD2.A + Math.PI) +
                             (point.Y - ZD2.Y) * Math.Sin(ZD2.A + Math.PI);
                        y0 = (point.X - ZD2.X) * Math.Sin(ZD2.A + Math.PI) -
                             (point.Y - ZD2.Y) * Math.Cos(ZD2.A + Math.PI);
                    }
                    double l = Common.Newton(ZD.R, ZD.l, x0, y0); //弧长
                    mile = ZD2.mileage - l;
                }
                //点B在圆曲线上
                if (ZD.pType == "HY" || ZD.pType == "YY" || ZD.pType == "ZY") {
                    double x0 = 0.0, y0 = 0.0;
                    //先计算实测点O在YH独立坐标系下坐标
                    if (ZD.R < 0) {
                        //左偏
                        x0 = (point.X - ZD.X) * Math.Cos(ZD.A) + (point.Y - ZD.Y) * Math.Sin(ZD.A);
                        y0 = (point.X - ZD.X) * Math.Sin(ZD.A) - (point.Y - ZD.Y) * Math.Cos(ZD.A);
                    }
                    if (ZD.R > 0) {
                        //右偏
                        x0 = (point.X - ZD.X) * Math.Cos(ZD.A) + (point.Y - ZD.Y) * Math.Sin(ZD.A);
                        y0 = -(point.X - ZD.X) * Math.Sin(ZD.A) + (point.Y - ZD.Y) * Math.Cos(ZD.A);
                    }
                    //曲线长l所对应的弧度
                    double bta = 0;
                    //小半径大曲线情况
                    if (Math.Abs(y0) <= Math.Abs(ZD.R)) {
                        //当独立坐标系中的y0小于半径时
                        bta = Math.Atan(x0 / Math.Abs(Math.Abs(ZD.R) - y0));
                    }
                    else {
                        //当独立坐标系中的y0大于半径时
                        bta = Math.PI - Math.Atan(x0 / Math.Abs(Math.Abs(ZD.R) - y0));
                    }
                    mile = ZD.mileage + Math.Abs(ZD.R) * bta;
                }
                mileages.Add(mile);
            }
            return mileages.ToArray();
        }

        /// <summary>
        /// 计算实测点的横垂偏差
        /// </summary>
        /// <param name="points">实测点list</param>
        /// <param name="horizontal">平曲线list</param>
        /// <param name="vertical">竖曲线list</param>
        /// <param name="settings">设置值</param>
        /// <returns>横垂偏差list</returns>
        public static List<Deviation> CalDeviation(List<Coordinate> points, List<Horizontal> horizontal, List<Vertical> vertical) {
            double[] mileages = CalPointMile(points, horizontal);
            List<Coordinate> coors = CalculateCoors(mileages, horizontal, vertical);//实测点反算里程的设计坐标，也就是原设计文件的坐标
            //分布为距离中线横偏，左轨横偏，右轨横偏，中线竖偏，左轨竖偏，右轨竖偏
            double fym = 0, fyl = 0, fyr = 0, fhm = 0, fhl = 0, fhr = 0;
            List<Deviation> deviations = new List<Deviation>();
            for (int i = 0; i < points.Count; i++) {
                Coordinate coor = coors[i]; //当前循环的测量点反算里程的设计坐标
                Coordinate survey = points[i]; //当前循环的测量点
                fym =
                  -1000 *
                  ((coor.X - survey.X) * Math.Sin(coor.A) -
                    (coor.Y - survey.Y) * Math.Cos(coor.A)); //中线横向偏差
                fhm = 1000 * ((survey.H - coor.H) * Math.Sin(coor.w)); //中线垂向偏差
                fyl =
                  -1000 *
                  ((coor.Xl - survey.X) * Math.Sin(coor.A) -
                    (coor.Yl - survey.Y) * Math.Cos(coor.A)); //左轨横向偏差
                fhl = 1000 * ((survey.H - coor.Hl) * Math.Sin(coor.w)); //左轨垂向偏差
                fyr =
                  -1000 *
                  ((coor.Xr - survey.X) * Math.Sin(coor.A) -
                    (coor.Yr - survey.Y) * Math.Cos(coor.A)); //右轨横向偏差
                fhr = 1000 * ((survey.H - coor.Hr) * Math.Sin(coor.w)); //右轨垂向偏差
                Deviation dva = new Deviation();
                dva.designMile = mileages[i];
                dva.fym = fym;
                dva.fhm = fhm;
                dva.fyl = fyl;
                dva.fhl = fhl;
                dva.fyr = fyr;
                dva.fhr = fhr;
                dva.ID = coor.pType;
                deviations.Add(dva);
            }
            return deviations;
        }

        /// <summary>
        /// 重构曲线表
        /// </summary>
        /// <param name="JDs">原曲线表</param>
        /// <param name="qd">起点数据</param>
        /// <param name="zd">终点数据</param>
        /// <param name="at">定位参数</param>
        /// <param name="changenum">转向角改变量</param>
        /// <returns></returns>
        public static List<JD> CalJD(List<JD> JDs, ZX qd, ZX zd, int at, double changenum) {
            List<JD> JDs1 = new List<JD>();
            double[] A = new double[JDs.Count + 1];//交点方位角数组
            double[] d = new double[JDs.Count + 1];//交点间距数组
            double[] a = new double[JDs.Count];//转向角
            #region 求方位角和交点间距
            A[0] = Common.GetAzimuth(qd.X, qd.Y, JDs[0].X, JDs[0].Y);
            d[0] = Math.Sqrt(Math.Pow(JDs[0].Y - qd.Y, 2) + Math.Pow(JDs[0].X - qd.X, 2));
            for (int i = 1; i < JDs.Count; i++) {
                //求取方位角
                A[i] = Common.GetAzimuth(
                  JDs[i - 1].X,
                  JDs[i - 1].Y,
                  JDs[i].X,
                  JDs[i].Y
                );
                //求交点间距
                d[i] = Math.Sqrt(Math.Pow(JDs[i].Y - JDs[i - 1].Y, 2) + Math.Pow(JDs[i].X - JDs[i - 1].X, 2));
            }
            JD lastJd = JDs[JDs.Count - 1];//获得最后一个交点的值（不是终点）
            A[A.Length - 1] = Common.GetAzimuth(lastJd.X, lastJd.Y, zd.X, zd.Y);//最后一个方位角
            d[d.Length - 1] = Math.Sqrt(Math.Pow(zd.Y - lastJd.Y, 2) + Math.Pow(zd.X - lastJd.X, 2));//最后一个点间距
            #endregion
            #region 求取转向角
            for (int i = 0; i < JDs.Count; i++) {
                JD JD = JDs[i];
                a[i] = A[i + 1] - A[i]; //求转角
                //使得该回头曲线的转向角绝对值始终大于180
                if (JD.curveType == 1 && Math.Abs(a[i]) < Math.PI && a[i] < 0) {
                    a[i] += 2 * Math.PI;
                }
                if (JD.curveType == 1 && a[i] < Math.PI && a[i] > 0) {
                    a[i] -= 2 * Math.PI;
                }
                //反向曲线情况
                if (JD.curveType == 2) a[i] += Math.PI;
                //修正南北向线性转向角出错
                else if (JD.curveType != 1 && a[i] > 0 && Math.Abs(a[i]) >= Math.PI)
                    a[i] -= 2 * Math.PI;
                else if (JD.curveType != 1 && a[i] < 0 && Math.Abs(a[i]) >= Math.PI)
                    a[i] += 2 * Math.PI;
            }
            #endregion
            //重新求取方位角，用于验证
            if (A[0] < 0) A[0] += 2 * Math.PI;
            for (int j = 1; j < JDs.Count + 1; j++) {
                A[j] = A[j - 1] + a[j - 1];
                A[j] = A[j] <= 0 ? A[j] + 2 * Math.PI : A[j];
                A[j] = A[j] >= 2 * Math.PI ? A[j] - 2 * Math.PI : A[j];
            }
            #region 重新计算转向角和切线方位角并修正
            a[at] = changenum;//获取到需要变化的方位角位置并加上变化量
            //使得该回头曲线的转向角绝对值始终大于180
            JD JDa = JDs[at];
            if (JDa.curveType == 1 && Math.Abs(a[at]) < Math.PI && a[at] < 0) {
                a[at] += 2 * Math.PI;
            }
            if (JDa.curveType == 1 && a[at] < Math.PI && a[at] > 0) {
                a[at] -= 2 * Math.PI;
            }
            //反向曲线情况
            if (JDa.curveType == 2) a[at] += Math.PI;
            //修正南北向线性转向角出错
            else if (JDa.curveType != 1 && a[at] > 0 && Math.Abs(a[at]) >= Math.PI)
                a[at] -= 2 * Math.PI;
            else if (JDa.curveType != 1 && a[at] < 0 && Math.Abs(a[at]) >= Math.PI)
                a[at] += 2 * Math.PI;
            #endregion
            //重新求取改线后方位角
            if (A[0] < 0) A[0] += 2 * Math.PI;
            for (int j = 1; j < JDs.Count + 1; j++) {
                A[j] = A[j - 1] + a[j - 1];
                A[j] = A[j] <= 0 ? A[j] + 2 * Math.PI : A[j];
                A[j] = A[j] >= 2 * Math.PI ? A[j] - 2 * Math.PI : A[j];
            }
            #region 重新计算交点坐标
            JDs1 = JDs;//对重构交点开空间并赋原始值
            //重新计算第一个交点的坐标
            JDs1[0].X = qd.X + d[0] * Math.Cos(A[0]);
            JDs1[0].Y = qd.Y + d[0] * Math.Sin(A[0]);
            JDs1[0].a = a[0];
            //重新计算后面所有交点坐标
            for (int i = 1; i < JDs.Count; i++) {
                JDs1[i].X = JDs1[i - 1].X + d[i] * Math.Cos(A[i]);
                JDs1[i].Y = JDs1[i - 1].Y + d[i] * Math.Sin(A[i]);
                JDs1[i].a = a[i];
            }
            #endregion
            return JDs1;
        }
    }
}
